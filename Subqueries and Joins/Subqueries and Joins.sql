SELECT TOP (5) 
	e.EmployeeID, 
	e.JobTitle, 
	a.AddressID, 
	a.AddressText
FROM Employees AS e
     JOIN Addresses AS a ON e.AddressID=a.AddressID
ORDER BY a.AddressID


SELECT TOP (50)
e.FirstName,
e.LastName,
t.[Name] AS Town,
a.AddressText
FROM Employees AS e
JOIN Addresses AS a ON e.AddressID=a.AddressID
JOIN Towns AS t ON a.TownID = t.TownID
ORDER BY FirstName ASC, LastName ASC


SELECT 
e.EmployeeID,
e.FirstName,
e.LastName,
d.[Name] AS DepartmentName
FROM Employees AS e
JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
WHERE d.Name = 'Sales'



SELECT TOP (5)
e.EmployeeID,
e.FirstName,
e.Salary,
d.[Name] AS DepartmentName
FROM Employees AS e
JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
WHERE Salary > 15000
ORDER BY d.DepartmentID ASC


SELECT TOP (3)
e.EmployeeID,
e.FirstName
FROM Employees AS e
LEFT JOIN EmployeesProjects AS p ON e.EmployeeID = p.EmployeeID
WHERE p.EmployeeID IS NULL
ORDER BY e.EmployeeID ASC



SELECT
e.FirstName,
e.LastName,
e.HireDate,
d.[Name] AS DeptName
FROM Employees AS e
JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
WHERE e.HireDate > '1/1/1999' AND
d.Name = 'Sales'  OR d.Name = 'Finance'
ORDER BY e.HireDate ASC


SELECT TOP (5)
e.EmployeeID,
e.FirstName,
pr.[Name]
FROM Employees AS e
JOIN EmployeesProjects AS p ON e.EmployeeID = p.EmployeeID
JOIN Projects AS pr ON p.ProjectID = pr.ProjectID
WHERE pr.StartDate > '2002-08-13' AND pr.EndDate IS NULL
ORDER BY e.EmployeeID


SELECT e.EmployeeID,
       e.FirstName,
       CASE
           WHEN p.StartDate > '2005'
           THEN NULL
           ELSE p.Name
       END AS ProjectName
FROM Employees AS e
     JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
     JOIN Projects AS p ON ep.ProjectID = p.ProjectID
WHERE e.EmployeeID = 24; 



SELECT e.EmployeeID,
       e.FirstName,
       e.ManagerID,
       m.FirstName AS ManagerName
FROM Employees AS e
     JOIN Employees AS m ON e.ManagerID = m.EmployeeID
WHERE e.ManagerID IN(3, 7)
ORDER BY e.EmployeeID;



SELECT TOP (50)
       e.EmployeeID,
       e.FirstName+' '+e.LastName AS EmployeeName,
       m.FirstName +' '+ m.LastName AS ManagerName,
	   d.Name AS DepartmentName
FROM Employees AS e
     JOIN Employees AS m ON e.ManagerID = m.EmployeeID
	 JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
	 ORDER BY EmployeeID


SELECT MIN(a.AverageSalary)
FROM
(
    SELECT AVG(Salary) AS AverageSalary
    FROM Employees
    GROUP BY DepartmentID
) AS a; 


USE Geography



SELECT 
m.CountryCode,
mou.MountainRange,
p.PeakName,
p.Elevation
FROM Mountains AS mou
JOIN MountainsCountries AS m ON m.MountainId = mou.Id
JOIN Peaks AS p ON m.MountainId = p.MountainId
WHERE m.CountryCode = 'BG' AND p.Elevation > 2835
ORDER BY p.Elevation DESC




SELECT c.CountryCode, COUNT(*)
FROM Countries AS c
JOIN MountainsCountries AS mc on mc.CountryCode = c.CountryCode
WHERE c.CountryCode in ('US', 'RU', 'BG')
GROUP BY c.CountryCode



SELECT TOP (5)
c.CountryName,
riv.RiverName
FROM Countries as c
LEFT JOIN CountriesRivers AS r ON r.CountryCode = c.CountryCode
LEFT JOIN Rivers AS riv ON riv.Id = r.RiverId
WHERE c.ContinentCode = 'AF'
ORDER BY c.CountryName



SELECT
ranked.ContinentCode,
ranked.CurrencyCode,
ranked.[Count]
FROM(
SELECT  DENSE_RANK () 
            OVER ( 
			PARTITION BY c.ContinentCode
			ORDER BY COUNT(c.CurrencyCode) DESC) AS [rank] ,
c.ContinentCode,
c.CurrencyCode,
COUNT(c.CurrencyCode) AS [Count]
FROM Countries AS c
GROUP BY c.ContinentCode, c.CurrencyCode) AS ranked
WHERE ranked.[rank] = 1 and ranked.[Count] > 1



SELECT COUNT(*)
FROM Countries AS c
FULL JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
WHERE mc.MountainId IS NULL



SELECT TOP (5)
c.CountryName,
MAX(p.Elevation) AS HighestPeak,
MAX(r.Length) AS LongestRiver
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Mountains AS mou ON mou.Id = mc.MountainId
LEFT JOIN Peaks AS p ON p.MountainId = mou.Id
GROUP BY c.CountryName
ORDER BY HighestPeak DESC, LongestRiver DESC




SELECT TOP (5)
c.CountryName AS Country,
ISNULL(p.PeakName, '(no highest peak)') AS 'HighestPeakName',
ISNULL(MAX(p.Elevation), 0) AS HighestPeak,
ISNULL(mou.MountainRange, '(no mountain)')
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Mountains AS mou ON mou.Id = mc.MountainId
LEFT JOIN Peaks AS p ON p.MountainId = mou.Id
GROUP BY c.CountryName, p.PeakName, mou.MountainRange
ORDER BY c.CountryName, p.PeakName
