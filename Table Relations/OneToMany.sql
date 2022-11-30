
CREATE TABLE Manufacturers
(
ManufacturerID INT NOT NULL PRIMARY KEY IDENTITY,
[Name] NVARCHAR(25),
EstablishedOn DATE
)
CREATE TABLE Models
(
ModelID INT NOT NULL PRIMARY KEY IDENTITY(101,1),
[Name] NVARCHAR(25),
ManufacturerID INT NOT NULL FOREIGN KEY REFERENCES Manufacturers(ManufacturerID)

)


INSERT INTO Manufacturers([Name], EstablishedOn) VALUES
('BMW', '07/03/1916'),
('Tesla', '01/01/2003'),
('Lada', '01/05/1966')

UPDATE Manufacturers
SET EstablishedOn='07/03/1916'
WHERE [Name] = 'BMW';
UPDATE Manufacturers
SET EstablishedOn='01/01/2003'
WHERE [Name] = 'Tesla';
UPDATE Manufacturers
SET EstablishedOn='01/05/1966'
WHERE [Name] = 'Lada';

ALTER TABLE Manufacturers
ALTER COLUMN EstablishedOn DATE

INSERT INTO Models VALUES
('X1',1),
('i6',1),
('Model S',2),
('Model X',2),
('Model 3',2),
('Nova',3)


