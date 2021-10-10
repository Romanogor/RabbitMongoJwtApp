using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMongoJwt.BL;
using RabbitMongoJwt.DAL;
using System.Text;

namespace RabbitMongoJwt.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                        };
                    });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Sample Api", Version = "Version 1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
            });
            services.AddControllers();
            services.AddSingleton(typeof(IAuthService), typeof(AuthService));
            services.AddSingleton(typeof(IUserService), typeof(UserService));
            services.AddSingleton(typeof(IUserRepository), typeof(UserRepository));
            services.AddSingleton(typeof(IMessageService), typeof(MessageService));
            services.AddSingleton<IAppSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value);
            
            
            var databaseSection = Configuration.GetSection("Database");
            services.Configure<AppSettings>(databaseSection);
            services.Configure<AppSettings>(Configuration.GetSection("DatabaseName"));
            
            var jwtSection = Configuration.GetSection("Jwt");
            services.Configure<AppSettings>(jwtSection);   
            services.Configure<AppSettings>(Configuration.GetSection("Key"));
            services.Configure<AppSettings>(Configuration.GetSection("Issuer"));
            
            var userCredentialsSection = Configuration.GetSection("UserCredentilas");
            services.Configure<AppSettings>(userCredentialsSection);
            services.Configure<AppSettings>(Configuration.GetSection("UserName"));
            services.Configure<AppSettings>(Configuration.GetSection("Password"));
            
            var rabbitMqSection = Configuration.GetSection("RabbitMq");
            services.Configure<AppSettings>(rabbitMqSection);
            services.Configure<AppSettings>(Configuration.GetSection("HostName"));
            services.Configure<AppSettings>(Configuration.GetSection("QueueName"));

    }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
