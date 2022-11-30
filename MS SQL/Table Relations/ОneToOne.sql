CREATE TABLE Persons
(
PersonalId INT NOT NULL,
FirstName NVARCHAR(25),
Salary DECIMAL(8,2),
PassportID INT UNIQUE
)

 

CREATE TABLE Passports
(
PassportID INT NOT NULL UNIQUE,
PassportNumber NVARCHAR(25)
)

INSERT INTO Persons (PersonalId,FirstName,Salary,PassportID) VALUES
(1,'Roberto',43300.00,102),
(2,'Tom',56100.00,103),
(3,'Yana',60200.00,101)

 

INSERT INTO Passports(PassportID, PassportNumber) VALUES 
(101,'N34FG21B'),
(102,'K65LO4R7'),
(103,'ZE657QP2')

ALTER TABLE Persons

ADD CONSTRAINT pk_PersonID PRIMARY KEY (PersonalID)

ALTER TABLE Passports

ADD CONSTRAINT pk_PassportsID PRIMARY KEY ([PassportID])

 

ALTER TABLE Persons

ADD CONSTRAINT fk_Pesrons_Passports FOREIGN KEY([PassportID])

REFERENCES Passports([PassportID])
