using EventAPI.Data;
using EventAPI.Repositories;
using EventAPI.Services;
using EventAPI.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;





var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<EventCreateValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddDbContext<EventDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

// Register the JWT service
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
var secret = appSettingsSection.GetValue<string>("Token");
var issuer = appSettingsSection.GetValue<string>("Issuer");
var audience = appSettingsSection.GetValue<string>("Audience");
var key = Convert.FromBase64String(secret);
var audiences = new List<string>
{
    audience
};

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
          .AddJwtBearer(x =>
          {
              x.RequireHttpsMetadata = false;
              x.SaveToken = true;
              x.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuerSigningKey = false,
                  IssuerSigningKey = new SymmetricSecurityKey(key),

                  ValidateIssuer = false,
                  ValidIssuer = issuer,

                  ValidateAudience = false,
                  ValidAudiences = audiences,

                  ValidateLifetime = false,
              };

          });

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
