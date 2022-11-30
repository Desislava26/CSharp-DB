--Using Softuni database
SELECT [FirstName], [LastName], [Salary] FROM Employees

SELECT [FirstName]+'.'+[LastName]+'@softuni.bg' 
AS [Full Email Address] FROM Employees 

SELECT DISTINCT [Salary] FROM Employees ORDER BY Salary ASC

SELECT * FROM Employees WHERE [JobTitle] = 'Sales Representative'

SELECT [FirstName], [LastName], [JobTitle] FROM Employees WHERE Salary BETWEEN 20000 AND 30000

SELECT [FirstName]+' '+[MiddleName]+' '+[LastName]
AS [Full Name] FROM Employees 
WHERE [Salary] = 14000 OR
[Salary] = 12500 OR
[Salary] = 23600 OR
[Salary] = 25000

SELECT [FirstName], [LastName]
FROM Employees WHERE ManagerID IS NULL

SELECT [FirstName], [LastName], [Salary]
FROM Employees WHERE Salary > 50000 ORDER BY Salary DESC

SELECT TOP 5 [FirstName], [LastName]
FROM Employees WHERE Salary > 50000 ORDER BY Salary DESC

SELECT FirstName,
       LastName
FROM Employees
WHERE NOT DepartmentID = 4;

SELECT *
FROM Employees
ORDER BY Salary DESC,
         FirstName ASC,
         LastName DESC,
         MiddleName ASC; 

CREATE VIEW V_EmployeesSalaries
AS
     SELECT FirstName,
            LastName,
            Salary
     FROM Employees;
GO

SELECT *
FROM V_EmployeesSalaries;

CREATE VIEW V_EmployeeNameJobTitle
AS
     SELECT FirstName + ' ' + ISNULL(MiddleName, '') + ' ' + LastName AS 'Full Naeme',
            JobTitle AS 'Job Title'
     FROM Employees;

SELECT DISTINCT [JobTitle] FROM Employees

SELECT TOP 10* FROM Projects ORDER BY StartDate, [Name]

SELECT TOP 7 [FirstName], [LastName], [HireDate] FROM Employees ORDER BY HireDate DESC

------------------------------------------------------------------------------------------------------
--Using Geography database

SELECT PeakName FROM Peaks 
ORDER BY PeakName ASC

SELECT TOP 30 CountryName, [Population] FROM Countries
WHERE ContinentCode = 'EU'
ORDER BY [Population] DESC,
CountryName DESC

SELECT CountryName,
       CountryCode,
       CASE CurrencyCode
           WHEN 'EUR'
           THEN 'Euro'
           ELSE 'Not Euro'
       END AS 'Currency'
FROM Countries
ORDER BY CountryName;
------------------------------------------------------------------------------
--Using Diablo database
SELECT [Name] FROM Characters ORDER BY [Name] ASC