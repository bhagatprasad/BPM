CREATE TABLE public.manufacturers
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),

    manufacturercode character varying(50) NOT NULL,
    manufacturername character varying(200) NOT NULL,

    contactperson character varying(150),
    email character varying(150),
    phone character varying(20),

    addressline1 character varying(255),
    addressline2 character varying(255),

    city character varying(100),
    state character varying(100),
    country character varying(100),
    postalcode character varying(20),

    website character varying(200),

    isactive boolean NOT NULL DEFAULT true,

    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,

    modifiedby uuid,
    modifiedon timestamp without time zone,

    CONSTRAINT manufacturers_pkey PRIMARY KEY (id),

    CONSTRAINT manufacturers_code_key UNIQUE (manufacturercode),

    CONSTRAINT manufacturers_email_key UNIQUE (email)
);
