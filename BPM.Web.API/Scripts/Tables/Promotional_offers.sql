CREATE TABLE public.promotional_offers
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    offername character varying(200) COLLATE pg_catalog."default" NOT NULL,
    supplierid uuid NOT NULL,
    drugid uuid,
    packagingid uuid,
    discountpercentage numeric(5,2) NOT NULL,
    startdate timestamp without time zone NOT NULL,
    expirydate timestamp without time zone NOT NULL,
    allowcombination boolean NOT NULL DEFAULT false,
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT promotional_offers_pkey PRIMARY KEY (id),
    CONSTRAINT fk_promotional_offers_drug FOREIGN KEY (drugid)
        REFERENCES public.drug (drugid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_promotional_offers_packaging FOREIGN KEY (packagingid)
        REFERENCES public.drug_packaging (packagingid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_promotional_offers_supplier FOREIGN KEY (supplierid)
        REFERENCES public.suppliers (supplierid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT promotional_offers_percentage_check CHECK (discountpercentage >= 0::numeric AND discountpercentage <= 50::numeric),
    CONSTRAINT promotional_offers_dates_check CHECK (expirydate >= startdate)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.promotional_offers
    OWNER to neondb_owner;
-- Index: idx_promotional_offers_drugid

-- DROP INDEX IF EXISTS public.idx_promotional_offers_drugid;

CREATE INDEX IF NOT EXISTS idx_promotional_offers_drugid
    ON public.promotional_offers USING btree
    (drugid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_promotional_offers_expirydate

-- DROP INDEX IF EXISTS public.idx_promotional_offers_expirydate;

CREATE INDEX IF NOT EXISTS idx_promotional_offers_expirydate
    ON public.promotional_offers USING btree
    (expirydate ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_promotional_offers_packagingid

-- DROP INDEX IF EXISTS public.idx_promotional_offers_packagingid;

CREATE INDEX IF NOT EXISTS idx_promotional_offers_packagingid
    ON public.promotional_offers USING btree
    (packagingid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_promotional_offers_supplierid

-- DROP INDEX IF EXISTS public.idx_promotional_offers_supplierid;

CREATE INDEX IF NOT EXISTS idx_promotional_offers_supplierid
    ON public.promotional_offers USING btree
    (supplierid ASC NULLS LAST)
    TABLESPACE pg_default;
