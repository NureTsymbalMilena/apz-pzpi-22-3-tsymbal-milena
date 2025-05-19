using InRoom.BLL.Contracts.User;
using InRoom.BLL.Helpers;
using InRoom.BLL.Interfaces;
using InRoom.DAL.Interfaces;
using InRoom.DLL.Models;


namespace InRoom.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IHospitalRepository _hospitalRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    
    // Constructor to inject dependencies required for user authentication
    public AuthService(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher, 
        IHospitalRepository hospitalRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _hospitalRepository = hospitalRepository;
        _tokenService = tokenService;
    }

    // Method to register a new user
    public async Task Register(string name, string surname, string email, string password, string hospitalName)
    {
        var user = await _userRepository.GetByEmail(email);
        if (user != null)
        {
            throw new ApiException($"This email address ({email}) is already registered", 400);
        }
        
        var hashedPassword = _passwordHasher.Generate(password);

        var hospital = await _hospitalRepository.GetByName(hospitalName);
        if (hospital == null)
        {
            throw new ApiException($"Hospital {hospitalName} is not found", 404);
        }

        var newUser = new User
        {
            UserId = Guid.NewGuid(),
            Name = name,
            Surname = surname,
            Email = email,
            Password = hashedPassword,
            HospitalId = hospital.HospitalId,
            Hospital = hospital
        };
        
        await _userRepository.Add(newUser);
    }

    // Method to authenticate a user and return a response with access and refresh tokens
    public async Task<LoginUserResponse> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);
        
        if (user == null)
        {
            throw new ApiException($"User with email {email} wasn't found", 404);
        }
        
        var result = _passwordHasher.Verify(password, user.Password);

        if (!result)
        {
            throw new ApiException("Incorrect password", 400);
        }

        var loginUserResult = new LoginUserResponse();
        
        var (accessToken, refreshToken) = await _tokenService.GenerateTokens(user);

        loginUserResult.AccessToken = accessToken;
        loginUserResult.RefreshToken = refreshToken;

        return loginUserResult;
    }
}
