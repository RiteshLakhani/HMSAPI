---Patient---
Create TABLE Patient (
    PatientID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    PatientName VARCHAR(255) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Age INT NOT NULL,
    Phone VARCHAR(15) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Gender VARCHAR(10) NOT NULL,
    Address TEXT NOT NULL,
    IsConfirmed BIT DEFAULT 0
);

select * from Patient

select * from Doctor
select * from Appointment
---Doctor----
CREATE TABLE Doctor (
    DoctorID INT IDENTITY(1,1) PRIMARY KEY,
    DoctorName VARCHAR(100) NOT NULL,
    Gender VARCHAR(10) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Age INT NOT NULL,
    Specialization VARCHAR(50) NOT NULL,
    Experience INT NOT NULL,
    DoctorDetail VARCHAR(250),
    Address VARCHAR(255),
    ContactNumber VARCHAR(15),
    Email VARCHAR(100),
	IsConfirmed BIT DEFAULT 0
);

ALTER TABLE Doctor
ADD ImagePath VARCHAR(255);

sp_help 'Doctor'

ALTER TABLE Doctor
DROP COLUMN DoctorID;

ALTER TABLE Doctor
ADD DoctorID INT IDENTITY(1,1) PRIMARY KEY;

-- Step 1: Drop Foreign Key Constraints
ALTER TABLE Payment DROP CONSTRAINT FK__Payment__DoctorI__693CA210;
ALTER TABLE Appointment DROP CONSTRAINT FK__Appointme__Docto__5535A963;
ALTER TABLE Room DROP CONSTRAINT FK__Room__DoctorID__4BAC3F29;

-- Step 2: Drop the Primary Key Constraint
ALTER TABLE Doctor DROP CONSTRAINT PK__Doctor__2DC00EDF5145B165;

-- Step 3: Drop the DoctorID Column
ALTER TABLE Doctor DROP COLUMN DoctorID;

-- Step 4: Recreate DoctorID as Identity Column
ALTER TABLE Doctor
ADD DoctorID INT IDENTITY(1,1) PRIMARY KEY;

-- Step 5: Recreate Foreign Key Constraints
ALTER TABLE Payment ADD CONSTRAINT FK_Payment_DoctorID FOREIGN KEY (DoctorID) REFERENCES Doctor(DoctorID);
ALTER TABLE Appointment ADD CONSTRAINT FK_Appointment_DoctorID FOREIGN KEY (DoctorID) REFERENCES Doctor(DoctorID);
ALTER TABLE Room ADD CONSTRAINT FK_Room_DoctorID FOREIGN KEY (DoctorID) REFERENCES Doctor(DoctorID);


---Apppointment----
CREATE TABLE Appointment (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incremented primary key
    PatientID INT NOT NULL, -- Foreign key referencing the Patient table
    DoctorID INT NOT NULL, -- Foreign key referencing the Doctor table
    AppointmentDate DATE NOT NULL,
    AppointmentTime TIME NOT NULL,
    TokenNumber AS (CONCAT(YEAR(AppointmentDate), RIGHT('00' + CAST(MONTH(AppointmentDate) AS VARCHAR), 2), '-', AppointmentID)) PERSISTED,
    Problem VARCHAR(255) NOT NULL, -- Problem described by the patient
    Status VARCHAR(50),
    IsConfirmed BIT DEFAULT 0,
    FOREIGN KEY (PatientID) REFERENCES Patient(PatientID), -- Foreign key to Patient table
    FOREIGN KEY (DoctorID) REFERENCES Doctor(DoctorID) -- Foreign key to Doctor table
);

---Payment-----
CREATE TABLE Payment (
    PaymentID INT IDENTITY(1,1) PRIMARY KEY, 
    PatientID INT NOT NULL, 
    DoctorID INT NOT NULL,
    Department VARCHAR(100) NOT NULL, 
    ServiceName VARCHAR(255) NOT NULL, 
    CostOfTreatment DECIMAL(10, 2) NOT NULL,
    AdmissionDate DATE, 
    DischargeDate DATE, 
    AdvancedPaid DECIMAL(10, 2), 
    Discount DECIMAL(10, 2),
    Amount DECIMAL (10,2),
    PaymentDate DATE NOT NULL, -- Payment date
    PaymentMethod VARCHAR(20) NOT NULL, -- Payment method (e.g., Cash, Card, etc.)
    PaymentType VARCHAR(50), -- Type of payment (e.g., Full Payment, Partial Payment)
    CardOrCheckNo VARCHAR(50), -- Card or check number if payment is not cash
    PaymentStatus VARCHAR(20) NOT NULL, -- Payment status (e.g., Paid, Pending)
    FOREIGN KEY (PatientID) REFERENCES Patient(PatientID),
    FOREIGN KEY (DoctorID) REFERENCES Doctor(DoctorID),
	IsConfirmed BIT DEFAULT 0 
);
select * from Payment
drop table Payment

---Room----
CREATE TABLE Room (
    RoomID INT IDENTITY(1,1) PRIMARY KEY,
    RoomNumber INT NOT NULL,         
    RoomType VARCHAR(50) NOT NULL,   
    PatientID INT NOT NULL,         
    AllotmentDate DATE NOT NULL,    
    DischargeDate DATE NOT NULL,    
    DoctorID INT NOT NULL,        
    IsConfirmed BIT NOT NULL DEFAULT 0, 
    FOREIGN KEY (PatientID) REFERENCES Patient(PatientID),
    FOREIGN KEY (DoctorID) REFERENCES Doctor(DoctorID)
);

select * from Room

------------Insert Data-------------------

