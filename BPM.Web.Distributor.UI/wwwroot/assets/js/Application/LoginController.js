function LoginController() {

    var self = this;

    self.init = function () {

        var form = $("#formAuthentication");
        var btnLogin = $("#btnSubmit");

        checkForm();

        form.on("keyup change", "input", function () {
            checkForm();
        });

        function checkForm() {

            if (form.length === 0)
                return;

            btnLogin.prop("disabled", !form[0].checkValidity());

        }

        btnLogin.on("click", function (e) {

            e.preventDefault();

            // Trigger jquery validation
            if (!form.valid()) {
                return;
            }

            btnLogin.prop("disabled", true);

            $(".loader").show();

            var model = {
                Username: $("#Username").val(),
                Password: $("#Password").val()
            };

            $.ajax({

                url: "/Account/Login",

                type: "POST",

                contentType: "application/json",

                data: JSON.stringify(model),

                success: function (response) {

                    $(".loader").hide();
                    btnLogin.prop("disabled", false);

                    if (!response || !response.appUser) {

                        toastr.error("Invalid username or password.");

                        return;
                    }

                    sessionStorage.setItem(
                        "ApplicationUser",
                        JSON.stringify(response.appUser));

                    window.location.href = "/Home/Index";
                },

                error: function () {

                    $(".loader").hide();

                    btnLogin.prop("disabled", false);

                    toastr.error("Unable to login.");
                }

            });

        });
    };

}