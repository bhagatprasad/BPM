CREATE TABLE Activities
(
    ActivityId UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    ActivityName VARCHAR(100) NOT NULL,

    Code VARCHAR(20) NOT NULL UNIQUE,

    Description VARCHAR(500),

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedBy UUID,

    CreatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    ModifiedBy UUID,

    ModifiedOn TIMESTAMP
);