--PatientData--
INSERT INTO Patient (PatientName, DateOfBirth, Age, Phone, Email, Gender, Address, IsConfirmed)
VALUES
('John Doe', '1985-06-15', 39, '1234567890', 'john.doe@example.com', 'Male', '123 Main Street, City A', 1),
('Jane Smith', '1992-03-22', 32, '9876543210', 'jane.smith@example.com', 'Female', '456 Elm Street, City B', 1),
('Robert Brown', '1978-11-05', 46, '5556667777', 'robert.brown@example.com', 'Male', '789 Pine Avenue, City C', 0),
('Emily Davis', '2000-08-12', 24, '4443332222', 'emily.davis@example.com', 'Female', '321 Oak Lane, City D', 1),
('Michael Johnson', '1995-01-30', 29, '1112223333', 'michael.johnson@example.com', 'Male', '654 Maple Road, City E', 0),
('Ritesh Lakhani', '1995-01-30', 29, '1112223333', 'michael.johnson@example.com', 'Male', '654 Maple Road, City E', 1),
('Olivia Green', '1987-11-10', 37, '6665554444', 'olivia.green@example.com', 'Female', '987 Birch Street, City F', 1),
('Liam White', '1990-04-20', 34, '2223334444', 'liam.white@example.com', 'Male', '123 Cedar Street, City G', 1),
('Sophia Martinez', '1989-02-18', 35, '5557778888', 'sophia.martinez@example.com', 'Female', '567 Walnut Lane, City H', 0),
('James Wilson', '1975-09-25', 49, '3334445555', 'james.wilson@example.com', 'Male', '234 Cherry Street, City I', 1);

--DocotrData---
DELETE FROM Doctor;
DBCC CHECKIDENT ('Doctor', RESEED, 0);
ALTER TABLE Doctor AUTO_INCREMENT = 1;



-- Insert 10 new records into the Doctor table
INSERT INTO Doctor (DoctorName, Gender, DateOfBirth, Age, Specialization, Experience, DoctorDetail, Address, ContactNumber, Email, IsConfirmed)
VALUES
('Dr. John Smith', 'Male', '1980-06-15', 44, 'Cardiology', 15, 'Experienced cardiologist specializing in heart disease treatment', '123 Heart Street, City J', '1234567890', 'john.smith@hospital.com', 1),
('Dr. Emily Johnson', 'Female', '1985-03-22', 39, 'Neurology', 12, 'Expert in neurological disorders and brain surgery', '456 Brain Avenue, City K', '9876543210', 'emily.johnson@hospital.com', 1),
('Dr. Robert Brown', 'Male', '1975-08-10', 49, 'Orthopedics', 20, 'Skilled in bone and joint surgeries', '789 Bone Road, City L', '5556667777', 'robert.brown@hospital.com', 0),
('Dr. Sarah Davis', 'Female', '1990-11-05', 34, 'Pediatrics', 8, 'Caring pediatrician focusing on children’s health', '321 Child Lane, City M', '4443332222', 'sarah.davis@hospital.com', 1),
('Dr. Michael Wilson', 'Male', '1982-12-01', 42, 'General Surgery', 18, 'Expert surgeon with experience in various surgeries', '654 Health Road, City N', '1112223333', 'michael.wilson@hospital.com', 0),
('Dr. Olivia Green', 'Female', '1988-05-22', 36, 'Dermatology', 10, 'Specializes in skin conditions and cosmetic procedures', '987 Skin Blvd, City O', '6665554444', 'olivia.green@hospital.com', 1),
('Dr. James White', 'Male', '1992-04-18', 32, 'Psychiatry', 6, 'Psychiatrist with expertise in mental health treatments', '123 Mind Avenue, City P', '2223334444', 'james.white@hospital.com', 0),
('Dr. Laura Martinez', 'Female', '1983-07-30', 41, 'Obstetrics and Gynecology', 14, 'Experienced in pregnancy and women’s health care', '567 Mother Road, City Q', '5557778888', 'laura.martinez@hospital.com', 1),
('Dr. William Harris', 'Male', '1976-09-25', 48, 'Anesthesiology', 22, 'Anesthesiologist specializing in pain management', '234 Anesthesia St, City R', '3334445555', 'william.harris@hospital.com', 1),
('Dr. Ava Clark', 'Female', '1994-01-12', 30, 'Ophthalmology', 5, 'Specializes in eye care and surgeries', '678 Vision Drive, City S', '7778889999', 'ava.clark@hospital.com', 1);


UPDATE Doctor
SET ImagePath = 'https://www.advinohealthcare.com/wp-content/uploads/2020/08/shutterstock_155685458.jpg'
WHERE DoctorName = 'Dr. John Smith';

UPDATE Doctor
SET ImagePath = 'https://wallpapers.com/images/hd/doctor-pictures-l5y1qs2998u7rf0x.jpg'
WHERE DoctorName = 'Dr. Emily Johnson';

UPDATE Doctor
SET ImagePath = 'https://cdn.siasat.com/wp-content/uploads/2023/04/Dr-Sudhir-Kumar.png'
WHERE DoctorName = 'Dr. Robert Brown';

UPDATE Doctor
SET ImagePath = 'https://th.bing.com/th/id/OIP.P1IfJNdtz7GmKkfPqR2yNAHaIO?rs=1&pid=ImgDetMain'
WHERE DoctorName = 'Dr. Sarah Davis';


5.UPDATE Doctor
SET ImagePath = 'https://th.bing.com/th/id/OIP.iVs3gb0RXSckBSdQL_mF_wHaF8?rs=1&pid=ImgDetMain'
WHERE DoctorName = 'Dr. Michael Wilson';

