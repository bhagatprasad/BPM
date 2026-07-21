CREATE TABLE IF NOT EXISTS public.drug_uom
(
    uomid uuid NOT NULL DEFAULT gen_random_uuid(),

    drugid uuid NOT NULL,

    uom_code varchar(20) NOT NULL,
    uom_name varchar(100) NOT NULL,

    -- BASE, PACK, BOX, CARTON, CASE, PALLET
    uom_type varchar(30) NOT NULL,

    -- Parent UOM (NULL for base unit)
    parent_uomid uuid,

    -- Quantity of parent
    quantity_per_parent integer,

    -- Quantity represented by this UOM in terms of base unit
    conversion_factor decimal(18,4) NOT NULL DEFAULT 1,

    -- Is this the smallest saleable unit?
    is_base_unit boolean DEFAULT false,

    -- Can this UOM be purchased?
    is_purchase_uom boolean DEFAULT false,

    -- Can this UOM be sold?
    is_sales_uom boolean DEFAULT true,

    -- Can inventory be maintained in this UOM?
    is_inventory_uom boolean DEFAULT true,

    display_order integer DEFAULT 1,

    remarks varchar(250),

    isactive boolean DEFAULT true,

    createdby uuid,
    createdon timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,

    CONSTRAINT drug_uom_pkey
        PRIMARY KEY (uomid),

    CONSTRAINT drug_uom_drugid_fkey
        FOREIGN KEY (drugid)
        REFERENCES public.drug(drugid),

    CONSTRAINT drug_uom_parent_fkey
        FOREIGN KEY (parent_uomid)
        REFERENCES public.drug_uom(uomid),

    CONSTRAINT drug_uom_unique
        UNIQUE (drugid, uom_code)
);