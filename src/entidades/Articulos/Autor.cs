using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.Articulos
{
  public class Autor
  {
    public int ID { get; set; }

    public string Nombre { get; set; }

    public string Biografia { get; set; }

    public ISet<Libro> Libros { get; set; }

    public Autor()
    {
      Libros = new HashSet<Libro>();
    }
  }
}
