using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TC1.RepairShop.Application.Auth;
using TC1.RepairShop.Application.Auth.UseCases;
using TC1.RepairShop.Application.Clients;
using TC1.RepairShop.Application.Clients.UseCases;
using TC1.RepairShop.Application.Registration;
using TC1.RepairShop.Application.Registration.UseCases;
using TC1.RepairShop.Domain.Entities.Clients;
using TC1.RepairShop.Infrastructure.Data;
using TC1.RepairShop.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
    ?? throw new InvalidOperationException("Configuration section 'Jwt' was not found.");

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TC1 RepairShop API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Enter the JWT token: Bearer {your token}",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme,
        },
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() },
    });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(nameof(Role.Admin)));
    options.AddPolicy("StaffOnly", policy => policy.RequireRole(nameof(Role.Admin), nameof(Role.Staff)));
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole(nameof(Role.Customer)));
});

DapperTypeHandlers.Register();

builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<GetUserUseCase>();
builder.Services.AddScoped<ListUsersUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<ChangeUserPasswordUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<CreateCustomerUseCase>();
builder.Services.AddScoped<GetCustomerUseCase>();
builder.Services.AddScoped<ListCustomersUseCase>();
builder.Services.AddScoped<UpdateCustomerUseCase>();
builder.Services.AddScoped<DeleteCustomerUseCase>();
builder.Services.AddScoped<ChangeCustomerPasswordUseCase>();
builder.Services.AddScoped<CreateVehicleUseCase>();
builder.Services.AddScoped<GetVehicleUseCase>();
builder.Services.AddScoped<ListVehiclesUseCase>();
builder.Services.AddScoped<ListVehiclesByCustomerUseCase>();
builder.Services.AddScoped<DeleteVehicleUseCase>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<AuthenticateUserUseCase>();
builder.Services.AddScoped<AuthenticateCustomerUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
