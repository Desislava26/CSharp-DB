CREATE DATABASE Hotel
USE Hotel
CREATE TABLE Employess
(
   Id INT PRIMARY KEY,
   FirstName VARCHAR(90) NOT NULL,
   LastName VARCHAR(90) NOT NULL,
   Title VARCHAR(90) NOT NULL,
   Notes VARCHAR(MAX),
   
)

CREATE TABLE Customers
(
   AccountNumber INT PRIMARY KEY,
   FirstName VARCHAR(90) NOT NULL,
   LastName VARCHAR(90) NOT NULL,
   PhoneNumber VARCHAR(30) NOT NULL,
   EmergencyName VARCHAR(90) NOT NULL,
   EmergencyNumber VARCHAR(30) NOT NULL,
   Notes VARCHAR(MAX),
   
)

CREATE TABLE RoomStatus
(
   RoomStatus INT PRIMARY KEY,
   Notes VARCHAR(MAX),
)

CREATE TABLE RoomTypes
(
   RoomType INT PRIMARY KEY,
   Notes VARCHAR(MAX),
)

CREATE TABLE BedTypes
(
   BedType INT PRIMARY KEY,
   Notes VARCHAR(MAX),
)

CREATE TABLE Rooms
(
   RoomNumber INT PRIMARY KEY,
   RoomType VARCHAR(90) NOT NULL,
   BedType VARCHAR(30) NOT NULL,
   Rate INT,
   RoomStatus BIT NOT NULL,
   Notes VARCHAR(MAX),
)

CREATE TABLE Payments
(
   Id INT PRIMARY KEY,
   EmployeeId VARCHAR(90) NOT NULL,
   PaymentDate DATETIME NOT NULL,
   AccountNumber INT NOT NULL,
   FirstDateOccupied DATETIME NOT NULL,
   LastDateOccupied DATETIME NOT NULL,
   TotalDays INT NOT NULL,
   AmountCharged DECIMAL NOT NULL,
   TaxRate INT NOT NULL,
   TaxAmount INT NOT NULL,
   PaymentTotal DECIMAL NOT NULL,
   Notes VARCHAR(MAX),
)

CREATE TABLE Occupancies
(
   Id INT PRIMARY KEY,
   EmployeeId VARCHAR(90) NOT NULL,
   DateOccupied DATETIME NOT NULL,
   AccountNumber INT NOT NULL,
   RoomNumber INT NOT NULL,
   RateApplied INT,
   PhoneCharge DECIMAL,
   Notes VARCHAR(MAX),
)

INSERT INTO Employess (Id, FirstName, LastName, Title, Notes) 
VALUES
(1, 'Desi', 'Papalakova', 'zaglavie', NULL)

INSERT INTO Customers (AccountNumber, FirstName, LastName, PhoneNumber, EmergencyName, EmergencyNumber, Notes)
VALUES
(1, 'Desi', 'Papalakova', '123456', 'desito', '55454', NULL)

INSERT INTO RoomStatus (RoomStatus, Notes)
VALUES
(0, 'not null')

INSERT INTO RoomTypes (RoomType, Notes)
VALUES
(0, 'not null')

INSERT INTO BedTypes (BedType, Notes)
VALUES
(0, 'not null')



SELECT * FROM Employess