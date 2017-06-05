Create Procedure SearchAspNetUsers
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
            dbo.AspNetUsers 
        Where Contains(*, @AndPredicate ) 
        )

    Select
		u.Id,
		u.UserName,
        ct.Rank,
		@TotalRecords As TotalRecords
    From 
        dbo.AspNetUsers u
            Inner Join ContainsTable (AspNetUsers, *, @NearPredicate ) As ct On u.Id = ct.[Key]
	Where Exists (	--El usuario esta vigente
		Select 1
		From dbo.UsersInfo ui
		Where ui.UsersId = u.Id
			And ui.DateNull Is Null
	)
    Order By 
        ct.Rank Desc
    
End