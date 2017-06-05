USE [db8ec05b901b98419ea5c1a74c014fbd40]
GO
/****** Object:  FullTextCatalog [ProductsCategoriesCatalog]    Script Date: 04/06/2017 2:56:49 ******/
CREATE FULLTEXT CATALOG [ProductsCategoriesCatalog]WITH ACCENT_SENSITIVITY = OFF

GO

/****** Object:  FullTextIndex     Script Date: 04/06/2017 2:56:49 ******/
CREATE FULLTEXT INDEX ON [dbo].[ProductsCategories](
[ProductCategoryDescriptions] LANGUAGE 'Spanish', 
[ProductCategoryName] LANGUAGE 'Spanish')
KEY INDEX [PK_ProductsCategories]ON ([ProductsCategoriesCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)


GO
