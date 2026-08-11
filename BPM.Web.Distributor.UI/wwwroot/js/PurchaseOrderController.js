function PurchaseOrderController() {
    var self = this;

    // Properties
    self.PurchaseOrders = [];
    self.filteredOrders = [];
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
        self.$tableBody = $('#orderTableBody');
        self.$pagination = $('#paginationControls');
        self.$entriesInfo = $('#entriesInfo');
        self.$searchInput = $('#searchInput');
        self.$checkAll = $('#checkAll');

        // Bind events
        self.bindEvents();

        // Fetch data
        self.fetchPurchaseOrdersAsync();
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

    self.fetchPurchaseOrdersAsync = function () {
        // Show loader manually
        showLoader('Loading purchase orders...');
        console.log('Loader shown - fetching purchase orders...');

        makeAjaxRequest({
            url: '/PurchaseOrder/GetAllPurchaseOrders',
            type: 'GET',
            showLoader: false, // Manual control
            successCallback: function (response) {
                console.log('PurchaseOrder response:', response);

                // Map response data
                if (response) {
                    self.PurchaseOrders = response.map(function (order) {
                        // Map PurchaseOrderItemResponse to purchaseOrderDetails
                        var items = order.PurchaseOrderItemResponse || [];

                        return {
                            Id: order.Id,
                            PONumber: order.PONumber || order.Id.substring(0, 8),
                            supplierId: order.SupplierId,
                            supplierName: order.supplierName || 'Supplier',
                            subTotal: order.SubTotal || 0,
                            taxAmount: order.TaxAmount || 0,
                            totalAmount: order.TotalAmount || 0,
                            discountAmount: order.DiscountAmount || 0,
                            status: order.Status || 'Draft',
                            orderDate: order.OrderDate || new Date(),
                            expectedDeliveryDate: order.ExpectedDeliveryDate,
                            actualDeliveryDate: order.ActualDeliveryDate,
                            deliveryTerms: order.DeliveryTerms || '',
                            paymentTerms: order.PaymentTerms || '',
                            remarks: order.Remarks || '',
                            selected: false,
                            expanded: false,
                            purchaseOrderDetails: items.map(function (item) {
                                return {
                                    Id: item.Id,
                                    DrugId: item.DrugId,
                                    DrugName: item.DrugName || 'Drug',
                                    BatchNumber: item.BatchNumber || 'B001',
                                    Quantity: item.Quantity || 0,
                                    PendingQuantity: item.PendingQuantity || 0,
                                    ReceivedQuantity: item.ReceivedQuantity || 0,
                                    UnitPrice: item.UnitPrice || 0,
                                    DiscountAmount: item.DiscountAmount || 0,
                                    DiscountPercentage: item.DiscountPercentage || 0,
                                    TaxAmount: item.TaxAmount || 0,
                                    TaxRate: item.TaxRate || 0,
                                    TotalAmount: item.TotalAmount || 0,
                                    PackagingId: item.PackagingId,
                                    ExpiryDate: item.ExpiryDate,
                                    Remarks: item.Remarks || ''
                                };
                            })
                        };
                    });

                    console.log('Mapped purchase orders:', self.PurchaseOrders);

                    // Initialize filtered orders
                    self.filteredOrders = self.PurchaseOrders;

                    // Render the table
                    console.log('Rendering table...');
                    self.render();

                    // Hide loader after rendering with a small delay
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
                console.log('Failed to fetch purchase orders:', error);
                // Show error message in table
                self.$tableBody.html(
                    '<tr><td colspan="10" class="text-center py-4 text-danger">' +
                    '<i class="material-symbols-outlined fs-40 mb-2 d-block">error</i>' +
                    'Failed to load purchase orders. Please try again.</td></tr>'
                );
                hideLoader();
            }
        });
    };

    self.filterOrders = function () {
        if (!self.searchTerm) {
            self.filteredOrders = self.PurchaseOrders;
        } else {
            self.filteredOrders = self.PurchaseOrders.filter(function (order) {
                return (order.PONumber && order.PONumber.toLowerCase().includes(self.searchTerm)) ||
                    (order.supplierName && order.supplierName.toLowerCase().includes(self.searchTerm)) ||
                    (order.status && order.status.toLowerCase().includes(self.searchTerm)) ||
                    (order.deliveryTerms && order.deliveryTerms.toLowerCase().includes(self.searchTerm)) ||
                    (order.paymentTerms && order.paymentTerms.toLowerCase().includes(self.searchTerm));
            });
        }
        self.totalPages = Math.ceil(self.filteredOrders.length / self.pageSize);
        if (self.currentPage > self.totalPages) {
            self.currentPage = self.totalPages || 1;
        }
    };

    self.getCurrentPageOrders = function () {
        var start = (self.currentPage - 1) * self.pageSize;
        var end = start + self.pageSize;
        return self.filteredOrders.slice(start, end);
    };

    self.render = function () {
        self.filterOrders();
        self.renderTableBody();
        self.renderPagination();
        self.renderEntriesInfo();
    };

    self.renderTableBody = function () {
        var currentPageOrders = self.getCurrentPageOrders();

        console.log('Rendering orders:', currentPageOrders);

        if (currentPageOrders.length === 0) {
            self.$tableBody.html(
                '<tr><td colspan="10" class="text-center py-4 text-muted">' +
                '<i class="material-symbols-outlined fs-40 mb-2 d-block">inbox</i>' +
                'No purchase orders found</td></tr>'
            );
            return;
        }

        var html = '';
        currentPageOrders.forEach(function (order, index) {
            // Main row
            html += '<tr data-order-id="' + order.Id + '" data-expanded="false">';
            html += '<td class="text-body" style="width: 80px;">';
            html += '<div class="d-flex align-items-center" style="gap: 6px;">';
            // Checkbox
            html += '<div class="form-check mb-0">';
            html += '<input class="form-check-input item-checkbox" type="checkbox" ' + (order.selected ? 'checked' : '') + ' />';
            html += '</div>';
            // Expand button with > icon
            html += '<button class="bg-transparent p-0 border-0 expand-btn" data-order-id="' + order.Id + '" data-bs-placement="top" data-bs-title="Expand/Collapse" data-bs-toggle="tooltip">';
            html += '<span class="expand-icon" style="font-size: 18px; font-weight: bold; color: #6c757d; cursor: pointer; transition: transform 0.3s;">&#9654;</span>';
            html += '</button>';
            html += '</div>';
            html += '</td>';
            html += '<td class="text-body"><span class="fw-semibold">' + self.escapeHtml(order.PONumber) + '</span></td>';
            html += '<td><div class="d-flex align-items-center">';
            html += '<span class="fs-16 fw-medium text-secondary">' + self.escapeHtml(order.supplierName) + '</span>';
            html += '</div></td>';
            html += '<td class="text-body">' + (order.purchaseOrderDetails ? order.purchaseOrderDetails.length : 0) + '</td>';
            html += '<td class="text-body">$' + (order.subTotal || 0).toFixed(2) + '</td>';
            html += '<td class="text-body">$' + (order.taxAmount || 0).toFixed(2) + '</td>';
            html += '<td class="text-body">$' + (order.totalAmount || 0).toFixed(2) + '</td>';
            html += '<td class="text-body">' + self.formatDate(order.orderDate) + '</td>';
            html += '<td>' + self.getStatusBadge(order.status) + '</td>';
            html += '<td>';
            html += '<div class="d-flex justify-content-end" style="gap: 6px;">';

            // Action buttons based on status
            var status = order.status || 'Draft';
            console.log('Order ID:', order.Id, 'Status:', status, 'Status Type:', typeof status);

            if (status === 'Draft') {
                // Draft: Show Accept and Reject buttons
                html += '<button class="btn btn-sm btn-outline-success accept-btn" data-order-details="' + order + '" data-order-id="' + order.Id + '" data-bs-placement="top" data-bs-title="Accept Order" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
                html += '<i class="material-symbols-outlined fs-16">accept</i>';
                html += '</button>';

                html += '<button class="btn btn-sm btn-outline-danger reject-btn" data-order-details="' + order + '" data-order-id="' + order.Id + '" data-bs-placement="top" data-bs-title="Reject Order" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
                html += '<i class="material-symbols-outlined fs-16">cancel</i>';
                html += '</button>';
            } else if (status === 'Accepted' || status === 'Confirmed') {
                // Accepted/Confirmed: Show Approve button only
                html += '<button class="btn btn-sm btn-outline-success approve-btn" data-order-details="' + order + '" data-order-id="' + order.Id + '" data-bs-placement="top" data-bs-title="Approve Order" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
                html += '<i class="material-symbols-outlined fs-16">view</i>';
                html += '</button>';
            }
            // Approved, Completed, Rejected, Cancelled: No action buttons (only view)

            html += '</div>';
            html += '</td>';
            html += '</tr>';

            // Expanded row for details
            html += '<tr class="expanded-row" data-order-id="' + order.Id + '" style="display:none;">';
            html += '<td colspan="10" class="p-0">';
            html += '<div class="p-20 bg-light rounded-3 mb-10" style="margin: 0 15px 15px 15px; background-color: #f8f9fa !important;">';

            // Show order summary info
            html += '<div class="row mb-15">';
            html += '<div class="col-md-3"><strong>PO Number:</strong> ' + self.escapeHtml(order.PONumber) + '</div>';
            html += '<div class="col-md-3"><strong>Delivery Terms:</strong> ' + self.escapeHtml(order.deliveryTerms || 'N/A') + '</div>';
            html += '<div class="col-md-3"><strong>Payment Terms:</strong> ' + self.escapeHtml(order.paymentTerms || 'N/A') + '</div>';
            html += '<div class="col-md-3"><strong>Expected Delivery:</strong> ' + self.formatDate(order.expectedDeliveryDate) + '</div>';
            html += '</div>';

            html += '<h6 class="mb-15">Order Items</h6>';
            html += '<div class="table-responsive">';
            html += '<table class="table table-sm table-bordered mb-0">';
            html += '<thead class="bg-white">';
            html += '<tr>';
            html += '<th>Drug Name</th>';
            html += '<th>Batch</th>';
            html += '<th>Quantity</th>';
            html += '<th>Pending</th>';
            html += '<th>Received</th>';
            html += '<th>Unit Price</th>';
            html += '<th>Discount</th>';
            html += '<th>Tax</th>';
            html += '<th>Total</th>';
            html += '</tr>';
            html += '</thead>';
            html += '<tbody>';

            var details = order.purchaseOrderDetails || [];
            if (details.length > 0) {
                details.forEach(function (detail) {
                    html += '<tr>';
                    html += '<td>' + self.escapeHtml(detail.DrugName || 'Drug') + '</td>';
                    html += '<td>' + self.escapeHtml(detail.BatchNumber || 'B001') + '</td>';
                    html += '<td>' + (detail.Quantity || 0) + '</td>';
                    html += '<td>' + (detail.PendingQuantity || 0) + '</td>';
                    html += '<td>' + (detail.ReceivedQuantity || 0) + '</td>';
                    html += '<td>$' + (detail.UnitPrice || 0).toFixed(2) + '</td>';
                    html += '<td>$' + (detail.DiscountAmount || 0).toFixed(2) + ' (' + (detail.DiscountPercentage || 0) + '%)</td>';
                    html += '<td>$' + (detail.TaxAmount || 0).toFixed(2) + ' (' + (detail.TaxRate || 0) + '%)</td>';
                    html += '<td>$' + (detail.TotalAmount || 0).toFixed(2) + '</td>';
                    html += '</tr>';
                });
            } else {
                html += '<tr><td colspan="9" class="text-center text-muted">No items found</td></tr>';
            }

            html += '</tbody>';
            html += '</table>';
            html += '</div>';

            // Show remarks if available
            if (order.remarks) {
                html += '<div class="mt-15"><strong>Remarks:</strong> ' + self.escapeHtml(order.remarks) + '</div>';
            }

            html += '</div>';
            html += '</td>';
            html += '</tr>';
        });

        self.$tableBody.html(html);

        // Bind expand/collapse events with rotation animation
        self.$tableBody.find('.expand-btn').on('click', function () {
            var orderId = $(this).data('order-id');
            self.toggleExpand(orderId);
        });

        // Re-bind checkbox events for new checkboxes
        self.$tableBody.find('.item-checkbox').off('change').on('change', function () {
            var totalCheckboxes = self.$tableBody.find('.item-checkbox').length;
            var checkedCheckboxes = self.$tableBody.find('.item-checkbox:checked').length;
            self.$checkAll.prop('checked', totalCheckboxes === checkedCheckboxes);
        });

        // Bind View, Accept, Reject, Approve events
        self.$tableBody.find('.view-btn').on('click', function () {
            var orderId = $(this).data('order-id');
            self.viewOrder(orderId);
        });

        self.$tableBody.find('.accept-btn').on('click', function () {
            var orderId = $(this).data('order-id');
            self.acceptOrder(orderId);
        });

        self.$tableBody.find('.reject-btn').on('click', function () {
            var orderId = $(this).data('order-id');
            self.rejectOrder(orderId);
        });

        self.$tableBody.find('.approve-btn').on('click', function () {
            var orderId = $(this).data('order-id');
            self.approveOrder(orderId);
        });
    };

    self.toggleExpand = function (orderId) {
        var expandedRow = self.$tableBody.find('.expanded-row[data-order-id="' + orderId + '"]');
        var expandIcon = self.$tableBody.find('.expand-btn[data-order-id="' + orderId + '"] .expand-icon');

        if (expandedRow.is(':visible')) {
            expandedRow.hide();
            expandIcon.css('transform', 'rotate(0deg)');
        } else {
            expandedRow.show();
            expandIcon.css('transform', 'rotate(90deg)');
        }
    };

    self.viewOrder = function (orderId) {
        console.log('View order:', orderId);
        var order = self.PurchaseOrders.find(function (o) { return o.Id === orderId; });
        if (order) {
            var message = 'Viewing Purchase Order:\n\n' +
                'PO Number: ' + order.PONumber + '\n' +
                'Supplier: ' + order.supplierName + '\n' +
                'Sub Total: $' + order.subTotal.toFixed(2) + '\n' +
                'Tax: $' + order.taxAmount.toFixed(2) + '\n' +
                'Total: $' + order.totalAmount.toFixed(2) + '\n' +
                'Status: ' + order.status + '\n' +
                'Order Date: ' + self.formatDate(order.orderDate) + '\n' +
                'Delivery Terms: ' + (order.deliveryTerms || 'N/A') + '\n' +
                'Payment Terms: ' + (order.paymentTerms || 'N/A') + '\n' +
                'Items: ' + (order.purchaseOrderDetails ? order.purchaseOrderDetails.length : 0);
            alert(message);
        }
    };

    self.acceptOrder = function (orderId) {
        console.log('Accept order:', orderId);
        if (confirm('Are you sure you want to accept this purchase order?')) {
            var order = self.PurchaseOrders.find(function (o) { return o.Id === orderId; });
            if (order) {
                showLoader('Accepting order...');

                makeAjaxRequest({
                    url: '/PurchaseOrder/AcceptOrder',
                    type: 'POST',
                    data: JSON.stringify({ orderId: orderId }),
                    contentType: 'application/json; charset=utf-8',
                    showLoader: false,
                    successCallback: function (response) {
                        hideLoader();
                        alert('Order ' + order.PONumber + ' has been accepted successfully!');
                        self.fetchPurchaseOrdersAsync(); // Refresh data
                    },
                    errorCallback: function (xhr, status, error) {
                        hideLoader();
                        alert('Failed to accept order. Please try again.');
                    }
                });
            }
        }
    };

    self.rejectOrder = function (orderId) {
        console.log('Reject order:', orderId);
        if (confirm('Are you sure you want to reject this purchase order?')) {
            var order = self.PurchaseOrders.find(function (o) { return o.Id === orderId; });
            if (order) {
                showLoader('Rejecting order...');

                makeAjaxRequest({
                    url: '/PurchaseOrder/RejectOrder',
                    type: 'POST',
                    data: JSON.stringify({ orderId: orderId }),
                    contentType: 'application/json; charset=utf-8',
                    showLoader: false,
                    successCallback: function (response) {
                        hideLoader();
                        alert('Order ' + order.PONumber + ' has been rejected.');
                        self.fetchPurchaseOrdersAsync(); // Refresh data
                    },
                    errorCallback: function (xhr, status, error) {
                        hideLoader();
                        alert('Failed to reject order. Please try again.');
                    }
                });
            }
        }
    };

    self.approveOrder = function (orderId) {
        console.log('Approve order:', orderId);
        if (confirm('Are you sure you want to approve this purchase order?')) {
            var order = self.PurchaseOrders.find(function (o) { return o.Id === orderId; });
            if (order) {
                showLoader('Approving order...');

                makeAjaxRequest({
                    url: '/PurchaseOrder/ApproveOrder',
                    type: 'POST',
                    data: JSON.stringify({ orderId: orderId }),
                    contentType: 'application/json; charset=utf-8',
                    showLoader: false,
                    successCallback: function (response) {
                        hideLoader();
                        alert('Order ' + order.PONumber + ' has been approved successfully!');
                        self.fetchPurchaseOrdersAsync(); // Refresh data
                    },
                    errorCallback: function (xhr, status, error) {
                        hideLoader();
                        alert('Failed to approve order. Please try again.');
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
        var total = self.filteredOrders.length;
        var start = total > 0 ? (self.currentPage - 1) * self.pageSize + 1 : 0;
        var end = Math.min(self.currentPage * self.pageSize, total);

        self.$entriesInfo.text('Showing ' + start + ' to ' + end + ' of ' + total + ' entries');
    };

    self.getStatusBadge = function (status) {
        var statusMap = {
            'Draft': 'text-warning bg-warning bg-opacity-10',
            'Accepted': 'text-info bg-info bg-opacity-10',
            'Confirmed': 'text-info bg-info bg-opacity-10',
            'Approved': 'text-success bg-success bg-opacity-10',
            'Completed': 'text-success bg-success bg-opacity-10',
            'Shipped': 'text-primary bg-primary bg-opacity-10',
            'Pending': 'text-warning bg-warning bg-opacity-10',
            'Rejected': 'text-danger bg-danger bg-opacity-10',
            'Cancelled': 'text-danger bg-danger bg-opacity-10'
        };

        var className = statusMap[status] || 'text-secondary bg-secondary bg-opacity-10';
        return '<span class="fs-15 fw-normal d-inline-block default-badge ' + className + '">' +
            self.escapeHtml(status || 'Draft') + '</span>';
    };

    self.formatDate = function (dateString) {
        if (!dateString) return 'N/A';
        try {
            var date = new Date(dateString);
            return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
        } catch (e) {
            return 'N/A';
        }
    };

    self.escapeHtml = function (text) {
        if (!text) return '';
        var div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    };
}