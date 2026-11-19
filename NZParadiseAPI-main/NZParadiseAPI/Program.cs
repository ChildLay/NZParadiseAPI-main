using Microsoft.EntityFrameworkCore;
using NZParadiseAPI.Model;
using NZParadiseAPI.Model.Abstract;
using NZParadiseAPI.Model.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Compress resources to save bandwidth
    builder.Services.AddResponseCompression();
    builder.Services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
    builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);


    #region "CORS"
    // Enable Entity Framework
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("NZParadiseDBConn"),
            sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: new[] { 40540 }
            )
        )
    );

    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();

    // Apply custom CORS
    var allowedOrigins = builder.Configuration
         .GetSection("Cors:AllowedOrigins").Value;
    //.Get<string[]>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp", policy =>
        {
            if (allowedOrigins != null)
            {
                if (allowedOrigins == "*")
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
                else
                {
                    string[] originList = allowedOrigins.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    policy.WithOrigins(originList)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // Optional, for cookies/auth
                }
            }
            else
            {
                throw new InvalidOperationException("No CORS origins configured in appsettings.json");
            }
        });
    });
    #endregion

    // Dependency Injection
    builder.Services.AddScoped<IUsersRepository, UsersRepository>();
    builder.Services.AddScoped<IPropertiesRepository, PropertiesRepository>();
    builder.Services.AddScoped<AzureBlobService>();

    #region "JWT Authentication"
    // Configure JWT Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

    //Init JWT settings
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
    #endregion

    var app = builder.Build();

    // attach middleware for exception handling
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }

    app.UseCors("AllowReactApp");
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.UseResponseCompression();

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine("=== APP CRASHED ===");
    Console.WriteLine(ex.ToString());
    throw;
}