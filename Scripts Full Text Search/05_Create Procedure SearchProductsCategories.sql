Create Procedure SearchProductsCategories
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
            dbo.ProductsCategories 
        Where Contains(*, @AndPredicate ) 
        )

    Select
		pc.ProductCategoryId,
		pc.ProductCategoryName,
		pc.ProductCategoryDescriptions,
        ct.Rank,
		@TotalRecords AS TotalRecords
    From 
        dbo.ProductsCategories pc
            Inner Join ContainsTable (ProductsCategories, *, @NearPredicate ) As ct On pc.ProductCategoryId = ct.[Key]
	Where pc.DateNull Is Null
    Order By 
        ct.Rank Desc
    
End