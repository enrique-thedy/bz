using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Articulos;
using entidades.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace datos
{
  #region ARTICULOS

  public class ConfigurarLibro : IEntityTypeConfiguration<Libro>
  {
    public void Configure(EntityTypeBuilder<Libro> builder)
    {
      //  builder.ToTable("Libros");

      //  si no declaramos la PK va a generar todos los ID con valor cero 00000000-0000-0000-0000-000000000000
      //  por supuesto cuando quiera ingresarlos a la tabla dara violacion de PK
      //
      builder.HasKey(lib => lib.ID_Real);

      //if (usarSqlDefaults)
      //{
      //  //  el valor que ponga en el default de sql sirve para las migraciones (o sea si voy a generar el modelo en sql)
      //  //  newid() o newsequentialid() / getdate()
      //  //  si no uso migraciones puedo poner cualquier cosa
      //  //
      //  builder.Property(lib => lib.ID_Real)
      //    .HasColumnName("ID")
      //    .HasDefaultValueSql("newsequentialid()"); //  puede ser cualquier nombre de funcion!!

      //  builder.Property(lib => lib.Publicacion)
      //    .HasColumnName("Fecha_Publicacion")
      //    .HasDefaultValueSql("getdate()");
      //}


      //  si no informamos nada al model builder, el GUID se genera en el cliente...
      //  si queremos que se genere en el server tenemos que agregar
      //    .HasDefaultValueSql("newsequentialid()"); 
      //  puede ser cualquier nombre de funcion o bien ningun argumento!!

      builder.Property(lib => lib.ID_Real)
        .HasColumnName("ID")
        .HasDefaultValueSql();

      //  en este caso...como Publicacion es nullable, si no pongo un valor por defecto, cualquier fecha nula me
      //  produciria un error de insert...
      //  Con HasDefaultValue, le aviso a EF que NO MANDE la columna y que despues la LEA desde la DB
      //  El valor que ponga por defecto ES IGNORADO
      //
      builder.Property(lib => lib.Publicacion)
        .HasColumnName("Fecha_Publicacion")
        .HasDefaultValue(DateTime.MinValue);

      builder.Property(lib => lib.ID).HasColumnName("Clave_Origen");

      builder.Property(lib => lib.Publico).HasColumnName("Tipo_Publico");

      //  TODO_HECHO configurar precision decimal para eliminar el warning
      //  Con EFC 5 vamos a poder setear la precision 
      //  https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-5.0/whatsnew#configure-database-precisionscale-in-model
      //
      builder
        .Property(lib => lib.Precio)
        .HasColumnType("numeric(10, 4)");

      //  setear el tipo de datos en SQL como float (que es la realidad) para que EF no tenga problemas
      //  en el mapeo e intente tomar un double
      //
      builder
        .Property(lib => lib.RatingPromedio)
        .HasColumnName("Promedio_Rating")
        .HasColumnType("float");

      builder
        .HasMany(lib => lib.Autores)
        .WithMany(aut => aut.Libros)
        .UsingEntity<Dictionary<string, object>>("Libros_Autores",
          x => x.HasOne<Autor>().WithMany().HasForeignKey("ID_Autor"),
          x => x.HasOne<Libro>().WithMany().HasForeignKey("ID_Libro"));

      //builder
      //  .HasOne(lib => lib.Editorial)
      //  .WithMany()
      //  .HasForeignKey("ID_Editorial");

    }
  }

  public class ConfigurarAutor : IEntityTypeConfiguration<Autor>
  {
    public void Configure(EntityTypeBuilder<Autor> builder)
    {
      builder.Property(aut => aut.Biografia).HasColumnName("Bio");
    }
  }



  #endregion


  #region SEGURIDAD

  public class ConfigurarUsuario : IEntityTypeConfiguration<Usuario>
  {
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
      //  Las dos siguientes son equivalentes...
      //
      //  builder.HasKey(usr => new {usr.Login, usr.FechaAlta});
      //  builder.HasKey("Login", "FechaAlta");
      //
      builder.HasKey(usr => usr.Login); 
      builder.Property(usr => usr.Correo).HasColumnName("Email");
      builder.Property(usr => usr.LastLogin).HasColumnName("Ultimo_Ingreso");
      builder.Property(usr => usr.FechaAlta).HasColumnName("Fecha_Alta");
      builder.Property(usr => usr.Nacimiento).HasColumnName("Fecha_Nacimiento");

      builder
        .HasOne<Perfil>("Perfil")
        .WithMany()
        .HasForeignKey("ID_Perfil");
    }
  }

  public class ConfigurarPerfil : IEntityTypeConfiguration<Perfil>
  {
    public void Configure(EntityTypeBuilder<Perfil> builder)
    {
      builder.Property<TipoPerfil>("Tipo").HasColumnName("Tipo_Perfil");

      builder.HasMany<Usuario>().WithOne(usr => usr.Perfil).HasForeignKey("ID_Perfil");
    }
  }

  #endregion

}
