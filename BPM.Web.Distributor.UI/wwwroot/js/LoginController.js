function LoginController() {
    var self = this;

    self.init = function () {
        // Initialize Sign In Form
        self.initializeSignInForm();

        // Initialize password toggle
        self.initPasswordToggle();
    };

    // Initialize Sign In Form
    self.initializeSignInForm = function () {
        var form = $('#loginForm');
        var signInButton = form.find('.btn-modern');

        // Real-time validation on input
        form.on('input', 'input, select, textarea', function () {
            self.checkFormValidity();
        });

        // Also validate on blur to handle edge cases
        form.on('blur', 'input, select, textarea', function () {
            self.checkFormValidity();
        });

        // Initial validation check
        self.checkFormValidity();

        // Handle form submission
        form.on('submit', function (e) {
            e.preventDefault();
            self.handleSignIn();
        });
    };

    // Initialize password toggle
    self.initPasswordToggle = function () {
        $('.password-toggle').off('click').on('click', function () {
            self.togglePasswordVisibility(this);
        });
    };

    // Check form validity
    self.checkFormValidity = function () {
        var email = $('#loginEmail').val().trim();
        var password = $('#loginPassword').val().trim();
        var signInButton = $('#loginForm .btn-modern');

        // Check if email is valid and password meets minimum length
        var isValid = self.validateEmail(email) && password.length >= 6;

        if (isValid) {
            signInButton.prop('disabled', false);
            signInButton.css('opacity', '1');
            signInButton.css('cursor', 'pointer');
        } else {
            signInButton.prop('disabled', true);
            signInButton.css('opacity', '0.6');
            signInButton.css('cursor', 'not-allowed');
        }
    };

    // Handle Sign In
    self.handleSignIn = function () {
        var email = $('#loginEmail').val().trim();
        var password = $('#loginPassword').val().trim();

        // Clear previous errors
        self.clearErrors();

        // Validate fields
        if (!self.validateEmail(email)) {
            self.showError('loginEmailError', 'Please enter a valid email address.');
            return;
        }

        if (!password || password.length < 6) {
            self.showError('loginPasswordError', 'Password must be at least 6 characters.');
            return;
        }

        // Show loading state using preloader
        $('#preloader').fadeIn(300);
        var signInButton = $('#loginForm .btn-modern');
        signInButton.prop('disabled', true);
        signInButton.html('<i class="ri-loader-4-line ri-spin me-2"></i> Signing In...');

        var userAuthentication = {
            username: email,
            password: password,
            rememberMe: $('#rememberMe').is(':checked')
        };

        // Make AJAX request for login using common function
        makeAjaxRequest({
            url: '/Account/Login',
            data: userAuthentication,
            type: 'POST',
            successCallback: function (response) {
                console.log('Login response:', response);
                self.handleAuthenticationSuccess(response);
            },
            errorCallback: function (xhr, status, error) {
                console.log('Login failed:', error);
                self.handleAuthenticationError(xhr, status, error);
            },
            completeCallback: function () {
                // Hide loader
                $('#preloader').fadeOut(300);
                signInButton.prop('disabled', false);
                signInButton.html('<i class="ri-login-circle-line me-2"></i> Sign In');

                // Re-check form validity after hiding loader
                self.checkFormValidity();
            }
        });
    };

    // Validate email format
    self.validateEmail = function (email) {
        var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    };

    // Show error message
    self.showError = function (errorElementId, message) {
        var errorElement = $('#' + errorElementId);
        errorElement.text(message);
        errorElement.show();

        // Auto-hide after 5 seconds
        setTimeout(function () {
            errorElement.fadeOut(300);
        }, 5000);
    };

    // Clear all errors
    self.clearErrors = function () {
        $('.text-danger').hide().text('');
    };

    // Handle authentication success
    self.handleAuthenticationSuccess = function (response) {
        console.info('Authentication response:', response);

        // Check if response has appUser
        if (response && response.appUser) {
            var appUser = response.appUser;

            // Check if user has access
            if (response.hasAccess === false) {
                // Show access denied error message
                var errorMsg = response.message || 'Access Denied. You do not have permission to access this portal.';
                self.showError('loginEmailError', errorMsg);
                // Hide loader
                $('#preloader').fadeOut(300);
                return;
            }

            // Check if user is authenticated successfully
            if (appUser.jwtToken) {
                // Store user info using common storage service
                storageService.set('ApplicationUser', appUser);

                // Get role name
                var roleName = appUser.authenticateResponseDto?.roleInfo?.name;

                // Hide loader before redirect
                $('#preloader').fadeOut(300);

                // Redirect based on role
                if (roleName === "Administrator" || roleName === "Operator") {
                    // Admin or Operator - redirect to Admin dashboard
                    window.location.href = '/AdminBoard/Index';
                } else {
                    // Regular user with dealer - redirect to User dashboard
                    window.location.href = '/UserBoard/Index';
                }
            } else {
                // JWT token is missing - show error message
                var errorMessage = appUser.message || 'Authentication failed. Please try again.';
                self.showError('loginEmailError', errorMessage);
                // Hide loader
                $('#preloader').fadeOut(300);
            }
        } else {
            // Invalid response
            self.showError('loginEmailError', 'Invalid response from server. Please try again.');
            // Hide loader
            $('#preloader').fadeOut(300);
        }
    };

    // Handle authentication error
    self.handleAuthenticationError = function (xhr, status, error) {
        console.error('Authentication error:', error);

        // Hide loader
        $('#preloader').fadeOut(300);

        // Show error message from server or generic message
        if (xhr.responseJSON && xhr.responseJSON.message) {
            self.showError('loginEmailError', xhr.responseJSON.message);
        } else if (xhr.responseJSON && xhr.responseJSON.error) {
            self.showError('loginEmailError', xhr.responseJSON.error);
        } else {
            self.showError('loginEmailError', 'Invalid email or password. Please try again.');
        }
    };

    // Update environment and version
    self.updateEnvironmentAndVersion = function () {
        storageService.set('Environment', window.location.hostname);
        storageService.set('Version', '1.0.0.0');
    };

    // Toggle password visibility
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
}