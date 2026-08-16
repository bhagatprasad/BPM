CREATE TABLE public.supplier_discounts
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    supplierid uuid NOT NULL,
    discountpercentage numeric(5,2) NOT NULL,
    validfrom timestamp without time zone NOT NULL,
    validto timestamp without time zone,
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT supplier_discounts_pkey PRIMARY KEY (id),
    CONSTRAINT fk_supplier_discounts_supplier FOREIGN KEY (supplierid)
        REFERENCES public.suppliers (supplierid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT supplier_discounts_percentage_check CHECK (discountpercentage >= 0::numeric AND discountpercentage <= 50::numeric),
    CONSTRAINT supplier_discounts_dates_check CHECK (validto IS NULL OR validto >= validfrom)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.supplier_discounts
    OWNER to neondb_owner;
-- Index: idx_supplier_discounts_isactive

-- DROP INDEX IF EXISTS public.idx_supplier_discounts_isactive;

CREATE INDEX IF NOT EXISTS idx_supplier_discounts_isactive
    ON public.supplier_discounts USING btree
    (isactive ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_supplier_discounts_supplierid

-- DROP INDEX IF EXISTS public.idx_supplier_discounts_supplierid;

CREATE INDEX IF NOT EXISTS idx_supplier_discounts_supplierid
    ON public.supplier_discounts USING btree
    (supplierid ASC NULLS LAST)
    TABLESPACE pg_default;
