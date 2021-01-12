using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;
using console;
using datos;
using Entidades.Articulos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using servicios;
using Servicios;

namespace cli
{
  class Program
  {
    static void Main(string[] args)
    {
      //  generar-construir un hosting
      //  -- maneja configuracion
      //  -- maneja inyeccion de dependencias
      //  -- maneja logging
      //  -- y lo hace para cualquier tipo de app y cualquier OS
      //
      var builder = Host.CreateDefaultBuilder();

      //  Func<int, bool> siempreVerdadero = (x) => true;
      //  Action<int> nada = (x) => Console.Clear();

      //  void Pirulo(IConfigurationBuilder xx) { }

      builder
        //  .ConfigureAppConfiguration(Pirulo)
        .ConfigureAppConfiguration(option =>
          option
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("cli.config.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
        )
        .ConfigureLogging(loggingBuilder =>
          {
            loggingBuilder.ClearProviders();

            loggingBuilder.AddSerilog(
              new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File(@"D:\bz.log")
                .WriteTo.Seq("http://localhost:5341/dev")
                .CreateLogger());
          }
        )
        .ConfigureServices((ctx, serv) =>
        {
          //  serv.AddScoped<ServiciosImportacion>();
          serv.AddScoped<IServiciosImportacion, ServiciosImportacion>();
          serv.AddScoped<IServiciosExportacion, ServiciosExportacion>();
          serv.AddScoped<IServiciosStock, ServiciosStock>();

          serv.AddDbContext<ExportacionContext>(build =>
          {
            //  https://www.connectionstrings.com/
            //  Sitio interesante para obtener cadenas de conexion para casi todos los proveedores...
            //
            build.UseSqlServer(ctx.Configuration.GetConnectionString("curso"));

            build.EnableDetailedErrors();
            build.EnableSensitiveDataLogging();
            //if (ctx.HostingEnvironment.IsDevelopment()) { }
          });

          serv.AddScoped<Aplicacion>();
        });

      var host = builder.Build();
      
      //  ServiciosImportacion imp = new ServiciosImportacion();
      //  Aplicacion app = new Aplicacion(imp, null);

      //  la diferencia entre los dos metodos de ejecucion es que en el siguiente no manejamos dependencias!
      //  o sea...todos los objetos necesarios se crean "magicamente" desde el contenedor de servicios...

      //  var xx = host.Services.GetService<ServiciosImportacion>();
      
      Aplicacion app = host.Services.GetService<Aplicacion>();

      app.Run();

      Console.WriteLine("Presionar ENTER");
      Console.ReadLine();
    }
  }

  public class Dummy : IServiciosImportacion
  {
    public IEnumerable<Libro> ImportarCSV(string file)
    {
      return new List<Libro>()
      {
        new Libro() {Titulo = "El jardin de senderos que se bifurcan"},
        new Libro() {Titulo = "El jardin de senderos que se bifurcan"}
      };
    }

    public IEnumerable<(string idLibro, string nombre)> ImportarAutores(string fileName)
    {
      throw new NotImplementedException();
    }
  }
}
