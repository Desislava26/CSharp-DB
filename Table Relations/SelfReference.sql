CREATE TABLE Teachers
(
TeacherID INT PRIMARY KEY NOT NULL IDENTITY(101,1),
[Name] VARCHAR(30),
ManegerID INT FOREIGN KEY REFERENCES Teachers(TeacherID)
)

INSERT INTO Teachers VALUES
('John',NULL),
('Maya',106),
('Silvia',106),
('Ted',105),
('Mark',101),
('Greta',101)
