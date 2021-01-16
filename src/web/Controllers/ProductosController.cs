using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades.Articulos;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    /// Consulta el stock de libros segun criterio de titulo y/o editorial
    /// </summary>
    /// <returns></returns>
    public IActionResult LibrosPorCriterio()
    {
      //  var modelo = new CriterioViewModel();

      return View();
    }

    [HttpPost]
    public IActionResult LibrosPorCriterio(CriterioViewModel criterio)
    {
      //  en este momento, el binder no solamente enlazo las propiedades sino que validó el modelo con los datos
      //  disponibles ==> data annotations!!!
      //
      if (ModelState.IsValid)
      {
        //  quiero que el modelo TAMBIEN sea invalido si AMBAS propiedades estan vacias...
        //
        if (string.IsNullOrWhiteSpace(criterio.Editorial) && string.IsNullOrWhiteSpace(criterio.Titulo))
        {
          ModelState.AddModelError<CriterioViewModel>(model => model,
            "[ERROR MODELO] No podemos buscar sin ningun criterio...");

          return View();
        }

        var resultado = _stock.GetLibrosFromCriterio($"{criterio.Titulo} {criterio.Editorial}");

        return View("ListaLibros", resultado);
      }

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
