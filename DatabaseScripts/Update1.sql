BEGIN TRANSACTION;

CREATE TABLE [ShiftType] (
    ShiftTypeID INT IDENTITY(1,1) PRIMARY KEY,
    ShiftTypeName NVARCHAR(400) NOT NULL
);

INSERT INTO [ShiftType] (Name) VALUES ('Regular shift');
INSERT INTO [ShiftType] (Name) VALUES ('Night shift');
INSERT INTO [ShiftType] (Name) VALUES ('Sunday or holiday');

ALTER TABLE [Task]
ADD ShiftTypeID INT;

ALTER TABLE [Task]
ADD CONSTRAINT FK_ConstraintShiftType
FOREIGN KEY (ShiftTypeID)
REFERENCES ShiftType (ShiftTypeID);

COMMIT TRANSACTION;

BEGIN TRANSACTION;

UPDATE [Task]
SET ShiftTypeID = 1
WHERE ShiftTypeID IS NULL;

COMMIT TRANSACTION;


