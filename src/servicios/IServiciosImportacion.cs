using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Articulos;

namespace servicios
{
  public interface IServiciosImportacion
  {
    IEnumerable<Libro> ImportarCSV(string file);
  }
}
