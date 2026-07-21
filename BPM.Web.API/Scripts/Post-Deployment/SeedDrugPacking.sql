MERGE INTO public.drug_packaging AS target
USING
(
    SELECT
        d.drugid,
        p.uomid AS package_uomid,
        c.uomid AS contains_uomid,

        CASE
            WHEN p.uom_type = 'PACK' THEN 10
            WHEN p.uom_type = 'BOX' THEN 10
            WHEN p.uom_type = 'CARTON' THEN 20
        END AS quantity,

        CASE
            WHEN p.uom_type = 'PACK' THEN 10
            WHEN p.uom_type = 'BOX' THEN 100
            WHEN p.uom_type = 'CARTON' THEN 2000
        END AS total_units,

        CASE
            WHEN MOD(ABS(hashtext(d.drugcode)),5)=0 THEN 2.50
            WHEN MOD(ABS(hashtext(d.drugcode)),5)=1 THEN 4.00
            WHEN MOD(ABS(hashtext(d.drugcode)),5)=2 THEN 6.50
            WHEN MOD(ABS(hashtext(d.drugcode)),5)=3 THEN 9.00
            ELSE 12.00
        END AS unit_price

    FROM public.drug d
    INNER JOIN public.drug_uom p
        ON d.drugid = p.drugid
    INNER JOIN public.drug_uom c
        ON p.drugid = c.drugid

    WHERE
           (p.uom_type = 'PACK'    AND c.uom_type = 'BASE')
        OR (p.uom_type = 'BOX'     AND c.uom_type = 'PACK')
        OR (p.uom_type = 'CARTON'  AND c.uom_type = 'BOX')

) AS source
(
    drugid,
    package_uomid,
    contains_uomid,
    quantity,
    total_units,
    unit_price
)

ON
(
    target.drugid = source.drugid
    AND target.package_uomid = source.package_uomid
)

WHEN MATCHED THEN
UPDATE SET
    contains_uomid = source.contains_uomid,
    quantity = source.quantity,
    total_units = source.total_units,
    unit_price = source.unit_price,
    package_price = ROUND(source.unit_price * source.total_units, 2),
    isactive = TRUE

WHEN NOT MATCHED THEN
INSERT
(
    packagingid,
    drugid,
    package_uomid,
    contains_uomid,
    quantity,
    total_units,
    unit_price,
    package_price,
    barcode,
    gross_weight,
    net_weight,
    length,
    width,
    height,
    isactive,
    createdby,
    createdon
)
VALUES
(
    gen_random_uuid(),
    source.drugid,
    source.package_uomid,
    source.contains_uomid,
    source.quantity,
    source.total_units,
    source.unit_price,
    ROUND(source.unit_price * source.total_units, 2),
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    TRUE,
    NULL,
    CURRENT_TIMESTAMP
);