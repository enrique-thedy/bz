using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using entidades.Seguridad;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using web.Extensiones;

namespace web.Components
{
  public class InfoUsuario : ViewComponent
  {
    public IViewComponentResult Invoke()
    {
      Usuario user = HttpContext.Session.Get<Usuario>("USER");

      if (user == null)
      {
        //  si la sesion expiró pero el usuario sigue autenticado...le hacemos signout
        //  PASADO A MIDDLEWARE
        //
        return View("UsuarioDesconectado");
      }

      //  el usuario tiene sesion...pero tambien tenemos que tener en cuenta si NO esta autenticado!!
      //  o sea el caso inverso al anterior
      //  PASADO A MIDDLEWARE
      //
      return (user.Perfil.Tipo == TipoPerfil.Empleado) ?
        View("UsuarioEmpleado", user) :
        View("UsuarioCliente", user);
    }
  }
}
