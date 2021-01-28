using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.Models
{
  public class SegundoViewModel
  {
    public string Nombre { get; set; }

    [Range(10, 20, ErrorMessage = "El rango numerico debe estar entre 10 y 20")]
    public int ID { get; set; }

    [Range(typeof(DateTime), "01/01/2000", "01/01/2999", ErrorMessage = "La fecha no esta dentro del rango adecuado")]
    public DateTime Fecha { get; set; }
  }
}
