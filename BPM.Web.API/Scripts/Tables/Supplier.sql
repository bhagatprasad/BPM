CREATE TABLE Supplier
(
    SupplierId UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    SupplierCode VARCHAR(50) NOT NULL UNIQUE,
    SupplierName VARCHAR(200) NOT NULL,

    ContactPerson VARCHAR(150),
    Email VARCHAR(150),
    Phone VARCHAR(20),
    AlternatePhone VARCHAR(20),

    GSTNumber VARCHAR(20),
    DrugLicenseNumber VARCHAR(100),

    AddressLine1 VARCHAR(200),
    AddressLine2 VARCHAR(200),
    City VARCHAR(100),
    State VARCHAR(100),
    Country VARCHAR(100),
    PostalCode VARCHAR(10),

    Website VARCHAR(200),

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedBy UUID,
    CreatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ModifiedBy UUID,
    ModifiedOn TIMESTAMP
);
