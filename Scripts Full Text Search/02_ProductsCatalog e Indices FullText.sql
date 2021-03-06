USE [db8ec05b901b98419ea5c1a74c014fbd40]
GO
/****** Object:  FullTextCatalog [ProductsCatalog]    Script Date: 03/06/2017 11:56:25 ******/
CREATE FULLTEXT CATALOG [ProductsCatalog]WITH ACCENT_SENSITIVITY = OFF

GO

/****** Object:  FullTextIndex     Script Date: 03/06/2017 11:56:25 ******/
CREATE FULLTEXT INDEX ON [dbo].[Products](
[ProductDescription] LANGUAGE 'Spanish', 
[ProductTitle] LANGUAGE 'Spanish')
KEY INDEX [PK_Products]ON ([ProductsCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)


GO
