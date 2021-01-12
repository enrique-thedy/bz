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


create table Autores 
(
  ID              int           identity      not null,
  Nombre          varchar(150)  not null,
  Bio             varchar(max),
  --
  constraint PK_Autores primary key (ID)
)

--  drop table Libros_Autores

create table Libros_Autores
(
  ID_Libro        uniqueidentifier    not null,
  ID_Autor        int                 not null,
  --
  constraint PK_Libros_Autores primary key (ID_Libro, ID_Autor),
  constraint FK_Libros_Autores_Libros foreign key (ID_Libro) references Libros(ID),
  constraint FK_Libros_Autores_Autores foreign key (ID_Autor) references Autores(ID)
)

/*

*/
create table Perfiles
(
  ID              int           not null      identity,
  Nombre          varchar(50)   not null,
  Tipo_Perfil     int           not null,
  Descripcion     varchar(400),
  --
  constraint PK_Perfiles primary key (ID)
)

/*
  Email_Valido --> la primera vez que ingresa tiene q validar el correo (no aplica si es empleado)
  Habilitado --> podria estar en false si hay muchos reintentos...si intenta recuperar contraseña...
*/
create table Usuarios
(
  Login               varchar(25)     not null,
  ID_Perfil           int             not null,
  --
  Nombre              varchar(25)     not null,
  Email               varchar(25)     not null,
  Email_Valido        bit             not null    default(0),
  Habilitado          bit             not null    default(0),
  Fecha_Alta          smalldatetime   not null,
  Fecha_Nacimiento    date            not null,
  Hashed_Password     varchar(100)    not null,
  Imagen              image,
  Ultimo_Ingreso      smalldatetime,
  --
  constraint PK_Usuarios primary key (Login),
  constraint FK_Usuarios_Perfiles foreign key (ID_Perfil) references Perfiles(ID)
)

