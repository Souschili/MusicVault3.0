using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicVault.Data.EF;
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
            services.AddScoped<DbContext,ApplicationContext>();
            services.AddScoped(EntityCheker)
            services.AddScoped<IUserManager, UserManager>();

            //mapper
            services.AddAutoMapper(typeof(Startup));



            services.AddDbContext<ApplicationContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Description = "Api for store music",
                    Title = "Music Vault Api",
                    Version = "3.0.0",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Email = "greatdragone75@gmail.com",
                        Name = "TengriBizMenen",
                        Url = new Uri("https://www.facebook.com/profile.php?id=100001107439258")
                    }

                });
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
            app.UseSwaggerUI(config=>
            {
                config.SwaggerEndpoint("./swagger/v1/swagger.json","Music Vault Api");
                config.RoutePrefix = String.Empty;
            });
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
