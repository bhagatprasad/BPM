
window.onbeforeunload = function () {
    sessionStorage.clear();

    // Get all cookies
    var cookies = document.cookie.split(";");

    // Loop through the cookies and delete each one
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var eqPos = cookie.indexOf("=");
        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/;Secure;SameSite=Strict;";
    }
};
window.addEventListener('unload', function () {
    window.location.href = '/Account/Login';
});
function switchCommonFormatter(cell, table, formatterParams) {
    var value = cell.getValue();
    var checked = value ? "checked" : "";
    return `
                <label class="switch">
                    <input type="checkbox" ${checked} onclick="toggleActive(${cell.getRow().getIndex(), table})">
                    <span class="slider"></span>
                </label>`;
}

function toggleActive(rowIndex, table) {
    var row = table.getRow(rowIndex);
    var data = row.getData();
    data.IsActive = !data.IsActive;
    row.update(data);
}

function makeFormGeneric(formSelector, submitButtonSelector) {
    var form = $(formSelector);
    var submitButton = $(submitButtonSelector);

    form.on('input change', 'input, select, textarea', checkFormValidity);
    checkFormValidity();

    function checkFormValidity() {
        if (form[0].checkValidity()) {
            submitButton.prop('disabled', false);
        } else {
            submitButton.prop('disabled', true);
        }
    }
}
function getFormData(formSelector) {
    var formData = {};
    $(formSelector).find('input, select, textarea').each(function () {
        var id = $(this).attr('id');
        if (id) {
            formData[id] = $(this).val();
        }
    });
    return formData;
}
// ajaxHelper.js
function makeAjaxRequest({
    url,
    data = {},
    type = 'GET',
    contentType = 'application/json; charset=utf-8',
    dataType = 'json',
    processData = true,
    cache = false,
    headers = {},
    successCallback = function (response) { console.log(response); },
    errorCallback = function (xhr, status, error) { console.error(`Error: ${error}`); }
}) {
    $.ajax({
        url: url,
        data: type === 'GET' ? data : JSON.stringify(data),
        type: type,
        contentType: contentType,
        dataType: dataType,
        processData: processData,
        cache: cache,
        headers: headers,
        success: successCallback,
        error: errorCallback
    });
}
const Id = "00000000-0000-0000-0000-000000000000";

