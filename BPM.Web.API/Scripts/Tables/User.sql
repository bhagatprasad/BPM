CREATE TABLE Users
(
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
 
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
 
    Email VARCHAR(150) NOT NULL UNIQUE,
    Phone VARCHAR(20)  NOT NULL UNIQUE,
 
    RoleId UUID NOT NULL,
    DealerId UUID,
 
    PasswordHash TEXT NOT NULL,
    PasswordSalt TEXT NOT NULL,
 
    LastLogin TIMESTAMP,
 
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
 
    CreatedBy UUID,
    CreatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ModifiedBy UUID,
    ModifiedOn TIMESTAMP,
 
    CONSTRAINT FK_Users_Roles
        FOREIGN KEY (RoleId)
        REFERENCES Roles(Id),
 
    CONSTRAINT FK_Users_Dealers
        FOREIGN KEY (DealerId)
        REFERENCES Dealers(Id)
);