6.UPDATE Doctor
SET ImagePath = 'https://static.vecteezy.com/system/resources/thumbnails/028/287/555/small_2x/an-indian-young-female-doctor-isolated-on-green-ai-generated-photo.jpg'
WHERE DoctorName = 'Dr. Olivia Green';

7.UPDATE Doctor
SET ImagePath = 'https://static.vecteezy.com/system/resources/previews/028/287/384/non_2x/a-mature-indian-male-doctor-on-a-white-background-ai-generated-photo.jpg'
WHERE DoctorName = 'Dr. James White';

8.UPDATE Doctor
SET ImagePath = 'https://leman-clinic.ch/wp-content/uploads/2018/11/02.jpg'
WHERE DoctorName = 'Dr. Laura Martinez';

9.UPDATE Doctor
SET ImagePath = 'https://png.pngtree.com/png-clipart/20231002/original/pngtree-young-afro-professional-doctor-png-image_13227671.png'
WHERE DoctorName = 'Dr. William Harris';


10.UPDATE Doctor
SET ImagePath = 'https://thumbs.dreamstime.com/b/female-doctor-23301058.jpg'
WHERE DoctorName = 'Dr. Ava Clark';

UPDATE Doctor
SET ImagePath = 'https://img.freepik.com/free-photo/doctor-with-his-arms-crossed-white-background_1368-5790.jpg'
WHERE DoctorName = 'Harsh'


---AppoitmentData---
INSERT INTO Appointment (PatientID, DoctorID, AppointmentDate, AppointmentTime, Problem, Status, IsConfirmed)
VALUES
(1, 1, '2024-12-01', '09:00:00', 'Headache', 'Scheduled', 1),
(2, 2, '2024-12-02', '10:30:00', 'Back Pain', 'Scheduled', 0),
(3, 3, '2024-12-03', '11:00:00', 'Stomach Pain', 'Scheduled', 1),
(4, 1, '2024-12-04', '14:00:00', 'Cold and Cough', 'Completed', 1),
(5, 4, '2024-12-05', '15:30:00', 'Fever', 'Canceled', 0),
(6, 5, '2024-12-06', '16:00:00', 'Skin Rash', 'Scheduled', 1),
(7, 2, '2024-12-07', '08:30:00', 'Joint Pain', 'Completed', 1),
(8, 3, '2024-12-08', '17:00:00', 'Allergy', 'Scheduled', 0),
(9, 4, '2024-12-09', '12:00:00', 'Migraine', 'Scheduled', 1),
(10, 5, '2024-12-10', '13:30:00', 'Diabetes Checkup', 'Completed', 1);

select * from Appointment

---PaymentData-----
-- Insert 10 fake records into the Payment table
INSERT INTO Payment (PatientID, DoctorID, Department, ServiceName, CostOfTreatment, AdmissionDate, DischargeDate, AdvancedPaid, Discount, PaymentDate, PaymentMethod, PaymentType, CardOrCheckNo, PaymentStatus,IsConfirmed)
VALUES
(1, 1, 'Cardiology', 'Heart Surgery', 2000.00, '2024-12-01', '2024-12-10', 500.00, 100.00, '2024-12-10', 'Card', 'Full Payment', NULL, 'Paid',0),
(2, 2, 'Neurology', 'Brain Surgery', 3000.00, '2024-12-02', '2024-12-11', 1000.00, 150.00, '2024-12-11', 'Cash', 'Partial Payment', NULL, 'Paid',1),
(3, 3, 'Orthopedics', 'Fracture Treatment', 1500.00, '2024-12-05', '2024-12-15', 300.00, 50.00, '2024-12-15', 'Card', 'Full Payment', NULL, 'Paid',1),
(4, 4, 'Pediatrics', 'Vaccination', 200.00, '2024-12-06', '2024-12-06', 50.00, 0.00, '2024-12-06', 'Cash', 'Full Payment', NULL, 'Paid',1),
(5, 5, 'Dermatology', 'Skin Treatment', 800.00, '2024-12-07', '2024-12-10', 200.00, 0.00, '2024-12-10', 'Card', 'Partial Payment', NULL, 'Pending',0),
(6, 6, 'Gynecology', 'Pregnancy Checkup', 500.00, '2024-12-08', '2024-12-12', 100.00, 25.00, '2024-12-12', 'Cash', 'Full Payment', NULL, 'Paid',0),
(7, 7, 'Psychiatry', 'Mental Health Consultation', 250.00, '2024-12-09', '2024-12-09', 50.00, 10.00, '2024-12-09', 'Card', 'Partial Payment', '1234567890', 'Pending',0),
(8, 8, 'Ophthalmology', 'Eye Checkup', 150.00, '2024-12-10', '2024-12-10', 0.00, 0.00, '2024-12-10', 'Cash', 'Full Payment', NULL, 'Paid',1),
(9, 9, 'Endocrinology', 'Hormonal Imbalance Treatment', 1200.00, '2024-12-12', '2024-12-20', 300.00, 50.00, '2024-12-20', 'Card', 'Partial Payment', '9876543210', 'Paid',1),
(10, 10, 'Orthopedics', 'Knee Surgery', 3500.00, '2024-12-14', '2024-12-21', 1000.00, 200.00, '2024-12-21', 'Cash', 'Full Payment', NULL, 'Paid',0);