const API_URLS = {
    AuthenticateAsync: "/Account/Login",
    RegistrationUserAsync: "/Account/SignUp",
    ForgotPasswordAsync: "/Account/ForgotPassword",
    ResetPasswordAsync: "/Account/ResetPassword",
    ExternalCallaBackResponseAsync: "/Account/ExternalCallaBackResponse",
    LoginOrRegisterExternalUserAsync: "/Account/LoginOrRegisterExternalUser",
    GetUserAccountsAsync: "/User/GetUserAccounts",
    InsertTenantAsync: '/Tenant/InsertTenant',
    FetchTenantDetailsAsync: '/Tenant/GetTenantDetails',
    FetchTenantsAsync: '/Tenant/fetchTenants',
    FetchDealersAsync: '/Dealer/GetDealers',
    InsertOrUpdateRoleAsync: '/Role/InsertOrUpdateRole',
    BulkInsertOrUpdateTenant: '/Tenant/BulkInsertOrUpdateTenant',
    InsertCampaignPeriodAsync: '/CampaignPeriod/InsertCampaignPeriod',
    InsertWorkFlowAsync: '/WorkFlow/InsertWorkFlow',
    BulkInsertOrUpdateWorkFlow: '/WorkFlow/BulkInsertOrUpdateWorkFlow',
    InsertWorkFlowActivityAsync: '/WorkFlowActivity/InsertWorkFlowActivity',
    FetchWorkFlowActivity: '/WorkFlowActivity/FetchWorkFlowActivity',
    BulkInsertOrUpdateWorkFlowActivity: '/WorkFlowActivity/BulkInsertOrUpdateWorkFlowActivity',
    FetchRolePermissionsAsync: '/Permission/FetchRolePermissions',
    FetchActivitiesAsync: '/Activity/FetchActivities',
    FetchFeaturesAsync: '/Feature/FetchFeatures',
    InsertOrUpdateRolePermissonsAsync: '/Permission/InsertOrUpdateRolePermissons',
    InsertDealerAdvantageAsync: '/DealerAdvantage/InsertDealerAdvantage',
    DeleteDealerAdvantageAsync: '/DealerAdvantage/DeleteDealerAdvantage',
    FetchCampaignPeriodsAsync: '/CampaignPeriod/FetchCampaignPeriod',
    FetchCampaignTypeAsync: '/CampaignType/FetchCampaignTypes',
    FetchCategoryAsync: '/Category/FetchCategories',
    FetchCampaignChannelsAsync: '/CampaignChannel/FetchCampaignChannels',
    FetchProductsAsync: '/Product/FetchProducts',
    FetchWorkFlowsAsync: '/WorkFlow/FetchWorkFlow',
    FetchStatusesAsync: '/Status/FetchStatuses',
    FetchWorkFlowTemplatesAsync: '/WorkFlowTemplate/FetchWorkFlowTemplates',
    InsertOrUpdateCampaignAsync: '/Campaign/InsertOrUpdateCampaign',
    InsertOrUpdateDealerAsync: '/Dealer/InsertOrUpdateDealer',
    InsertOrUpdateFlagshipDealerAsync: '/Dealer/InsertOrUpdateFlagshipDealer',
    InsertOrUpdateDealerAmenityAsync: '/Dealer/InsertOrUpdateDealerAmenity',
    FetchDealerAsync: '/Dealer/GetDealer',
    FetchFlahshipDealerInfoAsync: '/Dealer/GetDealerDetails',
    InsertOrUpdateDealerProduct: '/Dealer/InsertOrUpdateDealerProduct',
    InsertOrUpdateDealerIncentiveAsync: '/Dealer/InsertOrUpdateDealerIncentive',
    InsertOrUpdateDealerServiceType: '/Dealer/InsertOrUpdateDealerServiceType',
    InsertOrUpdateDealerURLType: '/Dealer/InsertOrUpdateDealerURLType',
    SyncFlagshipDealerAsync: '/Dealer/SyncFlagshipDealer',
    InsertOrUpdateDealerDeleveryType: '/Dealer/InsertOrUpdateDealerDeleveryType',
    InsertOrUpdateCustomerAsync: '/Dealer/InsertOrUpdateCustomer',
    BulkInsertCustomerAsync: '/Dealer/BulkInsertCustomer',
    InsertOrUpdateDealerBuyerAdvantage: '/Dealer/InsertOrUpdateDealerBuyerAdvantage',
    FetchAmenitiesAsync: '/Aminity/FetchAmenities',
    InsertOrUpdateAmenityAsync: '/Aminity/InsertOrUpdateAmenity',
    FetchCampaignTypesAsync: '/CampaignType/FetchCampaignTypes',
    InsertOrUpdateCampaignTypeAsync: '/CampaignType/InsertOrUpdateCampaignType',
    FetchCategoriesAsync: '/Category/FetchCategories',
    InsertOrUpdateCategoryAsync: '/Category/InsertOrUpdateCategory',
    FetchMessageTypesAsync: '/MessageType/GetMessageTypes',
    FetchMailboxMessagesAsync: '/MailBox/GetMailboxMessages',
    InsertOrUpdateMailBoxAsync: '/MailBox/InsertOrUpdateMailbox',
    FetchNotificationsAsync: '/UserMailBox/GetNotifications',
    FetchFeaturesAsync: '/Feature/FetchFeatures',
    InsertOrUpdateFeatureAsync: '/Feature/InsertOrUpdateFeature',
    FetchIncentivesAsync: '/Incentive/FetchIncentives',
    InsertOrUpdateIncentiveAsync: '/Incentive/InsertOrUpdateIncentive',
    FetchMakesAsync: '/Make/FetchMakes',
    InsertOrUpdateMakeAsync: '/Make/InsertOrUpdateMake',
    FetchMetrixsAsync: '/Metric/FetchMetrixs',
    InsertOrUpdateMetricAsync: '/Metric/InsertOrUpdateMetric',
    FetchCampaignTemplatesAsync: '/CampaignTemplate/FetchCampaignTemplates',
    InsertOrUpdateCampaignTemplateAsync: '/CampaignTemplate/InsertOrUpdateCampaignTemplate',
    FetchLeadSourcesAsync: '/LeadSource/FetchLeadSources',
    FetchProductsAsync: '/Product/FetchProducts',
    InsertOrUpdateProductAsync: '/Product/InsertOrUpdateProduct',
    GetSettingTypesAsync: '/SettingType/FetchSettingTypes',
    InsertOrUpdateSettingTypeAsync: '/SettingType/InsertOrUpdateSettingType',
    GetServiceTypeAsync: '/ServiceType/FetchServiceTypes',
    InsertOrUpdateServiceTypeAsync: '/ServiceType/InsertOrUpdateServiceType',
    FetchURLTypesAsync: '/URLType/FetchURLTypes',
    InsertOrUpdateURLTypeAsync: '/URLType/InsertOrUpdateURLType',
    InsertOrUpdateStatusAsync: '/Status/InsertOrUpdateStatus',
    FetchYearsAynsc: '/Year/FetchYears',
    InsertOrUpdateYearAsync: '/Year/InsertOrUpdateYear',
    FetchMesssageTypeAsync: '/MessageType/FetchMesssageType',
    InsertOrUpdateMessageTypeAsync: '/MessageType/InsertOrUpdateMessageType',
    GetSettingTypeElementsAsync: '/SettingTypeElement/FetchSettingTypeElements',
    InsertOrUpdateSettingTypeElementAsync: '/SettingTypeElement/InsertOrUpdateSettingTypeElement',
    GetFunnelStagesAsync: '/FunnelStage/FetchFunnelStages',
    InsertOrUpdateFunnelStageAsync: '/FunnelStage/InsertOrUpdateFunnelStage',
    GetCustomSegmentsAsync: '/CustomSegment/FetchCustomSegments',
    InsertOrUpdateCustomSegmentAsync: '/CustomSegment/InsertOrUpdateCustomSegment',
    GetSurveysAsync: '/Survey/FetchSurveys',
    InsertOrUpdateSurveyAsync: '/Survey/InsertOrUpdateSurvey',
    GetSurveyQuestionsAsync: '/SurveyQuestion/FetchSurveyQuestions',
    InsertOrUpdateSurveyQuestionAsync: '/SurveyQuestion/InsertOrUpdateSurveyQuestion',
    GenarateDealerAggrimentAsync: '/Dealer/GenarateDealerAggriment',
    GetSurveyResponsesAsync: '/SurveyResponse/FetchSurveyResponses',
    InsertOrUpdateSurveyResponseAsync: '/SurveyResponse/InsertOrUpdateSurveyResponse',
    GenarateTenantDealerDetailsAggriment: '/Tenant/GenarateTenantDealerDetailsAggriment',
    FetchPaymentMethodAsync: '/PaymentMethod/FetchPaymentMethod',
    InsertOrUpdatePaymentMethod: '/PaymentMethod/InsertOrUpdatePaymentMethod',
    InsertOrUpdateBillingAccount: '/BillingAccount/InsertOrUpdateBillingAccount',
    FetchBillingAccount: '/BillingAccount/FetchBillingAccount',
    GetAccountTypeAsync: '/AccountType/GetAccountType',
    GetAddressTypesAsync: '/AddressType/GetAddressTypes',
    GetBanksAsync: '/Bank/GetBanks',
    GetCardTypesAsync: '/CardType/GetCardTypes',
    GetStatusesAsync: '/Status/GetStatuses',
    GetTransactionTypesAsync: '/TransactionType/GetTransactionTypes',
    GetCountriesListAsync: '/Country/GetCountriesList',
    PlacePaymentOrderAsync: '/PaymentOrder/PlacePaymentOrder',
    CreatePortalUserAsync: '/User/CreatePortalUser',
    InsertAndUpdateUserKycAsync: '/User/InsertAndUpdateUserKyc'
};
function addCommonProperties(data) {
    var appuser = storageService.get("ApplicationUser");
    var userId = appuser ? appuser.Id : null;
    data.CreatedOn = new Date();
    data.CreatedBy = userId;
    data.ModifiedOn = new Date();
    data.ModifiedBy = userId;
    data.IsActive = true;
    return data;
}
function exportToExcel(data, columns, sheetName, filename, sortColumn, sortOrder) {
    // Filter columns to include in the Excel file
    var filteredData = [];
    if (data.length > 0) {
        filteredData = data.map(function (item) {
            var filteredItem = {};
            columns.forEach(function (col) {
                filteredItem[col] = item[col];
            });
            return filteredItem;
        });
        if (filteredData.length > 0 && sortColumn) {
            filteredData.sort(function (a, b) {
                if (a[sortColumn] < b[sortColumn]) return sortOrder === 'asc' ? -1 : 1;
                if (a[sortColumn] > b[sortColumn]) return sortOrder === 'asc' ? 1 : -1;
                return 0;
            });
        }
    }
    else {
        var headerRow = {};
        columns.forEach(function (col) {
            headerRow[col] = null;
        });
        filteredData.push(headerRow);
    }

    // Apply sorting if sortColumn is provided


    // Create a new workbook and add the data as a worksheet
    var workbook = XLSX.utils.book_new();
    var worksheet = XLSX.utils.json_to_sheet(filteredData);
    XLSX.utils.book_append_sheet(workbook, worksheet, sheetName);

    // Generate the Excel file
    var excelBuffer = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    var dataBlob = new Blob([excelBuffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

    // Create a link element and trigger the download
    var link = document.createElement('a');
    link.href = URL.createObjectURL(dataBlob);
    link.download = filename + '.xlsx';
    document.body.appendChild(link);
    link.click();
}
const gridColumns = {
    TenantGrid: ['TenantName', 'TenantCode', 'Logo', 'TenantWebsite', 'Location', 'Address', 'ContactName', 'ContactPhone', 'Products', 'Makes', 'DealerCount'],
    RoleGrid: ['Name', 'Code'],
    CustomerGrid: ['Id', 'VIN', 'Price', 'Miles', 'StockNo', 'Year', 'Make', 'Model', 'Trim', 'BodyType', 'VehicleType', 'Drivetrain', 'Transmission', 'FuelType', 'EngineSize', 'EngineBlock', 'SellerName', 'Street', 'City', 'State', 'Zip'],
    VehicleGrid: ["id", "vin", "price", "miles", "stock_no", "year", "make", "model", "trim", "body_type", "vehicle_type", "drivetrain", "transmission", "fuel_type", "engine_size", "engine_block", "seller_name", "street", "city", "state", "zip"]
};
const exportType = {
    "Exporttemplate": "Export Template",
    "Exportwithgriddata": "Export with Grid Data",
    "Exportwithoriginaldata": "Export with Original Data"
};

function processFiles(files, gridColumns, callback) {
    var importedData = [];
    var fileList = $('#fileInfo ul');
    fileList.empty();

    for (var i = 0; i < files.length; i++) {
        var file = files[i];
        fileList.append('<li>' + file.name + ' (' + file.size + ' bytes)</li>');
    }

    var reader = new FileReader();

    reader.onload = function (e) {
        var contents = e.target.result;

        var workbook = XLSX.read(contents, { type: 'binary' });
        var worksheet = workbook.Sheets[workbook.SheetNames[0]];
        var jsonData = XLSX.utils.sheet_to_json(worksheet, { header: 1 });

        // Define expected column headers
        var expectedHeaders = gridColumns;

        // Get actual column headers from the Excel file
        var actualHeaders = Object.values(jsonData[0]);

        // Validate column headers
        var isValid = compareColumnHeaders(expectedHeaders, actualHeaders);

        if (isValid) {
            console.log('Excel column headers are valid.');
        } else {
            console.log('Excel column headers are invalid.');
        }

        if (isValid) {
            var headerIndexMap = {};

            // Map actual headers to their indexes
            for (var j = 0; j < actualHeaders.length; j++) {
                headerIndexMap[actualHeaders[j]] = j;
            }

            for (var i = 1; i < jsonData.length; i++) {
                if (jsonData[i][0] != '' && jsonData[i][0] != 'undefined' && jsonData[i][0] != null) {
                    var rowData = mapRowToObject(jsonData[i], headerIndexMap);
                    var comon = addCommonProperties(rowData);
                    importedData.push(comon);
                }
            }
        }

        // Call the callback with the imported data
        callback(importedData);
    };

    reader.readAsBinaryString(file);
}

function mapRowToObject(row, headerIndexMap) {
    var obj = {};

    for (var key in headerIndexMap) {
        if (headerIndexMap.hasOwnProperty(key)) {
            obj[key] = row[headerIndexMap[key]];
        }
    }

    return obj;
}
function compareColumnHeaders(expectedHeaders, actualHeaders) {
    // Check if the number of columns match
    if (expectedHeaders.length !== actualHeaders.length) {
        return false;
    }

    // Check if the column headers match in order
    for (var i = 0; i < expectedHeaders.length; i++) {
        if (expectedHeaders[i].toLowerCase() !== actualHeaders[i].toLowerCase()) {
            return false;
        }
    }

    return true;
}
// Function to disable all buttons
function disableAllButtons() {

    $(".custom-cursor").addClass("disabled");
    $("#addBtn").removeClass("disabled");
    $("#exportBtn").removeClass("disabled");
    /* $("#importBtn").removeClass("disabled");*/
    /* $("#permissionBtn").removeClass("disabled");*/
}

// Function to enable buttons
function enableButtons(table) {
    $(".custom-cursor").removeClass("disabled");

    // Highlight buttons based on selection
    var selectedRows = table.getSelectedRows();
    var hasMultipleSelection = selectedRows.length > 1;

    if (hasMultipleSelection) {
        /* $("#addBtn").removeClass("selected");*/
        $("#editBtn").addClass("disabled");
        $("#deleteBtn").addClass("disabled");
        $("#copyBtn").addClass("disabled");
        $("#addBtn").removeClass("disabled");
        $("#permissionBtn").removeClass("disabled");
        $("#campaignBtn").removeClass("disabled");
    } else {
        selectedRows.forEach(function (row) {
            // Highlight based on specific conditions (e.g., edit and delete)
            var isSelected = row.isSelected();
            if (isSelected) {
                /*  $("#addBtn").addClass("selected");*/
                $("#editBtn").addClass("selected");
                $("#deleteBtn").addClass("selected");
                $("#copyBtn").addClass("selected");
                $("#addBtn").addClass("disabled");
                $("#permissionBtn").addClass("selected");
                $("#campaignBtn").addClass("selected");
            }
        });
    }
}

function getQueryStringParameter(name) {
    var urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
}
$(document).on("click", ".toggle-password", function () {
    var inputField = $(this).closest('.input-group').find('.form-control');
    var icon = $(this).find('i');

    if (inputField.attr('type') === 'password') {
        inputField.attr('type', 'text');
        icon.removeClass('fa-eye-slash').addClass('fa-eye');
    } else {
        inputField.attr('type', 'password');
        icon.removeClass('fa-eye').addClass('fa-eye-slash');
    }
});
const Activities = {
    EXPORT: 'Export',
    COPY: 'Copy',
    IMPORT: 'Import',
    DELETE: 'Delete',
    ADD: 'Add',
    SAVE: 'Save',
    UPDATE: 'Update',
    VIEW: 'View',
    EDIT: 'Edit'
};

const Features = {
    CAMPAIGN_CHANNEL: 'CampaignChannel',
    WORKFLOW_ACTIVITY: 'WorkFlowActivity',
    MAKE_MODEL: 'MakeModel',
    YEAR: 'Year',
    CAMPAIGN_TYPE: 'CampaignType',
    CATEGORY: 'Category',
    MAKE: 'Make',
    CAMPAIGN_PERIOD: 'CampaignPeriod',
    WORKFLOW_TEMPLATE: 'WorkFlowTemplate',
    URL_TYPE: 'URLType',
    USER: 'User',
    SERVICE_TYPE: 'ServiceType',
    AMINITY: 'Aminity',
    ROLE: 'Role',
    INCENTIVE: 'Incentive',
    ACTIVITY: 'Activity',
    WORKFLOW: 'WorkFlow',
    TENANT: 'Tenant',
    STATUS: 'Status',
    PRODUCT: 'Product',
    DELEVERY_TYPE: 'DeleveryType',
    DealerAdvantage: 'DealerAdvantage',
    Campaign: 'Campaign',
    SettingType: 'SettingType',
    Dealer: 'Dealer'
};
const hospitalStatuses = [
    { "StatusName": "Appointment booked", "StatusCode": "AppointmentBooked" },
    { "StatusName": "Hospital visited", "StatusCode": "HospitalVisited" },
    { "StatusName": "Doctor consultation created", "StatusCode": "DoctorConsultationCreated" },
    { "StatusName": "Doctor consulted", "StatusCode": "DoctorConsulted" },
    { "StatusName": "Sent for test", "StatusCode": "SentForTests" },
    { "StatusName": "Medicines are requested", "StatusCode": "SentForPharmacy" },
    { "StatusName": "Test completed", "StatusCode": "TestCompleted" },
    { "StatusName": "Reports Available", "StatusCode": "ReportsAvailable" },
    { "StatusName": "Doctor reports seen", "StatusCode": "DoctorReportsSeen" },
    { "StatusName": "Sent for medicine", "StatusCode": "SentToPharmacy" },
    { "StatusName": "Medicine given", "StatusCode": "MedicineGiven" },
    { "StatusName": "Hospital visit completed", "StatusCode": "HospitalVisitCompleted" },
    { "StatusName": "Follow-Up Scheduled", "StatusCode": "FollowUpScheduled" },
    { "StatusName": "Follow-Up Completed", "StatusCode": "FollowUpCompleted" },
    { "StatusName": "Referral Made", "StatusCode": "ReferralMade" },
    { "StatusName": "Referral Completed", "StatusCode": "ReferralCompleted" },
    { "StatusName": "Insurance Verified", "StatusCode": "InsuranceVerified" },
    { "StatusName": "Payment Processed", "StatusCode": "PaymentProcessed" },
    { "StatusName": "Patient Discharged", "StatusCode": "PatientDischarged" },
    { "StatusName": "Patient Feedback Received", "StatusCode": "PatientFeedbackReceived" },
    { "StatusName": "Emergency Visit", "StatusCode": "EmergencyVisit" },
    { "StatusName": "Telehealth Appointment Scheduled", "StatusCode": "TelehealthAppointmentScheduled" }
];
const statuses = {
    AppointmentBooked: "Appointment booked",
    HospitalVisited: "Hospital visited",
    DoctorConsulated: "Doctor consulated",
    SentForTests: "Sent for test",
    TestCompleted: "Test completed",
    ReportsAvailable: "Reports Available",
    DoctorReportsSeen: "Doctor reports seen",
    SentToPharmacy: "Sent for medicine",
    MediceneGiven: "Medicene given",
    HospitalVisitCompleted: "Hospital visit completed"
};
function hasPermission(featureName, activityName) {
    var userPermissions = storageService.get('UserPermissions');
    if (!userPermissions) {
        console.error('Permissions data not found in local storage.');
        return false;
    }
    for (var i = 0; i < userPermissions.length; i++) {
        var feature = userPermissions[i];
        if (feature.FeatureName === featureName) {
            for (var j = 0; j < feature.Activities.length; j++) {
                var activity = feature.Activities[j];
                if (activity.ActivityName === activityName) {
                    return activity.IsEnabled;
                }
                /*   return false;*/
            }
        }
    }
    return false;
}
const paymentTypes = [
    { "PaymentTypeName": "PhonePe", "PaymentTypeCode": "PhonePe" },
    { "PaymentTypeName": "Credit Card", "PaymentTypeCode": "CreditCard" },
    { "PaymentTypeName": "Debit Card", "PaymentTypeCode": "DebitCard" },
    { "PaymentTypeName": "Bajaj Card", "PaymentTypeCode": "BajajCard" },
    { "PaymentTypeName": "Discover", "PaymentTypeCode": "Discover" },
    { "PaymentTypeName": "Net Banking", "PaymentTypeCode": "NetBanking" },
    { "PaymentTypeName": "EMI", "PaymentTypeCode": "EMI" },
    { "PaymentTypeName": "Cash", "PaymentTypeCode": "Cash" },
    { "PaymentTypeName": "UPI", "PaymentTypeCode": "UPI" },
    { "PaymentTypeName": "Google Pay", "PaymentTypeCode": "GooglePay" },
    { "PaymentTypeName": "Paytm", "PaymentTypeCode": "Paytm" },
    { "PaymentTypeName": "Amazon Pay", "PaymentTypeCode": "AmazonPay" },
    { "PaymentTypeName": "Apple Pay", "PaymentTypeCode": "ApplePay" },
    { "PaymentTypeName": "Samsung Pay", "PaymentTypeCode": "SamsungPay" },
    { "PaymentTypeName": "Cryptocurrency", "PaymentTypeCode": "Crypto" }
];
function genarateDropdown(dropdownId, data, valueField, textField) {
    var $dropdown = $('#' + dropdownId);
    $dropdown.empty();

    var $defaultOption = $('<option>', {
        value: '',
        text: 'Select an option'
    });
    $dropdown.append($defaultOption);

    $.each(data, function (index, item) {
        var $option = $('<option>', {
            value: item[valueField],
            text: item[textField]
        });
        $dropdown.append($option);
    });

    $dropdown.trigger('change');

    /*$dropdown.dropdown();*/
};

function formatDate(date) {
    const dateParts = date.split('-');
    return `${dateParts[1]}/${dateParts[2]}/${dateParts[0]}`;
}

function generateDealerCode() {
    const now = new Date();
    var appuser = storageService.get("ApplicationUser");
    var tenantCode = appuser.TenantId.split('-')[0];
    // Get individual components of the date and time
    const day = String(now.getDate()).padStart(2, '0');
    const month = String(now.getMonth() + 1).padStart(2, '0'); // Months are zero-based
    const year = now.getFullYear();
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const seconds = String(now.getSeconds()).padStart(2, '0');
    const milliseconds = String(now.getMilliseconds()).padStart(3, '0');

    // Concatenate components to form the desired code
    const dateTimeCode = `${tenantCode}${month}${day}${year}${hours}${minutes}${seconds}${milliseconds}`;

    return dateTimeCode;
}
function getStatusBadge(isActive) {
    if (isActive) {
        return '<span class="badge bg-success status-badge">Active</span>';
    } else {
        return '<span class="badge bg-warning status-badge">Inactive</span>';
    }
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric'
    });
}
function showLoader() {
    $('#overlay').attr('style', 'display:grid');
    $('#overlay').show();
}

