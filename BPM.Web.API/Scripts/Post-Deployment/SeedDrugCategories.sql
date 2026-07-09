MERGE INTO DrugCategories AS Target

USING
(
    VALUES
    (
        'CAT001',
        'Tablet',
        'Solid dosage form administered orally.',
        TRUE
    ),
    (
        'CAT002',
        'Capsule',
        'Medicine enclosed in a hard or soft gelatin shell.',
        TRUE
    ),
    (
        'CAT003',
        'Syrup',
        'Liquid oral medication with sweetened base.',
        TRUE
    ),
    (
        'CAT004',
        'Injection',
        'Sterile medication administered by injection.',
        TRUE
    ),
    (
        'CAT005',
        'Ointment',
        'Semi-solid preparation for external application.',
        TRUE
    ),
    (
        'CAT006',
        'Cream',
        'Semi-solid topical formulation.',
        TRUE
    ),
    (
        'CAT007',
        'Drops',
        'Liquid medicine used for eyes, ears, or nose.',
        TRUE
    ),
    (
        'CAT008',
        'Powder',
        'Dry powdered pharmaceutical preparation.',
        TRUE
    ),
    (
        'CAT009',
        'Gel',
        'Transparent or semi-solid topical preparation.',
        TRUE
    ),
    (
        'CAT010',
        'Lotion',
        'Liquid preparation for external application.',
        TRUE
    )

) AS Source
(
    CategoryCode,
    CategoryName,
    Description,
    IsActive
)

ON Target.CategoryCode = Source.CategoryCode

WHEN MATCHED THEN
UPDATE SET
    CategoryName = Source.CategoryName,
    Description = Source.Description,
    IsActive = Source.IsActive,
    ModifiedOn = CURRENT_TIMESTAMP

WHEN NOT MATCHED THEN
INSERT
(
    CategoryCode,
    CategoryName,
    Description,
    IsActive,
    CreatedOn
)
VALUES
(
    Source.CategoryCode,
    Source.CategoryName,
    Source.Description,
    Source.IsActive,
    CURRENT_TIMESTAMP
);
