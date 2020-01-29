using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MusicVault.Data.EF;
using MusicVault.Services.Helpers;
using MusicVault.Services.Interfaces;
using MusicVault.Services.Services;


namespace VaultApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        public Startup(IConfiguration conf)
        {
            Configuration = conf;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //Di
            services.AddScoped<DbContext, ApplicationContext>();
            services.AddScoped<EntityCheker>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<IPlayListManager, PlayListManager>();
            //todo del
            //services.AddSingleton<JwtOptions>(); как и думал он сам по себе синглтон и система ег сама инжектит

            //mapper
            services.AddAutoMapper(typeof(Startup));

            //смотрим на файл настроек , так оптимальней чем привязка
            // services.AddOptions(); ненужно потом удалить

            //на выходе получаем IOptions<T> привязка к разделу конфигурации
            services.Configure<JwtOptions>(Configuration.GetSection("Jwt"));

            services.AddDbContext<ApplicationContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo
                {
                    Description = "Api for store music",
                    Title = "Music Vault Api",
                    Version = "3.0.0",
                    Contact = new OpenApiContact
                    {
                        Email = "greatdragone75@gmail.com",
                        Name = "TengriBizMenen",
                        Url = new Uri("https://www.facebook.com/profile.php?id=100001107439258")
                    }

                });
               

                cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {}

                    }
                });
               // Azure вырубает хз почему , по логам ненаходит xml файл
               // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
               // cfg.IncludeXmlComments(xmlPath,true);
            });

            //Jwt
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"])),
                    ClockSkew = TimeSpan.FromSeconds(10)
                };
                //допилить
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("./swagger/v1/swagger.json", "Music Vault Api");
                config.RoutePrefix = String.Empty;
            });

            //перенаправление на https
            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
