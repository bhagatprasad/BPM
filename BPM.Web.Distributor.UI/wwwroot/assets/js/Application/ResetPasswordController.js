function ResetPasswordController() {

    var self = this;

    self.init = function () {

        // Get UserId from query string
        var userId = getQueryStringParameter("userId");

        if (userId) {
            $("#userId").val(userId);
        }

        var form = $("#formResetPassword");
        var btnSubmit = $("#btnSubmit");

        form.on("input", "input", checkFormValidity);

        checkFormValidity();

        function checkFormValidity() {

            if (form[0].checkValidity()) {
                btnSubmit.prop("disabled", false);
            }
            else {
                btnSubmit.prop("disabled", true);
            }
        }

        $(document).on("click", "#btnSubmit", function (e) {

            e.preventDefault();

            // Password Match Validation
            if ($("#password").val() !== $("#confirmPassword").val()) {

                alert("Passwords do not match.");

                return;
            }

            var resetPassword = {

                UserId: $("#userId").val(),

                NewPassword: $("#password").val()

            };

            $.ajax({

                url: "/Account/ResetPassword",

                type: "POST",

                contentType: "application/json",

                data: JSON.stringify(resetPassword),

                success: function (response) {

                    if (response.success) {

                        alert("Password reset successfully.");

                        window.location.href = "/Account/Login";

                    }
                    else {

                        alert(response.message);

                    }

                },

                error: function () {

                    alert("Unable to connect to server.");

                }

            });

        });

    };

}

/* Query String Helper */

function getQueryStringParameter(name) {

    name = name.replace(/[[]/, "\\[").replace(/[\]]/, "\\]");

    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)");

    var results = regex.exec(location.search);

    return results === null
        ? ""
        : decodeURIComponent(results[1].replace(/\+/g, " "));

}