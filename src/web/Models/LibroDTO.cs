using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Models
{
  public class LibroDTO
  {
    public string Titulo { get; set; }

    public List<string> Autores { get; set; }

    public decimal? Precio { get; set; }

    public string Editorial { get; set; }

    public string Imagen { get; set; }
  }
}
