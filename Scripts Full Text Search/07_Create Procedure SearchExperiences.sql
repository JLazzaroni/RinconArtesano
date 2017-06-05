Create Procedure SearchExperiences
    @SearchTerm varchar(8000)
As
Begin

    Declare @NearPredicate Varchar(8000), 
            @AndPredicate Varchar(8000), 
            @TotalRecords Int

    Select 
        @NearPredicate = Coalesce(@NearPredicate + ' NEAR ', '') + items
    From Split(@SearchTerm, ' ') 
        Left Join sys.fulltext_system_stopwords ON items = stopword
    Where stopword Is Null

    Set @AndPredicate = Replace(@NearPredicate, 'NEAR', 'AND')
    Set @NearPredicate = '(' + @NearPredicate + ')'

    Set @TotalRecords  = (
        Select 
            Count(*) 
        From 
            dbo.Experiences 
        Where Contains(*, @AndPredicate ) 
        )

    Select
		e.ExperienceId,
		e.ExperienceTitle,
		e.ExperienceDescription,
        ct.Rank,
		@TotalRecords AS TotalRecords
    From 
        dbo.Experiences e
            Inner Join ContainsTable (Experiences, *, @NearPredicate ) As ct On e.ExperienceId = ct.[Key]
	Where e.IsBlocked = 0 And e.DateNull Is Null
    Order By 
        ct.Rank Desc
    
End