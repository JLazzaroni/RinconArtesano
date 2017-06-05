USE [db8ec05b901b98419ea5c1a74c014fbd40]
GO
/****** Object:  FullTextCatalog [MessagesCatalog]    Script Date: 04/06/2017 16:27:03 ******/
CREATE FULLTEXT CATALOG [MessagesCatalog]WITH ACCENT_SENSITIVITY = OFF

GO
/****** Object:  FullTextIndex     Script Date: 04/06/2017 16:27:03 ******/
CREATE FULLTEXT INDEX ON [dbo].[Messages](
[Message] LANGUAGE 'Spanish')
KEY INDEX [PK_Messages]ON ([MessagesCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)


GO