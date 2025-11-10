using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectCenter.API.Extensions;
using ProjectCenter.API.Middleware;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Application.Mappings;
using ProjectCenter.Application.Services;
using ProjectCenter.Core.Entities;
using ProjectCenter.Infrastructure.Persistence.Contexts;
using ProjectCenter.Infrastructure.Persistence.Repositories;
using ProjectCenter.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithBearer();

var app = builder.Build();
app.UseMiddleware<ProjectCenter.API.Middleware.ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ProjectCenter.API.Middleware.UserContextMiddleware>();


app.MapControllers();

app.Run();
