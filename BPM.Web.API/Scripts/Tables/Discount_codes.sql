CREATE TABLE public.discount_codes
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    discountcode character varying(50) COLLATE pg_catalog."default" NOT NULL,
    discountpercentage numeric(5,2) NOT NULL,
    supplierid uuid,
    startdate timestamp without time zone NOT NULL,
    expirydate timestamp without time zone NOT NULL,
    requiresapproval boolean NOT NULL DEFAULT true,
    isapproved boolean NOT NULL DEFAULT false,
    allowcombination boolean NOT NULL DEFAULT false,
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT discount_codes_pkey PRIMARY KEY (id),
    CONSTRAINT discount_codes_code_unique UNIQUE (discountcode),
    CONSTRAINT fk_discount_codes_supplier FOREIGN KEY (supplierid)
        REFERENCES public.suppliers (supplierid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT discount_codes_percentage_check CHECK (discountpercentage >= 0::numeric AND discountpercentage <= 50::numeric),
    CONSTRAINT discount_codes_dates_check CHECK (expirydate >= startdate)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.discount_codes
    OWNER to neondb_owner;
-- Index: idx_discount_codes_discountcode

-- DROP INDEX IF EXISTS public.idx_discount_codes_discountcode;

CREATE INDEX IF NOT EXISTS idx_discount_codes_discountcode
    ON public.discount_codes USING btree
    (discountcode COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_discount_codes_expirydate

-- DROP INDEX IF EXISTS public.idx_discount_codes_expirydate;

CREATE INDEX IF NOT EXISTS idx_discount_codes_expirydate
    ON public.discount_codes USING btree
    (expirydate ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_discount_codes_supplierid

-- DROP INDEX IF EXISTS public.idx_discount_codes_supplierid;

CREATE INDEX IF NOT EXISTS idx_discount_codes_supplierid
    ON public.discount_codes USING btree
    (supplierid ASC NULLS LAST)
    TABLESPACE pg_default;
