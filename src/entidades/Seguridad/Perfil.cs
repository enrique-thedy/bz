﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entidades.Seguridad
{
  public enum TipoPerfil
  {
    Empleado,
    Cliente
  }

  public class Perfil
  {
    public int ID { get; set; }

    public TipoPerfil Tipo { get; set; }

    public string Nombre { get; set; }
  }
}
