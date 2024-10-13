using ChallengeBackEnd.Data;
using ChallengeBackEnd.Service;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var keycloakSettings = builder.Configuration.GetSection("Keycloak");

// Configure Authentication
/*
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = $"{keycloakSettings["AuthServerUrl"]}/realms/{keycloakSettings["Realm"]}";
    options.ClientId = keycloakSettings["ClientId"];
    options.ClientSecret = keycloakSettings["ClientSecret"];
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false;
    options.CallbackPath = new PathString(keycloakSettings["CallbackPath"]);


});

*/

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyDatabaseContext>(options =>
    options.UseSqlite(connString));



// Register repositories/services with DI
builder.Services.AddScoped<IStudentService, StudentServiceImp>();
builder.Services.AddScoped<IClassService, ClassServiceImp>();
builder.Services.AddScoped<ICountryService, CountryServiceImp>();

// Add controllers
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File("./Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .ReadFrom.Configuration(ctx.Configuration));
// Cors service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
    builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        
    });
});

var app = builder.Build();


// Ensure the database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyDatabaseContext>();
    context.Database.Migrate();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Cors middleware
app.UseCors("AllowAll");
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
