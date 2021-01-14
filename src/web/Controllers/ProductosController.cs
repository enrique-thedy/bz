using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using servicios;
using web.Models;

namespace web.Controllers
{
  public class ProductosController : Controller
  {
    private readonly IServiciosStock _stock;
    public ProductosController(ILogger<ProductosController> logger, IServiciosStock stock)
    {
      logger.LogWarning("Creado controlador {nombre}", nameof(ProductosController));
      _stock = stock;
    }

    public IActionResult Inicio()
    {
      return View();
    }

    /// <summary>
    /// Retorna una lista de los libros mostrando ID y titulo
    /// </summary>
    /// <returns></returns>
    public IActionResult Listar()
    {
      var modelo = _stock.ObtenerTitulosDeEditorial(null);

      ViewData.Model = modelo;

      ViewBag.Encabezado = "Lista de TODOS los ID y Titulos";

      return View();
    }

    public IActionResult ObtenerDesdeCriterio(string crit)
    {
      IEnumerable<LibroDTO> modelo = null;//  _stock.GetLibrosFromCriterio(crit) --- mapear as LibroDTO

      //  AutoMapper
      //
      return View(modelo);
    }

  }
}
