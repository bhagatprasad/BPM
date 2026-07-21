-- Batch Master - Represents a complete shipment/batch received
CREATE TABLE public.batch_master
(
    batchid uuid NOT NULL DEFAULT gen_random_uuid(),
    batchnumber character varying(100)  NULL,
    batch_reference character varying(100), -- Supplier's batch reference
    
    -- Supplier Details
    supplierid uuid  NULL,
    supplier_invoice_number character varying(100),
    supplier_invoice_date timestamp without time zone,
    
    -- Batch Details
    received_date timestamp without time zone  NULL DEFAULT CURRENT_TIMESTAMP,
    manufacturing_date timestamp without time zone,
    expiry_date timestamp without time zone  NULL,
    
    -- Financial Summary
    total_quantity integer  NULL DEFAULT 0,
    total_value numeric(18,2)  NULL DEFAULT 0,
    total_tax numeric(18,2) DEFAULT 0,
    total_discount numeric(18,2) DEFAULT 0,
    net_amount numeric(18,2)  NULL DEFAULT 0,
    
    -- Status
    batch_status character varying(20) NOT NULL DEFAULT 'ACTIVE', -- 'ACTIVE', 'PARTIALLY_USED', 'COMPLETELY_USED', 'EXPIRED', 'QUARANTINED'
    
    -- Warehouse & Location
    warehouseid uuid,
    storage_location character varying(100),
    
    -- Additional
    payment_terms character varying(100),
    delivery_terms character varying(100),
    remarks text,
    
    isactive boolean  NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone  NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    
    CONSTRAINT batch_master_pkey PRIMARY KEY (batchid)
    );