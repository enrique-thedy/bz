﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datos;
using Entidades.Articulos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace servicios
{
  public class ServiciosStock : IServiciosStock
  {
    private readonly ExportacionContext _ctx;

    public ServiciosStock(ExportacionContext ctx, ILogger<ServiciosStock> logger)
    {
      _ctx = ctx;
    }

    public List<(Guid, string)> ObtenerTitulosDeEditorial(string editorial)
    {
      if (editorial != null)
        return _ctx
          .Libros
          .FromSqlInterpolated($"select * from Libros where Editorial={editorial}")
          .Select(x => Tuple.Create(x.ID_Real, x.Titulo).ToValueTuple())
          .ToList();
      else
        return _ctx
          .Libros
          .FromSqlInterpolated($"select * from Libros")
          .Select(x => Tuple.Create(x.ID_Real, x.Titulo).ToValueTuple())
          .ToList();
    }

    public IEnumerable<Libro> GetLibrosFromCriterio(string criterio)
    {
      return _ctx.Libros
        .Include("Autores")
        .Where(lib =>
          lib.Titulo.Contains(criterio) ||
          lib.Editorial.Contains(criterio) ||
          lib.Autores.Any(aut => aut.Nombre.Contains(criterio)));
    }

    public bool AgregarLibro(Libro nuevo)
    {
      throw new NotImplementedException();
    }

    public bool AgregarAutor(Autor nuevo)
    {
      try
      {
        if (_ctx.Autores.Any(aut => aut.Nombre == nuevo.Nombre))
          return false;

        _ctx.Autores.Add(nuevo);
        _ctx.SaveChanges();

        return true;
      }
      catch
      {
        //  LOG!!
        return false;
      }
    }
  }
}
