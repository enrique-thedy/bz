using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datos;
using Entidades.Articulos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
// ReSharper disable ReplaceWithSingleCallToSingleOrDefault

namespace servicios
{
  public class ServiciosExportacion : IServiciosExportacion
  {
    private readonly ExportacionContext _ctx;

    private readonly ILogger<ServiciosExportacion> _logger;

    public ServiciosExportacion(ExportacionContext ctx, ILogger<ServiciosExportacion> logger)
    {
      _ctx = ctx;
      _logger = logger;
    }

    /// <summary>
    /// Aplica reglas de negocio a la lista de origen e intenta guardar la informacion faltante en la base de datos
    /// Solo incorpora libros que NO EXISTEN
    /// </summary>
    /// <param name="lista"></param>
    public void ExportarListaDeLibros(IEnumerable<Libro> lista)
    {
      //  ver si el libro ya existe...(por ISBN13)
      //  si el ISBN es null lo agrego sin mas...
      //  si no existe lo agrego

      //  foreach (Libro libro in lista.Take(1))
      //  usar Take(1) si queremos ver el comportamiento para un unico libro
      //
      foreach (Libro libro in lista)
      {
        if (!_ctx.Libros.Any(lib => lib.ISBN13 == libro.ISBN13))
        {
          //  podriamos setear la PK desde nuestra aplicacion!
          //
          //  nuevo.ID_Real = Guid.NewGuid();
          //
          _ctx.Libros.Add(libro);
        }
        else
          _logger.LogWarning("Se intento ingresar un elemento existente {libro}", libro);
      }
      //  guardamos la operacion TOTAL
      //
      _ctx.SaveChanges();
    }

    public void ExportarListaDeAutores(IEnumerable<(string idLibro, string nombre)> autores)
    {
      /*
          id;nombre
          DKcWE3WXoj8C;Daniel H. Nexon
          DKcWE3WXoj8C;Iver B. Neumann
          3PGBUrScs-YC;Richard Abanes
          JGQBcu5O_ZcC;Nikita Agarwal
          JGQBcu5O_ZcC;Chitra Agarwal
          JGQBcu5O_ZcC;Benjamin Vincent
          JGQBcu5O_ZcC;Daniel H. Nexon
      */
      //  idLibro es la clave Google...
      //  si el libro no existe...podemos loguear y seguir...
      //  si el libro existe, buscamos el autor...y si no esta creamos uno nuevo...y lo agregamos al libro
      //  cuando terminamos, guardar cambios!
      foreach (var id_nombre in autores)
      {
        try
        {
          Libro libro = _ctx.Libros.Where(lib => lib.ID == id_nombre.idLibro).SingleOrDefault();

          if (libro != null)
          {
            Autor autor = _ctx.Autores.Where(aut => aut.Nombre == id_nombre.nombre).SingleOrDefault();

            autor ??= new Autor() {Nombre = id_nombre.nombre};

            //  No deberiamos usar esta version porque simplemente agregamos el autor sin su libro asociado!
            //  _ctx.Autores.Add(autor);
            //

            //  Esta version es correcta porque asocia la relacion desde el lado "autor"
            //
            //  autor.Libros.Add(libro);

            libro.Autores.Add(autor);
            _ctx.SaveChanges();
          }
          else
          {
            _logger.LogWarning("El libro solicitado no existe {id_libro}", id_nombre.idLibro);
          }
        }
        catch (Exception ex)
        {
          _logger.LogCritical(ex, "Se produjo una excepcion {id_libro} {autor} - Posible libro o autor duplicados",
            id_nombre.idLibro, id_nombre.nombre);
        }
      }
    }

    public void ClearDatabase()
    {
      _logger.LogWarning("Los datos de todas las tablas de articulos se eliminaran!!");

      _ctx.Database.ExecuteSqlRaw("delete from Libros_Autores; delete from Autores; delete from Libros;");
    }
  }
}



