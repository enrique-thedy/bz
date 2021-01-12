using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Articulos;

namespace servicios
{
  public interface IServiciosExportacion
  {
    void ExportarListaDeLibros(IEnumerable<Libro> lista);

    void ExportarListaDeAutores(IEnumerable<(string idLibro, string nombre)> autores);

    void ClearDatabase();
  }
}
