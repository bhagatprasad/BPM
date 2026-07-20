CREATE TABLE IF NOT EXISTS public.packaging_master
(
    packagingid uuid NOT NULL DEFAULT gen_random_uuid(),

    packaging_code varchar(20) NOT NULL,

    packaging_name varchar(100) NOT NULL,

    description text,

    contains_quantity decimal(10,2),

    uomid uuid,

    isactive boolean DEFAULT true,

    createdon timestamp without time zone DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT packaging_master_pkey
        PRIMARY KEY (packagingid)
);