function hideLoader() {
    $('#overlay').attr('style', 'display:none');
    $('#overlay').hide();
}
$(document).on('click', '.wizard li', function () {
    var $this = $(this);
    var $siblings = $this.parent().children('li');

    // Remove all classes from siblings
    $siblings.removeClass('completed active');

    // Mark the clicked item as active
    $this.addClass('active');

    // Mark all previous items as completed
    $this.prevAll().addClass('completed');

    // Ensure all future items are not completed
    $this.nextAll().removeClass('completed');
});

const next = e => {
    const current = e.target.closest('div');
    current.classList.remove('active');
    current.classList.add('complete');

    const next = current.nextElementSibling;
    if (next) {
        next.classList.remove('hidden');
        next.classList.add('active');
    }
};

document
    .querySelector('button')
    .addEventListener('click', next);

function generateUniquePhoneNumber() {
    var phoneNumber = '';

    // Ensure the first digit is not zero
    phoneNumber += Math.floor(Math.random() * 9) + 1;

    // Generate the remaining 9 digits
    for (var i = 0; i < 9; i++) {
        phoneNumber += Math.floor(Math.random() * 10);
    }

    return phoneNumber;
}
function generateUniqueEmail(baseName, domain) {
    // Generate a random string of 5 alphanumeric characters
    var randomString = Math.random().toString(36).substring(2, 7);

    // Combine the base name with the random string and domain to create the email
    var email = baseName + randomString + "@" + domain.replace(/ /g, '') + ".com";

    return email;
}
function generateOrderReference(userId, paymentType) {
    // Get current date and time
    var now = new Date();

    // Format day, month, year, hour, minute with leading zeros
    var day = String(now.getDate()).padStart(2, '0');
    var month = String(now.getMonth() + 1).padStart(2, '0'); // getMonth() is 0-based
    var year = now.getFullYear();
    var hour = String(now.getHours()).padStart(2, '0');
    var minute = String(now.getMinutes()).padStart(2, '0');

    // Create timestamp string: DDMMYYYYHHMM
    var timestamp = day + month + year + hour + minute;

    // Determine type code: DP for deposit, PY for payment
    var typeCode = paymentType.toLowerCase() === 'Withdrawal' ? 'WD' : (paymentType.toLowerCase() === 'deposit' ? 'DP' : 'PY');

    // Construct order reference: ORD-{typeCode}-{timestamp}{userId}
    var orderRef = 'ORD-' + typeCode + '-' + timestamp + userId;

    return orderRef;
}

