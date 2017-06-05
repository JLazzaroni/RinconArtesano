Create Function Split(@string varchar(8000), @delimiter char(1))
    RETURNS @temptable TABLE(nameIndex int identity(1,1), items varchar(8000))
AS
BEGIN
    DECLARE @index int, @slice varchar(8000)
    SELECT @index = 1
    IF LEN(@string) < 1 or @string IS NULL RETURN
    WHILE @index != 0
    BEGIN
        SET @index = CHARINDEX(@delimiter, @string)
        IF @index != 0
            SET @slice = LEFT(@string, @index-1)
        ELSE
            SET @slice = @string

        IF(LEN(@slice) > 0)
            INSERT INTO @temptable (items) VALUES (@slice)
            SET @string = RIGHT(@string, LEN(@string) - @index)
            IF LEN(@string) = 0 BREAK
    END
    RETURN
END