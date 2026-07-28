var ResetPasswordController = function () {

    var validatePassword = function () {

        var password = $("#newPassword").val();

        toggleRequirement("#reqLength", password.length >= 6);
        toggleRequirement("#reqUpper", /[A-Z]/.test(password));
        toggleRequirement("#reqLower", /[a-z]/.test(password));
        toggleRequirement("#reqNumber", /[0-9]/.test(password));

        validateConfirmPassword();

    };

    var validateConfirmPassword = function () {

        var password = $("#newPassword").val();
        var confirm = $("#confirmPassword").val();

        if (confirm === "")
            return;

        if (password === confirm) {

            $("#confirmPassword")
                .removeClass("is-invalid")
                

        }
        else {

            $("#confirmPassword")
                .removeClass("is-valid")
                

        }

    };

    var toggleRequirement = function (selector, valid) {

        var item = $(selector);

        if (valid) {

            item.addClass("valid");

            item.find("i")
                .removeClass("fa-circle")
                .addClass("fa-check-circle");

        }
        else {

            item.removeClass("valid");

            item.find("i")
                .removeClass("fa-check-circle")
                .addClass("fa-circle");

        }

    };

    var initializeEvents = function () {

        $("#newPassword").keyup(validatePassword);

        $("#confirmPassword").keyup(validateConfirmPassword);

        $("#resetPasswordForm").on("submit", function (e) {

            if (!$(this).valid()) {
                return false;
            }

            var password = $("#newPassword").val();
            var confirm = $("#confirmPassword").val();

            if (password !== confirm) {

                $("#confirmPassword")
                    .addClass("is-invalid");

                return false;
            }

            if (!$("#ConfirmReset").is(":checked")) {

                alert("Please confirm password reset.");

                return false;
            }

            $("#btnReset")
                .prop("disabled", true)
                .html('<span class="spinner-border spinner-border-sm me-2"></span> Updating...');

            return true;

        });
        

    };

    return {

        init: function () {

            initializeEvents();

        }

    };

};

function toggleNewPassword() {

    var input = $("#newPassword");
    var eye = $("#newEye");

    if (input.attr("type") === "password") {

        input.attr("type", "text");

        eye.removeClass("bi-eye")
            .addClass("bi-eye-slash");

    }
    else {

        input.attr("type", "password");

        eye.removeClass("bi-eye-slash")
            .addClass("bi-eye");

    }

}

function toggleConfirmPassword() {

    var input = $("#confirmPassword");
    var eye = $("#confirmEye");

    if (input.attr("type") === "password") {

        input.attr("type", "text");

        eye.removeClass("bi-eye")
            .addClass("bi-eye-slash");

    }
    else {

        input.attr("type", "password");

        eye.removeClass("bi-eye-slash")
            .addClass("bi-eye");

    }

}