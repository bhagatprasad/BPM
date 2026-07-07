/* CREATE TABLE Dealers
(
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
 
    DealershipName VARCHAR(200) NOT NULL,
    RegistrationNumber VARCHAR(100) UNIQUE,
    TradeLicenseNumber VARCHAR(100),
    GSTNumber VARCHAR(50),
 
    ContactPerson VARCHAR(150),
 
    Email VARCHAR(150),
    Phone VARCHAR(20),
    AlternatePhone VARCHAR(20),
 
    AddressLine1 VARCHAR(255),
    AddressLine2 VARCHAR(255),
    City VARCHAR(100),
    State VARCHAR(100),
    Country VARCHAR(100),
    PostalCode VARCHAR(20),
 
    Website VARCHAR(200),
 
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
 
    CreatedBy UUID,
    CreatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ModifiedBy UUID,
    ModifiedOn TIMESTAMP
); */
CREATE TABLE public.roles
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    name character varying(100) COLLATE pg_catalog."default" NOT NULL,
    code character varying(50) COLLATE pg_catalog."default" NOT NULL,
    description character varying(255) COLLATE pg_catalog."default",
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT roles_pkey PRIMARY KEY (id),
    CONSTRAINT roles_code_key UNIQUE (code)
)
 