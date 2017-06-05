USE [db8ec05b901b98419ea5c1a74c014fbd40]
GO
/****** Object:  FullTextCatalog [AspNetUsersCatalog]    Script Date: 04/06/2017 3:40:11 ******/
CREATE FULLTEXT CATALOG [AspNetUsersCatalog]WITH ACCENT_SENSITIVITY = OFF

GO
/****** Object:  FullTextIndex     Script Date: 04/06/2017 3:40:11 ******/
CREATE FULLTEXT INDEX ON [dbo].[AspNetUsers](
[UserName] LANGUAGE 'Spanish')
KEY INDEX [PK_dbo.AspNetUsers]ON ([AspNetUsersCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)


GO
