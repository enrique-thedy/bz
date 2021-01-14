using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Models
{
  public class CarritoCompraViewModel
  {
    public List<Guid> Libros { get; set; }

    public DateTime FechaCreacion { get; set; }

    public List<string> Cupones { get; set; }

    public float? Descuento { get; set; }
  }
}
