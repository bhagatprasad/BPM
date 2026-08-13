function PermissionController() {

    var self = this;

    self.permissions = [];

    self.filteredPermissions = [];

    self.currentPage = 1;

    self.pageSize = 10;

    self.totalPages = 1;

    self.searchTerm = "";


    self.$tableBody = null;

    self.$tableHead = null;

    self.$pagination = null;

    self.$entriesInfo = null;

    self.$searchInput = null;


    // =========================================================
    // INIT
    // =========================================================

    self.init = function () {

        self.$tableBody = $("#permissionTableBody");

        self.$tableHead = $("#permissionTableHead");

        self.$pagination = $("#permissionPagination");

        self.$entriesInfo = $("#permissionEntriesInfo");

        self.$searchInput = $("#permissionSearchInput");


        self.loadFromPage();

        self.bindEvents();

        self.render();

    };


    // =========================================================
    // LOAD DATA FROM RAZOR
    // =========================================================

    self.loadFromPage = function () {

        self.permissions = [];


        self.$tableBody
            .find("tr.permission-row")
            .each(function () {

                var $row = $(this);

                var $checkbox =
                    $row.find(".permission-toggle");


                var permission = {

                    PermissionId:
                        $checkbox.data("id"),

                    RoleId:
                        $checkbox.data("role-id"),

                    FeatureId:
                        $checkbox.data("feature-id"),

                    ActivityId:
                        $checkbox.data("activity-id"),

                    RoleName:
                        $row.find(".permission-role")
                            .text()
                            .trim(),

                    FeatureName:
                        $row.find(".permission-feature")
                            .text()
                            .trim(),

                    ActivityName:
                        $row.find(".permission-activity")
                            .text()
                            .trim(),

                    IsEnabled:
                        $checkbox.is(":checked")

                };


                if (
                    permission.PermissionId &&
                    permission.FeatureName &&
                    permission.ActivityName
                ) {

                    self.permissions.push(permission);

                }

            });


        self.filteredPermissions =
            self.permissions.slice();

    };


    // =========================================================
    // EVENTS
    // =========================================================

    self.bindEvents = function () {


        // -----------------------------------------------------
        // SEARCH
        // -----------------------------------------------------

        self.$searchInput.on(
            "keyup",
            function () {

                self.searchTerm =
                    ($(this).val() || "")
                        .toLowerCase()
                        .trim();


                self.currentPage = 1;

                self.render();

            }
        );


        // -----------------------------------------------------
        // PERMISSION SWITCH
        // -----------------------------------------------------

        self.$tableBody.on(
            "change",
            ".permission-toggle",
            function () {

                var $checkbox =
                    $(this);


                var permissionId =
                    $checkbox.data("id");


                var isEnabled =
                    $checkbox.is(":checked");


                self.updatePermission(
                    permissionId,
                    isEnabled,
                    $checkbox
                );

            }
        );


        // -----------------------------------------------------
        // PAGINATION
        // -----------------------------------------------------

        self.$pagination.on(
            "click",
            ".permission-page",
            function () {

                var page =
                    parseInt(
                        $(this).data("page")
                    );


                if (!page) {
                    return;
                }


                if (
                    page < 1 ||
                    page > self.totalPages
                ) {
                    return;
                }


                self.currentPage = page;

                self.render();

            }
        );


        // -----------------------------------------------------
        // BACK TO ROLES
        // -----------------------------------------------------

        $("#btnBackToRoles").on(
            "click",
            function () {

                window.location.href =
                    "/Role/Index";

            }
        );

    };


    // =========================================================
    // FILTER
    // =========================================================

    self.filterPermissions = function () {

        if (!self.searchTerm) {

            self.filteredPermissions =
                self.permissions.slice();

        }
        else {

            self.filteredPermissions =
                self.permissions.filter(
                    function (permission) {

                        return (

                            (permission.FeatureName || "")
                                .toLowerCase()
                                .includes(
                                    self.searchTerm
                                )

                            ||

                            (permission.ActivityName || "")
                                .toLowerCase()
                                .includes(
                                    self.searchTerm
                                )

                            ||

                            (permission.RoleName || "")
                                .toLowerCase()
                                .includes(
                                    self.searchTerm
                                )

                        );

                    }
                );

        }


        // -----------------------------------------------------
        // GROUP BY FEATURE
        // -----------------------------------------------------

        var featureNames = [];


        $.each(
            self.filteredPermissions,
            function (_, permission) {

                if (
                    featureNames.indexOf(
                        permission.FeatureName
                    ) === -1
                ) {

                    featureNames.push(
                        permission.FeatureName
                    );

                }

            }
        );


        self.totalPages =
            Math.ceil(
                featureNames.length /
                self.pageSize
            );


        if (self.totalPages < 1) {

            self.totalPages = 1;

        }


        if (
            self.currentPage >
            self.totalPages
        ) {

            self.currentPage =
                self.totalPages;

        }

    };


    // =========================================================
    // RENDER
    // =========================================================

    self.render = function () {

        self.filterPermissions();

        self.renderHeader();

        self.renderRows();

        self.renderPagination();

        self.renderEntriesInfo();

    };


    // =========================================================
    // GET UNIQUE ACTIVITIES
    // =========================================================

    self.getActivities = function () {

        var activities = [];


        $.each(
            self.permissions,
            function (_, permission) {

                if (
                    activities.indexOf(
                        permission.ActivityName
                    ) === -1
                ) {

                    activities.push(
                        permission.ActivityName
                    );

                }

            }
        );


        return activities;

    };


    // =========================================================
    // RENDER HEADER
    // =========================================================

    self.renderHeader = function () {

        var activities =
            self.getActivities();


        var html = "";


        html += `

            <tr>

                <th class="permission-feature-header">

                    Feature

                </th>

        `;


        $.each(
            activities,
            function (_, activity) {

                html += `

                    <th class="permission-activity-header">

                        ${self.escapeHtml(activity)}

                    </th>

                `;

            }
        );


        html += `

            </tr>

        `;


        self.$tableHead.html(html);

    };


    // =========================================================
    // RENDER ROWS
    // =========================================================

    self.renderRows = function () {

        var activities =
            self.getActivities();


        // -----------------------------------------------------
        // GET UNIQUE FEATURES
        // -----------------------------------------------------

        var featureNames = [];


        $.each(
            self.filteredPermissions,
            function (_, permission) {

                if (
                    featureNames.indexOf(
                        permission.FeatureName
                    ) === -1
                ) {

                    featureNames.push(
                        permission.FeatureName
                    );

                }

            }
        );


        // -----------------------------------------------------
        // PAGINATION
        // -----------------------------------------------------

        var start =
            (self.currentPage - 1) *
            self.pageSize;


        var end =
            start +
            self.pageSize;


        var pageFeatures =
            featureNames.slice(
                start,
                end
            );


        // -----------------------------------------------------
        // NO DATA
        // -----------------------------------------------------

        if (
            pageFeatures.length === 0
        ) {

            self.$tableBody.html(`

                <tr class="no-permission-row">

                    <td
                        colspan="${activities.length + 1}"
                        class="text-center py-5 text-muted">

                        No permissions found.

                    </td>

                </tr>

            `);

            return;

        }


        var html = "";


        // -----------------------------------------------------
        // EACH FEATURE
        // -----------------------------------------------------

        $.each(
            pageFeatures,
            function (_, featureName) {


                html += `

                    <tr class="permission-feature-row">

                        <td class="permission-feature-cell">

                            ${self.escapeHtml(featureName)}

                        </td>

                `;


                // -------------------------------------------------
                // EACH ACTIVITY
                // -------------------------------------------------

                $.each(
                    activities,
                    function (_, activityName) {


                        var permission =
                            self.permissions.find(
                                function (x) {

                                    return (

                                        x.FeatureName ===
                                        featureName

                                        &&

                                        x.ActivityName ===
                                        activityName

                                    );

                                }
                            );


                        // -----------------------------------------
                        // ACTIVITY DOES NOT EXIST FOR FEATURE
                        // -----------------------------------------

                        if (!permission) {

                            html += `

                                <td class="permission-activity-cell">

                                    <span class="text-muted">
                                        -
                                    </span>

                                </td>

                            `;

                            return;

                        }


                        // -----------------------------------------
                        // ACTIVITY EXISTS
                        // -----------------------------------------

                        html += `

                            <td class="permission-activity-cell">

                                <div class="permission-cell-content">

                                    <label class="permission-switch">

                                        <input
                                            type="checkbox"
                                            class="permission-toggle"
                                            data-id="${permission.PermissionId}"
                                            data-role-id="${permission.RoleId}"
                                            data-feature-id="${permission.FeatureId}"
                                            data-activity-id="${permission.ActivityId}"
                                            ${permission.IsEnabled
                                ? "checked"
                                : ""
                            }
                                        />

                                        <span class="permission-slider"></span>

                                    </label>

                                    <span class="permission-label">

                                        ${permission.IsEnabled
                                ? "Enabled"
                                : "Disabled"
                            }

                                    </span>

                                </div>

                            </td>

                        `;

                    }
                );


                html += `

                    </tr>

                `;

            }
        );


        self.$tableBody.html(html);

    };


    // =========================================================
    // UPDATE PERMISSION
    // =========================================================

    self.updatePermission = function (
        permissionId,
        isEnabled,
        $checkbox
    ) {


        // -----------------------------------------------------
        // FIND PERMISSION
        // -----------------------------------------------------

        var permission =
            self.permissions.find(
                function (x) {

                    return String(
                        x.PermissionId
                    ) === String(
                        permissionId
                    );

                }
            );


        if (!permission) {

            toastNotifyError(
                "Permission was not found."
            );


            $checkbox.prop(
                "checked",
                !isEnabled
            );


            return;

        }


        // -----------------------------------------------------
        // DTO
        // -----------------------------------------------------

        var dto = {

            RoleId:
                permission.RoleId,

            FeatureId:
                permission.FeatureId,

            ActivityId:
                permission.ActivityId,

            IsEnabled:
                isEnabled

        };


        // -----------------------------------------------------
        // LOADER
        // -----------------------------------------------------

        showLoader(
            isEnabled
                ? "Enabling Permission..."
                : "Disabling Permission..."
        );


        // -----------------------------------------------------
        // API CALL
        // -----------------------------------------------------

        $.ajax({

            url:
                "/Permission/Update/" +
                permissionId,

            type:
                "PUT",

            contentType:
                "application/json; charset=utf-8",

            dataType:
                "json",

            data:
                JSON.stringify(dto),


            success:
                function () {


                    hideLoader();


                    // -----------------------------------------
                    // UPDATE LOCAL DATA
                    // -----------------------------------------

                    permission.IsEnabled =
                        isEnabled;


                    // -----------------------------------------
                    // UPDATE LABEL
                    // -----------------------------------------

                    var $cell =
                        $checkbox.closest(
                            ".permission-cell-content"
                        );


                    $cell.find(
                        ".permission-label"
                    ).text(
                        isEnabled
                            ? "Enabled"
                            : "Disabled"
                    );


                    toastNotifySuccess(
                        isEnabled
                            ? "Permission enabled successfully."
                            : "Permission disabled successfully."
                    );

                },


            error:
                function (xhr) {


                    hideLoader();


                    console.log(
                        "Permission update failed."
                    );


                    console.log(
                        "Status:",
                        xhr.status
                    );


                    console.log(
                        "Response:",
                        xhr.responseText
                    );


                    // -----------------------------------------
                    // ROLLBACK SWITCH
                    // -----------------------------------------

                    $checkbox.prop(
                        "checked",
                        !isEnabled
                    );


                    toastNotifyError(
                        "Unable to update permission."
                    );

                }

        });

    };


    // =========================================================
    // PAGINATION
    // =========================================================

    self.renderPagination = function () {

        if (
            self.filteredPermissions.length === 0 ||
            self.totalPages <= 1
        ) {

            self.$pagination.html("");

            return;

        }


        var html = "";


        // -----------------------------------------------------
        // PREVIOUS
        // -----------------------------------------------------

        html += `

            <li class="page-item
                ${self.currentPage === 1
                ? "disabled"
                : ""
            }">

                <button
                    type="button"
                    class="page-link permission-page"
                    data-page="${self.currentPage - 1}"
                    aria-label="Previous">

                    <i class="ri-arrow-left-s-line"></i>

                </button>

            </li>

        `;


        // -----------------------------------------------------
        // PAGE NUMBERS
        // -----------------------------------------------------

        var maxPages = 5;


        var start =
            Math.max(
                1,
                self.currentPage -
                Math.floor(maxPages / 2)
            );


        var end =
            Math.min(
                self.totalPages,
                start + maxPages - 1
            );


        if (
            end - start <
            maxPages - 1
        ) {

            start =
                Math.max(
                    1,
                    end - maxPages + 1
                );

        }


        for (
            var i = start;
            i <= end;
            i++
        ) {

            html += `

                <li class="page-item
                    ${i === self.currentPage
                    ? "active"
                    : ""
                }">

                    <button
                        type="button"
                        class="page-link permission-page"
                        data-page="${i}">

                        ${i}

                    </button>

                </li>

            `;

        }


        // -----------------------------------------------------
        // NEXT
        // -----------------------------------------------------

        html += `

            <li class="page-item
                ${self.currentPage === self.totalPages
                ? "disabled"
                : ""
            }">

                <button
                    type="button"
                    class="page-link permission-page"
                    data-page="${self.currentPage + 1}"
                    aria-label="Next">

                    <i class="ri-arrow-right-s-line"></i>

                </button>

            </li>

        `;


        self.$pagination.html(html);

    };


    // =========================================================
    // ENTRIES INFO
    // =========================================================

    self.renderEntriesInfo = function () {

        var featureNames = [];


        $.each(
            self.filteredPermissions,
            function (_, permission) {

                if (
                    featureNames.indexOf(
                        permission.FeatureName
                    ) === -1
                ) {

                    featureNames.push(
                        permission.FeatureName
                    );

                }

            }
        );


        var total =
            featureNames.length;


        if (total === 0) {

            self.$entriesInfo.text(
                "Showing 0 to 0 of 0 entries"
            );

            return;

        }


        var start =
            ((self.currentPage - 1) *
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


    // =========================================================
    // ESCAPE HTML
    // =========================================================

    self.escapeHtml = function (text) {

        if (
            text === null ||
            text === undefined
        ) {

            return "";

        }


        var div =
            document.createElement("div");


        div.textContent =
            String(text);


        return div.innerHTML;

    };

}