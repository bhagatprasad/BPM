-- Table: public.sales_order_items

-- DROP TABLE IF EXISTS public.sales_order_items;

CREATE TABLE IF NOT EXISTS public.sales_order_items
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    salesorderid uuid NOT NULL,
    drugid uuid NOT NULL,
    packagingid uuid,  -- Made nullable since the table may not exist yet
    quantity integer NOT NULL CHECK (quantity > 0),
    unitprice numeric(18,2) NOT NULL CHECK (unitprice >= 0),
    discountpercentage numeric(5,2) NOT NULL DEFAULT 0 CHECK (discountpercentage >= 0 AND discountpercentage <= 100),
    discountamount numeric(18,2) NOT NULL DEFAULT 0 CHECK (discountamount >= 0),
    taxrate numeric(5,2) NOT NULL DEFAULT 0 CHECK (taxrate >= 0),
    taxamount numeric(18,2) NOT NULL DEFAULT 0 CHECK (taxamount >= 0),
    totalamount numeric(18,2) NOT NULL CHECK (totalamount >= 0),
    receivedquantity integer NOT NULL DEFAULT 0 CHECK (receivedquantity >= 0),
    pendingquantity integer NOT NULL DEFAULT 0 CHECK (pendingquantity >= 0),
    batchnumber character varying(100) COLLATE pg_catalog."default",
    expirydate timestamp without time zone,
    remarks character varying(500) COLLATE pg_catalog."default",
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT salesorderitems_pkey PRIMARY KEY (id),
    CONSTRAINT fk_salesorderitems_salesorder FOREIGN KEY (salesorderid)
        REFERENCES public.sales_orders (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT fk_salesorderitems_drug FOREIGN KEY (drugid)
        REFERENCES public.drug (drugid) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE RESTRICT,
    CONSTRAINT fk_salesorderitems_createdby FOREIGN KEY (createdby)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE SET NULL,
    CONSTRAINT fk_salesorderitems_modifiedby FOREIGN KEY (modifiedby)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE SET NULL,
    CONSTRAINT chk_received_quantity CHECK (receivedquantity <= quantity),
    CONSTRAINT chk_pending_quantity CHECK (pendingquantity <= quantity),
    CONSTRAINT chk_received_pending CHECK (receivedquantity + pendingquantity = quantity)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.sales_order_items
    OWNER to neondb_owner;

-- Create indexes for better performance
CREATE INDEX IF NOT EXISTS idx_salesorderitems_salesorderid ON public.sales_order_items(salesorderid);
CREATE INDEX IF NOT EXISTS idx_salesorderitems_drugid ON public.sales_order_items(drugid);
CREATE INDEX IF NOT EXISTS idx_salesorderitems_expirydate ON public.sales_order_items(expirydate);
CREATE INDEX IF NOT EXISTS idx_salesorderitems_batchnumber ON public.sales_order_items(batchnumber);