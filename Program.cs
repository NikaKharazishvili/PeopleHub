using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PeopleHub.Data;
using PeopleHub.Services;
using PeopleHub.Models;

var builder = WebApplication.CreateBuilder(args); // Sets up the app: config, DI container, logging
builder.Services.AddEndpointsApiExplorer(); // Discovers our API endpoints so Swagger can document them
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Registers our DB context, tells EF Core to use SQL Server with our connection string
builder.Services.AddControllers(); // Enables controller support (routes HTTP requests to Controller classes)
builder.Services.AddScoped<IPersonService, PersonService>(); // Registers Service in DI — whenever the interface is requested, provide the implementation instance (new one per request)
builder.Services.AddScoped<ITokenService, TokenService>(); // Registers Service in DI
builder.Services.AddScoped<IInterestService, InterestService>(); // Registers Service in DI
// Generates the OpenAPI/Swagger JSON doc, configured with JWT Bearer support for the Authorize button
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme // Tells Swagger UI how to accept a token (shows the Authorize button)
    {
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement // Applies the Bearer scheme to every endpoint in Swagger UI, so the lock icon shows up on each one
    { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() } });
});
// Registers Identity: password hashing, user creation, uniqueness checks — all handled internally
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<ApplicationDbContext>();
// Registers JWT Bearer authentication — tells the app how to validate incoming tokens
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!))
    };
});

var app = builder.Build(); // Builds the actual app from all the configured services above

// Seeds 4 demo Identity users (one per seeded Person) so a fresh clone is testable without manual registration
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    var demoUsers = new[]
    {
        new { Id = "11111111-1111-1111-1111-111111111111", Email = "nika@peoplehub.com", Password = "Demo123!" },
        new { Id = "22222222-2222-2222-2222-222222222222", Email = "misho@peoplehub.com", Password = "Demo123!" },
        new { Id = "33333333-3333-3333-3333-333333333333", Email = "bako@peoplehub.com", Password = "Demo123!" },
        new { Id = "44444444-4444-4444-4444-444444444444", Email = "avala@peoplehub.com", Password = "Demo123!" }
    };

    foreach (var u in demoUsers)
    {
        if (await userManager.FindByEmailAsync(u.Email) == null)
        {
            var user = new User { Id = u.Id, UserName = u.Email, Email = u.Email };
            await userManager.CreateAsync(user, u.Password);
        }
    }
}

app.UseExceptionHandler(errorApp => // Catches unhandled exceptions from any endpoint
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500; // Unhandled errors return HTTP 500
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\":\"Something went wrong.\"}"); // Generic message — hides internal details/stack trace from the client for security
    });
});
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); } // Serves swagger.json and the visual UI page, dev only
app.UseHttpsRedirection(); // Forces HTTP requests to redirect to HTTPS
app.UseAuthentication(); // Identifies who the user is (validates the JWT token) — must come before UseAuthorization
app.UseAuthorization(); // Decides whether the identified user is allowed to access the requested endpoint
app.MapControllers(); // Maps controller routes to actual HTTP endpoints — must be added or routes won't work
app.Run(); // Starts listening for requests (blocks here, keeps app alive)