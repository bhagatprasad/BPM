CREATE TABLE public.stock_movements
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    inventoryid uuid NOT NULL,
    drugid uuid NOT NULL,
    packagingid uuid NOT NULL,
    batchid uuid NOT NULL,
    warehouseid uuid NOT NULL,
    movementtype character varying(50) COLLATE pg_catalog."default" NOT NULL,
    quantity integer NOT NULL,
    quantitybefore integer NOT NULL,
    quantityafter integer NOT NULL,
    referencetype character varying(50) COLLATE pg_catalog."default",
    referenceid uuid,
    unitcost numeric(18,2),
    remarks character varying(500) COLLATE pg_catalog."default",
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    distributorid uuid,
    CONSTRAINT stock_movements_pkey PRIMARY KEY (id),
    CONSTRAINT fk_stock_movements_distributor FOREIGN KEY (distributorid)
        REFERENCES public.distributors (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT stock_movements_batch_fk FOREIGN KEY (batchid)
        REFERENCES public.batch_master (batchid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT stock_movements_drug_fk FOREIGN KEY (drugid)
        REFERENCES public.drug (drugid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT stock_movements_inventory_fk FOREIGN KEY (inventoryid)
        REFERENCES public.inventory (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT stock_movements_packaging_fk FOREIGN KEY (packagingid)
        REFERENCES public.drug_packaging (packagingid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT stock_movements_warehouse_fk FOREIGN KEY (warehouseid)
        REFERENCES public.warehouses (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT stock_movements_quantity_check CHECK (quantity <> 0),
    CONSTRAINT stock_movements_quantityafter_check CHECK (quantityafter >= 0),
    CONSTRAINT stock_movements_quantitybefore_check CHECK (quantitybefore >= 0)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.stock_movements
    OWNER to neondb_owner;
-- Index: idx_stock_movements_batchid

-- DROP INDEX IF EXISTS public.idx_stock_movements_batchid;

CREATE INDEX IF NOT EXISTS idx_stock_movements_batchid
    ON public.stock_movements USING btree
    (batchid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_stock_movements_createdon

-- DROP INDEX IF EXISTS public.idx_stock_movements_createdon;

CREATE INDEX IF NOT EXISTS idx_stock_movements_createdon
    ON public.stock_movements USING btree
    (createdon ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_stock_movements_distributorid

-- DROP INDEX IF EXISTS public.idx_stock_movements_distributorid;

CREATE INDEX IF NOT EXISTS idx_stock_movements_distributorid
    ON public.stock_movements USING btree
    (distributorid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_stock_movements_drugid

-- DROP INDEX IF EXISTS public.idx_stock_movements_drugid;

CREATE INDEX IF NOT EXISTS idx_stock_movements_drugid
    ON public.stock_movements USING btree
    (drugid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_stock_movements_inventoryid

-- DROP INDEX IF EXISTS public.idx_stock_movements_inventoryid;

CREATE INDEX IF NOT EXISTS idx_stock_movements_inventoryid
    ON public.stock_movements USING btree
    (inventoryid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_stock_movements_movementtype

-- DROP INDEX IF EXISTS public.idx_stock_movements_movementtype;

CREATE INDEX IF NOT EXISTS idx_stock_movements_movementtype
    ON public.stock_movements USING btree
    (movementtype COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_stock_movements_packagingid

-- DROP INDEX IF EXISTS public.idx_stock_movements_packagingid;

CREATE INDEX IF NOT EXISTS idx_stock_movements_packagingid
    ON public.stock_movements USING btree
    (packagingid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_stock_movements_warehouseid

-- DROP INDEX IF EXISTS public.idx_stock_movements_warehouseid;

CREATE INDEX IF NOT EXISTS idx_stock_movements_warehouseid
    ON public.stock_movements USING btree
    (warehouseid ASC NULLS LAST)
    TABLESPACE pg_default;