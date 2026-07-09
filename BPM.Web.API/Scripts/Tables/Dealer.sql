CREATE TABLE public.dealers
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    dealershipname character varying(200) COLLATE pg_catalog."default" NOT NULL,
    registrationnumber character varying(100) COLLATE pg_catalog."default",
    tradelicensenumber character varying(100) COLLATE pg_catalog."default",
    gstnumber character varying(50) COLLATE pg_catalog."default",
    contactperson character varying(150) COLLATE pg_catalog."default",
    email character varying(150) COLLATE pg_catalog."default",
    phone character varying(20) COLLATE pg_catalog."default",
    alternatephone character varying(20) COLLATE pg_catalog."default",
    addressline1 character varying(255) COLLATE pg_catalog."default",
    addressline2 character varying(255) COLLATE pg_catalog."default",
    city character varying(100) COLLATE pg_catalog."default",
    state character varying(100) COLLATE pg_catalog."default",
    country character varying(100) COLLATE pg_catalog."default",
    postalcode character varying(20) COLLATE pg_catalog."default",
    website character varying(200) COLLATE pg_catalog."default",
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT dealers_pkey PRIMARY KEY (id),
    CONSTRAINT dealers_registrationnumber_key UNIQUE (registrationnumber)
)
 