CREATE TABLE public.warehouses
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    warehousecode character varying(50) COLLATE pg_catalog."default" NOT NULL,
    warehousename character varying(200) COLLATE pg_catalog."default" NOT NULL,
    addressline1 character varying(255) COLLATE pg_catalog."default",
    addressline2 character varying(255) COLLATE pg_catalog."default",
    city character varying(100) COLLATE pg_catalog."default",
    state character varying(100) COLLATE pg_catalog."default",
    country character varying(100) COLLATE pg_catalog."default",
    postalcode character varying(20) COLLATE pg_catalog."default",
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    distributorid uuid,
    CONSTRAINT warehouses_pkey PRIMARY KEY (id),
    CONSTRAINT warehouses_warehousecode_key UNIQUE (warehousecode),
    CONSTRAINT fk_warehouses_distributor FOREIGN KEY (distributorid)
        REFERENCES public.distributors (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.warehouses
    OWNER to neondb_owner;
-- Index: idx_warehouses_distributorid

-- DROP INDEX IF EXISTS public.idx_warehouses_distributorid;

CREATE INDEX IF NOT EXISTS idx_warehouses_distributorid
    ON public.warehouses USING btree
    (distributorid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_warehouses_isactive

-- DROP INDEX IF EXISTS public.idx_warehouses_isactive;

CREATE INDEX IF NOT EXISTS idx_warehouses_isactive
    ON public.warehouses USING btree
    (isactive ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_warehouses_warehousecode

-- DROP INDEX IF EXISTS public.idx_warehouses_warehousecode;

CREATE INDEX IF NOT EXISTS idx_warehouses_warehousecode
    ON public.warehouses USING btree
    (warehousecode COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;