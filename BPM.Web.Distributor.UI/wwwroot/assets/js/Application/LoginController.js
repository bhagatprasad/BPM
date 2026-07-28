var LoginController = function () {

    var initializeEvents = function () {

        $("#loginForm").on("submit", function () {

            if (!$(this).valid()) {
                return false;
            }

            $("#btnLogin")
                .prop("disabled", true)
                .html('<span class="spinner-border spinner-border-sm me-2"></span> Signing In...');

            return true;
        });

    };

    return {

        init: function () {

            initializeEvents();

        }

    };

};

//=============================
// Toggle Password
//=============================

function togglePassword() {

    var password = document.getElementById("password");
    var eye = document.getElementById("eyeIcon");

    if (password.type === "password") {

        password.type = "text";

        eye.classList.remove("bi-eye");
        eye.classList.add("bi-eye-slash");

    }
    else {

        password.type = "password";

        eye.classList.remove("bi-eye-slash");
        eye.classList.add("bi-eye");

    }

}