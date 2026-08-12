CREATE TABLE public.invoices
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    invoicenumber character varying(30) COLLATE pg_catalog."default" NOT NULL,
    billingid uuid NOT NULL,
    purchaseorderid uuid NOT NULL,
    salesorderid uuid NOT NULL,
    dealerid uuid NOT NULL,
    invoicedate timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    subtotal numeric(18,2) NOT NULL DEFAULT 0,
    discountamount numeric(18,2) NOT NULL DEFAULT 0,
    taxamount numeric(18,2) NOT NULL DEFAULT 0,
    adjustmentamount numeric(18,2) NOT NULL DEFAULT 0,
    totalamount numeric(18,2) NOT NULL DEFAULT 0,
    paidamount numeric(18,2) NOT NULL DEFAULT 0,
    pendingamount numeric(18,2) NOT NULL DEFAULT 0,
    status character varying(30) COLLATE pg_catalog."default" NOT NULL DEFAULT 'Pending'::character varying,
    currencycode character varying(3) COLLATE pg_catalog."default" NOT NULL DEFAULT 'INR'::character varying,
    paymentterms character varying(100) COLLATE pg_catalog."default" NOT NULL,
    remarks character varying(500) COLLATE pg_catalog."default",
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT invoices_pkey PRIMARY KEY (id),
    CONSTRAINT invoices_invoicenumber_key UNIQUE (invoicenumber),
    CONSTRAINT fk_invoices_billing FOREIGN KEY (billingid)
        REFERENCES public.billings (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT fk_invoices_createdby FOREIGN KEY (createdby)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE SET NULL,
    CONSTRAINT fk_invoices_dealer FOREIGN KEY (dealerid)
        REFERENCES public.dealers (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT fk_invoices_modifiedby FOREIGN KEY (modifiedby)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE SET NULL,
    CONSTRAINT fk_invoices_purchaseorder FOREIGN KEY (purchaseorderid)
        REFERENCES public.purchase_orders (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT fk_invoices_salesorder FOREIGN KEY (salesorderid)
        REFERENCES public.sales_orders (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT chk_invoice_paidamount CHECK (paidamount >= 0::numeric),
    CONSTRAINT chk_invoice_pendingamount CHECK (pendingamount >= 0::numeric),
    CONSTRAINT chk_invoice_totalamount CHECK (totalamount >= 0::numeric)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.invoices
    OWNER to neondb_owner;
-- Index: idx_invoices_billingid

-- DROP INDEX IF EXISTS public.idx_invoices_billingid;

CREATE INDEX IF NOT EXISTS idx_invoices_billingid
    ON public.invoices USING btree
    (billingid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_invoices_dealerid

-- DROP INDEX IF EXISTS public.idx_invoices_dealerid;

CREATE INDEX IF NOT EXISTS idx_invoices_dealerid
    ON public.invoices USING btree
    (dealerid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_invoices_invoicedate

-- DROP INDEX IF EXISTS public.idx_invoices_invoicedate;

CREATE INDEX IF NOT EXISTS idx_invoices_invoicedate
    ON public.invoices USING btree
    (invoicedate ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_invoices_purchaseorderid

-- DROP INDEX IF EXISTS public.idx_invoices_purchaseorderid;

CREATE INDEX IF NOT EXISTS idx_invoices_purchaseorderid
    ON public.invoices USING btree
    (purchaseorderid ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: idx_invoices_salesorderid

-- DROP INDEX IF EXISTS public.idx_invoices_salesorderid;

CREATE INDEX IF NOT EXISTS idx_invoices_salesorderid
    ON public.invoices USING btree
    (salesorderid ASC NULLS LAST)
    TABLESPACE pg_default;