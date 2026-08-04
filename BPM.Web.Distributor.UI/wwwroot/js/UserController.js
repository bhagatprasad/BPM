function UserController() {
    var self = this;

    // Properties
    self.Users = [];
    self.filteredUsers = [];
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
        self.$tableBody = $('#userTableBody');
        self.$pagination = $('#paginationControls');
        self.$entriesInfo = $('#entriesInfo');
        self.$searchInput = $('#searchInput');
        self.$checkAll = $('#checkAll');

        // Bind events
        self.bindEvents();

        // Fetch data
        self.fetchUsersAsync();
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

    self.fetchUsersAsync = function () {
        makeAjaxRequest({
            url: '/User/GetAllUsersList',
            type: 'GET',
            showLoader: true,
            successCallback: function (response) {
                console.log('Users response:', response);

                // Map response data
                if (response && Array.isArray(response)) {
                    self.Users = response.map(function (user) {
                        // Debug log to see user object
                        console.log('Processing user:', user);

                        // Get dealer info
                        var dealerInfo = user.DealerInfo || null;

                        return {
                            UserId: user.UserId || '',
                            FirstName: user.FirstName || '',
                            LastName: user.LastName || '',
                            Email: user.Email || '',
                            Phone: user.Phone || '',
                            DealerId: user.DealerId || null,
                            RoleId: user.RoleId || null,
                            DealerInfo: dealerInfo,
                            RoleInfo: user.RoleInfo || null,
                            FullName: (user.FirstName || '') + ' ' + (user.LastName || ''),
                            DealershipName: dealerInfo ? (dealerInfo.DealershipName || 'N/A') : 'N/A',
                            RoleName: user.RoleInfo ? (user.RoleInfo.Name || 'N/A') : 'N/A',
                            IsActive: dealerInfo ? (dealerInfo.IsActive === true) : true,
                            selected: false,
                            expanded: false
                        };
                    });

                    console.log('Mapped users:', self.Users);

                    // Initialize filtered users
                    self.filteredUsers = self.Users;

                    // Render the table
                    self.render();
                } else {
                    console.error('Invalid response format:', response);
                    self.$tableBody.html(
                        '<tr><td colspan="10" class="text-center py-4 text-danger">' +
                        '<i class="material-symbols-outlined fs-40 mb-2 d-block">error</i>' +
                        'Invalid data format received from server.</td></tr>'
                    );
                }
            },
            errorCallback: function (xhr, status, error) {
                console.log('Failed to fetch users:', error);
                // Show error message in table
                self.$tableBody.html(
                    '<tr><td colspan="10" class="text-center py-4 text-danger">' +
                    '<i class="material-symbols-outlined fs-40 mb-2 d-block">error</i>' +
                    'Failed to load users. Please try again.</td></tr>'
                );
            }
        });
    };

    self.filterUsers = function () {
        if (!self.searchTerm) {
            self.filteredUsers = self.Users;
        } else {
            self.filteredUsers = self.Users.filter(function (user) {
                return (user.FullName && user.FullName.toLowerCase().includes(self.searchTerm)) ||
                    (user.Email && user.Email.toLowerCase().includes(self.searchTerm)) ||
                    (user.Phone && user.Phone.toLowerCase().includes(self.searchTerm)) ||
                    (user.DealershipName && user.DealershipName.toLowerCase().includes(self.searchTerm)) ||
                    (user.RoleName && user.RoleName.toLowerCase().includes(self.searchTerm));
            });
        }
        self.totalPages = Math.ceil(self.filteredUsers.length / self.pageSize);
        if (self.currentPage > self.totalPages) {
            self.currentPage = self.totalPages || 1;
        }
    };

    self.getCurrentPageUsers = function () {
        var start = (self.currentPage - 1) * self.pageSize;
        var end = start + self.pageSize;
        return self.filteredUsers.slice(start, end);
    };

    self.render = function () {
        self.filterUsers();
        self.renderTableBody();
        self.renderPagination();
        self.renderEntriesInfo();
    };

    self.renderTableBody = function () {
        var currentPageUsers = self.getCurrentPageUsers();

        console.log('Rendering users:', currentPageUsers);

        if (currentPageUsers.length === 0) {
            self.$tableBody.html(
                '<tr><td colspan="10" class="text-center py-4 text-muted">' +
                '<i class="material-symbols-outlined fs-40 mb-2 d-block">inbox</i>' +
                'No users found</td></tr>'
            );
            return;
        }

        var html = '';
        currentPageUsers.forEach(function (user, index) {
            // Main row
            html += '<tr data-user-id="' + user.UserId + '" data-expanded="false">';
            html += '<td class="text-body" style="width: 80px;">';
            html += '<div class="d-flex align-items-center" style="gap: 6px;">';
            // Checkbox
            html += '<div class="form-check mb-0">';
            html += '<input class="form-check-input item-checkbox" type="checkbox" ' + (user.selected ? 'checked' : '') + ' />';
            html += '</div>';
            // Expand button with > icon
            html += '<button class="bg-transparent p-0 border-0 expand-btn" data-user-id="' + user.UserId + '" data-bs-placement="top" data-bs-title="Expand/Collapse" data-bs-toggle="tooltip">';
            html += '<span class="expand-icon" style="font-size: 18px; font-weight: bold; color: #6c757d; cursor: pointer; transition: transform 0.3s;">&#9654;</span>';
            html += '</button>';
            html += '</div>';
            html += '</td>';
            html += '<td class="text-body"><span class="fw-semibold">' + self.escapeHtml(user.FullName || '') + '</span></td>';
            html += '<td><div class="d-flex align-items-center">';
            html += '<span class="fs-16 fw-medium text-secondary">' + self.escapeHtml(user.Email || '') + '</span>';
            html += '</div></td>';
            html += '<td class="text-body">' + self.escapeHtml(user.Phone || 'N/A') + '</td>';
            html += '<td class="text-body">' + self.escapeHtml(user.RoleName || 'N/A') + '</td>';
            html += '<td class="text-body">' + self.escapeHtml(user.DealershipName || 'N/A') + '</td>';
            html += '<td>' + self.getStatusBadge(user.IsActive) + '</td>';
            html += '<td>';
            html += '<div class="d-flex justify-content-end" style="gap: 6px;">';
            // View button - Blue
            html += '<button class="btn btn-sm btn-outline-primary view-btn" data-user-id="' + user.UserId + '" data-bs-placement="top" data-bs-title="View" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
            html += '<i class="material-symbols-outlined fs-16">visibility</i>';
            html += '</button>';

            // Activate/Deactivate button based on status
            if (user.IsActive) {
                // Deactivate button - Orange/Warning
                html += '<button class="btn btn-sm btn-outline-warning deactivate-btn" data-user-id="' + user.UserId + '" data-bs-placement="top" data-bs-title="Deactivate" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
                html += '<i class="material-symbols-outlined fs-16">block</i>';
                html += '</button>';
            } else {
                // Activate button - Green
                html += '<button class="btn btn-sm btn-outline-success activate-btn" data-user-id="' + user.UserId + '" data-bs-placement="top" data-bs-title="Activate" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
                html += '<i class="material-symbols-outlined fs-16">check_circle</i>';
                html += '</button>';
            }

            // Delete button - Red
            html += '<button class="btn btn-sm btn-outline-danger delete-btn" data-user-id="' + user.UserId + '" data-bs-placement="top" data-bs-title="Delete" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
            html += '<i class="material-symbols-outlined fs-16">delete</i>';
            html += '</button>';
            html += '</div>';
            html += '</td>';
            html += '</tr>';

            // Expanded row for details
            html += '<tr class="expanded-row" data-user-id="' + user.UserId + '" style="display:none;">';
            html += '<td colspan="10" class="p-0">';
            html += '<div class="p-20 bg-light rounded-3 mb-10" style="margin: 0 15px 15px 15px; background-color: #f8f9fa !important;">';

            // Show user details
            html += '<div class="row">';
            html += '<div class="col-md-6">';
            html += '<h6 class="mb-15">User Information</h6>';
            html += '<table class="table table-sm table-borderless">';
            html += '<tr><td><strong>Full Name:</strong></td><td>' + self.escapeHtml(user.FullName || '') + '</td></tr>';
            html += '<tr><td><strong>Email:</strong></td><td>' + self.escapeHtml(user.Email || '') + '</td></tr>';
            html += '<tr><td><strong>Phone:</strong></td><td>' + self.escapeHtml(user.Phone || 'N/A') + '</td></tr>';
            html += '<tr><td><strong>Role:</strong></td><td>' + self.escapeHtml(user.RoleName || 'N/A') + '</td></tr>';
            html += '<tr><td><strong>Status:</strong></td><td>' + (user.IsActive ? '<span class="text-success">Active</span>' : '<span class="text-danger">Inactive</span>') + '</td></tr>';
            html += '</table>';
            html += '</div>';

            html += '<div class="col-md-6">';
            if (user.DealerInfo) {
                var dealer = user.DealerInfo;
                html += '<h6 class="mb-15">Dealer Information</h6>';
                html += '<table class="table table-sm table-borderless">';
                html += '<tr><td><strong>Dealership Name:</strong></td><td>' + self.escapeHtml(dealer.DealershipName || 'N/A') + '</td></tr>';
                html += '<tr><td><strong>Registration Number:</strong></td><td>' + self.escapeHtml(dealer.RegistrationNumber || 'N/A') + '</td></tr>';
                html += '<tr><td><strong>Trade License:</strong></td><td>' + self.escapeHtml(dealer.TradeLicenseNumber || 'N/A') + '</td></tr>';
                html += '<tr><td><strong>GST Number:</strong></td><td>' + self.escapeHtml(dealer.GSTNumber || 'N/A') + '</td></tr>';
                html += '<tr><td><strong>Contact Person:</strong></td><td>' + self.escapeHtml(dealer.ContactPerson || 'N/A') + '</td></tr>';
                html += '<tr><td><strong>Phone:</strong></td><td>' + self.escapeHtml(dealer.Phone || 'N/A') + '</td></tr>';
                html += '<tr><td><strong>Alternate Phone:</strong></td><td>' + self.escapeHtml(dealer.AlternatePhone || 'N/A') + '</td></tr>';
                html += '<tr><td><strong>Email:</strong></td><td>' + self.escapeHtml(dealer.Email || 'N/A') + '</td></tr>';
                html += '<tr><td><strong>Website:</strong></td><td>' + self.escapeHtml(dealer.Website || 'N/A') + '</td></tr>';

                var address = '';
                if (dealer.AddressLine1) address += dealer.AddressLine1;
                if (dealer.AddressLine2) address += (address ? ', ' : '') + dealer.AddressLine2;
                if (dealer.City) address += (address ? ', ' : '') + dealer.City;
                if (dealer.State) address += (address ? ', ' : '') + dealer.State;
                if (dealer.Country) address += (address ? ', ' : '') + dealer.Country;
                if (dealer.PostalCode) address += (address ? ' - ' : '') + dealer.PostalCode;

                if (address) {
                    html += '<tr><td><strong>Address:</strong></td><td>' + self.escapeHtml(address) + '</td></tr>';
                }
                html += '</table>';
            } else {
                html += '<div class="text-muted">No dealer information available</div>';
            }
            html += '</div>';
            html += '</div>';

            html += '</div>';
            html += '</td>';
            html += '</tr>';
        });

        self.$tableBody.html(html);

        // Bind expand/collapse events with rotation animation
        self.$tableBody.find('.expand-btn').on('click', function () {
            var userId = $(this).data('user-id');
            self.toggleExpand(userId);
        });

        // Re-bind checkbox events for new checkboxes
        self.$tableBody.find('.item-checkbox').off('change').on('change', function () {
            var totalCheckboxes = self.$tableBody.find('.item-checkbox').length;
            var checkedCheckboxes = self.$tableBody.find('.item-checkbox:checked').length;
            self.$checkAll.prop('checked', totalCheckboxes === checkedCheckboxes);
        });

        // Bind View, Activate, Deactivate, Delete events
        self.$tableBody.find('.view-btn').on('click', function () {
            var userId = $(this).data('user-id');
            self.viewUser(userId);
        });

        self.$tableBody.find('.activate-btn').on('click', function () {
            var userId = $(this).data('user-id');
            self.activateUser(userId);
        });

        self.$tableBody.find('.deactivate-btn').on('click', function () {
            var userId = $(this).data('user-id');
            self.deactivateUser(userId);
        });

        self.$tableBody.find('.delete-btn').on('click', function () {
            var userId = $(this).data('user-id');
            self.deleteUser(userId);
        });
    };

    self.toggleExpand = function (userId) {
        var expandedRow = self.$tableBody.find('.expanded-row[data-user-id="' + userId + '"]');
        var expandIcon = self.$tableBody.find('.expand-btn[data-user-id="' + userId + '"] .expand-icon');

        if (expandedRow.is(':visible')) {
            expandedRow.hide();
            expandIcon.css('transform', 'rotate(0deg)');
        } else {
            expandedRow.show();
            expandIcon.css('transform', 'rotate(90deg)');
        }
    };

    self.viewUser = function (userId) {
        console.log('View user:', userId);
        var user = self.Users.find(function (u) { return u.UserId === userId; });
        if (user) {
            var message = 'Viewing User:\n\n' +
                'Name: ' + user.FullName + '\n' +
                'Email: ' + user.Email + '\n' +
                'Phone: ' + user.Phone + '\n' +
                'Role: ' + user.RoleName + '\n' +
                'Dealership: ' + user.DealershipName + '\n' +
                'Status: ' + (user.IsActive ? 'Active' : 'Inactive');
            alert(message);
        }
    };

    self.activateUser = function (userId) {
        console.log('Activate user:', userId);
        if (confirm('Are you sure you want to activate this user?')) {
            var user = self.Users.find(function (u) { return u.UserId === userId; });
            if (user) {
                makeAjaxRequest({
                    url: '/User/ActivateUser',
                    type: 'POST',
                    data: JSON.stringify(userId),
                    contentType: 'application/json; charset=utf-8',
                    showLoader: true,
                    successCallback: function (response) {
                        if (response.success) {
                            user.IsActive = true;
                            self.render();
                            alert('User activated successfully!');
                        } else {
                            alert(response.message || 'Failed to activate user.');
                        }
                    },
                    errorCallback: function (xhr, status, error) {
                        alert('Failed to activate user. Please try again.');
                    }
                });
            }
        }
    };

    self.deactivateUser = function (userId) {
        console.log('Deactivate user:', userId);
        if (confirm('Are you sure you want to deactivate this user?')) {
            var user = self.Users.find(function (u) { return u.UserId === userId; });
            if (user) {
                makeAjaxRequest({
                    url: '/User/DeactivateUser',
                    type: 'POST',
                    data: JSON.stringify(userId),
                    contentType: 'application/json; charset=utf-8',
                    showLoader: true,
                    successCallback: function (response) {
                        if (response.success) {
                            user.IsActive = false;
                            self.render();
                            alert('User deactivated successfully!');
                        } else {
                            alert(response.message || 'Failed to deactivate user.');
                        }
                    },
                    errorCallback: function (xhr, status, error) {
                        alert('Failed to deactivate user. Please try again.');
                    }
                });
            }
        }
    };

    self.deleteUser = function (userId) {
        console.log('Delete user:', userId);
        if (confirm('Are you sure you want to delete this user? This action cannot be undone.')) {
            var user = self.Users.find(function (u) { return u.UserId === userId; });
            if (user) {
                makeAjaxRequest({
                    url: '/User/DeleteUser',
                    type: 'POST',
                    data: JSON.stringify(userId),
                    contentType: 'application/json; charset=utf-8',
                    showLoader: true,
                    successCallback: function (response) {
                        if (response.success) {
                            // Remove user from list
                            var index = self.Users.indexOf(user);
                            if (index !== -1) {
                                self.Users.splice(index, 1);
                            }
                            self.render();
                            alert('User deleted successfully!');
                        } else {
                            alert(response.message || 'Failed to delete user.');
                        }
                    },
                    errorCallback: function (xhr, status, error) {
                        alert('Failed to delete user. Please try again.');
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
        var total = self.filteredUsers.length;
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