Select * from Payment
----RoomData---
INSERT INTO Room (RoomNumber, RoomType, PatientID, AllotmentDate, DischargeDate, DoctorID, IsConfirmed)
VALUES
(101, 'Single', 1, '2024-12-01', '2024-12-10', 1, 1),
(102, 'Double', 2, '2024-12-02', '2024-12-11', 2, 1),
(103, 'General', 3, '2024-12-05', '2024-12-15', 3, 0),
(104, 'Single', 4, '2024-12-06', '2024-12-16', 4, 1),
(105, 'Double', 5, '2024-12-07', '2024-12-17', 5, 0),
(106, 'Single', 6, '2024-12-08', '2024-12-18', 6, 1),
(107, 'General', 7, '2024-12-09', '2024-12-19', 7, 1),
(108, 'Single', 8, '2024-12-10', '2024-12-20', 8, 0),
(109, 'Double', 9, '2024-12-11', '2024-12-21', 9, 1),
(110, 'General', 10, '2024-12-12', '2024-12-22', 10, 0);

select * from Room


-------------Procedure-------------

---Patient Procedure---

----Select By All-------
CREATE PROCEDURE [dbo].[PR_Patient_SelectAll]
AS
BEGIN
    SELECT 
        [PatientID],
        [PatientName],
        [DateOfBirth],
        [Age],
        [Phone],
        [Email],
        [Gender],
        [Address],
        [IsConfirmed]
    FROM 
        [dbo].[Patient];
END;


-----SELECT BY PK--------
CREATE PROCEDURE [dbo].[PR_Patient_SelectByPK]
    @PatientID INT
AS
BEGIN
    SELECT 
        [PatientID],
        [PatientName],
        [DateOfBirth],
        [Age],
        [Phone],
        [Email],
        [Gender],
        [Address],
        [IsConfirmed]
    FROM 
        [dbo].[Patient]
    WHERE 
        [PatientID] = @PatientID;
END;

----Insert-----
Create PROCEDURE PR_Patient_Insert
@PatientName NVARCHAR(100),
@DateOfBirth DATE,
@Age INT,
@Phone NVARCHAR(15),
@Email NVARCHAR(100),
@Gender NVARCHAR(10),
@Address NVARCHAR(255),
@IsConfirmed BIT
AS
BEGIN
    INSERT INTO Patient (PatientName, DateOfBirth, Age, Phone, Email, Gender, Address, IsConfirmed)
    VALUES (@PatientName, @DateOfBirth, @Age, @Phone, @Email, @Gender, @Address, @IsConfirmed)
END

---Update--
CREATE PROCEDURE [dbo].[PR_Patient_Update]
    @PatientID INT,
    @PatientName NVARCHAR(255),
    @DateOfBirth DATE,
    @Age INT,
    @Phone NVARCHAR(15),
    @Email NVARCHAR(255),
    @Gender NVARCHAR(10),
    @Address NVARCHAR(MAX),
    @IsConfirmed BIT
AS
BEGIN
    UPDATE [dbo].[Patient]
    SET 
        [PatientName] = @PatientName,
        [DateOfBirth] = @DateOfBirth,
        [Age] = @Age,
        [Phone] = @Phone,
        [Email] = @Email,
        [Gender] = @Gender,
        [Address] = @Address,
        [IsConfirmed] = @IsConfirmed
    WHERE 
        [PatientID] = @PatientID;
END;


----Delete---
CREATE PROCEDURE [dbo].[PR_Patient_Delete]
    @PatientID INT
AS
BEGIN
    DELETE FROM [dbo].[Patient]
    WHERE 
        [PatientID] = @PatientID;
END;

---Patient DropDown---
CREATE PROCEDURE [dbo].[PR_Patient_Dropdown]
AS
BEGIN
    SELECT 
        PatientID,
        PatientName
    FROM 
        dbo.Patient
END;



-------Doctor Procedure------
select * from Doctor
--Select All--
ALTER PROCEDURE [dbo].[PR_Doctor_SelectAll]
AS
BEGIN
    SELECT 
        DoctorID, DoctorName, Gender, DateOfBirth, Age, Specialization, Experience, DoctorDetail, Address, ContactNumber, Email, IsConfirmed, ImagePath
    FROM 
        dbo.Doctor;
END;

--Select By PK--
Alter PROCEDURE [dbo].[PR_Doctor_SelectByPK]
    @DoctorID INT
AS
BEGIN
    SELECT 
        DoctorID, DoctorName, Gender, DateOfBirth, Age, Specialization, Experience, DoctorDetail, Address, ContactNumber, Email, IsConfirmed, ImagePath
    FROM 
        dbo.Doctor
    WHERE 
        DoctorID = @DoctorID;
END;


--Insert---
Alter PROCEDURE [dbo].[PR_Doctor_Insert]
    @DoctorName VARCHAR(100),
    @Gender VARCHAR(10),
    @DateOfBirth DATE,
    @Age INT,
    @Specialization VARCHAR(50),
    @Experience INT,
    @DoctorDetail VARCHAR(250),
    @Address VARCHAR(255),
    @ContactNumber VARCHAR(15),
    @Email VARCHAR(100),
    @IsConfirmed BIT,
    @ImagePath VARCHAR(255)
AS
BEGIN
    INSERT INTO dbo.Doctor (DoctorName, Gender, DateOfBirth, Age, Specialization, Experience, DoctorDetail, Address, ContactNumber, Email, IsConfirmed, ImagePath)
    VALUES
    (
        @DoctorName,
        @Gender,
        @DateOfBirth,
        @Age,
        @Specialization,
        @Experience,
        @DoctorDetail,
        @Address,
        @ContactNumber,
        @Email,
        @IsConfirmed,
        @ImagePath
    );
END;

