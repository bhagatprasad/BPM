CREATE TABLE public.volume_discount_tiers
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    supplierid uuid NOT NULL,
    minquantity integer NOT NULL,
    maxquantity integer,
    discountpercentage numeric(5,2) NOT NULL,
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT volume_discount_tiers_pkey PRIMARY KEY (id),
    CONSTRAINT fk_volume_discount_tiers_supplier FOREIGN KEY (supplierid)
        REFERENCES public.suppliers (supplierid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT volume_discount_tiers_quantity_check CHECK (minquantity > 0),
    CONSTRAINT volume_discount_tiers_maxquantity_check CHECK (maxquantity IS NULL OR maxquantity >= minquantity),
    CONSTRAINT volume_discount_tiers_percentage_check CHECK (discountpercentage >= 0::numeric AND discountpercentage <= 50::numeric)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.volume_discount_tiers
    OWNER to neondb_owner;
-- Index: idx_volume_discount_tiers_quantity

-- DROP INDEX IF EXISTS public.idx_volume_discount_tiers_quantity;

CREATE INDEX IF NOT EXISTS idx_volume_discount_tiers_quantity
    ON public.volume_discount_tiers USING btree
    (minquantity ASC NULLS LAST, maxquantity ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_volume_discount_tiers_supplierid

-- DROP INDEX IF EXISTS public.idx_volume_discount_tiers_supplierid;

CREATE INDEX IF NOT EXISTS idx_volume_discount_tiers_supplierid
    ON public.volume_discount_tiers USING btree
    (supplierid ASC NULLS LAST)
    TABLESPACE pg_default;
