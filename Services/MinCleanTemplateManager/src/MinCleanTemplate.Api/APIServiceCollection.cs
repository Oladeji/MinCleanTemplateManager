using Asp.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GlobalConstants;

namespace MinCleanTemplateManager.Api;

public static class APIServiceCollection
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services, IConfiguration configuration)
    {

        var applicationAssembly = typeof(APIServiceCollection).Assembly;

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
      

        services.AddCors();
        services.AddApiVersioning(
            option =>
            {
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new ApiVersion(2);// ApiVersion.Default;// new ApiVersion(2, 0);
                option.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("api-version"),
                    new MediaTypeApiVersionReader("version"),
                    new UrlSegmentApiVersionReader()
                    );


            }).AddApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'V";
                option.SubstituteApiVersionInUrl = true;

            });

        services.AddJWTAuth(configuration);
        services.AddCorsFromOrigin(configuration);
        services.AddAuthorizationPolicies();
        return services;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            foreach (var policy in ApplicationPolicies.AllPolicies)
            {
                options.AddPolicy(policy.Name, p => p.RequireClaim(policy.Type, policy.ClaimValue));
            }


        });

        return services;
    }


    public static IServiceCollection AddCorsFromOrigin(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetSection("CorsOrigins_PermittedClients").Get<string[]>();
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(origins)
                    .AllowCredentials();
            });
        });
        return services;
    }
    public static IServiceCollection AddJWTAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure GlobalConstants.JwtConfig
        GlobalConstants.JwtConfig jwtConfig = new GlobalConstants.JwtConfig();
        ConfigurationBinder.Bind(configuration.GetSection(GlobalConstants.JwtConfig.SectionName), jwtConfig);
        services.AddSingleton(jwtConfig);

        // Write the key to a file
        string key = jwtConfig.Secret;
        var filePath = Path.Combine(AppContext.BaseDirectory, "jwt_secret.txt");
        File.WriteAllText(filePath, key);

        // Configure JWT Authentication
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
            ValidateIssuer = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtConfig.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };

        services.AddSingleton(tokenValidationParameters);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
            options.SaveToken = true;
        });

        return services;
    }

    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());


        return services;
    }



}




