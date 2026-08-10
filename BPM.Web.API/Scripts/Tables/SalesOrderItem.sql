CREATE TABLE public.sales_order_items
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),

    salesorderid uuid NOT NULL,

    drugid uuid NOT NULL,

    quantity integer NOT NULL,

    unitprice numeric(18,2) NOT NULL,

    discountpercentage numeric(5,2) NOT NULL DEFAULT 0,

    discountamount numeric(18,2) NOT NULL DEFAULT 0,

    taxrate numeric(5,2) NOT NULL DEFAULT 0,

    taxamount numeric(18,2) NOT NULL DEFAULT 0,

    totalamount numeric(18,2) NOT NULL,

    receivedquantity integer NOT NULL DEFAULT 0,

    pendingquantity integer NOT NULL DEFAULT 0,

    batchnumber character varying(100) COLLATE pg_catalog."default",

    expirydate timestamp without time zone,

    remarks character varying(500) COLLATE pg_catalog."default",

    createdby uuid,

    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,

    modifiedby uuid,

    modifiedon timestamp without time zone,

    packagingid uuid NOT NULL,

    CONSTRAINT salesorderitems_pkey PRIMARY KEY (id),

    CONSTRAINT fk_salesorderitems_salesorder FOREIGN KEY (salesorderid)
        REFERENCES public.sales_orders (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,

    CONSTRAINT fk_salesorderitems_drug FOREIGN KEY (drugid)
        REFERENCES public.drug (drugid) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.sales_order_items
    OWNER TO neondb_owner;
