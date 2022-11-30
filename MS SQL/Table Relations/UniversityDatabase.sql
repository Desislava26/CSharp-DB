CREATE TABLE Majors 
(
MajorID INT PRIMARY KEY IDENTITY,
[Name] VARCHAR(50)
)

CREATE TABLE Students 
(
StudentID INT PRIMARY KEY IDENTITY,
StudentNumber VARCHAR(50),
StudentName VARCHAR(50),
MajorID INT FOREIGN KEY REFERENCES Majors(MajorID)
)

CREATE TABLE Payments 
(
PaymentID INT PRIMARY KEY IDENTITY,
PaymentDate DATE,
PaymentAmount DECIMAL,
StudentID INT FOREIGN KEY REFERENCES Students(StudentID)
)

CREATE TABLE Subjects
(
SubjectID INT PRIMARY KEY IDENTITY,
SubjectName VARCHAR(30),
)

CREATE TABLE Agenda
(
StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
SubjectID INT FOREIGN KEY REFERENCES Subjects(SubjectID),
CONSTRAINT PK_Student_Subject PRIMARY KEY (StudentID, SubjectID)
)