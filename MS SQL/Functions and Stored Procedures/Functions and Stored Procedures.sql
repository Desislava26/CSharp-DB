CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000
AS
SELECT 
FirstName,
LastName
FROM Employees
WHERE Salary > 35000

EXEC usp_GetEmployeesSalaryAbove35000
GO


CREATE PROCEDURE usp_GetEmployeesSalaryAboveNumber @Number DECIMAL(18,4)
AS
SELECT 
FirstName,
LastName
FROM Employees
WHERE Salary > @Number

EXEC usp_GetEmployeesSalaryAboveNumber 48100
GO

CREATE PROCEDURE usp_GetEmployeesFromTown @TownName NVARCHAR(50)
AS
SELECT 
FirstName,
LastName
FROM Employees as e
JOIN Addresses as a ON e.AddressID = a.AddressID
JOIN Towns AS t ON a.TownID = t.TownID
WHERE t.Name = @TownName

EXEC usp_GetEmployeesFromTown 'Sofia'
GO


CREATE FUNCTION ufn_GetSalaryLevel(@salary DECIMAL(18,4)) 
RETURNS NVARCHAR(20) AS
BEGIN
    DECLARE @SalaryLevel VARCHAR(10)
	SET @SalaryLevel =
		CASE
			WHEN @salary < 30000 THEN 'Low'
			WHEN @salary <= 50000 THEN 'Average'
			ELSE 'High'
		END
	RETURN @SalaryLevel
END
GO

SELECT Salary, dbo.ufn_GetSalaryLevel(Salary) AS [Salary Level] FROM Employees
GO

CREATE PROCEDURE usp_EmployeesBySalaryLevel @SalaryLevel NVARCHAR(10)
AS
SELECT 
FirstName,
LastName
FROM Employees
WHERE dbo.ufn_GetSalaryLevel(Salary) = @SalaryLevel

EXEC usp_EmployeesBySalaryLevel 'High'
GO

CREATE FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(50), @word NVARCHAR(50))
	RETURNS BIT AS
BEGIN
		DECLARE @WordLength INT = LEN(@word)
		DECLARE @Index INT = 1

		WHILE(@WordLength >= @Index)
		BEGIN
		IF (CHARINDEX(SUBSTRING(@word, @Index, 1), @setOfLetters) = 0)
		BEGIN
			RETURN 0
		END

		SET @Index += 1
		END
		RETURN 1
END
GO
SELECT dbo.ufn_IsWordComprised('oistmiahf', 'Sofia')
SELECT dbo.ufn_IsWordComprised('oistmiahf', 'halves')
SELECT dbo.ufn_IsWordComprised('bobr', 'Rob')
SELECT dbo.ufn_IsWordComprised('pppp', 'Guy')
GO


CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(MAX), @word VARCHAR(MAX))
RETURNS BIT AS
BEGIN
	DECLARE @WordLength INT = LEN(@word)
	DECLARE @Index INT = 1

	WHILE (@Index <= @WordLength)
	BEGIN
		IF (CHARINDEX(SUBSTRING(@word, @Index, 1), @setOfLetters) = 0)
		BEGIN
			RETURN 0
		END

		SET @Index += 1
	END

	RETURN 1
END
GO

SELECT dbo.ufn_IsWordComprised('oistmiahf', 'Sofia')
SELECT dbo.ufn_IsWordComprised('oistmiahf', 'halves')
SELECT dbo.ufn_IsWordComprised('bobr', 'Rob')
SELECT dbo.ufn_IsWordComprised('pppp', 'Guy')
GO

/*--08--*/
CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT) AS
ALTER TABLE Employees
DROP CONSTRAINT FK_Employees_Employees

ALTER TABLE EmployeesProjects
DROP CONSTRAINT FK_EmployeesProjects_Employees

ALTER TABLE EmployeesProjects
ADD CONSTRAINT FK_EmployeesProjects_Employees FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID) ON DELETE CASCADE

ALTER TABLE Departments
DROP CONSTRAINT FK_Departments_Employees

ALTER TABLE Departments
ALTER COLUMN ManagerID INT NULL

UPDATE Departments
SET ManagerID = NULL
WHERE DepartmentID = @departmentId

UPDATE Employees
SET ManagerID = NULL
WHERE DepartmentID = @departmentId

DELETE FROM Employees
WHERE DepartmentID = @departmentId AND ManagerID IS NULL

DELETE FROM Departments
WHERE DepartmentID = @departmentId

IF OBJECT_ID('[Employees].[FK_Employees_Employees]') IS NULL
    ALTER TABLE [Employees] WITH NOCHECK
        ADD CONSTRAINT [FK_Employees_Employees] FOREIGN KEY ([ManagerID]) REFERENCES [Employees]([EmployeeID]) ON DELETE NO ACTION ON UPDATE NO ACTION

IF OBJECT_ID('[Departments].[FK_Departments_Employees]') IS NULL
    ALTER TABLE [Departments] WITH NOCHECK
        ADD CONSTRAINT [FK_Departments_Employees] FOREIGN KEY ([ManagerID]) REFERENCES [Employees]([EmployeeID]) ON DELETE NO ACTION ON UPDATE NO ACTION

SELECT COUNT(*) FROM Employees
WHERE DepartmentID = @departmentId

EXEC usp_DeleteEmployeesFromDepartment 4
GO


USE Bank

GO
CREATE PROCEDURE usp_GetHoldersFullName 
AS
SELECT 
CONCAT(FirstName+' ', LastName) AS FullName
FROM AccountHolders

EXEC usp_GetHoldersFullName 
GO


CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan @money DECIMAL(16,2)
AS
SELECT 
h.FirstName,
h.LastName,
SUM(a.Balance) as summ
FROM AccountHolders as h
JOIN Accounts AS a ON h.Id = a.Id
GROUP BY h.FirstName, h.LastName
HAVING SUM(a.Balance) > @money

EXEC usp_GetHoldersWithBalanceHigherThan 10000
GO


CREATE FUNCTION ufn_CalculateFutureValue(@sum DECIMAL(20,2), @rate FLOAT, @years INT)
RETURNS DECIMAL(20, 4) AS
	BEGIN
	RETURN @Sum * POWER(1 + @rate, @years)

	END
	GO

SELECT dbo.ufn_CalculateFutureValue(1000, 0.1, 5)
GO


CREATE PROC usp_CalculateFutureValueForAccount(@AccountID INT, @InterestRate FLOAT) AS
SELECT 
	a.Id AS [Account Id], 
	ah.FirstName AS [First Name],
	ah.LastName AS [Last Name],
	a.Balance AS [Current Balance],
	dbo.ufn_CalculateFutureValue(a.Balance, @InterestRate, 5) AS [Balance in 5 years] 
	FROM Accounts AS a
JOIN AccountHolders AS ah
ON a.AccountHolderId = ah.Id AND a.Id = @AccountID

EXEC usp_CalculateFutureValueForAccount 1, 0.1
GO



use Diablo
GO
CREATE FUNCTION ufn_CashInUsersGames(@gameName VARCHAR(MAX))
RETURNS TABLE AS
RETURN	SELECT SUM(Cash) AS SumCash FROM
	(
		SELECT ug.Cash, 
		ROW_NUMBER() OVER(ORDER BY Cash DESC) AS RowNum 
		FROM UsersGames AS ug
		JOIN Games AS g ON g.Id = ug.GameId
		WHERE g.Name = @gameName
	) AS AllGameRows
	WHERE RowNum % 2 = 1
GO

SELECT * FROM dbo.ufn_CashInUsersGames('Lily Stargazer')