function DealerPurchaseOrderController() {

    var self = this;

    self.$tableBody = null;

    // Cache purchase orders
    self.purchaseOrderCache = {};

    //=========================
    // Initialize
    //=========================

    self.init = function () {

        self.$tableBody = $("#tblDealers tbody");

        self.loadDealers();
    };

    //=========================
    // Load Dealers
    //=========================

    self.loadDealers = function () {

        $.ajax({

            url: "/DealerPurchaseOrder/GetDealers",

            type: "GET",

            success: function (response) {

                self.renderDealers(response);

            },

            error: function () {

                self.$tableBody.html(
                    "<tr>" +
                    "<td colspan='7' class='text-center text-danger p-4'>" +
                    "Unable to load dealers." +
                    "</td>" +
                    "</tr>"
                );

            }

        });

    };

    //=========================
    // Render Dealers
    //=========================

    self.renderDealers = function (dealers) {

        var html = "";

        $.each(dealers, function (index, dealer) {

            html += "<tr>";

            html += "<td width='50'>";

            html += "<button class='btn btn-sm btn-outline-secondary expandDealer' data-id='" + dealer.Id + "'>";

            html += "<i class='ri-arrow-right-s-line'></i>";

            html += "</button>";

            html += "</td>";

            html += "<td>" + (dealer.DealershipName || "") + "</td>";

            html += "<td>" + (dealer.ContactPerson || "") + "</td>";

            html += "<td>" + (dealer.Email || "") + "</td>";

            html += "<td>" + (dealer.Phone || "") + "</td>";

            html += "<td>" + (dealer.City || "") + "</td>";

            html += "<td>";

            if (dealer.IsActive) {

                html += "<span class='badge bg-success'>Active</span>";

            }
            else {

                html += "<span class='badge bg-danger'>Inactive</span>";

            }

            html += "</td>";

            html += "</tr>";

            //--------------------------------------------------
            // Hidden Purchase Order Row
            //--------------------------------------------------

            html += "<tr class='purchaseOrderRow' data-id='" + dealer.Id + "' style='display:none;'>";

            html += "<td colspan='7' class='border-0 bg-light'>";

            html += "<div class='purchaseOrderContainer p-3'></div>";

            html += "</td>";

            html += "</tr>";

        });

        self.$tableBody.html(html);

        //--------------------------------------------------
        // Expand Dealer
        //--------------------------------------------------

        $(".expandDealer").off("click").on("click", function () {

            var dealerId = $(this).data("id");

            self.toggleDealer(dealerId, $(this));

        });

    };

    //=========================
    // Expand / Collapse Dealer
    //=========================

    self.toggleDealer = function (dealerId, button) {

        var row = $(".purchaseOrderRow[data-id='" + dealerId + "']");

        //--------------------------------------------------
        // Collapse
        //--------------------------------------------------

        if (row.is(":visible")) {

            row.hide();

            button.find("i")
                .removeClass("ri-arrow-down-s-line")
                .addClass("ri-arrow-right-s-line");

            return;
        }

        //--------------------------------------------------
        // Expand
        //--------------------------------------------------

        row.show();

        button.find("i")
            .removeClass("ri-arrow-right-s-line")
            .addClass("ri-arrow-down-s-line");
        //--------------------------------------------------
        // Already Loaded
        //--------------------------------------------------

        if (self.purchaseOrderCache[dealerId]) {

            self.renderPurchaseOrders(
                dealerId,
                self.purchaseOrderCache[dealerId]
            );

            return;
        }

        //--------------------------------------------------
        // Load Purchase Orders
        //--------------------------------------------------

        self.loadPurchaseOrders(dealerId);

    };
    //=========================================
    // Load Purchase Orders
    //=========================================

    self.loadPurchaseOrders = function (dealerId) {

        var container = $(".purchaseOrderRow[data-id='" + dealerId + "'] .purchaseOrderContainer");

        container.html(
            "<div class='text-center p-4'>" +
            "<div class='spinner-border text-primary'></div>" +
            "<div class='mt-2'>Loading Purchase Orders...</div>" +
            "</div>"
        );

        $.ajax({

            url: "/DealerPurchaseOrder/GetPurchaseOrders",

            type: "GET",

            data: {
                dealerId: dealerId
            },

            success: function (response) {

                self.purchaseOrderCache[dealerId] = response;

                self.renderPurchaseOrders(dealerId, response);

            },

            error: function () {

                container.html(
                    "<div class='alert alert-danger'>" +
                    "Unable to load Purchase Orders." +
                    "</div>"
                );

            }

        });

    };


    //=========================================
    // Render Purchase Orders
    //=========================================

    self.renderPurchaseOrders = function (dealerId, purchaseOrders) {

        var html = "";
        html += "<div class='card shadow-sm border-0'>";

        html += "<div class='card-header bg-white border-bottom'>";

        html += "<h6 class='mb-0 fw-semibold'>Purchase Orders</h6>";

        html += "</div>";

        html += "<div class='card-body p-0'>";

        if (!purchaseOrders || purchaseOrders.length == 0) {

            html += "<div class='text-center p-4'>";

            html += "No Purchase Orders Found.";

            html += "</div>";

        }
        else {

            html += "<table class='table table-hover table-bordered table-sm mb-0'>";

            html += "<thead class='table-secondary'>";

            html += "<tr>";

            html += "<th style='width:50px'></th>";

            html += "<th>PO Number</th>";

            html += "<th>Items</th>";

            html += "<th>Sub Total</th>";

            html += "<th>Tax</th>";

            html += "<th>Total</th>";

            html += "<th>Date</th>";

            html += "<th>Status</th>";

            html += "</tr>";

            html += "</thead>";

            html += "<tbody>";

            $.each(purchaseOrders, function (index, po) {

                var itemCount = 0;

                if (po.PurchaseOrderItemResponse) {

                    itemCount = po.PurchaseOrderItemResponse.length;

                }

                //----------------------------------

                html += "<tr>";

                html += "<td>";

                html += "<button class='btn btn-sm btn-outline-secondary expandPO' data-id='" + po.Id + "'>";

                html += "<i class='ri-arrow-right-s-line'></i>";

                html += "</button>";

                html += "</td>";

                html += "<td>";

                html += po.PONumber || "";

                html += "</td>";

                html += "<td>";

                html += itemCount;

                html += "</td>";

                html += "<td>";

                html += Number(po.SubTotal || 0).toFixed(2);

                html += "</td>";

                html += "<td>";

                html += Number(po.TaxAmount || 0).toFixed(2);

                html += "</td>";

                html += "<td>";

                html += Number(po.TotalAmount || 0).toFixed(2);

                html += "</td>";

                html += "<td>";

                html += self.formatDate(po.OrderDate);

                html += "</td>";

                html += "<td>";

                switch (po.Status) {

                    case "Approved":

                        html += "<span class='badge bg-success'>Approved</span>";

                        break;

                    case "Pending":

                        html += "<span class='badge bg-warning text-dark'>Pending</span>";

                        break;

                    case "Cancelled":

                        html += "<span class='badge bg-danger'>Cancelled</span>";

                        break;

                    default:

                        html += "<span class='badge bg-secondary'>" + (po.Status || "") + "</span>";

                        break;
                }

                html += "</td>";

                html += "</tr>";

                //---------------------------------------------------
                // Hidden Item Row
                //---------------------------------------------------

                html += "<tr class='itemRow' data-id='" + po.Id + "' style='display:none;'>";

                html += "<td colspan='8' class='border-0 bg-light'>";

                html += "<div class='itemContainer ms-4 p-3'></div>";

                html += "</td>";

                html += "</tr>";

            });

            html += "</tbody>";

            html += "</table>";

        }

        html += "</div>";

        html += "</div>";

        $(".purchaseOrderRow[data-id='" + dealerId + "'] .purchaseOrderContainer").html(html);

        //---------------------------------------------
        // Expand Purchase Order
        //---------------------------------------------

        $(".expandPO").off("click").on("click", function () {

            var purchaseOrderId = $(this).data("id");

            self.togglePurchaseOrder(
                purchaseOrderId,
                purchaseOrders,
                $(this)
            );

        });

    };
    //=========================================
    // Expand / Collapse Purchase Order
    //=========================================

    self.togglePurchaseOrder = function (purchaseOrderId, purchaseOrders, button) {

        var row = $(".itemRow[data-id='" + purchaseOrderId + "']");

        //-----------------------------------------
        // Collapse
        //-----------------------------------------

        if (row.is(":visible")) {

            row.hide();
            button.find("i")
                .removeClass("ri-arrow-down-s-line")
                .addClass("ri-arrow-right-s-line");

            return;
        }

        //-----------------------------------------
        // Expand
        //-----------------------------------------

        row.show();

        button.find("i")
            .removeClass("ri-arrow-right-s-line")
            .addClass("ri-arrow-down-s-line");

        //-----------------------------------------
        // Find Selected Purchase Order
        //-----------------------------------------

        $.each(purchaseOrders, function (index, po) {

            if (po.Id == purchaseOrderId) {

                self.renderItems(
                    purchaseOrderId,
                    po.PurchaseOrderItemResponse
                );

                return false;
            }

        });

    };


    //=========================================
    // Render Purchase Order Items
    //=========================================

    self.renderItems = function (purchaseOrderId, items) {

        var html = "";

        html += "<div class='card shadow-sm border-0'>";

        html += "<div class='card-header bg-white border-bottom'>";

        html += "<h6 class='mb-0 fw-semibold'>Purchase Order Items</h6>";

        html += "</div>";

        html += "<div class='card-body p-0'>";

        if (!items || items.length == 0) {

            html += "<div class='text-center p-4'>";

            html += "No Items Found.";

            html += "</div>";

        }
        else {

            html += "<table class='table table-bordered table-striped table-sm mb-0'>";

            html += "<thead class='table-light'>";

            html += "<tr>";

            html += "<th>Drug</th>";

            html += "<th>Drug Code</th>";

            html += "<th>Quantity</th>";

            html += "<th>Unit Price</th>";

            html += "<th>Discount</th>";

            html += "<th>Tax</th>";

            html += "<th>Total</th>";

            html += "<th>Received</th>";

            html += "<th>Pending</th>";

            html += "</tr>";

            html += "</thead>";

            html += "<tbody>";

            $.each(items, function (index, item) {

                html += "<tr>";

                html += "<td>" + (item.DrugName || "-") + "</td>";

                html += "<td>" + (item.DrugCode || "-") + "</td>";

                html += "<td>" + (item.Quantity || 0) + "</td>";

                html += "<td>₹ " + Number(item.UnitPrice || 0).toFixed(2) + "</td>";

                html += "<td>₹ " + Number(item.DiscountAmount || 0).toFixed(2) + "</td>";

                html += "<td>₹ " + Number(item.TaxAmount || 0).toFixed(2) + "</td>";

                html += "<td>₹ " + Number(item.TotalAmount || 0).toFixed(2) + "</td>";

                html += "<td>" + (item.ReceivedQuantity || 0) + "</td>";

                html += "<td>" + (item.PendingQuantity || 0) + "</td>";

                html += "</tr>";

            });

            html += "</tbody>";

            html += "</table>";

        }

        html += "</div>";

        html += "</div>";

        $(".itemRow[data-id='" + purchaseOrderId + "'] .itemContainer").html(html);

    };


    //=========================================
    // Date Format
    //=========================================

    self.formatDate = function (date) {

        if (!date)
            return "";

        var d = new Date(date);

        return d.toLocaleDateString("en-IN", {

            day: "2-digit",

            month: "short",

            year: "numeric"

        });

    };

}