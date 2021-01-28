using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using entidades.Seguridad;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using servicios;
using web.Extensiones;

namespace web.Controllers
{
  public class UserController : Controller
  {
    private readonly IServiciosSeguridad _seg;

    public UserController(IServiciosSeguridad seg)
    {
      _seg = seg;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
      TempData["url"] = returnUrl;

      return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(string login, string password)
    {
      string returnUrl = TempData["url"] as string;

      Usuario userAut = _seg.GetUsuarioFromCredenciales(login, password);

      if (userAut == null)
        return Unauthorized();

      List<Claim> claims = new List<Claim>
      {
        new (ClaimTypes.Name, userAut.Nombre),
        new (ClaimTypes.NameIdentifier, userAut.Login),
        new (ClaimTypes.DateOfBirth, userAut.Nacimiento.ToString("d")),
        new (ClaimTypes.Role, userAut.Perfil.Nombre),
        new ("TipoPerfil", userAut.Perfil.Tipo.ToString())
      };

      var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

      var principal = new ClaimsPrincipal(identity);

      //  La siguiente linea es para ejecutar Login sincronicamente, o sea que bloquea el pipeline de ASP
      //
      //  HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()).RunSynchronously();

      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
        new AuthenticationProperties());
      //
      HttpContext.Session.Set<Usuario>("USER", userAut);
      //
      return LocalRedirect(returnUrl);
    }

    public IActionResult Logout()
    {
      HttpContext.Session.Remove("USER");

      //  HttpContext.SignOutAsync();

      return LocalRedirect("/");

    }
  }
}
