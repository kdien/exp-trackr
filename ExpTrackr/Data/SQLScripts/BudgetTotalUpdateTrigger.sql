DROP TRIGGER IF EXISTS dbo.TR_Expenses_CRUD
GO

CREATE TRIGGER dbo.TR_Expenses_CRUD
ON dbo.Expenses
AFTER INSERT, UPDATE, DELETE
AS
	UPDATE dbo.Budgets
	SET BudgetTotal = (
		SELECT SUM(AMOUNT)
		FROM Expenses
		WHERE BudgetID = Deleted.BudgetID OR BudgetID = Inserted.BudgetID
	)