const OrderType = {
    Deposite: "deposit",
    Payment: "Payment",
    Withdrawal: "Withdrawal"
}

const FeeCollectionMethod = {
    Yes: "Yes",
    No: "No"
}

const statusConstants = {
    "Pending": 1,
    "In Progress": 2,
    "Active": 3,
    "Inactive": 4,
    "Approved": 5,
    "Rejected": 6,
    "Completed": 7,
    "Cancelled": 8,
    "Draft": 9,
    "Submitted": 10,
    "Under Review": 11,
    "Processing": 12,
    "Processed": 13,
    "Failed": 14,
    "Success": 15,
    "Expired": 16,
    "Suspended": 17,
    "Archived": 18,
    "Deleted": 19,
    "Locked": 20,
    "Unlocked": 21,
    "Verified": 22,
    "Unverified": 23,
    "Paid": 24,
    "Unpaid": 25,
    "Open": 26,
    "Closed": 27,
    "Resolved": 28,
    "New": 29,
    "On Hold": 30,
    "Awaiting Approval": 31,
    "Awaiting Payment": 32,
    "Shipped": 33,
    "Delivered": 34,
    "Returned": 35,
    "Refunded": 36,
    "Partially Paid": 37,
    "Overdue": 38,
    "Blocked": 39,
    "Enabled": 40,
    "Disabled": 41
};

