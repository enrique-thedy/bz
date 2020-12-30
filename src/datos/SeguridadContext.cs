using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace datos
{
  public class SeguridadContext : DbContext
  {
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new ConfigurarUsuario());
      
      base.OnModelCreating(modelBuilder);
    }
  }
}
