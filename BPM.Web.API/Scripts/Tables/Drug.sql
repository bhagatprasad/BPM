CREATE TABLE public.drug
(
    drugid uuid NOT NULL DEFAULT gen_random_uuid(),
    drugcode character varying(50) COLLATE pg_catalog."default" NOT NULL,
    drugname character varying(200) COLLATE pg_catalog."default" NOT NULL,
    genericname character varying(200) COLLATE pg_catalog."default",
    brandname character varying(200) COLLATE pg_catalog."default",
    manufacturer character varying(200) COLLATE pg_catalog."default",
    category character varying(100) COLLATE pg_catalog."default",
    hsncode character varying(20) COLLATE pg_catalog."default",
    scheduletype character varying(20) COLLATE pg_catalog."default",
    packing character varying(50) COLLATE pg_catalog."default",
    strength character varying(50) COLLATE pg_catalog."default",
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    imageurl text,
    CONSTRAINT drug_pkey PRIMARY KEY (drugid),
    CONSTRAINT drug_drugcode_key UNIQUE (drugcode)
)