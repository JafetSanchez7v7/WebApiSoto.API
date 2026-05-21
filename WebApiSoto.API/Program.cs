
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text;
using System.Threading.RateLimiting;
using WebApiSoto.API.Middleware;
using WebApiSoto.Application.DependencyInjection;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Infrastructure.Context;
using WebApiSoto.Infrastructure.DbTrigger;
using WebApiSoto.Infrastructure.DependencyInjection;
using WebApiSoto.Infrastructure.Repositories;

namespace WebApiSoto.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //servicios

            builder.Services.AddCors( opt =>
            {
                opt.AddPolicy("AllowAll", opt =>
                {
                    opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();
            // autenticacon
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidIssuer = jwtSettings["Issuer"],
                     ValidateAudience = true,
                     ValidAudience = jwtSettings["Audience"],
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
                     ClockSkew = TimeSpan.Zero // El token expira al segundo exacto
                 };
             });






            //Configurando los RateLimiters
            var limitOptions = builder.Configuration.GetSection(RateLimitingOptions.RateLimitOptions);
            var limitPolicies = builder.Configuration.GetSection(RateLimitingPolicies.RateLimitPolicies);
            // Bindeando las opciones para obtener las secciones del json Settings a memoria
            var rateLimitingOptions = new RateLimitingOptions();
            limitOptions.Bind(rateLimitingOptions);
            var rateLimitingPolicies = new RateLimitingPolicies();
            limitPolicies.Bind(rateLimitingPolicies);
            // Agregando los scopped
            //RateLimiter
            builder.Services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        await context.HttpContext.Response.WriteAsync($"Demasiadas solicitudes. Intenta de nuevo en {retryAfter.TotalSeconds} segundos.",
                        cancellationToken: token);
                    }
                    else
                    {
                        await context.HttpContext.Response.WriteAsync($"Demasiadas solicitudes. Intenta de nuevo más tarde.", cancellationToken: token);
                    }
                };
                // ESTE ES EL DE ALGORITMO DE VENTANAS
                options.AddFixedWindowLimiter(rateLimitingPolicies.FixedWindow ?? "Fixed", opt =>
                {

                    opt.PermitLimit = rateLimitingOptions.PermitLimit;
                    opt.Window = TimeSpan.FromSeconds(rateLimitingOptions.Window);
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
                // ESTE ES EL DE ALGORITMO DE VENTANAS DESLIZANTES
                options.AddSlidingWindowLimiter(rateLimitingPolicies.SlidingWindow ?? "SlidingWindowPolicy", opt =>
                {
                    opt.PermitLimit = rateLimitingOptions.PermitLimit;
                    opt.Window = TimeSpan.FromSeconds(rateLimitingOptions.Window);
                    opt.SegmentsPerWindow = rateLimitingOptions.SegmentsPerWindow;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
                // ESTE ES EL DE ALGORITMO DE TOKEN BUCKET
                options.AddTokenBucketLimiter(rateLimitingPolicies.TokenBucket ?? "TokenBucketPolicy", opt =>
                {
                    opt.TokenLimit = rateLimitingOptions.TokenLimit;
                    opt.TokensPerPeriod = rateLimitingOptions.TokensPerPeriod;
                    opt.ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimitingOptions.ReplenishmentPeriod);
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
                // ESTE ES EL DE ALGORITMO DE CONCURRENCY LIMITER
                options.AddConcurrencyLimiter(rateLimitingPolicies.ConcurrencyLimiter ?? "ConcurrencyLimiterPolicy", opt =>
                {
                    opt.PermitLimit = rateLimitingOptions.GlobalPermitLimit;
                    opt.QueueLimit = rateLimitingOptions.QueueLimit;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });

                //Algoritmo Global que se aplica a todas las solicitudes sera Uno mixeado
                options.GlobalLimiter = PartitionedRateLimiter.CreateChained(PartitionedRateLimiter.Create<HttpContext, string>(partitioner =>
                {
                    var userAgent = partitioner.Request.Headers.UserAgent.ToString();
                    return RateLimitPartition.GetFixedWindowLimiter(userAgent, httpContext => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitingOptions.PartitionedPermitLimit,
                        Window = TimeSpan.FromMinutes(rateLimitingOptions.Window),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });



                }),
                PartitionedRateLimiter.Create<HttpContext, string>(partitioner =>
                {
                    var userAgent = partitioner.Request.Headers.UserAgent.ToString();
                    return RateLimitPartition.GetFixedWindowLimiter(userAgent, context => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitingOptions.GlobalPermitLimit,
                        Window = TimeSpan.FromHours(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }); ;
                })

                );

            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info.Title = "Pasteleria Soto API";
                    document.Components ??= new();
                    document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Introduce tu token JWT"
                    });
                    document.SecurityRequirements.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
                    return Task.CompletedTask;
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(cfg =>
                {
                    cfg.WithTitle("WebApiSoto.").
                    WithTheme(ScalarTheme.Mars)
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<LoggingMiddleware>();

            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.UseRateLimiter();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var inicializador = services.GetRequiredService<IDbInicializador>();
                    inicializador.Inicializar();
                }
                catch (Exception ex)
                {

                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "Un Error ocurrio al ejecutar la migracion");
                }

                app.MapControllers();

                app.Run();
            }
        }
    }
}
