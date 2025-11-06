using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectCenter.API.Extensions;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Application.Mappings;
using ProjectCenter.Application.Services;
using ProjectCenter.Infrastructure.Persistence.Contexts;
using ProjectCenter.Infrastructure.Persistence.Repositories;
using ProjectCenter.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 JWT Auth (вынесено в Extension)
builder.Services.AddJwtAuthentication(builder.Configuration);

// 🔹 Dependency Injection
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();

// 🔹 AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// 🔹 Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithBearer(); // 👈 Теперь вот так просто

var app = builder.Build();

// 🔹 Swagger — только в режиме Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
