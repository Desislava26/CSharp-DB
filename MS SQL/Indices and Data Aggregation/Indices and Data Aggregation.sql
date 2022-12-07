SELECT COUNT(Id) AS [Count]
FROM WizzardDeposits

SELECT
DepositGroup,
MAX(MagicWandSize) AS LongestMagicWand
FROM WizzardDeposits
GROUP BY DepositGroup


SELECT TOP (2)
DepositGroup
FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY AVG(MagicWandSize) ASC


SELECT
DepositGroup,
SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
GROUP BY DepositGroup


SELECT
DepositGroup,
SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup


SELECT
DepositGroup,
SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
ORDER BY SUM(DepositAmount) DESC


SELECT
DepositGroup,
MagicWandCreator,
MIN(DepositCharge) AS MinDepositCharge
FROM WizzardDeposits
GROUP BY MagicWandCreator, DepositGroup
ORDER BY MagicWandCreator, DepositGroup

SELECT 
r.AgeGroup,
COUNT(r.AgeGroup)
FROM
(
SELECT
    CASE 
	WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
	WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
	WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
	WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
	WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
	WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
	WHEN Age >= 61 THEN '[61+]'
	END AS AgeGroup
FROM WizzardDeposits
) AS r
GROUP BY r.AgeGroup


SELECT
z.n
FROM
(
SELECT
SUBSTRING(FirstName,1,1) AS n
FROM WizzardDeposits
WHERE DepositGroup = 'Troll Chest'
) AS z
GROUP BY z.n
ORDER BY z.n ASC


SELECT
DepositGroup,
IsDepositExpired,
AVG(DepositInterest) AS AverageInterest
FROM WizzardDeposits
WHERE DepositStartDate > '01/01/1985'
GROUP BY DepositGroup, IsDepositExpired
ORDER BY DepositGroup DESC, IsDepositExpired


SELECT ABS(SUM(k.diff)) 
FROM(
SELECT 
Host.DepositAmount - LEAD(Host.DepositAmount, 1) OVER(ORDER BY Host.Id DESC) as diff
FROM WizzardDeposits AS Host
) AS k


select*from WizzardDeposits

USE SoftUni

SELECT 
d.DepartmentID,
SUM(e.Salary) AS TotalSalary
FROM Departments AS d
JOIN Employees AS e ON d.DepartmentID = e.DepartmentID
GROUP BY d.[Name], d.DepartmentID
ORDER BY d.DepartmentID
-- Another shorther way 
SELECT e.DepartmentID, SUM(e.salary)
FROM Employees AS e
GROUP BY e.DepartmentID
ORDER BY DepartmentID;

SELECT e.DepartmentID, MIN(e.salary)
FROM Employees AS e
WHERE e.DepartmentID IN (2,5,7) AND HireDate > '01/01/2000'
GROUP BY e.DepartmentID
ORDER BY DepartmentID;


SELECT 
  *
INTO NewTable
FROM Employees
WHERE Salary > 30000

DELETE
From NewTable
WHERE ManagerID = 42

UPDATE NewTable
SET Salary += 5000
WHERE DepartmentID = 1;

SELECT 
DepartmentID,
AVG(Salary) AS AverageSalary
FROM NewTable
GROUP BY DepartmentID


SELECT 
DepartmentID,
MAX(Salary) 
FROM Employees
GROUP BY DepartmentID
HAVING NOT MAX(Salary) BETWEEN 30000 AND 70000


SELECT 
COUNT(*)
FROM Employees
WHERE ManagerID IS NULL

SELECT
k.DepartmentID,
k.Salary
FROM(
SELECT DISTINCT
DepartmentID,
Salary,
DENSE_RANK () OVER (PARTITION BY DepartmentID
		ORDER BY Salary DESC
	) price_rank 
FROM Employees
) AS K
WHERE k.price_rank = 3


SELECT TOP(10) 
e.FirstName, 
e.LastName, 
e.DepartmentID
FROM Employees AS e
WHERE e.Salary >(SELECT AVG(e2.Salary)
FROM Employees AS e2
WHERE e.DepartmentID = e2.DepartmentID)
ORDER BY e.DepartmentID