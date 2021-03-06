Create Procedure SearchMessages
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
            dbo.[Messages]
        Where Contains(*, @AndPredicate ) 
        )

    Select
		m.IdComentario,
		m.IdComentarioPadre,
		m.UsersId,
		m.Category,		--	1-Productos / 2-Experiencias
		m.CategoryId,	--	Id de la entidad (Producto o Experiencia)
		m.[Message],
        ct.Rank,
        @TotalRecords AS TotalRecords
    From 
        dbo.[Messages] m
            Inner Join ContainsTable ([Messages], *, @NearPredicate ) As ct On m.IdComentario = ct.[Key]
	Where (m.IsBlocked = 0 And m.DateNull Is Null) And
		Not Exists (	-- Comentario padre NO esta bloqueado o borrado
			Select 1 From dbo.[Messages] As m2
			Where m2.IdComentario = m.IdComentarioPadre	-- m2 es el comentario padre de m
			And (m2.IsBlocked = 1 And m2.DateNull Is Not Null)	
		)
    Order By 
        ct.Rank Desc
    
End