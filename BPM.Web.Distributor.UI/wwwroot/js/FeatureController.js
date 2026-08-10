function FeatureController() {

    var self = this;


    // Properties

    self.features = [];
    self.filteredFeatures = [];

    self.currentPage = 1;
    self.pageSize = 10;
    self.totalPages = 1;
    self.searchTerm = "";


    // Cached DOM

    self.$tableBody = null;
    self.$pagination = null;
    self.$entriesInfo = null;
    self.$searchInput = null;
    self.$modal = null;


    self.init = function () {

        // Cache DOM

        self.$tableBody = $("#featureTableBody");
        self.$pagination = $("#paginationControls");
        self.$entriesInfo = $("#entriesInfo");
        self.$searchInput = $("#searchInput");
        self.$modal = $("#featureModal");


        self.bindEvents();

        self.loadFeatures();

    };


    self.bindEvents = function () {


        // Search

        self.$searchInput.on("keyup", function () {

            self.searchTerm = $(this).val().toLowerCase();

            self.currentPage = 1;

            self.render();

        });


        // Add Feature

        $("#btnAddFeature").on("click", function () {

            $("#FeatureId").val("");
            $("#FeatureName").val("");
            $("#Code").val("");
            $("#Description").val("");

            $("#featureModalTitle").text("Add Feature");

            self.$modal.modal("show");

        });


        // Save Feature

        $("#btnSaveFeature").on("click", function () {

            self.saveFeature();

        });


        // Edit Feature

        self.$tableBody.on("click", ".editFeature", function () {

            self.editFeature($(this).data("id"));

        });


        // Pagination

        self.$pagination.on("click", ".page-link", function () {

            var page = $(this).data("page");


            if (page === "prev") {

                if (self.currentPage > 1) {
                    self.currentPage--;
                }

            }
            else if (page === "next") {

                if (self.currentPage < self.totalPages) {
                    self.currentPage++;
                }

            }
            else {

                self.currentPage = parseInt(page);

            }


            self.render();

        });

    };


    // Load Features

    self.loadFeatures = function () {

        showLoader("Loading Features...");


        $.ajax({

            url: "/Feature/GetAllFeatures",

            type: "GET",

            dataType: "json",


            success: function (response) {

                self.features = Array.isArray(response)
                    ? response
                    : [];

                self.filteredFeatures = self.features;

                self.render();

                hideLoader();

            },


            error: function (xhr) {

                hideLoader();

                console.log(xhr.responseText);

                toastNotifyError("Unable to load features.");

            }

        });

    };


    // Render

    self.render = function () {

        self.filterFeatures();

        self.renderTableBody();

        self.renderPagination();

        self.renderEntriesInfo();

    };


    // Filter

    self.filterFeatures = function () {

        if (!self.searchTerm) {

            self.filteredFeatures = self.features;

        }
        else {

            self.filteredFeatures = self.features.filter(function (x) {

                return (x.FeatureName || "")
                    .toLowerCase()
                    .includes(self.searchTerm)

                    ||

                    (x.Code || "")
                        .toLowerCase()
                        .includes(self.searchTerm)

                    ||

                    (x.Description || "")
                        .toLowerCase()
                        .includes(self.searchTerm);

            });

        }


        self.totalPages =
            Math.ceil(
                self.filteredFeatures.length / self.pageSize
            );


        if (self.currentPage > self.totalPages) {

            self.currentPage =
                self.totalPages || 1;

        }

    };


    // Render Table

    self.renderTableBody = function () {

        var start =
            (self.currentPage - 1) * self.pageSize;

        var end =
            start + self.pageSize;


        var features =
            self.filteredFeatures.slice(start, end);


        if (features.length === 0) {

            self.$tableBody.html(`

                <tr>

                    <td colspan="5"
                        class="text-center py-4 text-muted">

                        No features found.

                    </td>

                </tr>

            `);

            return;

        }


        var html = "";


        $.each(features, function (i, item) {

            html += `

                <tr>

                    <!-- Feature Name -->

                    <td class="align-middle">

                        ${self.escapeHtml(item.FeatureName)}

                    </td>


                    <!-- Code -->

                    <td class="align-middle">

                        ${self.escapeHtml(item.Code)}

                    </td>


                    <!-- Description -->

                    <td class="align-middle">

                        ${self.escapeHtml(item.Description || "")}

                    </td>


                    <!-- Status -->

                    <td class="align-middle">

                        <span class="badge rounded-pill px-3 py-2 ${item.IsActive ? "bg-success" : "bg-danger"}">

                            ${item.IsActive ? "Active" : "Inactive"}

                        </span>

                    </td>


                    <!-- Action -->

                    <td class="align-middle">

                        <button

                            class="btn btn-sm btn-outline-primary editFeature"

                            data-id="${item.FeatureId}"

                            title="Edit">

                            <i class="ri-edit-line"></i>

                        </button>

                    </td>


                </tr>

            `;

        });


        self.$tableBody.html(html);

    };


    // Pagination

    self.renderPagination = function () {

        if (self.totalPages <= 1) {

            self.$pagination.html("");

            return;

        }


        var html = "";


        html += `

            <li class="page-item ${self.currentPage === 1 ? "disabled" : ""}">

                <button class="page-link"
                        data-page="prev">

                    Previous

                </button>

            </li>

        `;


        for (var i = 1; i <= self.totalPages; i++) {

            html += `

                <li class="page-item ${i === self.currentPage ? "active" : ""}">

                    <button class="page-link"
                            data-page="${i}">

                        ${i}

                    </button>

                </li>

            `;

        }


        html += `

            <li class="page-item ${self.currentPage === self.totalPages ? "disabled" : ""}">

                <button class="page-link"
                        data-page="next">

                    Next

                </button>

            </li>

        `;


        self.$pagination.html(html);

    };


    // Entries Information

    self.renderEntriesInfo = function () {

        var total =
            self.filteredFeatures.length;


        var start =
            total === 0
                ? 0
                : ((self.currentPage - 1) *
                    self.pageSize) + 1;


        var end =
            Math.min(
                self.currentPage *
                self.pageSize,
                total
            );


        self.$entriesInfo.text(

            "Showing " +
            start +
            " to " +
            end +
            " of " +
            total +
            " entries"

        );

    };


    // Escape HTML

    self.escapeHtml = function (text) {

        if (!text) {
            return "";
        }


        var div =
            document.createElement("div");


        div.textContent = text;


        return div.innerHTML;

    };


    // Save Feature

    self.saveFeature = function () {

        var id =
            $("#FeatureId").val();


        var feature = {

            featureName:
                $("#FeatureName").val(),

            code:
                $("#Code").val(),

            description:
                $("#Description").val()

        };


        var url =
            id === ""
                ? "/Feature/Create"
                : "/Feature/Edit?id=" + id;


        var method =
            id === ""
                ? "POST"
                : "PUT";


        showLoader(
            id === ""
                ? "Saving Feature..."
                : "Updating Feature..."
        );


        $.ajax({

            url: url,

            type: method,

            contentType: "application/json",

            data: JSON.stringify(feature),


            success: function () {

                hideLoader();

                self.$modal.modal("hide");

                self.loadFeatures();

            },


            error: function (xhr) {

                hideLoader();

                console.log(xhr.responseText);

                toastNotifyError(
                    xhr.responseText ||
                    "Unable to save feature."
                );

            }

        });

    };


    // Edit Feature

    self.editFeature = function (id) {

        showLoader("Loading Feature...");


        $.ajax({

            url: "/Feature/Get?id=" + id,

            type: "GET",


            success: function (feature) {

                hideLoader();


                $("#FeatureId")
                    .val(feature.FeatureId);

                $("#FeatureName")
                    .val(feature.FeatureName);

                $("#Code")
                    .val(feature.Code);

                $("#Description")
                    .val(feature.Description);


                $("#featureModalTitle")
                    .text("Edit Feature");


                self.$modal.modal("show");

            },


            error: function (xhr) {

                hideLoader();

                console.log(xhr.responseText);

                toastNotifyError(
                    "Unable to load feature."
                );

            }

        });

    };

}