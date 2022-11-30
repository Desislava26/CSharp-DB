CREATE TABLE Users
(
   Id BIGINT PRIMARY KEY IDENTITY,
   Username VARCHAR(30) NOT NULL,
   [Password] VARCHAR(26) NOT NULL,
   ProfilePicture VARCHAR(MAX),
   LastLoginTime DATETIME,
   IsDeleted BIT,
)
INSERT INTO Users
(Username, [Password], ProfilePicture, LastLoginTime, IsDeleted)
VALUES
('Desi', 'parola123456', 'https://image.shutterstock.com/image-vector/vector-antonyms-opposites-cartoon-characters-260nw-1246528834.jpg', '1/1/2002', 0),
('Veni', 'parola123456', 'https://image.shutterstock.com/image-vector/vector-antonyms-opposites-cartoon-characters-260nw-1246528834.jpg', '2/2/2002', 0),
('Kiki', 'parola123456', 'https://image.shutterstock.com/image-vector/vector-antonyms-opposites-cartoon-characters-260nw-1246528834.jpg', '3/2/2002', 0),
('Miki', 'parola123456', 'https://image.shutterstock.com/image-vector/vector-antonyms-opposites-cartoon-characters-260nw-1246528834.jpg', '4/5/2002', 0),
('Siso', 'parola123456', 'https://image.shutterstock.com/image-vector/vector-antonyms-opposites-cartoon-characters-260nw-1246528834.jpg', '6/4/2002', 0)

SELECT * FROM Users


ALTER TABLE Users
DROP CONSTRAINT PK__Users__3214EC076FEFE223;

ALTER TABLE Users 
ADD CONSTRAINT PK_Users_Id PRIMARY KEY (Id, Username)

ALTER TABLE Users
ADD CONSTRAINT CH_PasswordAtLeast5Symbols CHECK (LEN([Password]) >5);

ALTER TABLE Users
ADD CONSTRAINT DF_LastLoginTime
DEFAULT GETDATE() FOR LastLoginTime;

ALTER TABLE Users
DROP CONSTRAINT PK_Users_Id;

ALTER TABLE Users 
ADD CONSTRAINT PK_Users_Id PRIMARY KEY (Id)

-- Testing to write a commend. The next one will be another database but from the another query
