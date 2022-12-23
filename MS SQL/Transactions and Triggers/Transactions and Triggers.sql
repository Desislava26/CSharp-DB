USE Bank

CREATE TABLE Logs 
(
  LogID INT NOT NULL PRIMARY KEY IDENTITY,
  AccountID INT NOT NULL FOREIGN KEY REFERENCES Accounts(ID),
  OldSum DECIMAL NOT NULL,
  NewSum DECIMAL NOT NULL
)
go

CREATE OR ALTER TRIGGER tr_OldAndNewBalance
ON Accounts FOR UPDATE
AS 
BEGIN
DECLARE @oldSum DECIMAL(15,2) = (SELECT Balance FROM deleted)
DECLARE @newSum DECIMAL(15,2) = (SELECT Balance FROM inserted)
DECLARE @idAccount INT = (SELECT Id FROM deleted)

INSERT INTO Logs (AccountID, NewSum, OldSum)
VALUES (@idAccount, @newSum, @oldSum)
END

UPDATE Accounts
SET Balance += 10
Where Id = 1

-----------------------------------------------------------------------------------------------
CREATE TABLE NotificationEmails
(
  Id INT NOT NULL PRIMARY KEY IDENTITY,
  Recipient INT NOT NULL FOREIGN KEY REFERENCES Accounts(ID),
  [Subject] NVARCHAR(MAX) NOT NULL,
  Body NVARCHAR(MAX) NOT NULL
)
go


CREATE OR ALTER TRIGGER tr_createNewEmailWithInfo 
ON Logs FOR INSERT
AS
BEGIN
INSERT INTO NotificationEmails
VALUES
(
(SELECT AccountId FROM inserted),
CONCAT('Balance change for account: ',
(SELECT AccountId FROM inserted)),
CONCAT('On ', FORMAT(GETDATE(), 'dd-MM-yyyy HH:mm'), ' your balance was changed from ',
(SELECT OldSum FROM Logs), ' to ',
(SELECT NewSum FROM Logs), '.')
)

END
go

-----------------------------------------------------------------------------------------------
CREATE OR ALTER PROC usp_DepositMoney (@accountId INT, @moneyAmount DECIMAL(15,4))
AS
BEGIN TRANSACTION
IF(@accountId IS NULL)
BEGIN
ROLLBACK
RAISERROR ('Invalid account!',16,1) 
RETURN
END

IF (@moneyAmount < 0)
BEGIN
ROLLBACK
RAISERROR('Negative money!', 16,1)
RETURN
END

UPDATE Accounts
SET Balance += @moneyAmount
WHERE Id = @accountId
COMMIT

EXEC usp_DepositMoney 1, 10
SELECT*FROM Accounts WHERE id =1
go

-----------------------------------------------------------------------------------------------
CREATE PROC usp_WithdrawMoney (@AccountId INT, @MoneyAmount MONEY)
AS
BEGIN TRANSACTION
UPDATE Accounts
SET Balance -= @MoneyAmount
WHERE Id = @AccountId
DECLARE @LeftBalance MONEY = (SELECT Balance FROM Accounts WHERE Id = @AccountId)
IF(@LeftBalance < 0)
BEGIN
ROLLBACK
RAISERROR('',16,2)
RETURN
END
COMMIT

EXEC usp_WithdrawMoney 5, 25
SELECT*FROM Accounts WHERE id =5
go

-----------------------------------------------------------------------------------------------
CREATE OR ALTER PROC usp_TransferMoney(@SenderId INT, @ReceiverId INT, @Amount MONEY)
AS
BEGIN TRANSACTION
EXEC usp_DepositMoney @ReceiverId, @Amount
EXEC usp_WithdrawMoney @SenderId, @Amount
COMMIT

EXEC usp_TransferMoney 5,1,5000
go

-----------------------------------------------------------------------------------------------
USE SoftUni
Go

CREATE PROCEDURE usp_AssignProject(@emloyeeID INT, @ProjectID INT)
AS
BEGIN
DECLARE @MaxProjectsPerEmployee INT = 3
DECLARE @EmployeeProjectCount INT
BEGIN TRANSACTION
INSERT EmployeesProjects (EmployeeID, ProjectID)
VALUES
(@emloyeeID, @ProjectID)

SET @EmployeeProjectCount = (SELECT COUNT(*) FROM EmployeesProjects
WHERE EmployeeID = @emloyeeID)

IF(@EmployeeProjectCount > @MaxProjectsPerEmployee)
BEGIN
RAISERROR('The employee has too many projects!',16,1)
ROLLBACK
END
ELSE
BEGIN
COMMIT
END
END

-----------------------------------------------------------------------------------------------

CREATE TABLE Deleted_Employees
(
EmployeeId INT PRIMARY KEY IDENTITY, 
FirstName NVARCHAR, 
LastName NVARCHAR, 
MiddleName NVARCHAR, 
JobTitle NVARCHAR, 
DepartmentId INT, 
Salary MONEY
) 
go

CREATE OR ALTER TRIGGER tr_DeleteEmployees
ON Employees
AFTER DELETE
AS
BEGIN
INSERT INTO Deleted_Employees
SELECT FirstName,LastName,MiddleName,JobTitle,DepartmentID,Salary
FROM deleted
END

-----------------------------------------------------------------------------------------------
