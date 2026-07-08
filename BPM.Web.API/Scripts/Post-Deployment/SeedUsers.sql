MERGE INTO Users AS Target

USING
(
    -- Administrator
    SELECT
        'John' AS FirstName,
        'Admin' AS LastName,
        'admin@pharma.com' AS Email,
        '9000000001' AS Phone,
        r.Id AS RoleId,
        CAST(NULL AS UUID) AS DealerId,
        'PasswordHashHere' AS PasswordHash,
        'PasswordSaltHere' AS PasswordSalt,
        TRUE AS IsActive
    FROM Roles r
    WHERE r.Code = 'ADMINISTRATOR'

    UNION ALL

    SELECT
        'Ravi',
        'Kumar',
        'ravi@abcpharma.com',
        '9000000002',
        r.Id,
        d.Id,
        'PasswordHashHere',
        'PasswordSaltHere',
        TRUE
    FROM Roles r
    JOIN Dealers d
        ON d.RegistrationNumber = 'REG001'
    WHERE r.Code = 'DEALER'

    UNION ALL

    SELECT
        'Ramesh',
        'Kumar',
        'ramesh@xyzmedical.com',
        '9000000003',
        r.Id,
        d.Id,
        'PasswordHashHere',
        'PasswordSaltHere',
        TRUE
    FROM Roles r
    JOIN Dealers d
        ON d.RegistrationNumber = 'REG002'
    WHERE r.Code = 'DEALER'

    UNION ALL

    SELECT
        'Mahesh',
        'Reddy',
        'mahesh@medcare.com',
        '9000000004',
        r.Id,
        d.Id,
        'PasswordHashHere',
        'PasswordSaltHere',
        TRUE
    FROM Roles r
    JOIN Dealers d
        ON d.RegistrationNumber = 'REG003'
    WHERE r.Code = 'DEALER'

    UNION ALL

    SELECT
        'Suresh',
        'Rao',
        'suresh@srisai.com',
        '9000000005',
        r.Id,
        d.Id,
        'PasswordHashHere',
        'PasswordSaltHere',
        TRUE
    FROM Roles r
    JOIN Dealers d
        ON d.RegistrationNumber = 'REG004'
    WHERE r.Code = 'DEALER'

    UNION ALL

    SELECT
        'Praveen',
        'Kumar',
        'praveen@prime.com',
        '9000000006',
        r.Id,
        d.Id,
        'PasswordHashHere',
        'PasswordSaltHere',
        TRUE
    FROM Roles r
    JOIN Dealers d
        ON d.RegistrationNumber = 'REG005'
    WHERE r.Code = 'DEALER'

    UNION ALL

    SELECT
        'Naveen',
        'Kumar',
        'naveen@apollo.com',
        '9000000007',
        r.Id,
        d.Id,
        'PasswordHashHere',
        'PasswordSaltHere',
        TRUE
    FROM Roles r
    JOIN Dealers d
        ON d.RegistrationNumber = 'REG006'
    WHERE r.Code = 'DEALER'

    UNION ALL

    SELECT
        'Kiran',
        'Kumar',
        'kiran@caremedical.com',
        '9000000008',
        r.Id,
        d.Id,
        'PasswordHashHere',
        'PasswordSaltHere',
        TRUE
    FROM Roles r
    JOIN Dealers d
        ON d.RegistrationNumber = 'REG007'
    WHERE r.Code = 'DEALER'

    UNION ALL

    SELECT
        'Vinod',
        'Kumar',
        'vinod@healthplus.com',
        '9000000009',
        r.Id,
        d.Id,
        'PasswordHashHere',
        'PasswordSaltHere',
        TRUE
    FROM Roles r
    JOIN Dealers d
        ON d.RegistrationNumber = 'REG008'
    WHERE r.Code = 'DEALER'

    UNION ALL

    SELECT
        'Rajesh',
        'Kumar',
        'rajesh@medlife.com',
        '9000000010',
        r.Id,
        d.Id,
        'PasswordHashHere',
        'PasswordSaltHere',
        TRUE
    FROM Roles r
    JOIN Dealers d
        ON d.RegistrationNumber = 'REG009'
    WHERE r.Code = 'DEALER'

    UNION ALL

    SELECT
        'Srikanth',
        'Kumar',
        'srikanth@globalpharma.com',
        '9000000011',
        r.Id,
        d.Id,
        'PasswordHashHere',
        'PasswordSaltHere',
        TRUE
    FROM Roles r
    JOIN Dealers d
        ON d.RegistrationNumber = 'REG010'
    WHERE r.Code = 'DEALER'

) AS Source

ON Target.Email = Source.Email

WHEN MATCHED THEN
UPDATE SET
    FirstName = Source.FirstName,
    LastName = Source.LastName,
    Phone = Source.Phone,
    RoleId = Source.RoleId,
    DealerId = Source.DealerId,
    PasswordHash = Source.PasswordHash,
    PasswordSalt = Source.PasswordSalt,
    IsActive = Source.IsActive,
    ModifiedOn = CURRENT_TIMESTAMP

WHEN NOT MATCHED THEN
INSERT
(
    FirstName,
    LastName,
    Email,
    Phone,
    RoleId,
    DealerId,
    PasswordHash,
    PasswordSalt,
    IsActive,
    CreatedOn
)
VALUES
(
    Source.FirstName,
    Source.LastName,
    Source.Email,
    Source.Phone,
    Source.RoleId,
    Source.DealerId,
    Source.PasswordHash,
    Source.PasswordSalt,
    Source.IsActive,
    CURRENT_TIMESTAMP
);