const rolesList = [
    {
        Id: 1,
        Name: "Administrator",
        Code: "ADMIN",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 0
    },
    {
        Id: 2,
        Name: "User",
        Code: "USER",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 0
    },
    {
        Id: 3,
        Name: "Executive",
        Code: "EXEC",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 4,
        Name: "Manager",
        Code: "MANAGER",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 5,
        Name: "Supervisor",
        Code: "SUPERVISOR",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 6,
        Name: "Analyst",
        Code: "ANALYST",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 7,
        Name: "Support",
        Code: "SUPPORT",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 8,
        Name: "Viewer",
        Code: "VIEWER",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 9,
        Name: "Editor",
        Code: "EDITOR",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 10,
        Name: "Auditor",
        Code: "AUDITOR",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 11,
        Name: "Developer",
        Code: "DEV",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 12,
        Name: "Finance",
        Code: "FINANCE",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 13,
        Name: "HR",
        Code: "HR",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 14,
        Name: "Sales",
        Code: "SALES",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: null,
        ModifiedOn: null,
        IsActive: 1
    },
    {
        Id: 15,
        Name: "Marketing",
        Code: "MARKETING",
        CreatedBy: null,
        CreatedOn: "2025-10-11 07:04:16.8726881 -04:00",
        ModifiedBy: 1,
        ModifiedOn: "2025-10-11 07:23:52.2100000 +00:00",
        IsActive: 1
    }
];

