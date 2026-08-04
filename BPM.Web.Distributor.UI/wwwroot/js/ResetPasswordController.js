function ResetPasswordController() {
    var self = this;

    self.init = function () {
        var userId = storageService.get('ResetUserId');
        if (!userId) {
            window.location.href = '/Account/Login';
            return;
        }
        self.initializeForm();
        self.initPasswordToggle();
    };

    self.initializeForm = function () {
        var form = $('#resetPasswordForm');
        var submitButton = form.find('.btn-modern');

        form.on('input', 'input, select, textarea', function () {
            self.checkFormValidity();
        });

        self.checkFormValidity();

        form.on('submit', function (e) {
            e.preventDefault();
            self.handleResetPassword();
        });
    };

    self.initPasswordToggle = function () {
        $('.password-toggle').off('click').on('click', function () {
            self.togglePasswordVisibility(this);
        });
    };

    self.togglePasswordVisibility = function (element) {
        var input = $(element).closest('.input-group-enhanced').find('.password');
        if (input.length) {
            if (input.attr('type') === 'password') {
                input.attr('type', 'text');
                $(element).removeClass('ri-eye-off-line').addClass('ri-eye-line');
            } else {
                input.attr('type', 'password');
                $(element).removeClass('ri-eye-line').addClass('ri-eye-off-line');
            }
        }
    };

    self.checkFormValidity = function () {
        var newPassword = $('#newPassword').val().trim();
        var confirmPassword = $('#confirmPassword').val().trim();
        var submitButton = $('#resetPasswordForm .btn-modern');

        var isValid = newPassword.length >= 6 && confirmPassword.length >= 6 && newPassword === confirmPassword;

        if (isValid) {
            submitButton.prop('disabled', false);
            submitButton.css('opacity', '1');
            submitButton.css('cursor', 'pointer');
        } else {
            submitButton.prop('disabled', true);
            submitButton.css('opacity', '0.6');
            submitButton.css('cursor', 'not-allowed');
        }
    };

    self.handleResetPassword = function () {
        var newPassword = $('#newPassword').val().trim();
        var confirmPassword = $('#confirmPassword').val().trim();
        var userId = storageService.get('ResetUserId');

        self.clearErrors();

        if (!newPassword || newPassword.length < 6) {
            self.showError('newPasswordError', 'Password must be at least 6 characters.');
            return;
        }

        if (newPassword !== confirmPassword) {
            self.showError('confirmPasswordError', 'Passwords do not match.');
            return;
        }

        // Use global preloader
        $('#preloader').fadeIn(300);
        var submitButton = $('#resetPasswordForm .btn-modern');
        submitButton.prop('disabled', true);
        submitButton.html('<i class="ri-loader-4-line ri-spin me-2"></i> Resetting...');

        var resetPasswordData = {
            userId: userId,
            newPassword: newPassword
        };

        makeAjaxRequest({
            url: '/Account/ResetPassword',
            data: resetPasswordData,
            type: 'POST',
            successCallback: function (response) {
                self.handleResetPasswordSuccess(response);
            },
            errorCallback: function (xhr, status, error) {
                self.handleResetPasswordError(xhr, status, error);
            },
            completeCallback: function () {
                $('#preloader').fadeOut(300);
                submitButton.prop('disabled', false);
                submitButton.html('<i class="ri-refresh-line me-2"></i> Reset Password');
                self.checkFormValidity();
            }
        });
    };

    self.handleResetPasswordSuccess = function (response) {
        if (response === true || response === "true") {
            storageService.remove('ResetUserId');
            storageService.remove('ResetUsername');
            window.location.href = '/Account/Login?resetSuccess=true';
        } else {
            self.showError('newPasswordError', response.message || 'Failed to reset password. Please try again.');
        }
    };

    self.handleResetPasswordError = function (xhr, status, error) {
        if (xhr.responseJSON && xhr.responseJSON.message) {
            self.showError('newPasswordError', xhr.responseJSON.message);
        } else {
            self.showError('newPasswordError', 'An error occurred. Please try again.');
        }
    };

    self.showError = function (errorElementId, message) {
        var errorElement = $('#' + errorElementId);
        errorElement.text(message);
        errorElement.show();
        setTimeout(function () {
            errorElement.fadeOut(300);
        }, 5000);
    };

    self.clearErrors = function () {
        $('.text-danger').hide().text('');
    };
}