CREATE TABLE public.inventory
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    drugid uuid NOT NULL,
    packagingid uuid NOT NULL,
    batchid uuid NOT NULL,
    warehouseid uuid NOT NULL,
    quantity integer NOT NULL DEFAULT 0,
    reservedquantity integer NOT NULL DEFAULT 0,
    availablequantity integer NOT NULL DEFAULT 0,
    reorderlevel integer NOT NULL DEFAULT 0,
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    distributorid uuid,
    CONSTRAINT inventory_pkey PRIMARY KEY (id),
    CONSTRAINT fk_inventory_distributor FOREIGN KEY (distributorid)
        REFERENCES public.distributors (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT inventory_batch_fk FOREIGN KEY (batchid)
        REFERENCES public.batch_master (batchid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT inventory_drug_fk FOREIGN KEY (drugid)
        REFERENCES public.drug (drugid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT inventory_packaging_fk FOREIGN KEY (packagingid)
        REFERENCES public.drug_packaging (packagingid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT inventory_warehouse_fk FOREIGN KEY (warehouseid)
        REFERENCES public.warehouses (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT inventory_availablequantity_check CHECK (availablequantity >= 0),
    CONSTRAINT inventory_quantity_check CHECK (quantity >= 0),
    CONSTRAINT inventory_reorderlevel_check CHECK (reorderlevel >= 0),
    CONSTRAINT inventory_reserved_less_than_quantity CHECK (reservedquantity <= quantity),
    CONSTRAINT inventory_reservedquantity_check CHECK (reservedquantity >= 0)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.inventory
    OWNER to neondb_owner;
-- Index: idx_inventory_batchid

-- DROP INDEX IF EXISTS public.idx_inventory_batchid;

CREATE INDEX IF NOT EXISTS idx_inventory_batchid
    ON public.inventory USING btree
    (batchid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_inventory_distributorid

-- DROP INDEX IF EXISTS public.idx_inventory_distributorid;

CREATE INDEX IF NOT EXISTS idx_inventory_distributorid
    ON public.inventory USING btree
    (distributorid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_inventory_drugid

-- DROP INDEX IF EXISTS public.idx_inventory_drugid;

CREATE INDEX IF NOT EXISTS idx_inventory_drugid
    ON public.inventory USING btree
    (drugid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_inventory_packagingid

-- DROP INDEX IF EXISTS public.idx_inventory_packagingid;

CREATE INDEX IF NOT EXISTS idx_inventory_packagingid
    ON public.inventory USING btree
    (packagingid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_inventory_warehouseid

-- DROP INDEX IF EXISTS public.idx_inventory_warehouseid;

CREATE INDEX IF NOT EXISTS idx_inventory_warehouseid
    ON public.inventory USING btree
    (warehouseid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: uq_inventory_drug_packaging_batch_warehouse

-- DROP INDEX IF EXISTS public.uq_inventory_drug_packaging_batch_warehouse;

CREATE UNIQUE INDEX IF NOT EXISTS uq_inventory_drug_packaging_batch_warehouse
    ON public.inventory USING btree
    (drugid ASC NULLS LAST, packagingid ASC NULLS LAST, batchid ASC NULLS LAST, warehouseid ASC NULLS LAST)
    TABLESPACE pg_default;