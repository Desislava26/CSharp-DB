CREATE DATABASE MinionsDB

USE MinionsDB

CREATE TABLE Countries
(Id INT PRIMARY KEY, Name VARCHAR(50))

CREATE TABLE Towns
(Id INT PRIMARY KEY, Name VARCHAR(50), 
CountryCode INT REFERENCES Countries(Id))

CREATE TABLE Minions
(Id INT PRIMARY KEY, Name VARCHAR(50), 
Age INT, 
TownId INT REFERENCES Towns(Id))

CREATE TABLE EvilnessFactors
(Id INT PRIMARY KEY, Name VARCHAR(50))

CREATE TABLE Villains
(Id INT PRIMARY KEY, Name VARCHAR(50), 
EvilnessFactorId INT REFERENCES EvilnessFactors(Id))

CREATE TABLE MinionsVillains
(MinionId INT REFERENCES Minions(Id), 
VillainId INT REFERENCES Villains(Id), 
PRIMARY KEY(MinionId, VillainId))


INSERT INTO Countries(Id, Name) 
VALUES (1, 'Bulgaria'), (2, 'Norway'), (3, 'Cyprus'), (4, 'Greece'), (5, 'UK')
INSERT INTO Towns(Id, Name, CountryCode) 
VALUES (1, 'Plovdiv', 1), (2, 'Oslo', 2), (3, 'Larnaka', 3), (4, 'Athens', 4), (5, 'London', 5)
INSERT INTO Minions(Id, Name, Age, TownId) 
VALUES (1, 'Stoyan', 12, 1), (2, 'George', 22, 2), (3, 'Ivan', 25, 3), (4, 'Kiro', 35, 4), (5, 'Niki', 25, 5)
INSERT INTO EvilnessFactors(Id, Name) 
VALUES (1, 'super good'), (2, 'good'), (3, 'bad'), (4, 'evil'),(5, 'super evil')
INSERT INTO Villains(Id, Name, EvilnessFactorId) 
VALUES (1, 'Gru', 1), (2, 'Ivo', 2), (3, 'Teo', 3), (4, 'Sto', 4), (5, 'Pro', 5)
INSERT INTO MinionsVillains 
VALUES (1,1), (2,2), (3,3), (4,4), (5,5), 
INSERT INTO MinionsVillains 
VALUES (1, 2), (3, 1), (1, 3), (4, 1), (3, 4), (1, 4), (1, 5), (5, 1)

SELECT
v.Name,
COUNT(mv.MinionId) as Counting
FROM Villains AS v
JOIN MinionsVillains AS mv ON mv.VillainId = v.Id
GROUP BY v.Id, v.Name
HAVING COUNT(mv.MinionId) > 3


SELECT Name FROM Villains
WHERE Id = 1
JOIN Minions

SELECT 
m.Name, 
m.Age
FROM Villains AS v
JOIN MinionsVillains AS mv
ON mv.VillainId = v.Id
JOIN Minions AS m
ON m.Id = mv.MinionId
WHERE v.Id = 1
ORDER BY m.Name

Select * from Towns as t
JOIN Countries AS c ON c.Id = t.CountryCode


Select 
t.Name
from Towns as t
JOIN Countries AS c ON c.Id = t.CountryCode
WHERE c.Name = @country

UPDATE Towns
SET Name = @newName
WHERE Name = @country

select*from towns where name = 'PLOVDIV'

SELECT * FROM Villains WHERE Id = 1;

SELECT 
v.Name,
COUNT(m.Name)
FROM Villains AS v
JOIN MinionsVillains AS mv
ON mv.VillainId = v.Id
JOIN Minions AS m
ON m.Id = mv.MinionId
WHERE v.Id = 1
GROUP BY v.Name

SELECT
Id,
Name,
Age
From Minions

UPDATE Minions SET Age = Age+1 WHERE Name= Stoyan

CREATE OR ALTER PROCEDURE usp_GetOlder @idMinion INT
AS
UPDATE Minions SET Age = Age+1 WHERE Id= @idMinion

EXEC dbo.usp_GetOlder 1