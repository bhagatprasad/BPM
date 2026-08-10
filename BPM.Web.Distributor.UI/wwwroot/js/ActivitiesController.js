function ActivitiesController() {

    var self = this;


    // Properties

    self.activities = [];
    self.filteredActivities = [];

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

        self.$tableBody = $("#activityTableBody");
        self.$pagination = $("#paginationControls");
        self.$entriesInfo = $("#entriesInfo");
        self.$searchInput = $("#searchInput");
        self.$modal = $("#activityModal");

        self.bindEvents();

        self.loadActivities();

    };


    self.bindEvents = function () {

        // Search

        self.$searchInput.on("keyup", function () {

            self.searchTerm = $(this).val().toLowerCase();

            self.currentPage = 1;

            self.render();

        });


        // Add Activity

        $("#btnAddActivity").on("click", function () {

            $("#ActivityId").val("");
            $("#ActivityName").val("");
            $("#Code").val("");
            $("#Description").val("");

            $("#activityModalTitle").text("Add Activity");

            self.$modal.modal("show");

        });


        // Save Activity

        $("#btnSaveActivity").on("click", function () {

            self.saveActivity();

        });


        // Edit Activity

        self.$tableBody.on("click", ".editActivity", function () {

            self.editActivity($(this).data("id"));

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


    // Load Activities

    self.loadActivities = function () {

        showLoader("Loading Activities...");


        $.ajax({

            url: "/Activity/GetAllActivities",

            type: "GET",

            dataType: "json",


            success: function (response) {

                self.activities = Array.isArray(response)
                    ? response
                    : [];

                self.filteredActivities = self.activities;

                self.render();

                hideLoader();

            },


            error: function (xhr) {

                hideLoader();

                console.log(xhr.responseText);

                toastNotifyError("Unable to load activities.");

            }

        });

    };


    // Render

    self.render = function () {

        self.filterActivities();

        self.renderTableBody();

        self.renderPagination();

        self.renderEntriesInfo();

    };


    // Filter

    self.filterActivities = function () {

        if (!self.searchTerm) {

            self.filteredActivities = self.activities;

        }
        else {

            self.filteredActivities =
                self.activities.filter(function (x) {

                    return (x.ActivityName || "")
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
                self.filteredActivities.length /
                self.pageSize
            );


        if (self.currentPage > self.totalPages) {

            self.currentPage =
                self.totalPages || 1;

        }

    };


    // Render Table

    self.renderTableBody = function () {

        var start = (self.currentPage - 1) * self.pageSize;
        var end = start + self.pageSize;

        var activities = self.filteredActivities.slice(start, end);

        if (activities.length === 0) {

            self.$tableBody.html(`
            <tr>
                <td colspan="5"
                    class="text-center py-4 text-muted">
                    No activities found.
                </td>
            </tr>
        `);

            return;
        }

        var html = "";

        $.each(activities, function (i, item) {

            html += `
            <tr>

                <td class="align-middle">
                    ${self.escapeHtml(item.ActivityName)}
                </td>

                <td class="align-middle">
                    ${self.escapeHtml(item.Code)}
                </td>

                <td class="align-middle">
                    ${self.escapeHtml(item.Description || "")}
                </td>

                <td class="align-middle">

                    <span class="badge rounded-pill px-3 py-2 ${item.IsActive ? "bg-success" : "bg-danger"}">

                        ${item.IsActive ? "Active" : "Inactive"}

                    </span>

                </td>

                <td class="align-middle">

                    <button
                        class="btn btn-sm btn-outline-primary editActivity"
                        data-id="${item.ActivityId}"
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
            self.filteredActivities.length;


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


    // Save Activity

    self.saveActivity = function () {

        var id =
            $("#ActivityId").val();


        var activity = {

            activityName:
                $("#ActivityName").val(),

            code:
                $("#Code").val(),

            description:
                $("#Description").val()

        };


        var url =
            id === ""
                ? "/Activity/Create"
                : "/Activity/Edit?id=" + id;


        var method =
            id === ""
                ? "POST"
                : "PUT";


        showLoader(
            id === ""
                ? "Saving Activity..."
                : "Updating Activity..."
        );


        $.ajax({

            url: url,

            type: method,

            contentType: "application/json",

            data: JSON.stringify(activity),


            success: function () {

                hideLoader();

                self.$modal.modal("hide");

                self.loadActivities();

            },


            error: function (xhr) {

                hideLoader();

                console.log(xhr.responseText);

                toastNotifyError(
                    xhr.responseText ||
                    "Unable to save activity."
                );

            }

        });

    };


    // Edit Activity

    self.editActivity = function (id) {

        showLoader("Loading Activity...");


        $.ajax({

            url: "/Activity/Get?id=" + id,

            type: "GET",


            success: function (data) {

                hideLoader();


                $("#ActivityId")
                    .val(data.ActivityId);

                $("#ActivityName")
                    .val(data.ActivityName);

                $("#Code")
                    .val(data.Code);

                $("#Description")
                    .val(data.Description);


                $("#activityModalTitle")
                    .text("Edit Activity");


                self.$modal.modal("show");

            },


            error: function (xhr) {

                hideLoader();

                console.log(xhr.responseText);

                toastNotifyError(
                    "Unable to load activity."
                );

            }

        });

    };

}