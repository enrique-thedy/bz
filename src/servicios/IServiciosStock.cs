using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Articulos;

namespace servicios
{
  public interface IServiciosStock
  {
    List<(Guid, string)> ObtenerTitulosDeEditorial(string editorial);

    IEnumerable<Libro> GetLibrosFromCriterio(string criterio);

    bool AgregarLibro(Libro nuevo);

    bool AgregarAutor(Autor nuevo);
  }
}
