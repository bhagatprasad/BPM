CREATE TABLE public.purchaseorders
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    ponumber character varying(20) COLLATE pg_catalog."default" NOT NULL,
    supplierid uuid NOT NULL,
    dealerid uuid NOT NULL,
    orderdate timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    expecteddeliverydate timestamp without time zone NOT NULL,
    actualdeliverydate timestamp without time zone,
    status character varying(30) COLLATE pg_catalog."default" NOT NULL DEFAULT 'Draft'::character varying,
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
    CONSTRAINT purchaseorders_pkey PRIMARY KEY (id),
    CONSTRAINT purchaseorders_ponumber_key UNIQUE (ponumber),
    CONSTRAINT fk_purchaseorders_createdby FOREIGN KEY (createdby)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_purchaseorders_dealer FOREIGN KEY (dealerid)
        REFERENCES public.dealers (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_purchaseorders_modifiedby FOREIGN KEY (modifiedby)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_purchaseorders_supplier FOREIGN KEY (supplierid)
        REFERENCES public.suppliers (supplierid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)