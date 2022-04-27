-- Exemplo
DECLARE @MovLoc TABLE (RowId INT, MovimentacaoId INT, LocacaoId INT)
DECLARE @Counter INT, @TotalCount INT, @MovimentacaoId INT, @LocacaoId INT

INSERT INTO @MovLoc
select ROW_NUMBER() OVER(ORDER BY CodigoDaMovimentacao ASC) AS RowId,CodigoDaMovimentacao AS MovimentacaoId, LocacaoId FROM MovimentacaoDeContasBancarias WHERE LocacaoId IS NOT NULL 

SET @Counter = 1
 
SET @TotalCount = (SELECT COUNT(*) FROM @MovLoc)

WHILE (@Counter <= @TotalCount)
BEGIN
    SET @MovimentacaoId = (SELECT MovimentacaoId FROM @MovLoc WHERE RowId = @Counter);
    SET @LocacaoId = (SELECT LocacaoId FROM @MovLoc WHERE RowId = @Counter);

    INSERT INTO MovimentacaoLocacao(LocacaoId, MovimentacaoId)
	VALUES(@LocacaoId, @MovimentacaoId);
 
    SET @Counter = @Counter + 1
    CONTINUE;
END
