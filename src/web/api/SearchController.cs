using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entidades.Articulos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using servicios;
using web.Models;

namespace web.api
{
  [ApiController]
  [Route("api/libros")]
  public class SearchController : ControllerBase
  {
    private readonly ILogger<SearchController> _logger;

    private readonly IServiciosStock _stock;

    public SearchController(ILogger<SearchController> logger, IServiciosStock stock)
    {
      _stock = stock;
      _logger = logger;
    }

    [Route("{criterio:alpha}")]
    public ActionResult<IEnumerable<LibroDTO>> Get(string criterio, [FromServices] IMapper mapper)
    {
      //  sin atributos en el metodo...
      //
      //  http://localhost:5000/api/libros?editorial=apress
      //
      //  [Route("{criterio:alpha}")]
      //
      //  http://localhost:5000/api/libros/apress

      //  LibroDTO dto = mapper.Map<LibroDTO>(new Libro());
      //  
      var resultado = mapper.Map<IEnumerable<LibroDTO>>(_stock.GetLibrosFromCriterio(criterio));

      //  el ToList() es una restriccion del tipo generico ActionResult<T>
      //
      return resultado.ToList();
    }
  }
}
