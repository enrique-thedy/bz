using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entidades.Seguridad;

namespace servicios
{
  public interface IServiciosSeguridad
  {
    /// <summary>
    /// Retorna todos los perfiles encontrados...
    /// </summary>
    /// <returns></returns>
    IEnumerable<Perfil> GetPerfiles();

    bool AgregarPerfil(Perfil nuevo);

    bool AgregarUsuario(Usuario nuevo);

    Usuario GetUsuarioFromCredenciales(string login, string pass);
  }
}
