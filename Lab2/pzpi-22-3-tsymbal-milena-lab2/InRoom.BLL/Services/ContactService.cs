using InRoom.BLL.Contracts.User;
using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;

namespace InRoom.BLL.Services;

public class ContactService : GenericService<Contact>, IContactService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IContactRepository _contactRepository;
    private readonly IDiseaseRepository _diseaseRepository;

    // Constructor to inject required dependencies for the ContactService
    public ContactService(
        IUnitOfWork unitOfWork, 
        IUserRepository userRepository, 
        IDeviceRepository deviceRepository,
        IContactRepository contactRepository,
        IDiseaseRepository diseaseRepository) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _deviceRepository = deviceRepository;
        _contactRepository = contactRepository;
        _diseaseRepository = diseaseRepository;
    }
    
    // Method to create a new contact between two users with a specific distance and device
    public async Task<Contact> Add(Guid initiatorId, Guid receiverId, Guid deviceId,
        float initiatorX, float initiatorY, float initiatorZ, 
        float receiverX, float receiverY, float receiverZ)
    {
        const float MaxAllowedDistance = 5.0f;
        const int ContactTimeoutMinutes = 2;
        
        var initiator = await _userRepository.GetById(initiatorId);
        if (initiator == null)
        {
            throw new ApiException($"User initiator with ID {initiatorId} not found.", 404);
        }
        
        var receiver = await _userRepository.GetById(receiverId);
        if (receiver == null)
        {
            throw new ApiException($"User receiver with ID {receiverId} not found.", 404);
        }
        
        var distance = CalculateDistance(initiatorX, initiatorY, initiatorZ, receiverX, receiverY, receiverZ);

        if (distance > MaxAllowedDistance)
        {
            throw new ApiException($"Contact cannot be created. Distance ({distance}m) exceeds the maximum allowed value of {MaxAllowedDistance}m.", 400);
        }
        
        var device = await _deviceRepository.GetById(deviceId);
        if (device == null)
        {
            throw new ApiException($"Device with ID {deviceId} not found.", 404);
        }
        
        var now = DateTime.Now;
        
        var existingContact = (await Repository.GetAll())
            .Where(c =>
                ((c.ContactInitiatorId == initiatorId && c.ContactReceiverId == receiverId) ||
                 (c.ContactInitiatorId == receiverId && c.ContactReceiverId == initiatorId)) &&
                (
                    c.ContactEndTime == null || 
                    (c.ContactEndTime.HasValue && (now - c.ContactEndTime.Value).TotalMinutes <= ContactTimeoutMinutes)
                )
            )
            .OrderByDescending(c => c.ContactStartTime)
            .FirstOrDefault();

        if (existingContact != null)
        {
            existingContact.MinDistance = Math.Min(existingContact.MinDistance, distance);
            existingContact.ContactEndTime = now;

            await Repository.Update(existingContact);
            await _unitOfWork.SaveChangesAsync();

            return existingContact;
        }
        
        var newContact = new Contact()
        {
            ContactId = Guid.NewGuid(),
            ContactInitiatorId = initiatorId,
            ContactInitiator = initiator,
            ContactReceiverId = receiverId,
            ContactReceiver = receiver,
            DeviceId = deviceId,
            Device = device,
            MinDistance = distance
        };
      
        await Repository.Add(newContact);
        await _unitOfWork.SaveChangesAsync();

        return newContact;
    }
    
    // Method to calculate the distance between users
    private double CalculateDistance(float userX, float userY, float userZ, float deviceX, float deviceY, float deviceZ)
    {
        return Math.Sqrt(Math.Pow(deviceX - userX, 2) + Math.Pow(deviceY - userY, 2) + Math.Pow(deviceZ - userZ, 2));
    }

    // Method to update an existing contact with a new distance value
    public async Task<Contact> Update(Guid contactId, float distance)
    {
        var contact = await Repository.GetById(contactId);
        if (contact == null)
        {
            throw new ApiException($"Contact with ID {contactId} not found.", 404);
        }
        
        if (distance < 0)
        {
            throw new ApiException("Distance cannot be negative.", 400);
        }
        
        contact.ContactEndTime = DateTime.Now;
        contact.MinDistance = distance < contact.MinDistance ? distance : contact.MinDistance;
        
        await Repository.Update(contact);
        await _unitOfWork.SaveChangesAsync();

        return contact;
    }

    // Method to get movements report
    public async Task<ContacsReportResponse> GetContactsReport(Guid userId, DateTime startDate, DateTime endDate)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
        {
            throw new ApiException($"User with ID {userId} not found.", 404);
        }

        var contacts = await _contactRepository.GetContactsByDateRangeAndUserId(userId, startDate, endDate);
        
        var contactsReportResponse = new ContacsReportResponse()
        {
            User = user,
            StartDate = startDate,
            EndDate = endDate,
            Contacts = contacts
        };
        
        return contactsReportResponse;
    }

    // Method to get epidemiological risk analysis
   public async Task<EpidemiologicalRiskAnalysisResponse> AnalyzeEpidemiologicalRisk(Guid userId)
    {
        var user = await _userRepository.GetById(userId);
        if (user == null)
        {
            throw new ApiException($"User with ID {userId} not found.", 404);
        }

        var contacts = await _contactRepository.GetByUserIdAndDaysNumber(userId, 14);
        double totalRisk = 0;
        int highRiskContacts = 0;
        int totalContacts = contacts.Count;
        double totalContactDuration = 0;
        var potentialDiseases = new HashSet<Disease>();

        var userDisease = user.Disease;
        bool isContagious = userDisease != null && userDisease.Contagious;

        if (contacts.Any())
        {
            foreach (var contact in contacts)
            {
                double contactDuration = contact.ContactEndTime.HasValue
                    ? (contact.ContactEndTime.Value - contact.ContactStartTime).TotalMinutes
                    : 0;

                totalContactDuration += contactDuration;

                var contactInitiator = await _userRepository.GetById(contact.ContactInitiatorId);
                if (contactInitiator.DiseaseId.HasValue)
                {
                    contactInitiator.Disease = await _diseaseRepository.GetById(contactInitiator.DiseaseId.Value);
                }

                var contactReceiver = await _userRepository.GetById(contact.ContactReceiverId);
                if (contactReceiver.DiseaseId.HasValue)
                {
                    contactReceiver.Disease = await _diseaseRepository.GetById(contactReceiver.DiseaseId.Value);
                }

                bool initiatorHasDisease = contactInitiator.Disease != null && contactInitiator.Disease.Contagious;
                bool receiverHasDisease = contactReceiver.Disease != null && contactReceiver.Disease.Contagious;

                double risk = 0;

                if (contactDuration > 15)
                {
                    risk += 0.3;
                }

                if (contact.ContactInitiatorId == userId && receiverHasDisease)
                {
                    risk += 0.5;

                    if (contact.MinDistance < 2)
                    {
                        risk += 0.2;
                    }

                    totalRisk += risk;

                    if (contactReceiver.Disease != null)
                    {
                        potentialDiseases.Add(contactReceiver.Disease);
                    }
                }
                else if (contact.ContactReceiverId == userId && initiatorHasDisease)
                {
                    risk += 0.5;

                    if (contact.MinDistance < 2)
                    {
                        risk += 0.2;
                    }

                    totalRisk += risk;

                    if (contactInitiator.Disease != null)
                    {
                        potentialDiseases.Add(contactInitiator.Disease);
                    }
                }

                if (initiatorHasDisease || receiverHasDisease)
                {
                    highRiskContacts++;
                }
            }

            double averageRisk = totalRisk / totalContacts;
            TimeSpan averageContactDuration = TimeSpan.FromMinutes(totalContactDuration / totalContacts);

            string riskLevel = "Low";
            if (averageRisk > 0.6)
            {
                riskLevel = "High";
            }
            else if (averageRisk > 0.3)
            {
                riskLevel = "Moderate";
            }

            return new EpidemiologicalRiskAnalysisResponse
            {
                User = user,
                Contacts = contacts,
                TotalRisk = totalRisk,
                UserDisease = userDisease,
                IsContagious = isContagious,
                TotalContagionRate = totalRisk,
                AverageContactDuration = averageContactDuration,
                RiskLevel = riskLevel,
                PotentialDiseases = potentialDiseases.ToList()
            };
        }

        return new EpidemiologicalRiskAnalysisResponse
        {
            User = user,
            Contacts = new List<Contact>(),
            TotalRisk = 0,
            UserDisease = userDisease,
            IsContagious = isContagious,
            TotalContagionRate = 0,
            AverageContactDuration = TimeSpan.Zero,
            RiskLevel = "Low",
            PotentialDiseases = new List<Disease>() 
        };
    }
}