// Example usage:
// var statusId = statusConstants["Pending"]; // Returns 1
const statusMap = {
    1: 'Pending',
    2: 'In Progress',
    3: 'Active',
    4: 'Inactive',
    5: 'Approved',
    6: 'Rejected',
    7: 'Completed',
    8: 'Cancelled',
    9: 'Draft',
    10: 'Submitted',
    11: 'Under Review',
    12: 'Processing',
    13: 'Processed',
    14: 'Failed',
    15: 'Success',
    16: 'Expired',
    17: 'Suspended',
    18: 'Archived',
    19: 'Deleted',
    20: 'Locked',
    21: 'Unlocked',
    22: 'Verified',
    23: 'Unverified',
    24: 'Paid',
    25: 'Unpaid',
    26: 'Open',
    27: 'Closed',
    28: 'Resolved',
    29: 'New',
    30: 'On Hold',
    31: 'Awaiting Approval',
    32: 'Awaiting Payment',
    33: 'Shipped',
    34: 'Delivered',
    35: 'Returned',
    36: 'Refunded',
    37: 'Partially Paid',
    38: 'Overdue',
    39: 'Blocked',
    40: 'Enabled',
    41: 'Disabled',
    42: 'Payment Receved',
    43: 'Payment Deposited'
};

// Calculate SLA time remaining and get appropriate CSS class
function calculateSLATimer(order) {
    if (order.OrderStatus === 'Completed') {
        return {
            display: 'Completed',
            class: 'sla-completed'
        };
    }

    const createdDate = new Date(order.CreatedOn);
    const now = new Date();
    const elapsedMs = now - createdDate;

    let totalSlaMs;
    switch (order.TransactionFeeId) {
        case 1: // 5 minutes
            totalSlaMs = 5 * 60 * 1000;
            break;
        case 2: // 4 hours
            totalSlaMs = 4 * 60 * 60 * 1000;
            break;
        case 3: // 24 hours
            totalSlaMs = 24 * 60 * 60 * 1000;
            break;
        default:
            totalSlaMs = 24 * 60 * 60 * 1000; // Default to 24 hours
    }

    const remainingMs = totalSlaMs - elapsedMs;
    const isExpired = remainingMs <= 0;
    const absoluteMs = Math.abs(remainingMs);

    // Calculate display format based on SLA type and whether it's expired
    let displayText;

    if (order.TransactionFeeId === 1) {
        // For 5-minute SLA, show MM:SS
        const minutes = Math.floor(absoluteMs / (60 * 1000));
        const seconds = Math.floor((absoluteMs % (60 * 1000)) / 1000);
        displayText = `${isExpired ? '-' : ''}${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
    } else {
        // For longer SLAs, calculate days, hours, minutes, seconds
        const days = Math.floor(absoluteMs / (24 * 60 * 60 * 1000));
        const hours = Math.floor((absoluteMs % (24 * 60 * 60 * 1000)) / (60 * 60 * 1000));
        const minutes = Math.floor((absoluteMs % (60 * 60 * 1000)) / (60 * 1000));
        const seconds = Math.floor((absoluteMs % (60 * 1000)) / 1000);

        if (days > 0) {
            // Show days and hours when days > 0
            displayText = `${isExpired ? '-' : ''}${days}:${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
        } else {
            // Show only hours:minutes:seconds when no days
            displayText = `${isExpired ? '-' : ''}${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
        }
    }

    // Determine color class
    let slaClass;

    if (isExpired) {
        slaClass = 'sla-critical';
    } else {
        const percentageRemaining = (remainingMs / totalSlaMs) * 100;

        if (percentageRemaining > 60) {
            slaClass = 'sla-normal';
        } else if (percentageRemaining > 20) {
            slaClass = 'sla-warning';
        } else {
            slaClass = 'sla-critical';
        }
    }

    return {
        display: displayText,
        class: slaClass
    };
};
function mapStatus(statusId) {
    switch (statusId) {
        case 1: return "Pending";
        case 2: return "In Progress";
        case 3: return "Active";
        case 4: return "Inactive";
        case 5: return "Approved";
        case 6: return "Rejected";
        case 7: return "Completed";
        case 8: return "Cancelled";
        case 9: return "Draft";
        case 10: return "Submitted";
        case 11: return "Under Review";
        case 12: return "Processing";
        case 13: return "Processed";
        case 14: return "Failed";
        case 15: return "Success";
        case 16: return "Expired";
        case 17: return "Suspended";
        case 18: return "Archived";
        case 19: return "Deleted";
        case 20: return "Locked";
        case 21: return "Unlocked";
        case 22: return "Verified";
        case 23: return "Unverified";
        case 24: return "Paid";
        case 25: return "Unpaid";
        case 26: return "Open";
        case 27: return "Closed";
        case 28: return "Resolved";
        case 29: return "New";
        case 30: return "On Hold";
        case 31: return "Awaiting Approval";
        case 32: return "Awaiting Payment";
        case 33: return "Shipped";
        case 34: return "Delivered";
        case 35: return "Returned";
        case 36: return "Refunded";
        case 37: return "Partially Paid";
        case 38: return "Overdue";
        case 39: return "Blocked";
        case 40: return "Enabled";
        case 41: return "Disabled";
        case 42: return "Payment Received";
        case 43: return "Payment Deposited";
        default: return "Unknown";
    }
}
function mapStatusClass(statusId) {
    switch (statusId) {
        case 1: return "bg-warning text-dark"; // Pending
        case 2: return "bg-info"; // In Progress
        case 3: return "bg-success"; // Active
        case 4: return "bg-secondary"; // Inactive
        case 5: return "bg-success"; // Approved
        case 6: return "bg-danger"; // Rejected
        case 7: return "bg-success"; // Completed
        case 8: return "bg-danger"; // Cancelled
        default: return "bg-light text-dark";
    }
}