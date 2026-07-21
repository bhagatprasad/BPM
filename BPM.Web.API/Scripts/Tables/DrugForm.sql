CREATE TABLE public.drug_forms
(
    formid uuid NOT NULL DEFAULT gen_random_uuid(),
    formcode character varying(20) NOT NULL,
    formname character varying(100) NOT NULL, -- 'Tablet', 'Injection', 'Syrup', etc.
    formtype character varying(50), -- 'SOLID', 'LIQUID', 'SEMISOLID', etc.
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT drug_forms_pkey PRIMARY KEY (formid),
    CONSTRAINT drug_forms_formcode_key UNIQUE (formcode)
);