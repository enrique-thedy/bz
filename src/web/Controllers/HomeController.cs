using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using servicios;
using web.Models;

namespace web.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;

    private readonly IServiciosImportacion _imp;

    public HomeController(IServiciosImportacion imp, ILogger<HomeController> logger)
    {
      _logger = logger;
      _imp = imp;
    }

    public IActionResult Index()
    {
      //  var xx = _imp.ImportarCSV("D:\\CURSOS\\PTR2020_Avanzado\\repo\\listados\\libros.csv");

      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
