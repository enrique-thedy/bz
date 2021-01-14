using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host
        .CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(option =>
          option
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("cli.config.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
        )
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder
            .UseStartup<Startup>()
            .UseSerilog((ctx, cfg) =>
            {
              cfg
                .MinimumLevel.Verbose()
                //  .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341/dev");
            });
        });
  }
}
