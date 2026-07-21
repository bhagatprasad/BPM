MERGE INTO public.drug_forms AS target
USING
(
    VALUES
        ('TAB', 'Tablet', 'SOLID'),
        ('CAP', 'Capsule', 'SOLID'),
        ('INJ', 'Injection', 'LIQUID'),
        ('SYR', 'Syrup', 'LIQUID'),
        ('SUS', 'Suspension', 'LIQUID'),
        ('DRP', 'Drops', 'LIQUID'),
        ('CRM', 'Cream', 'SEMISOLID'),
        ('ONT', 'Ointment', 'SEMISOLID'),
        ('GEL', 'Gel', 'SEMISOLID'),
        ('PDR', 'Powder', 'SOLID'),
        ('SPY', 'Spray', 'LIQUID'),
        ('LOT', 'Lotion', 'LIQUID'),
        ('SOL', 'Solution', 'LIQUID'),
        ('DPS', 'Dispersion', 'LIQUID'),
        ('SUP', 'Suppository', 'SOLID'),
        ('PATCH', 'Transdermal Patch', 'SOLID'),
        ('INH', 'Inhaler', 'GAS'),
        ('AER', 'Aerosol', 'GAS')
) AS source(formcode, formname, formtype)
ON target.formcode = source.formcode

WHEN MATCHED THEN
    UPDATE SET
        formname   = source.formname,
        formtype   = source.formtype,
        isactive   = TRUE,
        modifiedon = CURRENT_TIMESTAMP

WHEN NOT MATCHED THEN
    INSERT
    (
        formid,
        formcode,
        formname,
        formtype,
        isactive,
        createdby,
        createdon
    )
    VALUES
    (
        gen_random_uuid(),
        source.formcode,
        source.formname,
        source.formtype,
        TRUE,
        NULL,
        CURRENT_TIMESTAMP
    );