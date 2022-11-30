USE SoftUni
SELECT FirstName, LastName FROM Employees
WHERE FirstName LIKE 'Sa%'

SELECT FirstName, LastName FROM Employees
WHERE LastName LIKE '%ei%'

SELECT FirstName FROM Employees
WHERE DepartmentID=3 OR DepartmentID=10
AND (SELECT YEAR(HireDate))>= 1995 
AND (SELECT YEAR(HireDate)) <= 2005

SELECT FirstName, LastName FROM Employees
WHERE NOT JobTitle LIKE '%engineer%'

SELECT [Name] FROM Towns
WHERE LEN([Name]) IN (5,6)
ORDER BY [Name] ASC

SELECT * FROM Towns
WHERE LEFT([Name],1) IN ('M','K','B','E')
ORDER BY [Name]

SELECT * FROM Towns
WHERE NOT LEFT([Name],1) IN ('R','B','D')
ORDER BY [Name]

CREATE VIEW V_EmployeesHiredAfter2000 AS
SELECT FirstName,LastName 
  FROM Employees
 WHERE (SELECT YEAR(HireDate)) > 2000

SELECT FirstName, LastName FROM Employees
WHERE LEN(LastName) = 5


SELECT EmployeeID, FirstName, LastName, Salary,
DENSE_RANK () OVER ( 
		ORDER BY EmployeeID DESC
	) [Rank]
FROM Employees
WHERE Salary BETWEEN 10000 AND 50000

SELECT *
FROM (
       SELECT EmployeeID,
              FirstName,
              LastName,
              Salary,
       DENSE_RANK() 
	   OVER(partition by Salary ORDER BY EmployeeID) 
	   AS Rank
       FROM Employees
       WHERE Salary BETWEEN 10000 AND 50000) AS MyTable
ORDER BY Salary DESC


SELECT *
FROM (
       SELECT EmployeeID,
              FirstName,
              LastName,
              Salary,
       DENSE_RANK() 
	   OVER(partition by Salary ORDER BY EmployeeID) 
	   AS Rank
       FROM Employees
       WHERE Salary BETWEEN 10000 AND 50000) AS MyTable
WHERE Rank = 2
ORDER BY Salary DESC

USE Geography

SELECT CountryName AS [Country Name], IsoCode AS [Iso Code] 
  FROM Countries
 WHERE LEN(CountryName) - LEN(REPLACE(CountryName,'a','')) >=3
 ORDER BY [Iso Code]

SELECT PeakName, RiverName, LOWER(CONCAT(PeakName, '', SUBSTRING(RiverName, 2, LEN(RiverName) - 1))) AS Mix 
FROM Peaks, Rivers 
WHERE RIGHT(PeakName, 1) = LEFT(RiverName, 1)
ORDER BY Mix

USE Diablo

SELECT TOP 50 [Name], 
FORMAT([Start], 'yyyy-MM-dd') AS DATE
FROM Games
WHERE (SELECT YEAR([Start])) IN (2011, 2012)
ORDER BY CAST([Start] AS DATE), [Name]


SELECT Username, SUBSTRING(Email,CHARINDEX('@',Email)+1, LEN(Email)) AS[Email Provider] 
FROM Users
ORDER BY [Email Provider],Username

SELECT Username, IpAddress 
FROM Users
WHERE IpAddress LIKE '___.1%.%.___'
ORDER BY Username


SELECT Name, 
	'Part of the day' =
	CASE 
		WHEN DATEPART(HOUR, Start) >= 0 AND DATEPART(HOUR, Start) < 12 THEN 'Morning'
		WHEN DATEPART(HOUR, Start) >= 12 AND DATEPART(HOUR, Start) < 18 THEN 'Afternoon'
		ELSE 'Evening'
	END,
	Duration =
	CASE 
		WHEN Duration <= 3 THEN 'Extra Short'
		WHEN Duration > 3 AND Duration <= 6 THEN 'Short'
		WHEN Duration > 6 THEN 'Long'
		ELSE 'Extra Long'
	END
FROM Games
ORDER BY Name, Duration, [Part of the day]

CREATE TABLE People
(
Id int PRIMARY KEY IDENTITY,
[Name] VARCHAR(20),
Birthdate DATETIME
)

INSERT INTO People([Name], Birthdate)
VALUES
('Desi', '2002-09-26')


SELECT Name,
       DATEDIFF(YEAR, Birthdate, GETDATE()) AS [Age in Years],
       DATEDIFF(MONTH, Birthdate, GETDATE()) AS [Age in Months],
       DATEDIFF(DAY, Birthdate, GETDATE()) AS [Age in Days],
       DATEDIFF(MINUTE, Birthdate, GETDATE()) AS [Age in Minutes]
FROM People;