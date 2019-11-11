using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nota2.Data;
using Nota2.Services;
using System;

namespace Nota2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            if ( ! string.IsNullOrEmpty(Configuration.GetConnectionString("UsePostgres")) ) {
            //PEGA STRING DE CONEXÃO
            services.AddDbContext<MyContext>(p => p.UseNpgsql(Configuration.GetConnectionString("Matilda")));
            } else {
              //PEGA STRING DE CONEXÃO  
             services.AddDbContext<MyContext>(p => p.UseSqlServer(Configuration.GetConnectionString("ConnectionMyPc")));
            }
            services.AddScoped<VotosService>();
            services.AddScoped<CampanhaService>();
          
            //ATIVA SESSION
            services.AddSession();           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //ATIVA SESSION
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Home}/{action=Index}/{id?}");
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });

            

        }
    }
}
