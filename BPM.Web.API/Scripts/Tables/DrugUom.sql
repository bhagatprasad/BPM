CREATE TABLE IF NOT EXISTS public.drug_uom
(
    uomid uuid NOT NULL DEFAULT gen_random_uuid(),

    drugid uuid NOT NULL,

    uom_code varchar(20) NOT NULL,

    uom_name varchar(100) NOT NULL,

    uom_type varchar(20) NOT NULL,

    conversion_factor decimal(10,4),

    is_base_unit boolean DEFAULT false,

    isactive boolean DEFAULT true,

    createdby uuid,

    createdon timestamp without time zone DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT drug_uom_pkey
        PRIMARY KEY (uomid),

    CONSTRAINT drug_uom_drugid_fkey
        FOREIGN KEY (drugid)
        REFERENCES public.drug(drugid),

    CONSTRAINT drug_uom_unique
        UNIQUE (drugid, uom_code)
);