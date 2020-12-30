use CURSO

drop table Libros 
/*
  - Editorial deberia ser FK a otra tabla, por ahora la dejamos sin normalizar
  - Tipo_Publico es la representacion del enum, si quisieramos que nos aparezca el texto deberiamos poner un converter en C#
  - Clave_Origen es la que nos retorna Google Books, la dejamos para hacer el programa mas sencillo, pero deberia ser parte
    de una tabla temporal de importacion!

*/
create table Libros 
(
  ID                    uniqueidentifier     not null default(newid()),
  Clave_Origen          varchar(50),
  ISBN13                varchar(20),
  ISBN10                varchar(20),
  Titulo                varchar(150),
  Subtitulo             varchar(250),
  Fecha_Publicacion     date,
  Paginas               int,
  Editorial             varchar(100),
  --  ID_Editorial          uniqueidentifier,
  Tipo_Publico          int,
  --  Tipo_Publico          varchar(20),
  Descripcion           varchar(max),
  Categoria             varchar(150),
  Precio                numeric(10, 4),
  Moneda                varchar(10),
  Promedio_Rating       float,
  Comentarios           int,
  Idioma                varchar(5),
  LinkCanonico          varchar(400),
  LinkImagen            varchar(400),
  LinkInfo              varchar(400),
  --
  constraint PK_Libros primary key (ID)
)

/*
  El indice nos ayuda a las busquedas por ISBN13 que realizamos para validar que el libro no exista...
*/
create index IX_Libros_ISBN on Libros(ISBN13)

select * from Libros

--  delete from Libros




