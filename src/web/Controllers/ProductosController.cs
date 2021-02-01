using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades.Articulos;
using Microsoft.AspNetCore.Authorization;
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

    [AllowAnonymous]
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
    public IActionResult LibrosPorCriterio(CriterioViewModel criterio, SegundoViewModel modelo)
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
      else
      {
        ModelState.Clear();

        if (TryValidateModel(criterio))
        {
          if (TryValidateModel(modelo))
          {
            var resultado = _stock.GetLibrosFromCriterio($"{criterio.Titulo} {criterio.Editorial}");

            return View("ListaLibros", resultado);
          }

          //  quiere decir que el error esta en el segundo...
          //  iteramos en la coleccion de errores...
          foreach (var campo in ModelState.Keys)
          {
            //  ojo cada campo puede tener mas de un error diferente...
            var errores = ModelState[campo].Errors;

            foreach (var error in errores)
              ModelState.AddModelError<SegundoViewModel>(model => model,
                $"Campo {campo} ==> {error.ErrorMessage} - Valor actual: {ModelState[campo].AttemptedValue}");
          }
          return View();
        }
        return View();
      }
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

    [HttpGet]
    public IActionResult BuscarAPI()
    {
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "Administrador, StockAdmin")]
    public IActionResult NuevoAutor()
    {
      return View();
    }

    [HttpPost]
    [Authorize(Roles = "Administrador, StockAdmin")]
    public IActionResult NuevoAutor(Autor nuevo)
    {
      //  aca no tenemos atributos ni problemas de binding por lo tanto siempre el modelo sera correcto!
      //
      if (ModelState.IsValid)
      {
        if (string.IsNullOrWhiteSpace(nuevo.Nombre))
          ModelState.AddModelError<Autor>(autor => autor.Nombre, "El nombre no puede estar en blanco!!!");
        else
        {
          if (nuevo.Nombre == "Sarasa")
            ModelState.AddModelError<Autor>(autor => autor.Nombre, "Ese nombre de autor no puede existir...");
          else
          {
            //  modelo correcto para agregarlo a la base de datos
            //
            if (_stock.AgregarAutor(nuevo))
            {
              ViewBag.NuevoAutor = nuevo;

              ModelState.Clear();

              return View();
            }
            else
            {
              ModelState.AddModelError<Autor>(autor => autor, "Error ingresando datos en la base de datos... revisar LOG");

              return View(nuevo);
            }
          }
        }

        return View(nuevo);
      }
      return View();
    }

  }
}
