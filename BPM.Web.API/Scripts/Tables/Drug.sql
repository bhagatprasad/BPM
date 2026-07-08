CREATE TABLE Drug
(
    DrugId UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    DrugCode VARCHAR(50) NOT NULL UNIQUE,
    DrugName VARCHAR(200) NOT NULL,
    GenericName VARCHAR(200),
    BrandName VARCHAR(200),
    Manufacturer VARCHAR(200),

    Category VARCHAR(100),
    HSNCode VARCHAR(20),
    ScheduleType VARCHAR(20),

    Packing VARCHAR(50),
    Strength VARCHAR(50),

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedBy UUID,
    CreatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    ModifiedBy UUID,
    ModifiedOn TIMESTAMP
);