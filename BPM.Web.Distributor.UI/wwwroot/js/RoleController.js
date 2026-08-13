function RolesController() {

    var self = this;

    // Properties
    self.roles = [];
    self.filteredRoles = [];
    self.currentPage = 1;
    self.pageSize = 10;
    self.totalPages = 1;
    self.searchTerm = '';

    // DOM references
    self.$tableBody = null;
    self.$pagination = null;
    self.$entriesInfo = null;
    self.$searchInput = null;
    self.$modal = null;


    // =========================================================
    // INIT
    // =========================================================

    self.init = function () {

        self.$tableBody = $("#roleTableBody");
        self.$pagination = $("#paginationControls");
        self.$entriesInfo = $("#entriesInfo");
        self.$searchInput = $("#searchInput");
        self.$modal = $("#roleModal");

        self.bindEvents();

        self.loadRoles();

    };


    // =========================================================
    // EVENTS
    // =========================================================

    self.bindEvents = function () {

        // Search
        self.$searchInput.on("keyup", function () {

            self.searchTerm =
                ($(this).val() || "")
                    .toLowerCase()
                    .trim();

            self.currentPage = 1;

            self.render();

        });


        // Add Role
        $("#btnAddRole").on("click", function () {

            $("#RoleId").val("");
            $("#RoleName").val("");
            $("#Code").val("");
            $("#Description").val("");

            $("#roleModalTitle")
                .text("Add Role");

            self.$modal.modal("show");

        });


        // Save Role
        $("#btnSaveRole").on("click", function () {

            self.saveRole();

        });


        // Edit Role
        self.$tableBody.on(
            "click",
            ".editRole",
            function () {

                var id =
                    $(this).data("id");

                self.editRole(id);

            }
        );


        // View Permissions
        self.$tableBody.on(
            "click",
            ".viewPermissions",
            function () {

                var id =
                    $(this).data("id");

                self.viewPermissions(id);

            }
        );


        // Pagination
        self.$pagination.on(
            "click",
            ".page-link",
            function () {

                var page =
                    $(this).data("page");


                if (page === "prev") {

                    if (self.currentPage > 1) {

                        self.currentPage--;

                    }

                }
                else if (page === "next") {

                    if (
                        self.currentPage <
                        self.totalPages
                    ) {

                        self.currentPage++;

                    }

                }
                else {

                    self.currentPage =
                        parseInt(page);

                }


                self.render();

            }
        );

    };


    // =========================================================
    // LOAD ROLES
    // =========================================================

    self.loadRoles = function () {

        showLoader("Loading Roles...");


        $.ajax({

            url: "/Role/GetAllRoles",

            type: "GET",

            dataType: "json",


            success: function (response) {

                self.roles =
                    Array.isArray(response)
                        ? response
                        : [];


                self.filteredRoles =
                    self.roles;


                self.render();


                hideLoader();

            },


            error: function (xhr) {

                console.log(
                    xhr.responseText
                );


                hideLoader();


                toastNotifyError(
                    "Unable to load roles."
                );

            }

        });

    };


    // =========================================================
    // RENDER
    // =========================================================

    self.render = function () {

        self.filterRoles();

        self.renderTableBody();

        self.renderPagination();

        self.renderEntriesInfo();

    };


    // =========================================================
    // FILTER
    // =========================================================

    self.filterRoles = function () {

        if (!self.searchTerm) {

            self.filteredRoles =
                self.roles;

        }
        else {

            self.filteredRoles =
                self.roles.filter(function (role) {

                    return (

                        (role.Name || "")
                            .toLowerCase()
                            .includes(
                                self.searchTerm
                            )

                        ||

                        (role.Code || "")
                            .toLowerCase()
                            .includes(
                                self.searchTerm
                            )

                        ||

                        (role.Description || "")
                            .toLowerCase()
                            .includes(
                                self.searchTerm
                            )

                    );

                });

        }


        self.totalPages =
            Math.ceil(
                self.filteredRoles.length /
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
    // RENDER TABLE
    // =========================================================

    self.renderTableBody = function () {

        var start =
            (self.currentPage - 1) *
            self.pageSize;


        var end =
            start +
            self.pageSize;


        var roles =
            self.filteredRoles.slice(
                start,
                end
            );


        if (roles.length === 0) {

            self.$tableBody.html(`

                <tr>

                    <td colspan="5"
                        class="text-center py-5 text-muted">

                        No roles found.

                    </td>

                </tr>

            `);

            return;

        }


        var html = "";


        $.each(
            roles,
            function (i, role) {

                html += `

                    <tr>

                        <!-- Role Name -->
                        <td class="align-middle">

                            ${self.escapeHtml(
                    role.Name
                )}

                        </td>


                        <!-- Code -->
                        <td class="align-middle">

                            ${self.escapeHtml(
                    role.Code
                )}

                        </td>


                        <!-- Description -->
                        <td class="align-middle">

                            ${self.escapeHtml(
                    role.Description ||
                    "N/A"
                )}

                        </td>


                        <!-- Status -->
                        <td class="align-middle">

                            <span class="
                                fs-15
                                fw-normal
                                d-inline-block
                                default-badge
                                ${role.IsActive
                        ? "text-success bg-success bg-opacity-10"
                        : "text-danger bg-danger bg-opacity-10"
                    }
                            ">

                                ${role.IsActive
                        ? "Active"
                        : "Inactive"
                    }

                            </span>

                        </td>


                        <!-- Action -->
                        <td class="align-middle text-center">

                            <div class="role-action-buttons">

                                <!-- View Permissions -->
                                <button
                                    type="button"
                                    class="
                                        role-action-btn
                                        role-view-btn
                                        viewPermissions
                                    "
                                    data-id="${role.Id}"
                                    title="View Permissions"
                                    aria-label="View Permissions">

                                    <i class="ri-eye-line"></i>

                                </button>


                                <!-- Edit Role -->
                                <button
                                    type="button"
                                    class="
                                        role-action-btn
                                        role-edit-btn
                                        editRole
                                    "
                                    data-id="${role.Id}"
                                    title="Edit Role"
                                    aria-label="Edit Role">

                                    <i class="ri-pencil-line"></i>

                                </button>

                            </div>

                        </td>

                    </tr>

                `;

            }
        );


        self.$tableBody.html(html);

    };


    // =========================================================
    // VIEW PERMISSIONS
    // =========================================================

    self.viewPermissions = function (roleId) {

        if (!roleId) {

            toastNotifyError(
                "Invalid role."
            );

            return;

        }


        window.location.href =
            "/Permission/GetPermissionsByRole/" +
            roleId;

    };


    // =========================================================
    // PAGINATION
    // =========================================================

    self.renderPagination = function () {

        if (
            self.totalPages <= 1 ||
            self.filteredRoles.length === 0
        ) {

            self.$pagination.html("");

            return;

        }


        var html = "";


        // Previous
        html += `

            <li class="page-item
                ${self.currentPage === 1
                ? "disabled"
                : ""
            }">

                <button
                    type="button"
                    aria-label="Previous"
                    class="page-link"
                    data-page="prev">

                    ‹

                </button>

            </li>

        `;


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


        // Page numbers
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
                        class="page-link"
                        data-page="${i}">

                        ${i}

                    </button>

                </li>

            `;

        }


        // Next
        html += `

            <li class="page-item
                ${self.currentPage ===
                self.totalPages
                ? "disabled"
                : ""
            }">

                <button
                    type="button"
                    aria-label="Next"
                    class="page-link"
                    data-page="next">

                    ›

                </button>

            </li>

        `;


        self.$pagination.html(html);

    };


    // =========================================================
    // ENTRIES INFO
    // =========================================================

    self.renderEntriesInfo = function () {

        var total =
            self.filteredRoles.length;


        var start =
            total > 0
                ? (
                    (self.currentPage - 1) *
                    self.pageSize
                ) + 1
                : 0;


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
    // SAVE ROLE
    // =========================================================

    self.saveRole = function () {

        var id =
            $("#RoleId").val();


        var role = {

            Name:
                $("#RoleName").val(),

            Code:
                $("#Code").val(),

            Description:
                $("#Description").val(),

            IsActive:
                true

        };


        var url =
            id === ""
                ? "/Role/Create"
                : "/Role/Update?id=" + id;


        var method =
            id === ""
                ? "POST"
                : "PUT";


        if (id !== "") {

            role.Id = id;

        }


        showLoader(

            id === ""
                ? "Saving Role..."
                : "Updating Role..."

        );


        $.ajax({

            url: url,

            type: method,

            contentType:
                "application/json; charset=utf-8",

            dataType: "json",

            data:
                JSON.stringify(role),


            success: function () {

                hideLoader();

                self.$modal.modal("hide");

                self.loadRoles();

            },


            error: function (xhr) {

                hideLoader();


                console.log(
                    xhr.responseText
                );


                toastNotifyError(

                    xhr.responseText ||
                    "Unable to save role."

                );

            }

        });

    };


    // =========================================================
    // EDIT ROLE
    // =========================================================

    self.editRole = function (id) {

        showLoader(
            "Loading Role..."
        );


        $.ajax({

            url:
                "/Role/Get?id=" +
                id,

            type: "GET",

            dataType: "json",


            success: function (data) {

                hideLoader();


                $("#RoleId")
                    .val(data.Id);


                $("#RoleName")
                    .val(data.Name);


                $("#Code")
                    .val(data.Code);


                $("#Description")
                    .val(
                        data.Description || ""
                    );


                $("#roleModalTitle")
                    .text("Edit Role");


                self.$modal.modal("show");

            },


            error: function (xhr) {

                hideLoader();


                console.log(
                    xhr.responseText
                );


                toastNotifyError(
                    "Unable to load role."
                );

            }

        });

    };


    // =========================================================
    // ESCAPE HTML
    // =========================================================

    self.escapeHtml = function (text) {

        if (!text) {

            return "";

        }


        var div =
            document.createElement("div");


        div.textContent = text;


        return div.innerHTML;

    };

}