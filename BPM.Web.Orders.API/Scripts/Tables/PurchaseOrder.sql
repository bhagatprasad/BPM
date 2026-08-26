CREATE TABLE public.purchase_order_approvals
(
    id uuid NOT NULL DEFAULT gen_random_uuid(),
    purchaseorderid uuid NOT NULL,
    approverid uuid NOT NULL,
    approvallevel integer NOT NULL,
    status character varying(30) COLLATE pg_catalog."default" NOT NULL DEFAULT 'Pending'::character varying,
    comments character varying(500) COLLATE pg_catalog."default",
    actiondate timestamp without time zone,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    CONSTRAINT purchase_order_approvals_pkey PRIMARY KEY (id),
    CONSTRAINT fk_purchase_order_approvals_approver FOREIGN KEY (approverid)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE RESTRICT,
    CONSTRAINT fk_purchase_order_approvals_purchase_order FOREIGN KEY (purchaseorderid)
        REFERENCES public.purchase_orders (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.purchase_order_approvals
    OWNER to neondb_owner;
