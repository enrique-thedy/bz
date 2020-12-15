using System;
using console;
using Servicios;

namespace cli
{
  class Program
  {
    static void Main(string[] args)
    {
      ServiciosImportacion imp = new ServiciosImportacion();

      Aplicacion app = new Aplicacion(imp);

      app.Run();


      Console.WriteLine("Presionar ENTER");
      Console.ReadLine();
    }
  }
}
