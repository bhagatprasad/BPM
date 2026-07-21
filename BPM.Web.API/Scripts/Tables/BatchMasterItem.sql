CREATE TABLE IF NOT EXISTS public.batch_items
(
    batchitemid uuid NOT NULL DEFAULT gen_random_uuid(),
    batchid uuid NOT NULL,
    drugid uuid NOT NULL,
    formid uuid NOT NULL, -- Denormalized for faster queries
    uomid uuid NOT NULL,
    
    -- Drug Details in this Batch
    batchnumber character varying(100) NOT NULL, -- Individual drug batch number
    manufacturer_batch_number character varying(100),
    drug_code character varying(50), -- Denormalized from drug table
    drug_name character varying(200), -- Denormalized for quick display
    generic_name character varying(200),
    form_name character varying(100), -- Denormalized form name
    form_type character varying(50), -- Denormalized form type
    
    -- Quantities
    received_quantity integer NOT NULL,
    available_quantity integer NOT NULL DEFAULT 0, -- Available for sale
    reserved_quantity integer NOT NULL DEFAULT 0, -- Reserved for orders
    damaged_quantity integer NOT NULL DEFAULT 0,
    expired_quantity integer NOT NULL DEFAULT 0,
    returned_quantity integer NOT NULL DEFAULT 0,
    
    -- Per Unit Costs (This is what you need for each drug)
    unit_cost_price numeric(18,2) NOT NULL, -- Cost price per unit (what you paid)
    unit_mrp numeric(18,2) NOT NULL, -- Maximum Retail Price
    unit_selling_price numeric(18,2) NOT NULL, -- Your selling price
    
    -- Tax Details
    gst_rate numeric(5,2) DEFAULT 0,
    gst_amount numeric(18,2) DEFAULT 0,
    cess_rate numeric(5,2) DEFAULT 0,
    cess_amount numeric(18,2) DEFAULT 0,
    
    -- Discount Details (if any from supplier)
    trade_discount_percentage numeric(5,2) DEFAULT 0,
    trade_discount_amount numeric(18,2) DEFAULT 0,
    cash_discount_percentage numeric(5,2) DEFAULT 0,
    cash_discount_amount numeric(18,2) DEFAULT 0,
    
    -- Final Costs
    total_cost numeric(18,2) NOT NULL, -- (quantity * unit_cost_price) + tax - discounts
    total_selling_value numeric(18,2) NOT NULL, -- (quantity * unit_selling_price)
    
    -- Expiry Details
    manufacturing_date timestamp without time zone,
    expiry_date timestamp without time zone NOT NULL,
    
    -- Location Details (if different from batch master)
    warehouseid uuid,
    rack_location character varying(50),
    bin_location character varying(50),
    
    -- Storage Requirements (denormalized from form)
    storage_condition character varying(100),
    requires_prescription boolean DEFAULT true,
    
    -- Status
    item_status character varying(20) NOT NULL DEFAULT 'ACTIVE', 
    -- 'ACTIVE', 'PARTIALLY_SOLD', 'COMPLETELY_SOLD', 'EXPIRED', 'DAMAGED', 'QUARANTINED'
    
    isactive boolean NOT NULL DEFAULT true,
    createdby uuid,
    createdon timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    modifiedby uuid,
    modifiedon timestamp without time zone,
    
    CONSTRAINT batch_items_pkey PRIMARY KEY (batchitemid),
    CONSTRAINT batch_items_batch_fkey FOREIGN KEY (batchid) 
        REFERENCES public.batch_master(batchid) ON DELETE CASCADE,
    CONSTRAINT batch_items_drug_fkey FOREIGN KEY (drugid) 
        REFERENCES public.drug(drugid),
    CONSTRAINT batch_items_form_fkey FOREIGN KEY (formid) 
        REFERENCES public.drug_forms(formid),
    CONSTRAINT batch_items_uom_fkey FOREIGN KEY (uomid) 
        REFERENCES public.drug_uom(uomid),
    CONSTRAINT batch_items_unique UNIQUE (batchid, drugid, manufacturer_batch_number)
);