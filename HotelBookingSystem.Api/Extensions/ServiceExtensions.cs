using FluentValidation;
using FluentValidation.AspNetCore;
using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Application.DTO.RoomDTO;
using HotelBookingSystem.Application.JWT;
using HotelBookingSystem.Application.MappingProfiles;
using HotelBookingSystem.Application.PasswordHasher;
using HotelBookingSystem.Application.Services;
using HotelBookingSystem.Application.Utilities;
using HotelBookingSystem.Application.Validators;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Enums;
using HotelBookingSystem.Domain.Interfaces;
using HotelBookingSystem.Domain.Interfaces.Repository;
using HotelBookingSystem.Infrastructure.Data;
using HotelBookingSystem.Infrastructure.EmailSender;
using HotelBookingSystem.Infrastructure.PdfGen;
using HotelBookingSystem.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Security.Claims;
using System.Text;

namespace HotelBookingSystem.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureLogging(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();
        }

        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void ConfigureFluentValidation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<BookingRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<CityRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<GuestReviewRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<HotelRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<PaymentRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<RoomRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UserRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<HotelSearchParametersValidator>();
        }

        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });
        }

        public static void ConfigureAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole(UserRole.Admin.ToString()));
                options.AddPolicy("CustomerPolicy", policy => policy.RequireRole(UserRole.Customer.ToString()));
                options.AddPolicy("AdminOrCustomer", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.IsInRole(UserRole.Admin.ToString()) ||
                        context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier));
                });
            });
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<BookingProfile>();
                cfg.AddProfile<CityProfile>();
                cfg.AddProfile<GuestReviewProfile>();
                cfg.AddProfile<HotelProfile>();
                cfg.AddProfile<PaymentProfile>();
                cfg.AddProfile<RoomProfile>();
                cfg.AddProfile<UserProfile>();
            }, typeof(Program).Assembly);
        }

        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Repositories
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IGuestReviewRepository, GuestReviewRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IGuestReviewService, GuestReviewService>();
            services.AddScoped<IHotelSearchParameters, HotelSearchParameters>();
            services.AddScoped<IRoomSearchParameters, RoomSearchParameters>();
            services.AddScoped<ITokenService, TokenService>();

            // Additional services
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped(typeof(IPdfGenerator<>), typeof(PdfGenerator<>));
            services.AddScoped<IPdfGenerator<Booking>, BookingPdfGenerator>();
            services.AddScoped<IBookingPdfGenerator, BookingPdfGenerator>();
            services.AddScoped<IBookingEmailGenerator, BookingEmailGenerator>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddHttpContextAccessor();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
