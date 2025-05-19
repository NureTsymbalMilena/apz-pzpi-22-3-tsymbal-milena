using InRoom.API.Extensions;
using InRoom.API.Helpers;
using InRoom.API.Middleware;
using InRoom.BLL.Interfaces;
using InRoom.BLL.Jwt;
using InRoom.BLL.PasswordHasher;
using InRoom.BLL.Services;
using InRoom.DAL;
using InRoom.DAL.Interfaces;
using InRoom.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace InRoom.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

        builder.Services.AddApiAuthentication(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
        });
        builder.Services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введіть токен у форматі 'Bearer {your JWT token}'"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IHospitalRepository, HospitalRepository>();
        builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
        builder.Services.AddScoped<IRoomRepository, RoomRepository>();
        builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
        builder.Services.AddScoped<IContactRepository, ContactRepository>();
        builder.Services.AddScoped<IMovementRepository, MovementRepository>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        builder.Services.AddScoped<IDiseaseRepository, DiseaseRepository>();
        builder.Services.AddScoped<IRequestLogRepository, RequestLogRepository>();
        
        builder.Services.AddScoped<IHospitalService, HospitalService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IZoneService, ZoneService>();
        builder.Services.AddScoped<IRoomService, RoomService>();
        builder.Services.AddScoped<IDeviceService, DeviceService>();
        builder.Services.AddScoped<IContactService, ContactService>();
        builder.Services.AddScoped<IMovementService, MovementService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IDiseaseService, DiseaseService>();
        builder.Services.AddScoped<IRequestLogService, RequestLogService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        builder.Services.AddTransient<IAdminService>(provider =>
        {
            string? connectionString = builder.Configuration.GetConnectionString("DefaultString");
            IUserRepository userRepository = provider.GetRequiredService<IUserRepository>();
            IDeviceRepository deviceRepository = provider.GetRequiredService<IDeviceRepository>();
            IUnitOfWork unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            return new AdminService(connectionString, unitOfWork, userRepository, deviceRepository);
        });
        
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<IJwtProvider, JwtProvider>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultString"));
        });
        
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        
        app.UseMiddleware<ExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.Run();
    }
}