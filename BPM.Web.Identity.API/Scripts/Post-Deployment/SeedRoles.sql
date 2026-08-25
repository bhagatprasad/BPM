MERGE INTO Roles AS target

USING (

    VALUES

        ('Administrator', 'ADMIN', 'System Administrator with full access.'),

        ('Operator', 'OPERATOR', 'Operator with access to manage daily operations.'),

        ('Dealer', 'DEALER', 'Dealer user with access to dealer-specific features.')

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
 