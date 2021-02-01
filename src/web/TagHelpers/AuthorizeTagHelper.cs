using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using entidades.Seguridad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using web.Extensiones;

namespace web.TagHelpers
{
  [HtmlTargetElement("authorize")]
  public class AuthorizeTagHelper : TagHelper
  {
    public string Roles { get; set; }

    private readonly IHttpContextAccessor _contextAccessor;

    public AuthorizeTagHelper(IHttpContextAccessor httpContextAccessor)
    {
      _contextAccessor = httpContextAccessor;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      bool incluirContenido = false;

      if (string.IsNullOrWhiteSpace(Roles))
      {
        base.Process(context, output);
        return;
      }
      //  si tenemos que procesar, primero obtenemos la sesion actual (si no hay sesion vamos a suprimir el contenido...)
      //
      Usuario userAct = _contextAccessor.HttpContext?.Session.Get<Usuario>("USER");

      if (userAct != null)
      {
        if (Roles != null)
        {
          List<string> listaInclude = new List<string>(Roles.Split(',', StringSplitOptions.RemoveEmptyEntries));

          //  lo mas facil siempre es la "positiva" o sea ver que cuando alguno de los claims del usuario coinciden con la lista
          //  entonces tenemos que incluir el contenido
          //
          if (listaInclude.Any(item => userAct.Perfil.Nombre == item.Trim()))
            incluirContenido = true;
        }
      }

      if (incluirContenido)
      {
        var contenido = output.GetChildContentAsync().Result;

        output.Content.SetHtmlContent(contenido);
      }
      else
      {
        output.SuppressOutput();
      }

    }
  }
}