--UPDATE--
ALTER PROCEDURE [dbo].[PR_Doctor_Update]
    @DoctorID INT,
    @DoctorName VARCHAR(100),
    @Gender VARCHAR(10),
    @DateOfBirth DATE,
    @Age INT,
    @Specialization VARCHAR(50),
    @Experience INT,
    @DoctorDetail VARCHAR(250),
    @Address VARCHAR(255),
    @ContactNumber VARCHAR(15),
    @Email VARCHAR(100),
    @IsConfirmed BIT,
    @ImagePath VARCHAR(255)
AS
BEGIN
    UPDATE dbo.Doctor
    SET 
        DoctorName = @DoctorName,
        Gender = @Gender,
        DateOfBirth = @DateOfBirth,
        Age = @Age,
        Specialization = @Specialization,
        Experience = @Experience,
        DoctorDetail = @DoctorDetail,
        Address = @Address,
        ContactNumber = @ContactNumber,
        Email = @Email,
        IsConfirmed = @IsConfirmed,
        ImagePath = @ImagePath
    WHERE 
        DoctorID = @DoctorID;
END;


--DELETE--
ALTER PROCEDURE [dbo].[PR_Doctor_Delete]
    @DoctorID INT
AS
BEGIN
    DELETE FROM dbo.Doctor
    WHERE DoctorID = @DoctorID;
END;


---DOCTOR DROPDOWN--
ALTER PROCEDURE [dbo].[PR_Doctor_Dropdown]
AS
BEGIN
    SELECT 
        DoctorID,
        DoctorName
    FROM 
        dbo.Doctor
END;


-----Appoitment Procedure----
Select * from Appointment
drop procedure PR_Appointment_Insert
--Select All--
ALTER PROCEDURE [dbo].[PR_Appointment_SelectAll]
    @PatientName NVARCHAR(100) = NULL,
    @DoctorName NVARCHAR(100) = NULL,
    @AppointmentDate DATE = NULL,
    @TokenNumber NVARCHAR(50) = NULL,
    @Status NVARCHAR(50) = NULL
AS
BEGIN
    SELECT 
        A.AppointmentID,
        A.PatientID,
        P.PatientName,
        A.DoctorID,
        D.DoctorName,
        A.AppointmentDate,
        A.AppointmentTime,
        A.TokenNumber,
        A.Problem,
        A.Status,
        A.IsConfirmed
    FROM 
        dbo.Appointment A
    INNER JOIN 
        dbo.Patient P ON A.PatientID = P.PatientID
    INNER JOIN 
        dbo.Doctor D ON A.DoctorID = D.DoctorID
    WHERE 
        (@PatientName IS NULL OR P.PatientName LIKE '%' + @PatientName + '%')
        AND (@DoctorName IS NULL OR D.DoctorName LIKE '%' + @DoctorName + '%')
        AND (@AppointmentDate IS NULL OR A.AppointmentDate = @AppointmentDate)
        AND (@TokenNumber IS NULL OR A.TokenNumber = @TokenNumber)
        AND (@Status IS NULL OR A.Status = @Status);
END;



--SELECT BY PK--
CREATE PROCEDURE [dbo].[PR_Appointment_SelectByPK]
    @AppointmentID INT
AS
BEGIN
    SELECT 
        AppointmentID,
        PatientID,
        DoctorID,
        AppointmentDate,
        AppointmentTime,
        TokenNumber,
        Problem,
        Status,
        IsConfirmed
    FROM 
        dbo.Appointment
    WHERE 
        AppointmentID = @AppointmentID;
END;

--INSERT--
CREATE PROCEDURE [dbo].[PR_Appointment_Insert]
    @PatientID INT,
    @DoctorID INT,
    @AppointmentDate DATE,
    @AppointmentTime TIME,
    @Problem VARCHAR(255),
    @Status VARCHAR(50),
    @IsConfirmed BIT
AS
BEGIN
    -- Insert into the Appointment table
    INSERT INTO dbo.Appointment
    (
        PatientID,
        DoctorID,
        AppointmentDate,
        AppointmentTime,
        Problem,
        Status,
        IsConfirmed
    )
    VALUES
    (
        @PatientID,
        @DoctorID,
        @AppointmentDate,
        @AppointmentTime,
        @Problem,
        @Status,
        @IsConfirmed
    );
END;

EXEC dbo.PR_Appointment_Insert
    @PatientID = 1,         
    @DoctorID = 2,          
    @AppointmentDate = '2024-12-25', 
    @AppointmentTime = '14:00',
    @Problem = 'General Checkup',     
    @Status = 'Scheduled',            
    @IsConfirmed = 1;                 

select * from Appointment


--UPDATE--
CREATE PROCEDURE [dbo].[PR_Appointment_Update]
    @AppointmentID INT,
    @PatientID INT,
    @DoctorID INT,
    @AppointmentDate DATE,
    @AppointmentTime TIME,
    @Problem VARCHAR(255),
    @Status VARCHAR(50),
    @IsConfirmed BIT
AS
BEGIN
    UPDATE dbo.Appointment
    SET 
        PatientID = @PatientID,
        DoctorID = @DoctorID,
        AppointmentDate = @AppointmentDate,
        AppointmentTime = @AppointmentTime,
        Problem = @Problem,
        Status = @Status,
        IsConfirmed = @IsConfirmed
    WHERE 
        AppointmentID = @AppointmentID;
END;


--DELETE--
CREATE PROCEDURE [dbo].[PR_Appointment_Delete]
    @AppointmentID INT
AS
BEGIN
    DELETE FROM dbo.Appointment
    WHERE AppointmentID = @AppointmentID;
END;

