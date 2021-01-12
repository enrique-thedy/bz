using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Articulos;
using entidades.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace datos
{
  public class ExportacionContext : DbContext
  {
    private readonly ILogger<ExportacionContext> _logger;

    //
    //  plural de Libro --> Libroes
    //
    public DbSet<Libro> Libros { get; set; }

    public DbSet<Autor> Autores { get; set; }
    
    //  public DbSet<Usuario> Usuarios { get; set; }

    public ExportacionContext(DbContextOptions<ExportacionContext> options, ILogger<ExportacionContext> logger) :
      base(options)
    {
      //  TODO_HECHO cambiar nombre del contexto para adecuarlo a la funcion
      //  TODO usar nombre del contexto para obtener la cadena de conexion

      //  _config = config;
      _logger = logger;

      _logger.LogWarning("Creado contexto {contexto} desde AddDbContext<T>", nameof(ExportacionContext));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new ConfigurarLibro());
      modelBuilder.ApplyConfiguration(new ConfigurarAutor());
      base.OnModelCreating(modelBuilder);
    }
  }
}
