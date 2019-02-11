DROP TRIGGER IF EXISTS dbo.TR_Expenses_AfterChanges
GO

CREATE TRIGGER dbo.TR_Expenses_AfterChanges
ON dbo.Expenses
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	DECLARE @BudgetID INT

	IF EXISTS (SELECT 1 FROM inserted)
		DECLARE db_cursor CURSOR FOR
		SELECT BudgetID FROM inserted
	ELSE IF EXISTS (SELECT 1 FROM deleted)
		DECLARE db_cursor CURSOR FOR
		SELECT BudgetID FROM deleted

	OPEN db_cursor
	FETCH NEXT FROM db_cursor INTO @BudgetID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		UPDATE dbo.Budgets
		SET BudgetTotal = (
			SELECT COALESCE( SUM(Amount), 0 )
			FROM Expenses
			WHERE BudgetID = @BudgetID
		)
		WHERE BudgetID = @BudgetID

		FETCH NEXT FROM db_cursor INTO @BudgetID
	END
END
GO
