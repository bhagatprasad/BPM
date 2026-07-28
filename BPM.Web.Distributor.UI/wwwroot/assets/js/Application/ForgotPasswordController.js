var ForgotPasswordController = function () {

    var initializeEvents = function () {

        $("#forgotPasswordForm").on("submit", function () {

            if (!$(this).valid()) {
                return false;
            }

            $("#btnSubmit")
                .prop("disabled", true)
                .html('<span class="spinner-border spinner-border-sm me-2"></span> Sending...');

            return true;

        });

    };

    return {

        init: function () {

            initializeEvents();

        }

    };

};