USE [db8ec05b901b98419ea5c1a74c014fbd40]
GO
/****** Object:  FullTextCatalog [UsersInfoCatalog]    Script Date: 04/06/2017 12:23:44 ******/
CREATE FULLTEXT CATALOG [UsersInfoCatalog]WITH ACCENT_SENSITIVITY = OFF

GO
/****** Object:  FullTextIndex     Script Date: 04/06/2017 12:20:55 ******/
CREATE FULLTEXT INDEX ON [dbo].[UsersInfo](
[Apellido] LANGUAGE 'English', 
[Direccion] LANGUAGE 'English', 
[Intereses] LANGUAGE 'English', 
[Localidad] LANGUAGE 'English', 
[Nombre] LANGUAGE 'English', 
[Pais] LANGUAGE 'English', 
[Provincia] LANGUAGE 'English', 
[Rubro] LANGUAGE 'English', 
[Sexo] LANGUAGE 'English', 
[Telefono] LANGUAGE 'English')
KEY INDEX [PK__UsersInf__A349B06229221CFB]ON ([UsersInfoCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)


GO
--/****** Object:  FullTextIndex     Script Date: 04/06/2017 4:06:04 ******/
--CREATE FULLTEXT INDEX ON [dbo].[UsersInfo](
--[Apellido] LANGUAGE 'Spanish', 
--[Direccion] LANGUAGE 'Spanish', 
--[Intereses] LANGUAGE 'Spanish', 
--[Localidad] LANGUAGE 'Spanish', 
--[Nombre] LANGUAGE 'Spanish', 
--[Pais] LANGUAGE 'Spanish', 
--[Provincia] LANGUAGE 'Spanish', 
--[Rubro] LANGUAGE 'Spanish', 
--[Sexo] LANGUAGE 'Spanish', 
--[Telefono] LANGUAGE 'Spanish')
--KEY INDEX [PK__UsersInf__A349B062E1D42D57]ON ([UsersInfoCatalog], FILEGROUP [PRIMARY])
--WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)


--GO