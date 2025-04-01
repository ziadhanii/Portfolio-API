using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Portfolio API",
        Version = "v1",
        Description =
            "This is the API for my portfolio, showcasing my projects and experience in software development.",
        Contact = new OpenApiContact
        {
            Name = "Ziad Hani",
            Email = "ziadhani64@gmail.com",
            Url = new Uri("https://linkedin.com/in/ziadhani")
        }
    });

    c.CustomSchemaIds(type => type.FullName);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your token in the format: Bearer {your_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddHttpClient("GitHub", client => { client.DefaultRequestHeaders.Add("User-Agent", "ASP-Monster"); });

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API v1");
    c.RoutePrefix = "";
});

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler();
app.UseStaticFiles();


using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    await ApplicationDbSeeder.SeedAdmin(userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();