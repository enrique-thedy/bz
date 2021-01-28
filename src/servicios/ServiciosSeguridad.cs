using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using datos;
using entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace servicios
{
  public class ServiciosSeguridad : IServiciosSeguridad
  {
    private readonly SeguridadContext _ctx;

    public ServiciosSeguridad(SeguridadContext ctx)
    {
      _ctx = ctx;
    }

    public IEnumerable<Perfil> GetPerfiles()
    {
      return _ctx.Perfiles;
    }

    public bool AgregarPerfil(Perfil nuevo)
    {
      try
      {
        _ctx.Perfiles.Add(nuevo);
        _ctx.SaveChanges();

        return true;
      }
      catch
      {
        //  log
        return false;
      }
    }

    public bool AgregarUsuario(Usuario nuevo)
    {
      try
      {
        _ctx.Usuarios.Add(nuevo);
        _ctx.SaveChanges();

        return true;
      }
      catch
      {
        //  log
        return false;
      }
    }

    public Usuario GetUsuarioFromCredenciales(string login, string pass)
    {
      Usuario usr = _ctx.Usuarios.Include("Perfil").FirstOrDefault(usr => usr.Login == login);

      if (usr != null)
      {
        //  podemos invocar algun SP de la base de datos que valide la pass que nos pasan 
        //  SIN NECESIDAD de traer la que esta guardada en la DB
        //  execute ValidarUsuario(login, pass)
        return usr;
      }

      return null;
    }
  }
}
