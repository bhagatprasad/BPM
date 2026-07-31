function ForgotPasswordController() {

    this.init = function () {

        var form = $("#formForgotPassword");
        var btnSubmit = $("#btnSubmit");

        form.on("input", "input", function () {
            btnSubmit.prop("disabled", !form[0].checkValidity());
        });

        btnSubmit.prop("disabled", !form[0].checkValidity());

        btnSubmit.on("click", function (e) {

            e.preventDefault();

            if (!form[0].checkValidity()) {
                form[0].reportValidity();
                return;
            }

            var model = {
                Username: $("#username").val()
            };

            $.ajax({

                url: "/Account/ForgotPassword",

                type: "POST",

                contentType: "application/json",

                data: JSON.stringify(model),

                success: function (response) {

                    console.log(response);

                    if (response.success) {

                        toastr.success(response.message);

                        if (response.userId) {

                            window.location.href =
                                "/Account/ResetPassword?userId=" + response.userId;
                        }
                    }
                    else {

                        toastr.error(response.message);

                    }

                },

                error: function () {

                    toastr.error("Server Error");

                }

            });

        });

    };

}