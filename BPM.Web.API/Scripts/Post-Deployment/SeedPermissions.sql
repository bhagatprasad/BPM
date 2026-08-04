INSERT INTO Permissions
(
    RoleId,
    FeatureId,
    ActivityId,
    IsEnabled,
    CreatedBy,
    CreatedOn,
    ModifiedBy,
    ModifiedOn
)
SELECT
    r.id,
    f.FeatureId,
    a.ActivityId,
    FALSE,
    NULL,
    CURRENT_TIMESTAMP,
    NULL,
    NULL
FROM public.roles r
CROSS JOIN Features f
CROSS JOIN Activities a
ON CONFLICT (RoleId, FeatureId, ActivityId)
DO NOTHING;
