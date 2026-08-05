INSERT INTO Feature
(
    FeatureName,
    Code,
    CreatedBy,
    CreatedOn,
    ModifiedBy,
    ModifiedOn,
    IsActive
)
VALUES
('AccountController', 'Account', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('DealerController', 'Dealer', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('DrugCategoryController', 'DrugCategory', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('DrugController', 'Drug', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('DrugFormController', 'DrugForm', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('DrugPackagingController', 'DrugPackaging', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('DrugUomController', 'DrugUOM', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('ManufacturerController', 'Manufacturer', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('PackagingMasterController', 'PackagingMaster', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('PurchaseOrderController', 'PurchaseOrder', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('RoleController', 'Role', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('SupplierController', 'Supplier', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('UserController', 'User', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('ActivityController', 'Activity', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE),
('FeatureController', 'Feature', NULL, CURRENT_TIMESTAMP, NULL, NULL, TRUE)
ON CONFLICT (Code) DO NOTHING;
