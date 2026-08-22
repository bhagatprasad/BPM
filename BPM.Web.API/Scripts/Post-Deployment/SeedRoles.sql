MERGE INTO Roles AS target
USING (
    VALUES
        ('SuperAdmin',  'SUPER_ADMIN', 'Super Administrator with unrestricted access to all system features.'),
        ('Administrator', 'ADMIN', 'System Administrator with full access.'),
        ('Operator', 'OPERATOR', 'Operator with access to manage daily operations.'),
        ('Dealer', 'DEALER', 'Dealer user with access to dealer-specific features.'),
        ('Distributor', 'DISTRIBUTOR', 'Distributor user with access to distributor-specific features.')
) AS source (Name, Code, Description)
ON target.Code = source.Code

WHEN MATCHED THEN
    UPDATE SET
        Name = source.Name,
        Description = source.Description,
        IsActive = TRUE,
        ModifiedOn = CURRENT_TIMESTAMP

WHEN NOT MATCHED THEN
    INSERT
    (
        Id,
        Name,
        Code,
        Description,
        IsActive,
        CreatedBy,
        CreatedOn
    )
    VALUES
    (
        gen_random_uuid(),
        source.Name,
        source.Code,
        source.Description,
        TRUE,
        NULL,
        CURRENT_TIMESTAMP
    );