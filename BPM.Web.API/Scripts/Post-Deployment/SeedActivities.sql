INSERT INTO Activities
(
    ActivityName,
    Code,
    Description
)
VALUES
('Add',    'ADD', 'Add Record'),
('Edit',   'EDT', 'Edit Record'),
('Delete', 'DEL', 'Delete Record'),
('View',   'VIE', 'View Record'),
('Import', 'IMP', 'Import Record'),
('Export', 'EXP', 'Export Record'),
('Update', 'UPD', 'Update Record'),
('Save',   'SAV', 'Save Record'),
('Copy',   'CPY', 'Copy Record')
ON CONFLICT (Code) DO NOTHING;
