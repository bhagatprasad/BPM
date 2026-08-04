function ForgotPasswordController() {
    var self = this;

    self.init = function () {
        self.initializeForm();
        self.initPasswordToggle();
    };

    self.initializeForm = function () {
        var form = $('#forgotPasswordForm');
        var submitButton = form.find('.btn-modern');

        form.on('input', 'input, select, textarea', function () {
            self.checkFormValidity();
        });

        self.checkFormValidity();

        form.on('submit', function (e) {
            e.preventDefault();
            self.handleForgotPassword();
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
        var username = $('#username').val().trim();
        var submitButton = $('#forgotPasswordForm .btn-modern');

        if (username && username.length > 0) {
            submitButton.prop('disabled', false);
            submitButton.css('opacity', '1');
            submitButton.css('cursor', 'pointer');
        } else {
            submitButton.prop('disabled', true);
            submitButton.css('opacity', '0.6');
            submitButton.css('cursor', 'not-allowed');
        }
    };

    self.handleForgotPassword = function () {
        var username = $('#username').val().trim();
        self.clearErrors();

        if (!username || username.length < 1) {
            self.showError('usernameError', 'Please enter your email address.');
            return;
        }

        // Use global preloader
        $('#preloader').fadeIn(300);
        var submitButton = $('#forgotPasswordForm .btn-modern');
        submitButton.prop('disabled', true);
        submitButton.html('<i class="ri-loader-4-line ri-spin me-2"></i> Sending...');

        var forgotPasswordData = {
            username: username
        };

        makeAjaxRequest({
            url: '/Account/ForgotPassword',
            data: forgotPasswordData,
            type: 'POST',
            successCallback: function (response) {
                self.handleForgotPasswordSuccess(response);
            },
            errorCallback: function (xhr, status, error) {
                self.handleForgotPasswordError(xhr, status, error);
            },
            completeCallback: function () {
                $('#preloader').fadeOut(300);
                submitButton.prop('disabled', false);
                submitButton.html('<i class="ri-send-plane-line me-2"></i> Send Reset Link');
                self.checkFormValidity();
            }
        });
    };

    self.handleForgotPasswordSuccess = function (response) {
        if (response && response.success) {
            if (response.userId) {
                storageService.set('ResetUserId', response.userId);
                storageService.set('ResetUsername', $('#username').val().trim());
                window.location.href = '/Account/ResetPassword';
            } else {
                self.showError('usernameError', response.message || 'User not found. Please check your email.');
            }
        } else {
            self.showError('usernameError', response.message || 'Failed to send reset link. Please try again.');
        }
    };

    self.handleForgotPasswordError = function (xhr, status, error) {
        if (xhr.responseJSON && xhr.responseJSON.message) {
            self.showError('usernameError', xhr.responseJSON.message);
        } else {
            self.showError('usernameError', 'An error occurred. Please try again.');
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