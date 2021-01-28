using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using datos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using servicios;
using Servicios;

namespace web
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
      services.AddSession();

      //  por defecto, todos los controller necesitan autorizacion!!
      //
      services.AddControllersWithViews(options => options.Filters.Add(new AuthorizeFilter()));

      services.AddDbContext<ExportacionContext>(build =>
      {
        build.UseSqlServer(Configuration.GetConnectionString("curso"));

        build.EnableDetailedErrors();
        build.EnableSensitiveDataLogging();
      });

      services.AddDbContext<SeguridadContext>(build =>
      {
        build.UseSqlServer(Configuration.GetConnectionString("curso"));

        build.EnableDetailedErrors();
        build.EnableSensitiveDataLogging();
      });

      services.AddScoped<IServiciosImportacion, ServiciosImportacion>();
      services.AddScoped<IServiciosStock, ServiciosStock>();
      services.AddScoped<IServiciosSeguridad, ServiciosSeguridad>();

      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => options.LoginPath = "/user/login");
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseSession();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Productos}/{action=Inicio}/{id?}");
      });
    }
  }
}
