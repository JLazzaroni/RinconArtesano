USE [db8ec05b901b98419ea5c1a74c014fbd40]
GO
/****** Object:  FullTextCatalog [ExperiencesCatalog]    Script Date: 04/06/2017 3:24:25 ******/
CREATE FULLTEXT CATALOG [ExperiencesCatalog]WITH ACCENT_SENSITIVITY = OFF

GO
/****** Object:  FullTextIndex     Script Date: 04/06/2017 3:24:25 ******/
CREATE FULLTEXT INDEX ON [dbo].[Experiences](
[ExperienceDescription] LANGUAGE 'Spanish', 
[ExperienceTitle] LANGUAGE 'Spanish')
KEY INDEX [PK_Experiences]ON ([ExperiencesCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)