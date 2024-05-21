--Create tables
BEGIN TRANSACTION;

CREATE TABLE [Role] (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE [User] (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(300) NOT NULL,
    Password NVARCHAR(500) NOT NULL,
    RoleID INT,
    CONSTRAINT FK_RoleID FOREIGN KEY (RoleID) REFERENCES Role(RoleID)
);


CREATE TABLE Job (
    JobID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Employee (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    JobID INT,
    CONSTRAINT FK_JobID FOREIGN KEY (JobID) REFERENCES Job(JobID)
);

CREATE TABLE Task (
    TaskID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeID INT,
    StartDateTime DATETIME,
    EndDateTime DATETIME,
    Description NVARCHAR(MAX),
    CONSTRAINT FK_EmployeeID FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);

CREATE TABLE Notice (
    NoticeID INT IDENTITY(1,1) PRIMARY KEY,
    StartDate DATE,
    EndDateTime DATE,
    NoticeText NVARCHAR(MAX) NOT NULL,
);

CREATE TABLE WorkingHours (
    DayOfWeek TINYINT PRIMARY KEY CHECK (DayOfWeek BETWEEN 1 AND 7),
    StartTime TIME,
    EndTime TIME
);

CREATE TABLE WorkingHoursChange (
    [Date] DATE PRIMARY KEY,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL
);

CREATE TABLE TicketCategory (
    TicketCategoryID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
	RegularPrice DECIMAL(5, 2) NOT NULL,
	Description NVARCHAR(MAX)
);

CREATE TABLE TicketPriceChange (
    TicketPriceChangeID INT IDENTITY(1,1) PRIMARY KEY,
	StartDate DATE NOT NULL,
    EndDateTime DATE NOT NULL,
	NewPrice DECIMAL(5, 2) NOT NULL,
	TicketCategoryID INT,
	CONSTRAINT FK_TicketPriceChange_Category FOREIGN KEY (TicketCategoryID) REFERENCES TicketCategory(TicketCategoryID)
);

CREATE TABLE Room (
    RoomID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
	Floor TINYINT NOT NULL,
	Description NVARCHAR(MAX)
);

CREATE TABLE MuseumItem (
    MuseumItemID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(300) NOT NULL,
	Description NVARCHAR(MAX),
	RoomID INT,
	Period NVARCHAR(300),
	MultimediaContentType NVARCHAR(300),
	CONSTRAINT FK_RoomID FOREIGN KEY (RoomID) REFERENCES Room(RoomID)
);


CREATE TABLE TicketLimit (
    [Date] DATE PRIMARY KEY,
    [Limit] INT NOT NULL
);

CREATE TABLE Reservation (
    ReservationID INT IDENTITY(1,1) PRIMARY KEY,
    VisitorFirstName NVARCHAR(300) NOT NULL,
	VisitorLastName NVARCHAR(300) NOT NULL,
	VisitorEmail NVARCHAR(300) NOT NULL,
	ReservationNumber NVARCHAR(300) NOT NULL,
	[Date] DATE NOT NULL,
	UNIQUE (ReservationNumber)
);

CREATE TABLE ReservationContent (
	ReservationID INT,
	CategoryID INT,
	[Count] INT,
	PRIMARY KEY (ReservationID, CategoryID),
    FOREIGN KEY (ReservationID) REFERENCES Reservation(ReservationID),
    FOREIGN KEY (CategoryID) REFERENCES TicketCategory(TicketCategoryID)
);

ALTER TABLE ReservationContent
ADD PriceOfSingleTicket DECIMAL(5, 2) NOT NULL;

COMMIT TRANSACTION;

--Insert sample data
BEGIN TRANSACTION;

-- Inserting data into the Job table
INSERT INTO Job (Name) VALUES ('Security guard');
INSERT INTO Job (Name) VALUES ('Ticket office worker');
INSERT INTO Job (Name) VALUES ('Guide');

-- Inserting data into the Role table
INSERT INTO [Role] (Name) VALUES ('Schedule maker');
INSERT INTO [Role] (Name) VALUES ('Marketing');
INSERT INTO [Role] (Name) VALUES ('Content administrator');
INSERT INTO [Role] (Name) VALUES ('Administrator');

-- Inserting data into the [User] table
INSERT INTO [User] (Email, Password, RoleID) VALUES ('admin@example.com', 'adminpassword', 4);
INSERT INTO [User] (Email, Password, RoleID) VALUES ('ivan@example.com', '12345678', 2);
INSERT INTO [User] (Email, Password, RoleID) VALUES ('frane@example.com', '12345678', 1);
INSERT INTO [User] (Email, Password, RoleID) VALUES ('marko@example.com', '12345678', 3);

-- Inserting data into the Employee table
INSERT INTO Employee (FirstName, LastName, Email, JobID) VALUES ('Ivo', 'Ivic', 'ivo.ivic@example.com', 1);
INSERT INTO Employee (FirstName, LastName, Email, JobID) VALUES ('Marko', 'Maric', 'maki@example.com', 1);
INSERT INTO Employee (FirstName, LastName, Email, JobID) VALUES ('Iva', 'Ivic', 'iva@example.com', 2);
INSERT INTO Employee (FirstName, LastName, Email, JobID) VALUES ('Tanja', 'T.', 'tanja@example.com', 3);

-- Inserting data into the Task table
INSERT INTO Task (EmployeeID, StartDateTime, EndDateTime, Description) VALUES (1, '2024-04-14 09:00:00', '2024-04-14 12:00:00', 'Prepare museum for opening');
INSERT INTO Task (EmployeeID, StartDateTime, EndDateTime, Description) VALUES (2, '2024-04-14 10:00:00', '2024-04-14 11:30:00', 'Curate new exhibit');
INSERT INTO Task (EmployeeID, StartDateTime, EndDateTime, Description) VALUES (3, '2024-04-14 12:00:00', '2024-04-14 16:00:00', 'Lead museum tour');

-- Inserting data into the Notice table
INSERT INTO Notice (StartDate, EndDateTime, NoticeText) VALUES ('2024-04-14', '2024-04-21', 'The museum will be closed for maintenance from April 14th to April 21st.');

-- Inserting data into the WorkingHours table
INSERT INTO WorkingHours (DayOfWeek, StartTime, EndTime)
VALUES (1, NULL, NULL), -- Monday
       (2, '09:00:00', '17:00:00'), -- Tuesday
       (3, '09:00:00', '17:00:00'), -- Wednesday
       (4, '09:00:00', '17:00:00'), -- Thursday
       (5, '09:00:00', '17:00:00'), -- Friday
       (6, '09:00:00', '18:00:00'), -- Saturday
       (7, '09:00:00', '18:00:00'); -- Sunday

-- Inserting data into the TicketCategory table
INSERT INTO TicketCategory (Name, RegularPrice, Description) VALUES ('Adult', 15.00, 'Ticket for adults');
INSERT INTO TicketCategory (Name, RegularPrice, Description) VALUES ('Child', 5.00, 'Ticket for children under 12');
INSERT INTO TicketCategory (Name, RegularPrice, Description) VALUES ('Student', 12.00, 'Ticket for students with valid ID');

-- Inserting data into the Room table
INSERT INTO Room (Name, Floor, Description) VALUES ('Gallery 1', 1, 'Modern art collection');
INSERT INTO Room (Name, Floor, Description) VALUES ('Gallery 2', 2, 'Historical artifacts');
INSERT INTO Room (Name, Floor, Description) VALUES ('Exhibition Hall', 1, 'Temporary exhibits');

-- Inserting data into the MuseumItem table
INSERT INTO MuseumItem (Name, Description, RoomID, Period, MultimediaContentType) VALUES ('Painting 1', 'Abstract painting', 1, '19th century', NULL);
INSERT INTO MuseumItem (Name, Description, RoomID, Period, MultimediaContentType) VALUES ('Artifact 1', 'Ancient vase', 2, 'Antique', NULL);
INSERT INTO MuseumItem (Name, Description, RoomID, Period, MultimediaContentType) VALUES ('Sculpture 1', 'Bronze sculpture', 3, 'Renaissance', NULL);
INSERT INTO MuseumItem (Name, Description, RoomID, Period, MultimediaContentType) VALUES ('Video with about painting in Ancient Greece', 'Press play to start interaction', 3, NULL, 'Video');


-- Inserting data into the TicketLimit table
INSERT INTO TicketLimit ([Date], [Limit]) VALUES ('2024-04-14', 200);
INSERT INTO TicketLimit ([Date], [Limit]) VALUES ('2024-04-15', 250);


-- Inserting data into the Reservation table
INSERT INTO Reservation (VisitorFirstName, VisitorLastName, VisitorEmail, ReservationNumber, [Date]) VALUES ('Alice', 'Johnson', 'alice.johnson@example.com', 'RES001', '2024-04-16');
INSERT INTO Reservation (VisitorFirstName, VisitorLastName, VisitorEmail, ReservationNumber, [Date]) VALUES ('Bob', 'Smith', 'bob.smith@example.com', 'RES002', '2024-04-18');

-- Inserting data into the ReservationContent table
INSERT INTO ReservationContent (ReservationID, CategoryID, [Count], PriceOfSingleTicket) VALUES (1, 1, 2, 10.00);
INSERT INTO ReservationContent (ReservationID, CategoryID, [Count], PriceOfSingleTicket) VALUES (1, 2, 1, 5.00);
INSERT INTO ReservationContent (ReservationID, CategoryID, [Count], PriceOfSingleTicket) VALUES (2, 1, 1, 15.00);

COMMIT TRANSACTION;
