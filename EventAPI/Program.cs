using EventAPI.Data;
using EventAPI.Helpers;
using EventAPI.Repositories;
using EventAPI.Services;
using EventAPI.Validators;
using EventAPI.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;






var builder = WebApplication.CreateBuilder(args);


// Scan the assembly for all IRegister mapping configs
TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

// Add Mapster mapper service
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
//builder.Services.AddScoped<IMapper, ServiceMapper>();

// Add services to the container.

builder.Services.AddControllers((options =>
{
    options.Filters.Add<ValidationFilter>();
}));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<EventCreateValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Add IHttpContextAccessor for JWT context
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IJwtContext, JwtContext>();

builder.Services.AddDbContext<EventDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

// Register the JWT service
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
var secret = appSettingsSection.GetValue<string>("Token");
var issuer = appSettingsSection.GetValue<string>("Issuer");
var audience = appSettingsSection.GetValue<List<string>>("Audience");
var key = Convert.FromBase64String(secret);


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
          .AddJwtBearer(x =>
          {
              x.RequireHttpsMetadata = true;
              x.SaveToken = true;
              x.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuerSigningKey = false,
                  IssuerSigningKey = new SymmetricSecurityKey(key),

                  ValidateIssuer = false,
                  ValidIssuer = issuer,

                  ValidateAudience = false,
                  ValidAudiences = audience,

                  ValidateLifetime = false,
              };

          });

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseGlobalExceptionHandling();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use the global exception middleware


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
