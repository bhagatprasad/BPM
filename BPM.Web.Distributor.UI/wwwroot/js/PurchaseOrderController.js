function PurchaseOrderController() {
    var self = this;

    // Properties
    self.PurchaseOrders = [];
    self.filteredOrders = [];
    self.currentPage = 1;
    self.pageSize = 10;
    self.totalPages = 1;
    self.searchTerm = '';

    // PO Status Constants
    self.PO_STATUS = {
        DRAFT: 'Draft',
        SUBMITTED: 'Submitted',
        ACCEPTED: 'Accepted',
        PENDING_VERIFICATION: 'Pending Verification',
        VERIFIED: 'Verified',
        PENDING_APPROVAL: 'Pending Approval',
        APPROVED: 'Approved',
        REJECTED: 'Rejected',
        CANCELLED: 'Cancelled',
        PROCESSING: 'Processing',
        SENT_TO_INVENTORY: 'Sent to Inventory',
        INVENTORY_CONFIRMED: 'Inventory Confirmed',
        PARTIALLY_AVAILABLE: 'Partially Available',
        OUT_OF_STOCK: 'Out of Stock',
        READY_FOR_DISPATCH: 'Ready for Dispatch',
        DISPATCHED: 'Dispatched',
        IN_TRANSIT: 'In Transit',
        PARTIALLY_DELIVERED: 'Partially Delivered',
        DELIVERED: 'Delivered',
        BILL_GENERATED: 'Bill Generated',
        PAYMENT_PENDING: 'Payment Pending',
        PARTIALLY_PAID: 'Partially Paid',
        PAID: 'Paid',
        PAYMENT_FAILED: 'Payment Failed',
        PAYMENT_OVERDUE: 'Payment Overdue',
        COMPLETED: 'Completed',
        CLOSED: 'Closed'
    };

    // Color palette for alternating rows
    self.rowColors = [
        '#f8f9fa',  // Light gray
        '#ffffff',  // White
        '#f0f4f8',  // Light blue
        '#fafafa',  // Off white
        '#f5f5f5',  // Light gray
        '#faf3e8',  // Cream
        '#f0f0f0',  // Gray
        '#f8f0f0',  // Light pink
        '#f0f8f0',  // Light green
        '#f0f0f8'   // Light purple
    ];

    // DOM references
    self.$tableBody = null;
    self.$pagination = null;
    self.$entriesInfo = null;
    self.$searchInput = null;
    self.$checkAll = null;
    self.$modal = null;
    self.$viewModal = null;

    self.init = function () {
        // Cache DOM elements
        self.$tableBody = $('#orderTableBody');
        self.$pagination = $('#paginationControls');
        self.$entriesInfo = $('#entriesInfo');
        self.$searchInput = $('#searchInput');
        self.$checkAll = $('#checkAll');
        self.$modal = $('#processOrderModal');
        self.$viewModal = $('#viewOrderModal');

        console.log('Modal element:', self.$modal.length ? 'Found' : 'Not found');

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

        // Modal confirm button
        $('#confirmProcessOrder').on('click', function () {
            console.log('Confirm button clicked');
            var orderId = $(this).data('order-id');
            var action = $(this).data('action');
            var notes = $('#processNotes').val();
            console.log('OrderId:', orderId, 'Action:', action, 'Notes:', notes);
            self.processOrder(orderId, action, notes);
        });

        // Modal close - clear notes
        self.$modal.on('hidden.bs.modal', function () {
            console.log('Modal hidden');
            $('#processNotes').val('');
            $('#confirmProcessOrder').removeData('order-id').removeData('action');
            // Reset modal header color
            $('#modalHeader').css('border-bottom-color', '');
        });
    };

    self.fetchPurchaseOrdersAsync = function () {
        // Show loader manually
        showLoader('Loading purchase orders...');
        console.log('Loader shown - fetching purchase orders...');

        makeAjaxRequest({
            url: '/PurchaseOrder/GetAllPurchaseOrders',
            type: 'GET',
            showLoader: false,
            successCallback: function (response) {
                console.log('PurchaseOrder response:', response);

                if (response) {
                    self.PurchaseOrders = response.map(function (order) {
                        var items = order.PurchaseOrderItemResponse || [];

                        // Get supplier name - check if Dealer exists and use its name
                        var supplierName = order.supplierName || '';

                        // If supplier name is empty or null, try to get from Dealer
                        if (!supplierName && order.dealer && order.dealer.dealershipName) {
                            supplierName = order.dealer.dealershipName;
                        }

                        // If still empty, use default
                        if (!supplierName) {
                            supplierName = 'Supplier';
                        }

                        return {
                            Id: order.Id,
                            PONumber: order.PONumber || order.Id.substring(0, 8),
                            supplierId: order.SupplierId,
                            supplierName: supplierName,
                            dealerId: order.DealerId,
                            dealer: order.Dealer || null,
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
                    self.filteredOrders = self.PurchaseOrders;
                    self.render();

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
                    (order.paymentTerms && order.paymentTerms.toLowerCase().includes(self.searchTerm)) ||
                    (order.remarks && order.remarks.toLowerCase().includes(self.searchTerm));
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
            // Calculate row color based on global index
            var globalIndex = (self.currentPage - 1) * self.pageSize + index;
            var colorIndex = globalIndex % self.rowColors.length;
            var rowColor = self.rowColors[colorIndex];

            // Prepare tooltip content with notes
            var tooltipContent = '';
            if (order.remarks) {
                tooltipContent = 'Notes: ' + self.escapeHtml(order.remarks);
            } else {
                tooltipContent = 'No notes available';
            }

            // Get display name - show dealer name if available, otherwise supplier name
            var displayName = order.supplierName || 'Supplier';

            // If dealer exists and has dealership name, use it
            if (order.dealer && order.dealer.DealershipName) {
                displayName = order.dealer.DealershipName;
            }

            html += '<tr data-order-id="' + order.Id + '" data-expanded="false" style="background-color: ' + rowColor + ';" data-bs-toggle="tooltip" data-bs-placement="top" title="' + tooltipContent + '">';
            html += '<td class="text-body" style="width: 80px;">';
            html += '<div class="d-flex align-items-center" style="gap: 6px;">';
            html += '<div class="form-check mb-0">';
            html += '<input class="form-check-input item-checkbox" type="checkbox" ' + (order.selected ? 'checked' : '') + ' />';
            html += '</div>';
            html += '<button class="bg-transparent p-0 border-0 expand-btn" data-order-id="' + order.Id + '" data-bs-placement="top" data-bs-title="Expand/Collapse" data-bs-toggle="tooltip">';
            html += '<span class="expand-icon" style="font-size: 18px; font-weight: bold; color: #6c757d; cursor: pointer; transition: transform 0.3s;">&#9654;</span>';
            html += '</button>';
            html += '</div>';
            html += '</td>';
            html += '<td class="text-body"><span class="fw-semibold">' + self.escapeHtml(order.PONumber) + '</span></td>';
            html += '<td><div class="d-flex align-items-center">';
            html += '<span class="fs-16 fw-medium text-secondary">' + self.escapeHtml(displayName) + '</span>';
            // Show dealer badge if it's a dealer
            if (order.dealer && order.dealer.dealershipName) {
                html += ' <span class="badge bg-info ms-2" style="font-size: 10px;">Dealer</span>';
            }
            html += '</div></td>';
            html += '<td class="text-body">' + (order.purchaseOrderDetails ? order.purchaseOrderDetails.length : 0) + '</td>';
            html += '<td class="text-body">$' + (order.subTotal || 0).toFixed(2) + '</td>';
            html += '<td class="text-body">$' + (order.taxAmount || 0).toFixed(2) + '</td>';
            html += '<td class="text-body">$' + (order.totalAmount || 0).toFixed(2) + '</td>';
            html += '<td class="text-body">' + self.formatDate(order.orderDate) + '</td>';
            html += '<td>' + self.getStatusBadge(order.status) + '</td>';
            html += '<td>';
            html += '<div class="d-flex justify-content-end" style="gap: 6px;">';

            var status = order.status || 'Draft';

            // Action buttons based on status
            if (status === 'Submitted') {
                // Submitted: Show Accept and Reject buttons
                html += '<button class="btn btn-sm btn-outline-success accept-btn" data-order-id="' + order.Id + '" data-bs-placement="top" data-bs-title="Accept Order" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
                html += '<i class="material-symbols-outlined fs-16">check_circle</i>';
                html += '</button>';

                html += '<button class="btn btn-sm btn-outline-danger reject-btn" data-order-id="' + order.Id + '" data-bs-placement="top" data-bs-title="Reject Order" data-bs-toggle="tooltip" style="padding: 4px 10px;">';
                html += '<i class="material-symbols-outlined fs-16">cancel</i>';
                html += '</button>';
            }

            html += '</div>';
            html += '</td>';
            html += '</tr>';

            // Expanded row
            html += '<tr class="expanded-row" data-order-id="' + order.Id + '" style="display:none; background-color: ' + rowColor + ';">';
            html += '<td colspan="10" class="p-0">';
            html += '<div class="p-20 bg-light rounded-3 mb-10" style="margin: 0 15px 15px 15px; background-color: #f8f9fa !important;">';
            html += '<div class="row mb-15">';
            html += '<div class="col-md-3"><strong>PO Number:</strong> ' + self.escapeHtml(order.PONumber) + '</div>';
            html += '<div class="col-md-3"><strong>Supplier/Dealer:</strong> ' + self.escapeHtml(displayName) + '</div>';
            html += '<div class="col-md-3"><strong>Delivery Terms:</strong> ' + self.escapeHtml(order.deliveryTerms || 'N/A') + '</div>';
            html += '<div class="col-md-3"><strong>Payment Terms:</strong> ' + self.escapeHtml(order.paymentTerms || 'N/A') + '</div>';
            html += '</div>';
            html += '<div class="row mb-15">';
            html += '<div class="col-md-3"><strong>Expected Delivery:</strong> ' + self.formatDate(order.expectedDeliveryDate) + '</div>';
            html += '<div class="col-md-3"><strong>Actual Delivery:</strong> ' + self.formatDate(order.actualDeliveryDate) + '</div>';
            if (order.dealer && order.dealer.dealershipName) {
                html += '<div class="col-md-6"><strong>Dealer Details:</strong> ' + self.escapeHtml(order.dealer.dealershipName) + ' (' + self.escapeHtml(order.dealer.contactPerson || 'N/A') + ')</div>';
            }
            html += '</div>';

            // Display notes in expanded row with icon
            if (order.remarks) {
                html += '<div class="mb-15 p-10" style="background-color: #fff3cd; border-radius: 8px; border-left: 4px solid #ffc107;">';
                html += '<strong><i class="material-symbols-outlined fs-18" style="vertical-align: middle;">notes</i> Notes:</strong> ' + self.escapeHtml(order.remarks).replace(/\n/g, '<br/>');
                html += '</div>';
            }

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
            html += '</div>';
            html += '</td>';
            html += '</tr>';
        });

        self.$tableBody.html(html);

        // Initialize tooltips for rows
        self.$tableBody.find('tr[data-bs-toggle="tooltip"]').tooltip();

        // Bind events
        self.$tableBody.find('.expand-btn').on('click', function () {
            var orderId = $(this).data('order-id');
            self.toggleExpand(orderId);
        });

        self.$tableBody.find('.item-checkbox').off('change').on('change', function () {
            var totalCheckboxes = self.$tableBody.find('.item-checkbox').length;
            var checkedCheckboxes = self.$tableBody.find('.item-checkbox:checked').length;
            self.$checkAll.prop('checked', totalCheckboxes === checkedCheckboxes);
        });

        self.$tableBody.find('.accept-btn').on('click', function () {
            var orderId = $(this).data('order-id');
            console.log('Accept button clicked for order:', orderId);
            self.showProcessModal(orderId, 'Accept');
        });

        self.$tableBody.find('.reject-btn').on('click', function () {
            var orderId = $(this).data('order-id');
            console.log('Reject button clicked for order:', orderId);
            self.showProcessModal(orderId, 'Reject');
        });

        // Re-bind tooltips
        $('[data-bs-toggle="tooltip"]').tooltip();
    };

    self.showProcessModal = function (orderId, action) {
        console.log('Showing modal for order:', orderId, 'Action:', action);

        var order = self.PurchaseOrders.find(function (o) { return o.Id === orderId; });
        if (!order) {
            console.error('Order not found:', orderId);
            self.showNotification('Order not found. Please refresh and try again.', 'error');
            return;
        }

        console.log('Order found:', order);

        var modalTitle = '';
        var modalMessage = '';
        var status = '';
        var iconName = '';
        var iconColor = '';
        var bgColor = '';
        var borderColor = '';
        var buttonClass = '';
        var buttonText = '';
        var headerColor = '';
        var headerBgColor = '';
        var iconContainerBg = '';

        // Get display name for modal
        var displayName = order.supplierName || 'Supplier';
        if (order.dealer && order.dealer.dealershipName) {
            displayName = order.dealer.dealershipName;
        }

        switch (action) {
            case 'Accept':
                modalTitle = 'Accept Purchase Order';
                modalMessage = 'Are you sure you want to accept this purchase order? <br/><br/> <strong>PO Number:</strong> ' + order.PONumber + '<br/> <strong>Supplier/Dealer:</strong> ' + displayName + '<br/> <strong>Total Amount:</strong> $' + (order.totalAmount || 0).toFixed(2);
                status = self.PO_STATUS.VERIFIED;
                iconName = 'check_circle';
                iconColor = '#28a745';
                bgColor = '#d4edda';
                borderColor = '#28a745';
                buttonClass = 'btn-success';
                buttonText = 'Accept Order';
                headerColor = '#28a745';
                headerBgColor = '#f0fff4';
                iconContainerBg = '#d4edda';
                break;
            case 'Reject':
                modalTitle = 'Reject Purchase Order';
                modalMessage = 'Are you sure you want to reject this purchase order? <br/><br/> <strong>PO Number:</strong> ' + order.PONumber + '<br/> <strong>Supplier/Dealer:</strong> ' + displayName + '<br/> <strong>Total Amount:</strong> $' + (order.totalAmount || 0).toFixed(2);
                status = self.PO_STATUS.REJECTED;
                iconName = 'warning';
                iconColor = '#856404';
                bgColor = '#fff3cd';
                borderColor = '#ffc107';
                buttonClass = 'btn-warning';
                buttonText = 'Reject Order';
                headerColor = '#ffc107';
                headerBgColor = '#fff8e1';
                iconContainerBg = '#fff3cd';
                break;
            default:
                console.error('Unknown action:', action);
                return;
        }

        // Update modal elements
        $('#processOrderModalLabel').text(modalTitle);
        $('#modalSubtitle').text('Please confirm your action for PO #' + order.PONumber);

        // Update modal header
        $('#modalHeader')
            .css('border-bottom-color', headerColor)
            .css('background-color', headerBgColor);

        // Update header icon
        $('#modalHeaderIcon')
            .text(iconName)
            .css('color', iconColor);

        // Update icon container
        $('#modalIconContainer')
            .css('background-color', iconContainerBg);

        // Update icon
        $('#modalIcon')
            .text(iconName)
            .css('color', iconColor);

        // Update message with border and background
        $('#processOrderMessage')
            .html(modalMessage)
            .css('border-left-color', borderColor)
            .css('background-color', bgColor);

        // Update confirm button
        var confirmBtn = $('#confirmProcessOrder');
        confirmBtn
            .data('order-id', orderId)
            .data('action', status)
            .removeClass('btn-success btn-danger btn-primary btn-warning')
            .addClass(buttonClass)
            .html('<span>' + buttonText + '</span>');

        // Clear notes
        $('#processNotes').val('');

        console.log('Modal updated, showing...');

        // Show modal using Bootstrap
        var modalInstance = new bootstrap.Modal(document.getElementById('processOrderModal'));
        modalInstance.show();
    };

    self.processOrder = function (orderId, status, notes) {
        console.log('Processing order:', orderId, 'Status:', status, 'Notes:', notes);

        var order = self.PurchaseOrders.find(function (o) { return o.Id === orderId; });
        if (!order) {
            console.error('Order not found:', orderId);
            self.showNotification('Order not found. Please refresh and try again.', 'error');
            return;
        }

        var processDto = {
            PurchaseOrderId: orderId,
            Status: status,
            Notes: notes || ''
        };

        console.log('DTO:', processDto);

        showLoader('Processing order...');

        makeAjaxRequest({
            url: '/PurchaseOrder/ProcessPurchaseOrder',
            type: 'POST',
            data: JSON.stringify(processDto),
            contentType: 'application/json; charset=utf-8',
            showLoader: false,
            successCallback: function (response) {
                console.log('Success response:', response);
                hideLoader();

                // Hide modal
                var modalInstance = bootstrap.Modal.getInstance(document.getElementById('processOrderModal'));
                if (modalInstance) {
                    modalInstance.hide();
                }

                var actionText = status === 'Verified' ? 'accepted' : 'rejected';
                self.showNotification('Order ' + order.PONumber + ' has been ' + actionText + ' successfully!', 'success');

                // Refresh data
                self.fetchPurchaseOrdersAsync();
            },
            errorCallback: function (xhr, status, error) {
                console.error('Error:', error);
                hideLoader();

                var modalInstance = bootstrap.Modal.getInstance(document.getElementById('processOrderModal'));
                if (modalInstance) {
                    modalInstance.hide();
                }

                var errorMessage = 'Failed to process order. Please try again.';
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    errorMessage = xhr.responseJSON.message;
                }
                self.showNotification(errorMessage, 'error');
            }
        });
    };

    self.showNotification = function (message, type) {
        var colors = {
            success: '#28a745',
            error: '#dc3545',
            warning: '#ffc107',
            info: '#17a2b8'
        };

        var icons = {
            success: 'check_circle',
            error: 'error',
            warning: 'warning',
            info: 'info'
        };

        var bgColor = colors[type] || colors.info;
        var icon = icons[type] || icons.info;

        $('#customNotification').remove();

        var notificationHtml = '<div id="customNotification" style="position: fixed; top: 20px; right: 20px; background: ' + bgColor + '; color: white; padding: 16px 24px; border-radius: 8px; box-shadow: 0 4px 12px rgba(0,0,0,0.15); z-index: 9999; max-width: 450px; animation: slideIn 0.3s ease;">' +
            '<div style="display: flex; align-items: center; gap: 12px;">' +
            '<i class="material-symbols-outlined" style="font-size: 24px;">' + icon + '</i>' +
            '<span style="font-size: 14px; line-height: 1.5;">' + message + '</span>' +
            '<button onclick="$(\'#customNotification\').remove()" style="background: none; border: none; color: white; font-size: 20px; cursor: pointer; margin-left: auto; opacity: 0.7; padding: 0 4px;">&times;</button>' +
            '</div>' +
            '</div>';

        $('body').append(notificationHtml);

        setTimeout(function () {
            $('#customNotification').fadeOut(300, function () {
                $(this).remove();
            });
        }, 4000);
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
            'Draft': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
            'Submitted': { text: 'text-info', bg: 'bg-info', border: 'border-info' },
            'Pending Verification': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
            'Verified': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
            'Pending Approval': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
            'Approved': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
            'Accepted': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
            'Confirmed': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
            'Completed': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
            'Shipped': { text: 'text-primary', bg: 'bg-primary', border: 'border-primary' },
            'Processing': { text: 'text-info', bg: 'bg-info', border: 'border-info' },
            'Pending': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
            'Rejected': { text: 'text-danger', bg: 'bg-danger', border: 'border-danger' },
            'Cancelled': { text: 'text-danger', bg: 'bg-danger', border: 'border-danger' },
            'Dispatched': { text: 'text-primary', bg: 'bg-primary', border: 'border-primary' },
            'Delivered': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
            'In Transit': { text: 'text-info', bg: 'bg-info', border: 'border-info' },
            'Ready for Dispatch': { text: 'text-primary', bg: 'bg-primary', border: 'border-primary' },
            'Bill Generated': { text: 'text-info', bg: 'bg-info', border: 'border-info' },
            'Payment Pending': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
            'Partially Paid': { text: 'text-warning', bg: 'bg-warning', border: 'border-warning' },
            'Paid': { text: 'text-success', bg: 'bg-success', border: 'border-success' },
            'Payment Failed': { text: 'text-danger', bg: 'bg-danger', border: 'border-danger' },
            'Payment Overdue': { text: 'text-danger', bg: 'bg-danger', border: 'border-danger' },
            'Closed': { text: 'text-secondary', bg: 'bg-secondary', border: 'border-secondary' }
        };

        var statusKey = status || 'Draft';
        var style = statusMap[statusKey] || { text: 'text-secondary', bg: 'bg-secondary', border: 'border-secondary' };

        return '<span class="' + style.text + ' ' + style.bg + ' bg-opacity-10 fs-15 fw-normal d-inline-block default-badge style-two border ' + style.border + '">' +
            self.escapeHtml(statusKey) + '</span>';
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