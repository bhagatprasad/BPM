CREATE TABLE Permissions
(
    PermissionId UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    RoleId UUID NOT NULL,

    FeatureId UUID NOT NULL,

    ActivityId UUID NOT NULL,

    IsEnabled BOOLEAN NOT NULL DEFAULT FALSE,

    CreatedBy UUID,

    CreatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    ModifiedBy UUID,

    ModifiedOn TIMESTAMP,

    CONSTRAINT FK_Permissions_Roles
        FOREIGN KEY (RoleId)
        REFERENCES roles(id)
        ON DELETE CASCADE,

    CONSTRAINT FK_Permissions_Feature
        FOREIGN KEY (FeatureId)
        REFERENCES Feature(FeatureId)
        ON DELETE CASCADE,

    CONSTRAINT FK_Permissions_Activities
        FOREIGN KEY (ActivityId)
        REFERENCES Activities(ActivityId)
        ON DELETE CASCADE,

    CONSTRAINT UQ_Permissions
        UNIQUE (RoleId, FeatureId, ActivityId)
);