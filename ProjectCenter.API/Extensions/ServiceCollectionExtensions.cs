using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Application.Mappings;
using ProjectCenter.Application.Services;
using ProjectCenter.Application.Services.ProjectCenter.Application.Services;
using ProjectCenter.Core.Exceptions;
using ProjectCenter.Infrastructure.Persistence.Contexts;
using ProjectCenter.Infrastructure.Persistence.Repositories;
using ProjectCenter.Infrastructure.Services;
using System.Text;

namespace ProjectCenter.API.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

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
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception is SecurityTokenExpiredException)
                            {
                                throw new TokenExpiredException("Срок действия токена истёк. Пожалуйста, войдите снова.");
                            }

                            return Task.CompletedTask;
                        }
                    };

                });

            return services;
        }
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDirectoryRepository, DirectoryRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDirectoryService, DirectoryService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }

        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
                cfg.AddProfile<UserProfile>();
            });

            return services;
        }

  
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return services;
        }

        public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddJwtAuthentication(configuration);
            services.AddRepositories();
            services.AddApplicationServices();
            services.AddCustomAutoMapper();
            services.AddCustomCors();

            return services;
        }
    }
}