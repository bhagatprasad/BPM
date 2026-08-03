function DrugController() {
    var self = this;

    // Properties
    self.Drugs = [];
    self.filteredDrugs = [];
    self.currentPage = 1;
    self.pageSize = 10;
    self.totalPages = 1;
    self.searchTerm = '';

    // DOM references
    self.$tableBody = null;
    self.$pagination = null;
    self.$entriesInfo = null;
    self.$searchInput = null;
    self.$checkAll = null;

    self.init = function () {
        // Cache DOM elements
        self.$tableBody = $('#drugTableBody');
        self.$pagination = $('#paginationControls');
        self.$entriesInfo = $('#entriesInfo');
        self.$searchInput = $('#searchInput');
        self.$checkAll = $('#checkAll');

        // Bind events
        self.bindEvents();

        // Fetch data
        self.fetchDrugsAsync();
    };

    self.bindEvents = function () {
        // Search functionality
        self.$searchInput.on('keyup', function () {
            self.searchTerm = $(this).val().toLowerCase();
            self.currentPage = 1;
            self.render();
        });

        // Check All functionality
        self.$checkAll.on('change', function () {
            var checked = $(this).prop('checked');
            self.$tableBody.find('.item-checkbox').prop('checked', checked);
        });

        // Individual checkbox - update check all state
        self.$tableBody.on('change', '.item-checkbox', function () {
            var totalCheckboxes = self.$tableBody.find('.item-checkbox').length;
            var checkedCheckboxes = self.$tableBody.find('.item-checkbox:checked').length;
            self.$checkAll.prop('checked', totalCheckboxes === checkedCheckboxes);
        });
    };

    self.fetchDrugsAsync = function () {
        // Show loader
        showLoader('Loading drugs...');
        console.log('Loader shown - starting fetch...');

        makeAjaxRequest({
            url: '/Drug/GetAllDrugsList',
            type: 'GET',
            showLoader: false, // Manual control
            successCallback: function (response) {
                console.log('Response received - processing data...');

                if (response && Array.isArray(response)) {
                    // Process data - Map response to Drug objects
                    self.Drugs = response.map(function (drug) {
                        // Get UOMs and Packagings
                        var drugUoms = drug.DrugUoms || [];
                        var drugPackagings = drug.DrugPackagings || [];

                        // Find base UOM
                        var baseUom = drugUoms.find(function (uom) { return uom.IsBaseUnit === true; });
                        var purchaseUom = drugUoms.find(function (uom) { return uom.IsPurchaseUom === true; });
                        var salesUom = drugUoms.find(function (uom) { return uom.IsSalesUom === true; });

                        return {
                            DrugId: drug.DrugId || '',
                            DrugCode: drug.DrugCode || '',
                            DrugName: drug.DrugName || '',
                            GenericName: drug.GenericName || '',
                            BrandName: drug.BrandName || '',
                            Manufacturer: drug.Manufacturer || '',
                            Category: drug.Category || '',
                            HSNCode: drug.HSNCode || '',
                            ScheduleType: drug.ScheduleType || '',
                            Packing: drug.Packing || '',
                            Strength: drug.Strength || '',
                            IsActive: drug.IsActive === true,
                            DrugUoms: drugUoms,
                            DrugPackagings: drugPackagings,
                            BaseUomName: baseUom ? (baseUom.UomName || 'N/A') : 'N/A',
                            PurchaseUomName: purchaseUom ? (purchaseUom.UomName || 'N/A') : 'N/A',
                            SalesUomName: salesUom ? (salesUom.UomName || 'N/A') : 'N/A',
                            TotalUoms: drugUoms.length,
                            TotalPackagings: drugPackagings.length,
                            selected: false,
                            expanded: false
                        };
                    });

                    console.log('Mapped drugs:', self.Drugs);

                    // Initialize filtered drugs
                    self.filteredDrugs = self.Drugs;

                    // Render the table
                    console.log('Rendering table...');
                    self.render();

                    // Hide loader after rendering with a small delay
                    // to ensure DOM is updated
                    setTimeout(function () {
                        console.log('Hiding loader...');
                        hideLoader();
                    }, 300);

                } else {
                    console.error('Invalid response');
                    self.$tableBody.html(
                        '<tr><td colspan="10" class="text-center py-4 text-danger">' +
                        '<i class="material-symbols-outlined fs-40 mb-2 d-block">error</i>' +
                        'Invalid data format received from server.</td></tr>'
                    );
                    hideLoader();
                }
            },
            errorCallback: function (xhr, status, error) {
                console.log('Failed to fetch drugs:', error);
                self.$tableBody.html(
                    '<tr><td colspan="10" class="text-center py-4 text-danger">' +
                    '<i class="material-symbols-outlined fs-40 mb-2 d-block">error</i>' +
                    'Failed to load drugs. Please try again.</td></tr>'
                );
                hideLoader();
            }
        });
    };

    self.filterDrugs = function () {
        if (!self.searchTerm) {
            self.filteredDrugs = self.Drugs;
        } else {
            self.filteredDrugs = self.Drugs.filter(function (drug) {
                return (drug.DrugName && drug.DrugName.toLowerCase().includes(self.searchTerm)) ||
                    (drug.DrugCode && drug.DrugCode.toLowerCase().includes(self.searchTerm)) ||
                    (drug.GenericName && drug.GenericName.toLowerCase().includes(self.searchTerm)) ||
                    (drug.BrandName && drug.BrandName.toLowerCase().includes(self.searchTerm)) ||
                    (drug.Manufacturer && drug.Manufacturer.toLowerCase().includes(self.searchTerm)) ||
                    (drug.Category && drug.Category.toLowerCase().includes(self.searchTerm)) ||
                    (drug.HSNCode && drug.HSNCode.toLowerCase().includes(self.searchTerm));
            });
        }
        self.totalPages = Math.ceil(self.filteredDrugs.length / self.pageSize);
        if (self.currentPage > self.totalPages) {
            self.currentPage = self.totalPages || 1;
        }
    };

    self.getCurrentPageDrugs = function () {
        var start = (self.currentPage - 1) * self.pageSize;
        var end = start + self.pageSize;
        return self.filteredDrugs.slice(start, end);
    };

    self.render = function () {
        self.filterDrugs();
        self.renderTableBody();
        self.renderPagination();
        self.renderEntriesInfo();
    };

    self.renderTableBody = function () {
        var currentPageDrugs = self.getCurrentPageDrugs();

        console.log('Rendering drugs:', currentPageDrugs);

        if (currentPageDrugs.length === 0) {
            self.$tableBody.html(
                '<tr><td colspan="10" class="text-center py-4 text-muted">' +
                '<i class="material-symbols-outlined fs-40 mb-2 d-block">inbox</i>' +
                'No drugs found</td></tr>'
            );
            return;
        }

        var html = '';
        currentPageDrugs.forEach(function (drug, index) {
            // Main row
            html += '<tr data-drug-id="' + drug.DrugId + '" data-expanded="false">';
            html += '<td class="text-body" style="width: 80px;">';
            html += '<div class="d-flex align-items-center" style="gap: 6px;">';
            // Checkbox
            html += '<div class="form-check mb-0">';
            html += '<input class="form-check-input item-checkbox" type="checkbox" ' + (drug.selected ? 'checked' : '') + ' />';
            html += '</div>';
            // Expand button with > icon
            html += '<button class="bg-transparent p-0 border-0 expand-btn" data-drug-id="' + drug.DrugId + '" data-bs-placement="top" data-bs-title="Expand/Collapse" data-bs-toggle="tooltip">';
            html += '<span class="expand-icon" style="font-size: 18px; font-weight: bold; color: #6c757d; cursor: pointer; transition: transform 0.3s;">&#9654;</span>';
            html += '</button>';
            html += '</div>';
            html += '</td>';
            html += '<td class="text-body"><span class="fw-semibold">' + self.escapeHtml(drug.DrugCode || '') + '</span></td>';
            html += '<td><div class="d-flex align-items-center">';
            html += '<span class="fs-16 fw-medium text-secondary">' + self.escapeHtml(drug.DrugName || '') + '</span>';
            html += '</div></td>';
            html += '<td class="text-body">' + self.escapeHtml(drug.GenericName || 'N/A') + '</td>';
            html += '<td class="text-body">' + self.escapeHtml(drug.BrandName || 'N/A') + '</td>';
            html += '<td class="text-body">' + self.escapeHtml(drug.Manufacturer || 'N/A') + '</td>';
            html += '<td class="text-body">' + self.escapeHtml(drug.Category || 'N/A') + '</td>';
            html += '<td>' + self.getStatusBadge(drug.IsActive) + '</td>';
            html += '<td>';
            html += '<div class="d-flex justify-content-end" style="gap: 6px;">';
            // View button - Blue
            html += '<button class="btn btn-sm btn-outline-primary view-btn" data-drug-id="' + drug.DrugId + '" data-bs-placement="top" data-bs-title="View" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
            html += '<i class="material-symbols-outlined fs-16">visibility</i>';
            html += '</button>';

            // Activate/Deactivate button based on status
            if (drug.IsActive) {
                // Deactivate button - Orange/Warning
                html += '<button class="btn btn-sm btn-outline-warning deactivate-btn" data-drug-id="' + drug.DrugId + '" data-bs-placement="top" data-bs-title="Deactivate" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
                html += '<i class="material-symbols-outlined fs-16">block</i>';
                html += '</button>';
            } else {
                // Activate button - Green
                html += '<button class="btn btn-sm btn-outline-success activate-btn" data-drug-id="' + drug.DrugId + '" data-bs-placement="top" data-bs-title="Activate" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
                html += '<i class="material-symbols-outlined fs-16">check_circle</i>';
                html += '</button>';
            }

            // Delete button - Red
            html += '<button class="btn btn-sm btn-outline-danger delete-btn" data-drug-id="' + drug.DrugId + '" data-bs-placement="top" data-bs-title="Delete" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
            html += '<i class="material-symbols-outlined fs-16">delete</i>';
            html += '</button>';
            html += '</div>';
            html += '</td>';
            html += '</tr>';

            // Expanded row for details
            html += '<tr class="expanded-row" data-drug-id="' + drug.DrugId + '" style="display:none;">';
            html += '<td colspan="10" class="p-0">';
            html += '<div class="p-20 bg-light rounded-3 mb-10" style="margin: 0 15px 15px 15px; background-color: #f8f9fa !important;">';

            // Drug Information in Grid Format
            html += '<h6 class="mb-15">Drug Information</h6>';
            html += '<div class="table-responsive">';
            html += '<table class="table table-sm table-bordered mb-15">';
            html += '<tbody>';
            html += '<tr>';
            html += '<td style="width: 150px;"><strong>Drug Code</strong></td>';
            html += '<td>' + self.escapeHtml(drug.DrugCode || 'N/A') + '</td>';
            html += '<td style="width: 150px;"><strong>Drug Name</strong></td>';
            html += '<td>' + self.escapeHtml(drug.DrugName || 'N/A') + '</td>';
            html += '</tr>';
            html += '<tr>';
            html += '<td><strong>Generic Name</strong></td>';
            html += '<td>' + self.escapeHtml(drug.GenericName || 'N/A') + '</td>';
            html += '<td><strong>Brand Name</strong></td>';
            html += '<td>' + self.escapeHtml(drug.BrandName || 'N/A') + '</td>';
            html += '</tr>';
            html += '<tr>';
            html += '<td><strong>Manufacturer</strong></td>';
            html += '<td>' + self.escapeHtml(drug.Manufacturer || 'N/A') + '</td>';
            html += '<td><strong>Category</strong></td>';
            html += '<td>' + self.escapeHtml(drug.Category || 'N/A') + '</td>';
            html += '</tr>';
            html += '<tr>';
            html += '<td><strong>HSN Code</strong></td>';
            html += '<td>' + self.escapeHtml(drug.HSNCode || 'N/A') + '</td>';
            html += '<td><strong>Schedule Type</strong></td>';
            html += '<td>' + self.escapeHtml(drug.ScheduleType || 'N/A') + '</td>';
            html += '</tr>';
            html += '<tr>';
            html += '<td><strong>Packing</strong></td>';
            html += '<td>' + self.escapeHtml(drug.Packing || 'N/A') + '</td>';
            html += '<td><strong>Strength</strong></td>';
            html += '<td>' + self.escapeHtml(drug.Strength || 'N/A') + '</td>';
            html += '</tr>';
            html += '<tr>';
            html += '<td><strong>Status</strong></td>';
            html += '<td colspan="3">' + (drug.IsActive ? '<span class="text-success">Active</span>' : '<span class="text-danger">Inactive</span>') + '</td>';
            html += '</tr>';
            html += '</tbody>';
            html += '</table>';
            html += '</div>';

            // UOM Information
            html += '<h6 class="mb-15 mt-15">UOM Information</h6>';
            if (drug.DrugUoms && drug.DrugUoms.length > 0) {
                html += '<div class="table-responsive">';
                html += '<table class="table table-sm table-bordered mb-15">';
                html += '<thead class="bg-white">';
                html += '<tr>';
                html += '<th>UOM Name</th>';
                html += '<th>Code</th>';
                html += '<th>Type</th>';
                html += '<th>Base</th>';
                html += '<th>Purchase</th>';
                html += '<th>Sales</th>';
                html += '<th>Status</th>';
                html += '</tr>';
                html += '</thead>';
                html += '<tbody>';
                drug.DrugUoms.forEach(function (uom) {
                    html += '<tr>';
                    html += '<td>' + self.escapeHtml(uom.UomName || 'N/A') + '</td>';
                    html += '<td>' + self.escapeHtml(uom.UomCode || 'N/A') + '</td>';
                    html += '<td>' + self.escapeHtml(uom.UomType || 'N/A') + '</td>';
                    html += '<td>' + (uom.IsBaseUnit ? '<span class="text-success"><i class="material-symbols-outlined fs-16">check_circle</i></span>' : '<span class="text-muted">-</span>') + '</td>';
                    html += '<td>' + (uom.IsPurchaseUom ? '<span class="text-success"><i class="material-symbols-outlined fs-16">check_circle</i></span>' : '<span class="text-muted">-</span>') + '</td>';
                    html += '<td>' + (uom.IsSalesUom ? '<span class="text-success"><i class="material-symbols-outlined fs-16">check_circle</i></span>' : '<span class="text-muted">-</span>') + '</td>';
                    html += '<td>' + (uom.IsActive ? '<span class="text-success">Active</span>' : '<span class="text-danger">Inactive</span>') + '</td>';
                    html += '</tr>';
                });
                html += '</tbody>';
                html += '</table>';
                html += '</div>';
            } else {
                html += '<div class="text-muted mb-15">No UOMs available</div>';
            }

            // Packaging Information
            html += '<h6 class="mb-15">Packaging Information</h6>';
            if (drug.DrugPackagings && drug.DrugPackagings.length > 0) {
                html += '<div class="table-responsive">';
                html += '<table class="table table-sm table-bordered">';
                html += '<thead class="bg-white">';
                html += '<tr>';
                html += '<th>Package UOM</th>';
                html += '<th>Contains UOM</th>';
                html += '<th>Quantity</th>';
                html += '<th>Total Units</th>';
                html += '<th>Unit Price</th>';
                html += '<th>Package Price</th>';
                html += '<th>Barcode</th>';
                html += '<th>Status</th>';
                html += '</tr>';
                html += '</thead>';
                html += '<tbody>';
                drug.DrugPackagings.forEach(function (packaging) {
                    html += '<tr>';
                    html += '<td>' + self.escapeHtml(packaging.PackageUomName || packaging.PackageUomCode || 'N/A') + '</td>';
                    html += '<td>' + self.escapeHtml(packaging.ContainsUomName || packaging.ContainsUomCode || 'N/A') + '</td>';
                    html += '<td>' + (packaging.Quantity || 0) + '</td>';
                    html += '<td>' + (packaging.TotalUnits || 0) + '</td>';
                    html += '<td>$' + (packaging.UnitPrice || 0).toFixed(2) + '</td>';
                    html += '<td>$' + (packaging.PackagePrice || 0).toFixed(2) + '</td>';
                    html += '<td>' + self.escapeHtml(packaging.Barcode || 'N/A') + '</td>';
                    html += '<td>' + (packaging.IsActive ? '<span class="text-success">Active</span>' : '<span class="text-danger">Inactive</span>') + '</td>';
                    html += '</tr>';
                });
                html += '</tbody>';
                html += '</table>';
                html += '</div>';
            } else {
                html += '<div class="text-muted">No packaging information available</div>';
            }

            html += '</div>';
            html += '</td>';
            html += '</tr>';
        });

        self.$tableBody.html(html);

        // Bind expand/collapse events with rotation animation
        self.$tableBody.find('.expand-btn').on('click', function () {
            var drugId = $(this).data('drug-id');
            self.toggleExpand(drugId);
        });

        // Re-bind checkbox events for new checkboxes
        self.$tableBody.find('.item-checkbox').off('change').on('change', function () {
            var totalCheckboxes = self.$tableBody.find('.item-checkbox').length;
            var checkedCheckboxes = self.$tableBody.find('.item-checkbox:checked').length;
            self.$checkAll.prop('checked', totalCheckboxes === checkedCheckboxes);
        });

        // Bind View, Activate, Deactivate, Delete events
        self.$tableBody.find('.view-btn').on('click', function () {
            var drugId = $(this).data('drug-id');
            self.viewDrug(drugId);
        });

        self.$tableBody.find('.activate-btn').on('click', function () {
            var drugId = $(this).data('drug-id');
            self.activateDrug(drugId);
        });

        self.$tableBody.find('.deactivate-btn').on('click', function () {
            var drugId = $(this).data('drug-id');
            self.deactivateDrug(drugId);
        });

        self.$tableBody.find('.delete-btn').on('click', function () {
            var drugId = $(this).data('drug-id');
            self.deleteDrug(drugId);
        });
    };

    self.toggleExpand = function (drugId) {
        var expandedRow = self.$tableBody.find('.expanded-row[data-drug-id="' + drugId + '"]');
        var expandIcon = self.$tableBody.find('.expand-btn[data-drug-id="' + drugId + '"] .expand-icon');

        if (expandedRow.is(':visible')) {
            expandedRow.hide();
            expandIcon.css('transform', 'rotate(0deg)');
        } else {
            expandedRow.show();
            expandIcon.css('transform', 'rotate(90deg)');
        }
    };

    self.viewDrug = function (drugId) {
        console.log('View drug:', drugId);
        var drug = self.Drugs.find(function (d) { return d.DrugId === drugId; });
        if (drug) {
            var message = 'Viewing Drug:\n\n' +
                'Code: ' + drug.DrugCode + '\n' +
                'Name: ' + drug.DrugName + '\n' +
                'Generic: ' + (drug.GenericName || 'N/A') + '\n' +
                'Brand: ' + (drug.BrandName || 'N/A') + '\n' +
                'Manufacturer: ' + (drug.Manufacturer || 'N/A') + '\n' +
                'Category: ' + (drug.Category || 'N/A') + '\n' +
                'HSN Code: ' + (drug.HSNCode || 'N/A') + '\n' +
                'Schedule Type: ' + (drug.ScheduleType || 'N/A') + '\n' +
                'Packing: ' + (drug.Packing || 'N/A') + '\n' +
                'Strength: ' + (drug.Strength || 'N/A') + '\n' +
                'Total UOMs: ' + drug.TotalUoms + '\n' +
                'Total Packagings: ' + drug.TotalPackagings + '\n' +
                'Status: ' + (drug.IsActive ? 'Active' : 'Inactive');
            alert(message);
        }
    };

    self.activateDrug = function (drugId) {
        console.log('Activate drug:', drugId);
        if (confirm('Are you sure you want to activate this drug?')) {
            var drug = self.Drugs.find(function (d) { return d.DrugId === drugId; });
            if (drug) {
                // Show loader manually
                showLoader('Activating drug...');

                makeAjaxRequest({
                    url: '/Drug/ActivateDrug',
                    type: 'POST',
                    data: JSON.stringify(drugId),
                    contentType: 'application/json; charset=utf-8',
                    showLoader: false,
                    successCallback: function (response) {
                        if (response.success) {
                            drug.IsActive = true;
                            self.render();
                            // Hide loader after render
                            setTimeout(function () {
                                hideLoader();
                                alert('Drug activated successfully!');
                            }, 300);
                        } else {
                            hideLoader();
                            alert(response.message || 'Failed to activate drug.');
                        }
                    },
                    errorCallback: function (xhr, status, error) {
                        hideLoader();
                        alert('Failed to activate drug. Please try again.');
                    }
                });
            }
        }
    };

    self.deactivateDrug = function (drugId) {
        console.log('Deactivate drug:', drugId);
        if (confirm('Are you sure you want to deactivate this drug?')) {
            var drug = self.Drugs.find(function (d) { return d.DrugId === drugId; });
            if (drug) {
                // Show loader manually
                showLoader('Deactivating drug...');

                makeAjaxRequest({
                    url: '/Drug/DeactivateDrug',
                    type: 'POST',
                    data: JSON.stringify(drugId),
                    contentType: 'application/json; charset=utf-8',
                    showLoader: false,
                    successCallback: function (response) {
                        if (response.success) {
                            drug.IsActive = false;
                            self.render();
                            setTimeout(function () {
                                hideLoader();
                                alert('Drug deactivated successfully!');
                            }, 300);
                        } else {
                            hideLoader();
                            alert(response.message || 'Failed to deactivate drug.');
                        }
                    },
                    errorCallback: function (xhr, status, error) {
                        hideLoader();
                        alert('Failed to deactivate drug. Please try again.');
                    }
                });
            }
        }
    };

    self.deleteDrug = function (drugId) {
        console.log('Delete drug:', drugId);
        if (confirm('Are you sure you want to delete this drug? This action cannot be undone.')) {
            var drug = self.Drugs.find(function (d) { return d.DrugId === drugId; });
            if (drug) {
                // Show loader manually
                showLoader('Deleting drug...');

                makeAjaxRequest({
                    url: '/Drug/DeleteDrug',
                    type: 'POST',
                    data: JSON.stringify(drugId),
                    contentType: 'application/json; charset=utf-8',
                    showLoader: false,
                    successCallback: function (response) {
                        if (response.success) {
                            // Remove drug from list
                            var index = self.Drugs.indexOf(drug);
                            if (index !== -1) {
                                self.Drugs.splice(index, 1);
                            }
                            self.render();
                            setTimeout(function () {
                                hideLoader();
                                alert('Drug deleted successfully!');
                            }, 300);
                        } else {
                            hideLoader();
                            alert(response.message || 'Failed to delete drug.');
                        }
                    },
                    errorCallback: function (xhr, status, error) {
                        hideLoader();
                        alert('Failed to delete drug. Please try again.');
                    }
                });
            }
        }
    };

    self.renderPagination = function () {
        if (self.totalPages <= 1) {
            self.$pagination.html('');
            return;
        }

        var html = '';
        html += '<li class="page-item' + (self.currentPage === 1 ? ' disabled' : '') + '">';
        html += '<button aria-label="Previous" class="page-link icon" data-page="prev">';
        html += '<i class="material-symbols-outlined">west</i>';
        html += '</button>';
        html += '</li>';

        var maxPages = 5;
        var start = Math.max(1, self.currentPage - Math.floor(maxPages / 2));
        var end = Math.min(self.totalPages, start + maxPages - 1);

        if (end - start < maxPages - 1) {
            start = Math.max(1, end - maxPages + 1);
        }

        for (var i = start; i <= end; i++) {
            html += '<li class="page-item' + (i === self.currentPage ? ' active' : '') + '">';
            html += '<button class="page-link" data-page="' + i + '">' + i + '</button>';
            html += '</li>';
        }

        html += '<li class="page-item' + (self.currentPage === self.totalPages ? ' disabled' : '') + '">';
        html += '<button aria-label="Next" class="page-link icon" data-page="next">';
        html += '<i class="material-symbols-outlined">east</i>';
        html += '</button>';
        html += '</li>';

        self.$pagination.html(html);

        // Bind pagination events
        self.$pagination.find('.page-link').on('click', function () {
            var page = $(this).data('page');
            if (page === 'prev') {
                self.currentPage--;
            } else if (page === 'next') {
                self.currentPage++;
            } else {
                self.currentPage = parseInt(page);
            }
            self.render();
        });
    };

    self.renderEntriesInfo = function () {
        var total = self.filteredDrugs.length;
        var start = total > 0 ? (self.currentPage - 1) * self.pageSize + 1 : 0;
        var end = Math.min(self.currentPage * self.pageSize, total);

        self.$entriesInfo.text('Showing ' + start + ' to ' + end + ' of ' + total + ' entries');
    };

    self.getStatusBadge = function (isActive) {
        if (isActive) {
            return '<span class="fs-15 fw-normal d-inline-block default-badge text-success bg-success bg-opacity-10">Active</span>';
        } else {
            return '<span class="fs-15 fw-normal d-inline-block default-badge text-danger bg-danger bg-opacity-10">Inactive</span>';
        }
    };

    self.escapeHtml = function (text) {
        if (!text) return '';
        var div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    };
}