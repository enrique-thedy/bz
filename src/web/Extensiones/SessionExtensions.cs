using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace web.Extensiones
{
  public static class SessionExtensions
  {
    /// <summary>
    /// Evitamos serializar tipos valor con este metodo...
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void Set<T>(this ISession src, string key, T value) //where T: class
    {
      src.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T Get<T>(this ISession src, string key) //where T: class
    {
      if (src.IsAvailable)
      {
        string valor = src.GetString(key);

        return valor is null ? default(T) : JsonConvert.DeserializeObject<T>(valor);
      }

      return default;
    }

    public static void Clear(this ISession src, string key)
    {
      src.Remove(key);
    }

    public static T GetSession<T>(this Controller src, string key)
    {
      return src.HttpContext.Session.Get<T>(key);
    }

    public static void SetSession<T>(this Controller src, string key, T value)
    {
      src.HttpContext.Session.Set<T>(key, value);
    }
  }
}
