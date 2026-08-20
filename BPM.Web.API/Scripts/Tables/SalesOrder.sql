CREATE TABLE public.sales_orders
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    sonumber character varying(20) COLLATE pg_catalog."default" NOT NULL,
    purchaseorderid uuid NOT NULL,
    supplierid uuid NOT NULL,
    dealerid uuid NOT NULL,
    distributorid uuid NOT NULL,  
    orderdate timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    expecteddeliverydate timestamp without time zone NOT NULL,
    actualdeliverydate timestamp without time zone,
    status character varying(30) COLLATE pg_catalog."default" NOT NULL DEFAULT 'Created'::character varying,
    subtotal numeric(18,2) DEFAULT 0,
    taxamount numeric(18,2) DEFAULT 0,
    discountamount numeric(18,2) DEFAULT 0,
    totalamount numeric(18,2) DEFAULT 0,
    currencycode character varying(3) COLLATE pg_catalog."default" NOT NULL DEFAULT 'INR'::character varying,
    paymentterms character varying(100) COLLATE pg_catalog."default" NOT NULL,
    deliveryterms character varying(100) COLLATE pg_catalog."default",
    remarks character varying(500) COLLATE pg_catalog."default",
    internalnotes character varying(500) COLLATE pg_catalog."default",
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT salesorders_pkey PRIMARY KEY (id),
    CONSTRAINT salesorders_sonumber_key UNIQUE (sonumber),
    CONSTRAINT fk_salesorders_purchaseorder FOREIGN KEY (purchaseorderid)
        REFERENCES public.purchase_orders (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_salesorders_supplier FOREIGN KEY (supplierid)
        REFERENCES public.suppliers (supplierid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_salesorders_dealer FOREIGN KEY (dealerid)
        REFERENCES public.dealers (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_salesorders_distributor FOREIGN KEY (distributorid)  
        REFERENCES public.distributors (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_salesorders_createdby FOREIGN KEY (createdby)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_salesorders_modifiedby FOREIGN KEY (modifiedby)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.sales_orders
    OWNER TO neondb_owner;