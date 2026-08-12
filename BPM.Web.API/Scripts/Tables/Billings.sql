CREATE TABLE public.billings
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),

    billingnumber character varying(30) NOT NULL,

    purchaseorderid uuid NOT NULL,

    salesorderid uuid NOT NULL,

    dealerid uuid NOT NULL,

    billingdate timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,

    subtotal numeric(18,2) NOT NULL DEFAULT 0,

    discountamount numeric(18,2) NOT NULL DEFAULT 0,

    taxamount numeric(18,2) NOT NULL DEFAULT 0,

    adjustmentamount numeric(18,2) NOT NULL DEFAULT 0,

    totalamount numeric(18,2) NOT NULL DEFAULT 0,

    paidamount numeric(18,2) NOT NULL DEFAULT 0,

    pendingamount numeric(18,2) NOT NULL DEFAULT 0,

    status character varying(30) NOT NULL DEFAULT 'Pending',

    currencycode character varying(3) NOT NULL DEFAULT 'INR',

    paymentterms character varying(100),

    remarks character varying(500),

    isactive boolean NOT NULL DEFAULT true,

    createdby uuid,

    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,

    modifiedby uuid,

    modifiedon timestamp without time zone,

    CONSTRAINT billings_pkey
        PRIMARY KEY (id),

    CONSTRAINT billings_billingnumber_key
        UNIQUE (billingnumber),

    CONSTRAINT billings_salesorder_key
        UNIQUE (salesorderid),

    CONSTRAINT fk_billings_purchaseorder
        FOREIGN KEY (purchaseorderid)
        REFERENCES public.purchase_orders (id)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,

    CONSTRAINT fk_billings_salesorder
        FOREIGN KEY (salesorderid)
        REFERENCES public.sales_orders (id)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,

    CONSTRAINT fk_billings_dealer
        FOREIGN KEY (dealerid)
        REFERENCES public.dealers (id)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,

    CONSTRAINT fk_billings_createdby
        FOREIGN KEY (createdby)
        REFERENCES public.users (id)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,

    CONSTRAINT fk_billings_modifiedby
        FOREIGN KEY (modifiedby)
        REFERENCES public.users (id)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,

    CONSTRAINT chk_billings_amounts
        CHECK (
            subtotal >= 0
            AND discountamount >= 0
            AND taxamount >= 0
            AND totalamount >= 0
            AND paidamount >= 0
            AND pendingamount >= 0
        ),

    CONSTRAINT chk_billings_status
        CHECK (
            status IN ('Pending', 'PartiallyPaid', 'Paid', 'Cancelled')
        )
);

CREATE INDEX IF NOT EXISTS idx_billings_purchaseorderid
    ON public.billings(purchaseorderid);

CREATE INDEX IF NOT EXISTS idx_billings_salesorderid
    ON public.billings(salesorderid);

CREATE INDEX IF NOT EXISTS idx_billings_dealerid
    ON public.billings(dealerid);

CREATE INDEX IF NOT EXISTS idx_billings_status
    ON public.billings(status);