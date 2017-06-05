Create Procedure SearchProducts
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
            dbo.Products 
        Where Contains(*, @AndPredicate ) 
        )

    Select
		p.ProductId,
		p.ProductTitle,
		p.ProductDescription,
        ct.Rank,
        Stuff
        (
            (
                Select ',' + c1.ProductCategoryName
                From Products p1 
                Inner Join ProductsCategories c1 On p1.IdCategory = c1.ProductCategoryId
                Where p.ProductId = p1.ProductId
                Order By c1.ProductCategoryName
                For XML Path('')
            ), 1, 1, ''
        ) As Categories,
        @TotalRecords AS TotalRecords
    From 
        dbo.Products p
            Inner Join ContainsTable (Products, *, @NearPredicate ) As ct On p.ProductId = ct.[Key]
	Where p.IsBlocked = 0 And p.DateNull Is Null
    Order By 
        ct.Rank Desc
    
END