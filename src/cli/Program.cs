using System;
using System.IO;
using console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

      builder.ConfigureAppConfiguration(option =>
        {
          option.SetBasePath(Directory.GetCurrentDirectory());
          option.AddJsonFile("cli.config.json", optional: true);
          option.AddEnvironmentVariables();
          option.AddCommandLine(args);
        })
        .ConfigureServices((ctx, serv) =>
        {
          serv.AddScoped<ServiciosImportacion>();
          serv.AddScoped<Aplicacion>();
          //  serv.AddScoped<ServiciosExportacion>();
        });

      var host = builder.Build();

      //  ServiciosImportacion imp = new ServiciosImportacion();
      //  Aplicacion app = new Aplicacion(imp, null);

      Aplicacion app = host.Services.GetService<Aplicacion>();

      app.Run();

      Console.WriteLine("Presionar ENTER");
      Console.ReadLine();
    }
  }
}
