using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.Models
{
  public class CriterioViewModel
  {
    [MinLength(3, ErrorMessage = "Al menos deben ingresarse 3 caracteres")]
    public string Titulo { get; set; }

    [MinLength(3, ErrorMessage = "Al menos deben ingresarse 3 caracteres")]
    public string Editorial { get; set; }
  }
}
