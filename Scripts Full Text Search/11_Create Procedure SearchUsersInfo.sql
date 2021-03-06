Alter Procedure SearchUsersInfo
    @SearchTerm varchar(8000)
As
Begin

    Declare @NearPredicate Varchar(8000), 
            @AndPredicate Varchar(8000), 
            @TotalRecords Int

    Select 
        @NearPredicate = Coalesce(@NearPredicate + ' NEAR ', '') + items
    From Split(@SearchTerm, ' ') 
        Left Join sys.fulltext_system_stopwords On items = stopword
    Where stopword Is Null

    Set @AndPredicate = Replace(@NearPredicate, 'NEAR', 'AND')
    Set @NearPredicate = '(' + @NearPredicate + ')'

    Set @TotalRecords  = (
        Select 
            Count(*) 
        From 
            dbo.UsersInfo 
        Where Contains(*, @AndPredicate ) 
        )

    Select
		u.UsersId,
		a.UserName,
		u.Nombre,
		u.Apellido,
		u.Edad,
		u.Sexo,
		u.Pais,
		u.Provincia,
		u.Localidad,
		u.Direccion,
		u.Telefono,
		u.Intereses,
		u.Rubro,
        ct.Rank,
        @TotalRecords AS TotalRecords
    From 
        dbo.UsersInfo u
            Inner Join ContainsTable (UsersInfo, *, @NearPredicate ) As ct On u.UsersId = ct.[Key]
			Inner Join dbo.AspNetUsers As a On a.Id = u.UsersId
	Where u.DateNull Is Null
    Order By 
        ct.Rank Desc
    
End