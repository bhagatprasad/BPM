CREATE TABLE public.purchaseorderitems
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),

    purchaseorderid uuid NOT NULL,

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

    batchnumber character varying(100),

    expirydate timestamp,

    remarks character varying(500),

    createdby uuid,

    createdon timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,

    modifiedby uuid,

    modifiedon timestamp,

    CONSTRAINT purchaseorderitems_pkey PRIMARY KEY (id),

    CONSTRAINT fk_purchaseorderitems_purchaseorder
        FOREIGN KEY (purchaseorderid)
        REFERENCES public.purchaseorders(id),

    CONSTRAINT fk_purchaseorderitems_drug
        FOREIGN KEY (drugid)
        REFERENCES public.drug(drugid)
);
