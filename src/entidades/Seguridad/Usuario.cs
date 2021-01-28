using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entidades.Seguridad
{
  public class Usuario
  {
    public string Login { get; set; }

    public string Nombre { get; set; }

    public string Correo { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime FechaAlta { get; set; }

    public DateTime Nacimiento { get; set; }

    public Perfil Perfil { get; set; }
  }
}
