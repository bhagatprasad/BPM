MERGE INTO public.drug_uom AS target
USING
(
    SELECT d.drugid,
           u.uom_code,
           u.uom_name,
           u.uom_type,
           u.display_order,
           u.is_base_unit,
           u.is_purchase_uom,
           u.is_sales_uom,
           u.is_inventory_uom,
           u.conversion_factor
    FROM public.drug d
    CROSS JOIN
    (
        VALUES
        ('EA','Each','BASE',1,TRUE,FALSE,TRUE,TRUE,1),
        ('STR','Strip','PACK',2,FALSE,FALSE,TRUE,TRUE,10),
        ('BOX','Box','BOX',3,FALSE,TRUE,TRUE,TRUE,100),
        ('CTN','Carton','CARTON',4,FALSE,TRUE,TRUE,TRUE,1000)
    ) u
    (
        uom_code,
        uom_name,
        uom_type,
        display_order,
        is_base_unit,
        is_purchase_uom,
        is_sales_uom,
        is_inventory_uom,
        conversion_factor
    )
) AS source
(
    drugid,
    uom_code,
    uom_name,
    uom_type,
    display_order,
    is_base_unit,
    is_purchase_uom,
    is_sales_uom,
    is_inventory_uom,
    conversion_factor
)

ON
(
    target.drugid=source.drugid
    AND target.uom_code=source.uom_code
)

WHEN MATCHED THEN
UPDATE SET

    uom_name=source.uom_name,
    uom_type=source.uom_type,
    display_order=source.display_order,
    conversion_factor=source.conversion_factor,
    is_purchase_uom=source.is_purchase_uom,
    is_sales_uom=source.is_sales_uom,
    is_inventory_uom=source.is_inventory_uom,
    modifiedon=CURRENT_TIMESTAMP

WHEN NOT MATCHED THEN

INSERT
(
    uomid,
    drugid,
    uom_code,
    uom_name,
    uom_type,
    conversion_factor,
    display_order,
    is_base_unit,
    is_purchase_uom,
    is_sales_uom,
    is_inventory_uom,
    isactive,
    createdon
)
VALUES
(
    gen_random_uuid(),
    source.drugid,
    source.uom_code,
    source.uom_name,
    source.uom_type,
    source.conversion_factor,
    source.display_order,
    source.is_base_unit,
    source.is_purchase_uom,
    source.is_sales_uom,
    source.is_inventory_uom,
    TRUE,
    CURRENT_TIMESTAMP
);