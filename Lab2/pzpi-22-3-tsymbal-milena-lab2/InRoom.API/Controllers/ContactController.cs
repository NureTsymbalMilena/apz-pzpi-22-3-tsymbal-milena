using InRoom.API.Contracts.Contact;
using InRoom.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InRoom.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }
    
    // Endpoint for creating a new contact between users
    [HttpPost]
    [SwaggerOperation("Create contact")]
    public async Task<IActionResult> AddContact(CreateContactRequest request)
    {
        var contact = await _contactService.Add(
            request.InitiatorId, 
            request.ReceiverId, 
            request.DeviceId, 
            request.InitiatorX, 
            request.InitiatorY, 
            request.InitiatorZ, 
            request.ReceiverX, 
            request.ReceiverY, 
            request.ReceiverZ
        );

        return Ok(contact);
    }
    
    // Endpoint for retrieving a contact by its ID
    [HttpGet("{contactId:Guid}")]
    [SwaggerOperation("Get contact by id")]
    public async Task<IActionResult> GetContact([FromRoute] Guid contactId)
    {
        var contact = await _contactService.GetById(contactId);
        return Ok(contact);
    }
    
    // Endpoint for retrieving all contacts
    [HttpGet]
    [SwaggerOperation("Get all contacts")]
    public async Task<IActionResult> GetAllContacts()
    {
        var contacts = await _contactService.GetAll();
        return Ok(contacts);
    }
    
    // Endpoint for updating the contact's distance by its ID
    [HttpPatch("{contactId:Guid}")]
    [SwaggerOperation("Edit contact by id")]
    public async Task<IActionResult> EditContact([FromRoute] Guid contactId, UpdateContactRequest request)
    {
        var contact = await _contactService.Update(contactId, request.Distance);
        return Ok(contact);
    }
    
    // Endpoint for deleting a contact by its ID
    [HttpDelete("{contactId:Guid}")]
    [SwaggerOperation("Delete contact by id")]
    public async Task<IActionResult> DeleteContact([FromRoute] Guid contactId)
    {
        var contact = await _contactService.Delete(contactId);
        return Ok(contact);
    }
    
    // Endpoint for generating a report on contacts for a specified user and time period
    [HttpGet("{userId:Guid}/report")]
    [SwaggerOperation("Get contact report for a specific period")]
    public async Task<IActionResult> GenerateReport([FromRoute] Guid userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var contactsReport = await _contactService.GetContactsReport(userId, startDate, endDate);
        return Ok(contactsReport);
    }
    
    // Endpoint for analyzing contacts for epidemiological risks
    [HttpPost("{userId:Guid}/epidemiological-risk-analysis")]
    [SwaggerOperation("Analyze contacts for epidemiological risks")]
    public async Task<IActionResult> AnalyzeEpidemiologicalRisk([FromRoute] Guid userId)
    {
        var analysisResult = await _contactService.AnalyzeEpidemiologicalRisk(userId);
        return Ok(analysisResult);
    }
}
