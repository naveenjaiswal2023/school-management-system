using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolManagement.API.Authorization;
using SchoolManagement.API.Middleware;
using SchoolManagement.Application.Auth.Handler;
using SchoolManagement.Application.Interfaces;
using SchoolManagement.Application.Mappings;
using SchoolManagement.Application.Services;
using SchoolManagement.Application.Students.Commands;
using SchoolManagement.Application.Validators;
using SchoolManagement.Domain.Services;
using SchoolManagement.Infrastructure.BackgroundServices;
using SchoolManagement.Infrastructure.Configuration;
using SchoolManagement.Infrastructure.Services;
using SchoolManagement.Persistence;
using SchoolManagement.Persistence.Repositories;
using System.Text;
using System.Threading.RateLimiting;

namespace SchoolManagement.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // --- AUTO MAPPER (correct DI registration) ---
            // Ensure you have the NuGet package:
            // AutoMapper.Extensions.Microsoft.DependencyInjection
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Optional: validate mapping configuration at startup (will throw if invalid)
            // Note: building a temporary provider for validation is acceptable here.
            var spForValidation = services.BuildServiceProvider();
            var mapper = spForValidation.GetService<IMapper>();
            mapper?.ConfigurationProvider.AssertConfigurationIsValid();

            // --- MVC / JSON ---
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });

            // API Explorer
            services.AddEndpointsApiExplorer();

            // Database Configuration
            services.AddDbContext<SchoolManagementDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SchoolManagementDbConnectionString"),
                    b => b.MigrationsAssembly("SchoolManagement.API")));

            // Memory Cache (required for caching services)
            services.AddMemoryCache();

            // JWT Authentication Configuration
            var jwtSettings = Configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"] ?? "DefaultSecretKeyForDevelopment123456789!";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Set to true in production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"] ?? "SchoolManagementSystem",
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"] ?? "SchoolManagementUsers",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true
                };
            });

            // Dependency Injection
            services.AddHttpContextAccessor();

            // Menu and Permission Services
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleMenuPermissionRepository, RoleMenuPermissionRepository>();
            services.AddScoped<IMenuPermissionService, MenuPermissionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Authorization Handlers
            services.AddScoped<IAuthorizationHandler, MenuPermissionHandler>();

            // Repository Services
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Business Services
            services.AddScoped<IBiometricVerificationService, BiometricVerificationService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAttendanceCalculationService, AttendanceCalculationService>();
            services.AddScoped<ISalaryCalculationService, SalaryCalculationService>();

            // Auth Services
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<TokenService>(); // Register concrete class first
            services.AddScoped<ITokenService, CachedTokenService>(); // Register decorator
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IAuditService, AuditService>();
            //services.AddSingleton<INotificationQueue, InMemoryNotificationQueue>();
            //services.AddHostedService<NotificationProcessorService>();

            // MediatR for CQRS
            services.AddMediatR(typeof(CreateStudentCommand).Assembly);
            if (typeof(LoginCommandHandler).Assembly != typeof(CreateStudentCommand).Assembly)
            {
                services.AddMediatR(typeof(LoginCommandHandler).Assembly);
            }

            // FluentValidation
            services.AddValidatorsFromAssembly(typeof(CreateStudentCommandValidator).Assembly);

            // HTTP Client Factory
            services.AddHttpClient("SMS", client =>
            {
                var smsBaseUrl = Configuration["NotificationSettings:SMS:BaseUrl"];
                if (!string.IsNullOrEmpty(smsBaseUrl))
                {
                    client.BaseAddress = new Uri(smsBaseUrl);
                }
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient("Email", client =>
            {
                var emailBaseUrl = Configuration["NotificationSettings:Email:BaseUrl"];
                if (!string.IsNullOrEmpty(emailBaseUrl))
                {
                    client.BaseAddress = new Uri(emailBaseUrl);
                }
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            // Authorization Policies
            services.AddAuthorization(options =>
            {
                // Role-based policies
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("TeacherOrAdmin", policy => policy.RequireRole("Teacher", "Admin"));
                options.AddPolicy("StudentOrParent", policy => policy.RequireRole("Student", "Parent"));
                options.AddPolicy("StaffAccess", policy => policy.RequireRole("Admin", "Principal", "Teacher"));
                options.AddPolicy("ParentAccess", policy => policy.RequireRole("Parent"));

                // Permission-based policies
                options.AddPolicy("StudentManagement", policy =>
                    policy.Requirements.Add(new MenuPermissionAttribute("StudentManagement", "view")));

                options.AddPolicy("StudentAdd", policy =>
                    policy.Requirements.Add(new MenuPermissionAttribute("StudentManagement", "add")));

                options.AddPolicy("EmployeeManagement", policy =>
                    policy.Requirements.Add(new MenuPermissionAttribute("HRMSManagement", "view")));

                options.AddPolicy("AttendanceView", policy =>
                    policy.Requirements.Add(new MenuPermissionAttribute("AttendanceManagement", "view")));

                options.AddPolicy("SystemAdmin", policy =>
                    policy.Requirements.Add(new MenuPermissionAttribute("SystemSettings", "view")));
            });

            // Configuration Options
            services.Configure<BiometricSettings>(Configuration.GetSection("BiometricSettings"));
            services.Configure<NotificationSettings>(Configuration.GetSection("NotificationSettings"));
            services.Configure<AttendanceSettings>(Configuration.GetSection("AttendanceSettings"));

            // SWAGGER CONFIGURATION
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "School Management System API",
                    Version = "v1",
                    Description = "Comprehensive School Management System API with Clean Architecture",
                    Contact = new OpenApiContact
                    {
                        Name = "Development Team",
                        Email = "dev@schoolmanagement.com"
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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

                // Include XML comments if available
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            // CORS Policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://192.168.241.175:8080", "https://localhost:8080", "http://localhost:8080")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Health Checks
            services.AddHealthChecks()
                .AddDbContextCheck<SchoolManagementDbContext>("database");

            // Caching
            var redisConnectionString = Configuration.GetConnectionString("Redis");
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                try
                {
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = redisConnectionString;
                    });
                }
                catch
                {
                    services.AddMemoryCache();
                }
            }

            // Background Services (Optional)
            services.AddHostedService<AttendanceSyncService>();
            services.AddHostedService<NotificationProcessorService>();
            services.AddHostedService<TokenCleanupService>();

            // Rate Limiting
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddFixedWindowLimiter("AuthPolicy", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 10;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 5;
                });

                options.AddFixedWindowLimiter("GeneralPolicy", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 100;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 10;
                });
            });

            // Add logging
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                if (!builder.Services.Any(x => x.ServiceType == typeof(ILoggerProvider)))
                {
                    builder.SetMinimumLevel(LogLevel.Information);
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/api/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            if (HasMiddleware<ErrorHandlingMiddleware>())
            {
                app.UseMiddleware<ErrorHandlingMiddleware>();
            }

            if (HasMiddleware<RateLimitingMiddleware>())
            {
                app.UseMiddleware<RateLimitingMiddleware>();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "School Management System API V1");
                c.RoutePrefix = "swagger"; // URL: /swagger
                c.DisplayRequestDuration();
                c.EnableTryItOutByDefault();
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            app.UseCors("AllowReactApp");

            app.UseRateLimiter();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("School Management System API is running! Go to /swagger to view API documentation.");
                });
            });
        }

        // Helper method to check if middleware exists
        private bool HasMiddleware<T>()
        {
            try
            {
                var type = typeof(T);
                return type != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
