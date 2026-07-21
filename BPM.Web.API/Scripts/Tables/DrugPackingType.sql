CREATE TABLE public.drug_packaging
(
    packagingid uuid PRIMARY KEY DEFAULT gen_random_uuid(),

    drugid uuid NOT NULL
        REFERENCES public.drug(drugid),

    package_uomid uuid NOT NULL
        REFERENCES public.drug_uom(uomid),

    contains_uomid uuid NOT NULL
        REFERENCES public.drug_uom(uomid),

    quantity integer NOT NULL,

    -- Total base units inside this package
    total_units integer NOT NULL,

    -- Price per contained unit
    unit_price decimal(18,2) NOT NULL,

    -- Total package price
    package_price decimal(18,2) NOT NULL,

    barcode varchar(100),

    gross_weight decimal(10,2),
    net_weight decimal(10,2),

    length decimal(10,2),
    width decimal(10,2),
    height decimal(10,2),

    isactive boolean DEFAULT TRUE,

    createdby uuid,
    createdon timestamp DEFAULT CURRENT_TIMESTAMP
);