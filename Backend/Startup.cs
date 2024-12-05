using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DDDSample1.Infrastructure;
using DDDSample1.Domain.Shared;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Infrastructure.OperationTypes;
using DDDSample1.Domain.Events.Handlers;
using DDDSample1.Domain.OperationRequests;
using DDDSample1.Infrastructure.OperationRequests;
using DDDSample1.Domain.Patients;
using DDDSample1.Infrastructure.Patients;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.NETCore.Setup;
using DDDNetCore.Domain.Email;
using DDDSample1.Domain.StaffProfile;
using DDDSample1.Infrastructure.StaffProfile;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Coravel;
using DDDSample1.Domain.Appointments;
using DDDSample1.Infrastructure.Appointments;
using DDDSample1.Domain.SurgeryRooms;
using DDDSample1.Infrastructure.SurgeryRooms;
using DDDSample1.Domain.HospitalMaps;

namespace DDDNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Env.Load("Confidential/.env"); // Load environment variables
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Define uma política CORS que permita o acesso do frontend.
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins(Env.GetString("FRONTEND_URL"))
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            // Configura a string de conexão do SQL Server
            var connectionString = Env.GetString("DATABASE_CONNECTION_STRING");

            // Adiciona o DbContext ao container de serviços
            services.AddDbContext<DDDSample1DbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            // Configura a autenticação com AWS Cognito
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://cognito-idp.{Env.GetString("AWS_REGION")}.amazonaws.com/{Env.GetString("COGNITO_USER_POOL_ID")}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = $"https://cognito-idp.{Env.GetString("AWS_REGION")}.amazonaws.com/{Env.GetString("COGNITO_USER_POOL_ID")}",
                    ValidAudience = Env.GetString("COGNITO_CLIENT_ID"),
                    RoleClaimType = "cognito:groups"
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminsOnly", policy =>
                    policy
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme) // Define o esquema JwtBearer
                    .RequireClaim("cognito:groups", "Admins"));

                options.AddPolicy("DoctorsOnly", policy =>
                    policy
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme) // Define o esquema JwtBearer
                    .RequireClaim("cognito:groups", "Doctors"));

                options.AddPolicy("NursesOnly", policy =>
                    policy
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme) // Define o esquema JwtBearer
                    .RequireClaim("cognito:groups", "Nurses"));
                
                options.AddPolicy("PatientsOnly", policy =>
                    policy
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme) // Define o esquema JwtBearer
                    .RequireClaim("cognito:groups", "Patients"));
                
                options.AddPolicy("TechniciansOnly", policy =>
                    policy
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme) // Define o esquema JwtBearer
                    .RequireClaim("cognito:groups", "Technicians"));

                options.AddPolicy("AdminsOrDoctors", policy =>
                    policy.
                    RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAssertion(context =>
                  context.User.HasClaim(c => 
                      (c.Type == "cognito:groups" && c.Value == "Admins") ||
                      (c.Type == "cognito:groups" && c.Value == "Doctors"))));    
            });

            // Configura os serviços personalizados do projeto.
            ConfigureMyServices(services);

            // Adiciona suporte a controladores com JSON usando Newtonsoft.
            services.AddControllers().AddNewtonsoftJson();

            // Adiciona controladores com suporte a MVC e views.
            services.AddControllersWithViews();

            // Add Coravel services
            services.AddScheduler();
            services.AddQueue();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowFrontend");

            app.UseAuthentication(); 
            app.UseAuthorization();

            // Use Coravel scheduler
            app.ApplicationServices.UseScheduler(scheduler =>
            {
                // Schedule recurring tasks here if needed
            });

            // Use Coravel queue
            app.ApplicationServices.ConfigureQueue();
            
            // Configura os endpoints para os controladores e rotas padrão.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Verifica a conexão ao banco de dados e inicializa
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DDDSample1DbContext>();
                try
                {
                    // Tenta acessar o banco de dados
                    if (context.Database.CanConnect())
                    {
                        //Log.Information("Database connection established.");
                    }

                    // Inicializa o banco de dados
                    context.Database.EnsureCreated(); // Ou usar Migrate para aplicar migrações pendentes
                    //SeedDatabase(context); // SeedDatabase aka Bootstrap
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao conectar ao banco de dados: {ex.Message}");
                    throw; // Lança a exceção para interromper a inicialização se houver um erro
                }
            }
        }

        public void ConfigureMyServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddAWSService<IAmazonCognitoIdentityProvider>();

            // Configuração do AWS
            services.AddDefaultAWSOptions(new AWSOptions
            {
                Region = Amazon.RegionEndpoint.GetBySystemName(Env.GetString("AWS_REGION"))
            });

            // Adiciona o serviço do Cognito
            services.AddSingleton<IAmazonCognitoIdentityProvider, AmazonCognitoIdentityProviderClient>();

            services.AddScoped<IAuthService, CognitoAuthService>(sp =>
            {
                var cognitoProvider = sp.GetRequiredService<IAmazonCognitoIdentityProvider>();
                var clientId = Environment.GetEnvironmentVariable("COGNITO_CLIENT_ID");
                var userPoolId = Environment.GetEnvironmentVariable("COGNITO_USER_POOL_ID");
                var patientProfileRepository = sp.GetRequiredService<IPatientProfileRepository>();
                return new CognitoAuthService(cognitoProvider, clientId, userPoolId, patientProfileRepository);
            });

            services.AddHostedService<PatientDeletionService>();
            services.AddSingleton<PatientDeletionService>();

            // Repositórios e serviços para o domínio de operações.
            services.AddTransient<IOperationTypeRepository, OperationTypeRepository>();
            services.AddTransient<OperationTypeService>();

            services.AddTransient<IOperationRequestRepository, OperationRequestRepository>();
            services.AddTransient<OperationRequestService>();

            // Repositórios e serviços para o domínio de Perfis de Pacientes.
            services.AddTransient<IPatientProfileRepository, PatientProfileRepository>();
            services.AddTransient<PatientProfileService>();

            // Register TokenManager as a singleton or transient service
            services.AddSingleton<ITokenManager, TokenManager>();

            // Repositórios e serviços para o domínio de Perfis de Staff.
            services.AddTransient<IStaffRepository, StaffRepository>();
            services.AddTransient<StaffService>();

            // Repositórios e serviços para o domínio de Appointments.
            services.AddTransient<AppointmentService>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();

            services.AddTransient<SurgeryRoomService>();
            services.AddTransient<ISurgeryRoomRepository, SurgeryRoomRepository>();

            services.AddTransient<HospitalMapService>();

            // Event Handlers
            services.AddScoped<OperationTypeCreatedEventHandler>();
            services.AddScoped<OperationRequestCreatedEventHandler>();
            services.AddScoped<OperationRequestUpdatedEventHandler>();
            services.AddScoped<OperationRequestDeletedEventHandler>();
            services.AddScoped<PatientProfileCreatedEventHandler>();
            services.AddScoped<PatientProfileDeletedEventHandler>();
            services.AddScoped<PatientProfileUpdatedEventHandler>();
            services.AddScoped<StaffCreatedEventHandler>();
            services.AddScoped<StaffDeletedEventHandler>();
            services.AddScoped<StaffUpdatedEventHandler>();

            // Email Sender
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}