-------PAYMENT PROCEDURE-------
select * from Payment
drop procedure PR_Payment_Delete
--SELECT BY ALL---
ALTER PROCEDURE [dbo].[PR_Payment_SelectAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.PaymentID,
        P.PatientID,
        COALESCE(PT.PatientName, 'N/A') AS PatientName, -- Handle NULL PatientName
        P.DoctorID,
        COALESCE(D.DoctorName, 'N/A') AS DoctorName, -- Handle NULL DoctorName
        P.Department,
        P.ServiceName,
        P.CostOfTreatment,
        P.AdmissionDate,
        P.DischargeDate,
        P.AdvancedPaid,
        P.Discount,
        P.Amount,
        P.PaymentDate,
        P.PaymentMethod,
        P.PaymentType,
        P.CardOrCheckNo,
        P.PaymentStatus,
        P.IsConfirmed
    FROM 
        dbo.Payment P
    LEFT JOIN 
        dbo.Patient PT ON P.PatientID = PT.PatientID
    LEFT JOIN 
        dbo.Doctor D ON P.DoctorID = D.DoctorID;
END;

EXEC PR_Payment_SelectAll;




--SELECT BY PK---
ALTER PROCEDURE [dbo].[PR_Payment_SelectByPK]
    @PaymentID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.PaymentID,
        P.PatientID,
        COALESCE(PT.PatientName, 'N/A') AS PatientName, -- Handle NULL PatientName
        P.DoctorID,
        COALESCE(D.DoctorName, 'N/A') AS DoctorName, -- Handle NULL DoctorName
        P.Department,
        P.ServiceName,
        P.CostOfTreatment,
        P.AdmissionDate,
        P.DischargeDate,
        P.AdvancedPaid,
        P.Discount,
        P.Amount,
        P.PaymentDate,
        P.PaymentMethod,
        P.PaymentType,
        P.CardOrCheckNo,
        P.PaymentStatus,
        P.IsConfirmed
    FROM 
        dbo.Payment P
    LEFT JOIN 
        dbo.Patient PT ON P.PatientID = PT.PatientID
    LEFT JOIN 
        dbo.Doctor D ON P.DoctorID = D.DoctorID
    WHERE 
        P.PaymentID = @PaymentID;
END;

EXEC PR_Payment_SelectByPK @PaymentID = 22;


--INSERT--
Alter PROCEDURE [dbo].[PR_Payment_Insert]
    @PatientID INT,
    @DoctorID INT,
    @Department VARCHAR(100),
    @ServiceName VARCHAR(255),
    @CostOfTreatment DECIMAL(10, 2),
    @AdmissionDate DATE,
    @DischargeDate DATE,
    @AdvancedPaid DECIMAL(10, 2),
    @Discount DECIMAL(10, 2),
	@Amount DECIMAL(10,2),
    @PaymentDate DATE,
    @PaymentMethod VARCHAR(20),
    @PaymentType VARCHAR(50),
    @CardOrCheckNo VARCHAR(50),
    @PaymentStatus VARCHAR(20),
	@IsConfirmed BIT
AS
BEGIN
    INSERT INTO dbo.Payment
    (
        PatientID,
        DoctorID,
        Department,
        ServiceName,
        CostOfTreatment,
        AdmissionDate,
        DischargeDate,
        AdvancedPaid,
        Discount,
		Amount,
        PaymentDate,
        PaymentMethod,
        PaymentType,
        CardOrCheckNo,
        PaymentStatus,
		IsConfirmed
    )
    VALUES
    (
        @PatientID,
        @DoctorID,
        @Department,
        @ServiceName,
        @CostOfTreatment,
        @AdmissionDate,
        @DischargeDate,
        @AdvancedPaid,
        @Discount,
		@Amount,
        @PaymentDate,
        @PaymentMethod,
        @PaymentType,
        @CardOrCheckNo,
        @PaymentStatus,
		@IsConfirmed
    );
END;

--UPDATE--
CREATE PROCEDURE [dbo].[PR_Payment_Update]
    @PaymentID INT,
    @PatientID INT,
    @DoctorID INT,
    @Department VARCHAR(100),
    @ServiceName VARCHAR(255),
    @CostOfTreatment DECIMAL(10, 2),
    @AdmissionDate DATE,
    @DischargeDate DATE,
    @AdvancedPaid DECIMAL(10, 2),
    @Discount DECIMAL(10, 2),
	@Amount DECIMAL(10,2),
    @PaymentDate DATE,
    @PaymentMethod VARCHAR(20),
    @PaymentType VARCHAR(50),
    @CardOrCheckNo VARCHAR(50),
    @PaymentStatus VARCHAR(20),
	@IsConfirmed BIT
AS
BEGIN
    UPDATE dbo.Payment
    SET 
        PatientID = @PatientID,
        DoctorID = @DoctorID,
        Department = @Department,
        ServiceName = @ServiceName,
        CostOfTreatment = @CostOfTreatment,
        AdmissionDate = @AdmissionDate,
        DischargeDate = @DischargeDate,
        AdvancedPaid = @AdvancedPaid,
        Discount = @Discount,
		Amount = @Amount,
        PaymentDate = @PaymentDate,
        PaymentMethod = @PaymentMethod,
        PaymentType = @PaymentType,
        CardOrCheckNo = @CardOrCheckNo,
        PaymentStatus = @PaymentStatus,
		IsConfirmed = @IsConfirmed
    WHERE 
        PaymentID = @PaymentID;
END;

Select * from Payment
--DELETE--
CREATE PROCEDURE [dbo].[PR_Payment_Delete]
    @PaymentID INT
AS
BEGIN
    DELETE FROM dbo.Payment
    WHERE PaymentID = @PaymentID;
END;

----ROOM PROCEDURE-----
SELECT * FROM ROOM
--SELECT BY ALL---
ALTER PROCEDURE [dbo].[PR_Room_SelectAll]
    @RoomNumber INT = NULL,  
    @RoomType NVARCHAR(50) = NULL,
    @PatientName NVARCHAR(100) = NULL,
    @DoctorName NVARCHAR(100) = NULL,
    @AllotmentDate DATE = NULL,
    @DischargeDate DATE = NULL,
    @IsConfirmed BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        r.RoomID,           
        r.RoomNumber,   
        r.RoomType,
        r.PatientID,
        COALESCE(p.PatientName, 'N/A') AS PatientName,  -- Handle NULL Patient Name
        r.AllotmentDate,
        r.DischargeDate,
        r.DoctorID,
        COALESCE(d.DoctorName, 'N/A') AS DoctorName,  -- Handle NULL Doctor Name
        r.IsConfirmed
    FROM 
        dbo.Room r
    LEFT JOIN 
        dbo.Doctor d ON r.DoctorID = d.DoctorID
    LEFT JOIN 
        dbo.Patient p ON r.PatientID = p.PatientID
    WHERE 
        (@RoomNumber IS NULL OR r.RoomNumber = @RoomNumber)  -- Changed to = for INT comparison
        AND (@RoomType IS NULL OR r.RoomType LIKE '%' + @RoomType + '%')
        AND (@PatientName IS NULL OR p.PatientName LIKE '%' + @PatientName + '%')
        AND (@DoctorName IS NULL OR d.DoctorName LIKE '%' + @DoctorName + '%')
        AND (@AllotmentDate IS NULL OR r.AllotmentDate = @AllotmentDate) -- No need for CAST
        AND (@DischargeDate IS NULL OR r.DischargeDate = @DischargeDate) -- No need for CAST
        AND (@IsConfirmed IS NULL OR r.IsConfirmed = @IsConfirmed);
END;

EXEC PR_Room_SelectAll



SELECT COLUMN_NAME 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Room' AND COLUMN_NAME IN ('PatientName', 'DoctorName');

Select * from Room

--SELECT BY PK---
ALTER PROCEDURE [dbo].[PR_Room_SelectByPK]
    @RoomID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        r.RoomID,
        r.RoomNumber,
        r.RoomType,
        r.PatientID,
        COALESCE(p.PatientName, 'N/A') AS PatientName, -- Get Patient Name
        r.AllotmentDate,
        r.DischargeDate,
        r.DoctorID,
        COALESCE(d.DoctorName, 'N/A') AS DoctorName, -- Get Doctor Name
        r.IsConfirmed
    FROM 
        dbo.Room r
    LEFT JOIN 
        dbo.Patient p ON r.PatientID = p.PatientID
    LEFT JOIN 
        dbo.Doctor d ON r.DoctorID = d.DoctorID
    WHERE 
        r.RoomID = @RoomID;
END;

EXEC PR_Room_SelectByPK @RoomID = 5


--INSERT--
ALTER PROCEDURE [dbo].[PR_Room_Insert]
    @RoomNumber INT,
    @RoomType VARCHAR(50),
    @PatientID INT,
    @AllotmentDate DATE,
    @DischargeDate DATE,
    @DoctorID INT,
    @IsConfirmed BIT
AS
BEGIN
    INSERT INTO dbo.Room
    (
        RoomNumber,
        RoomType,
        PatientID,
        AllotmentDate,
        DischargeDate,
        DoctorID,
        IsConfirmed
    )
    VALUES
    (
        @RoomNumber,
        @RoomType,
        @PatientID,
        @AllotmentDate,
        @DischargeDate,
        @DoctorID,
        @IsConfirmed
    );
END;



--UPDATE--
ALTER PROCEDURE [dbo].[PR_Room_Update]
    @RoomID INT,
    @RoomNumber INT,
    @RoomType VARCHAR(50),
    @PatientID INT,
    @AllotmentDate DATE,
    @DischargeDate DATE,
    @DoctorID INT,
    @IsConfirmed BIT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the RoomID exists
    IF NOT EXISTS (SELECT 1 FROM dbo.Room WHERE RoomID = @RoomID)
    BEGIN
        PRINT 'Error: RoomID does not exist.';
        RETURN;
    END

    -- Update the Room record
    UPDATE dbo.Room
    SET 
        RoomNumber = @RoomNumber,
        RoomType = @RoomType,
        PatientID = @PatientID,
        AllotmentDate = @AllotmentDate,
        DischargeDate = @DischargeDate,
        DoctorID = @DoctorID,
        IsConfirmed = @IsConfirmed
    WHERE 
        RoomID = @RoomID;

    -- Check if the update was successful
    IF @@ROWCOUNT = 0
    BEGIN
        PRINT 'Update failed: No rows affected.';
    END
    ELSE
    BEGIN
        PRINT 'Update successful.';
    END
END;


--DELETE--
CREATE PROCEDURE [dbo].[PR_Room_Delete]
    @RoomID INT
AS
BEGIN
    DELETE FROM dbo.Room
    WHERE RoomID = @RoomID;
END;





CREATE PROCEDURE sp_GetDashboardCounts
AS
BEGIN
    SELECT 
        (SELECT COUNT(*) FROM Doctors) AS TotalDoctors,
        (SELECT COUNT(*) FROM Patients) AS TotalPatients,
        (SELECT COUNT(*) FROM Appointments) AS TotalAppointments,
        (SELECT COUNT(*) FROM Users) AS TotalUsers;
END;

EXEC sp_GetDashboardCounts;



Alter PROCEDURE sp_GetDashboardCounts
AS
BEGIN
    SELECT 
        (SELECT COUNT(*) FROM Patient WHERE IsConfirmed = 1) AS TotalPatients,
        (SELECT COUNT(*) FROM Doctor WHERE IsConfirmed = 1) AS TotalDoctors,
        (SELECT COUNT(*) FROM Payment WHERE PaymentStatus = 'Paid') AS TotalPayments,
        (SELECT COUNT(*) FROM Room WHERE IsConfirmed = 1) AS TotalRooms,
		(SELECT COUNT(*) FROM Appointment WHERE IsConfirmed = 1) AS TotalAppointments;
END;



ALTER PROCEDURE sp_GetRecentEntries
AS
BEGIN
    -- Get last 5 Registered Patients
    SELECT TOP 10
        PatientName, 
        ISNULL(Age, 0) AS Age, 
        Phone, 
        Email, 
        Gender 
    FROM Patient 
    ORDER BY PatientID DESC;

    -- Get last 5 Registered Doctors
    SELECT TOP 10
        DoctorName, 
        Specialization, 
        ISNULL(Experience, 0) AS Experience, 
        ContactNumber, 
        Email 
    FROM Doctor 
    ORDER BY DoctorID DESC;

    -- Get last 5 Payments
    SELECT TOP 10
        P.PatientName, 
        D.DoctorName, 
        Pay.ServiceName, 
        ISNULL(Pay.Amount, 0) AS Amount, 
        Pay.PaymentStatus, 
        ISNULL(Pay.PaymentDate, '1900-01-01') AS PaymentDate 
    FROM Payment Pay
    INNER JOIN Patient P ON Pay.PatientID = P.PatientID
    INNER JOIN Doctor D ON Pay.DoctorID = D.DoctorID
    ORDER BY Pay.PaymentID DESC;
END;


EXEC sp_GetRecentEntries;



CREATE PROCEDURE sp_GetMonthlyStats
AS
BEGIN
    -- Count of Patients admitted per month
    SELECT FORMAT(AdmissionDate, 'yyyy-MM') AS Month, COUNT(*) AS TotalAdmissions
    FROM Payment
    WHERE AdmissionDate IS NOT NULL
    GROUP BY FORMAT(AdmissionDate, 'yyyy-MM')
    ORDER BY Month DESC;

    -- Total Payments received per month
    SELECT FORMAT(PaymentDate, 'yyyy-MM') AS Month, SUM(Amount) AS TotalRevenue
    FROM Payment
    WHERE PaymentStatus = 'Paid'
    GROUP BY FORMAT(PaymentDate, 'yyyy-MM')
    ORDER BY Month DESC;
END;


CREATE TABLE DashboardStats (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    TotalPatients INT,
    TotalDoctors INT,
    TotalPayments DECIMAL(18,2),
    TotalRooms INT,
    LastUpdated DATETIME
);



ALTER PROCEDURE sp_UpdateDashboardData
AS
BEGIN
    -- Check if a record exists
    IF EXISTS (SELECT 1 FROM DashboardStats)
    BEGIN
        -- Update existing record
        UPDATE DashboardStats
        SET 
            TotalPatients = (SELECT COUNT(*) FROM Patient),
            TotalDoctors = (SELECT COUNT(*) FROM Doctor),
            TotalPayments = (SELECT SUM(Amount) FROM Payment WHERE PaymentStatus = 'Paid'),
            TotalRooms = (SELECT COUNT(*) FROM Room WHERE IsConfirmed = 1),
            LastUpdated = GETDATE();
    END
    ELSE
    BEGIN
        -- Insert initial record if not exists
        INSERT INTO DashboardStats (TotalPatients, TotalDoctors, TotalPayments, TotalRooms, LastUpdated)
        VALUES (
            (SELECT COUNT(*) FROM Patient),
            (SELECT COUNT(*) FROM Doctor),
            (SELECT SUM(Amount) FROM Payment WHERE PaymentStatus = 'Paid'),
            (SELECT COUNT(*) FROM Room WHERE IsConfirmed = 1),
            GETDATE()
        );
    END
END;

EXEC sp_UpdateDashboardData


Alter PROCEDURE sp_GetPatientDashboard
    @PatientID INT
AS
BEGIN
    -- Get Patient Details
    SELECT PatientID, PatientName, Email, Phone 
    FROM Patient 
    WHERE PatientID = @PatientID;

    -- Get Patient Appointments
    SELECT AppointmentID, AppointmentDate, AppointmentTime, Problem, Status 
    FROM Appointment
    WHERE PatientID = @PatientID;

    -- Get Patient Payments
    SELECT PaymentID, ServiceName, CostOfTreatment, PaymentStatus 
    FROM Payment 
    WHERE PatientID = @PatientID;

    -- Get Room Details
    SELECT RoomID, RoomNumber, RoomType, AllotmentDate, DischargeDate 
    FROM Room
    WHERE PatientID = @PatientID;
END;


EXEC sp_GetPatientDashboard @PatientID = 1;


CREATE PROCEDURE GetUserDashboardData
    @UserEmail NVARCHAR(100)
AS
BEGIN
    -- Fetch patient details
    SELECT 
        PatientID, PatientName, Email, MobileNumber, Address 
    FROM Patient 
    WHERE Email = @UserEmail;

    -- Fetch booked appointments (if any)
    SELECT 
        AppointmentID, DoctorName, AppointmentDate, Status 
    FROM Appointments 
    WHERE UserEmail = @UserEmail AND Status = 'Booked';
END;
