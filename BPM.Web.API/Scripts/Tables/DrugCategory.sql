CREATE TABLE public.drugcategories
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),

    categorycode character varying(50) NOT NULL,
    categoryname character varying(100) NOT NULL,

    description character varying(255),

    isactive boolean NOT NULL DEFAULT true,

    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,

    modifiedby uuid,
    modifiedon timestamp without time zone,

    CONSTRAINT drugcategories_pkey PRIMARY KEY (id),

    CONSTRAINT drugcategories_code_key UNIQUE (categorycode),

    CONSTRAINT drugcategories_name_key UNIQUE (categoryname)
);
