// ============================================
// LOADER CONTROLLER - Manual Control
// ============================================

var loaderTimer = null;
var loaderVisible = false;
var loaderMinDisplayTime = 500; // Minimum 500ms display time

function showLoader(message) {
    console.log('showLoader called with message:', message);

    var $preloader = $('#preloader');
    if ($preloader.length) {
        if (message) {
            $preloader.find('.loading-message').text(message);
        }

        // Clear any existing timer
        if (loaderTimer) {
            clearTimeout(loaderTimer);
            loaderTimer = null;
        }

        // Remove hiding class if exists
        $preloader.removeClass('hiding');

        // Show the loader using class
        $preloader.css('display', 'flex');
        $preloader.addClass('active');
        loaderVisible = true;

        // Force reflow for animation
        void $preloader[0].offsetHeight;

        console.log('Loader shown successfully');
    } else {
        console.error('Preloader element not found!');
        // Create a fallback loader
        var fallbackLoader = $(
            '<div id="preloader" class="preloader active" style="display:flex !important;">' +
            '<div class="preloader-content">' +
            '<div class="pharmacy-loader-icon">' +
            '<svg viewBox="0 0 100 100" fill="none" xmlns="http://www.w3.org/2000/svg">' +
            '<circle cx="50" cy="50" r="48" fill="url(#loaderGradient)" stroke="#0d9488" stroke-width="2"/>' +
            '<rect x="40" y="20" width="20" height="60" rx="3" fill="white" opacity="0.95"/>' +
            '<rect x="20" y="40" width="60" height="20" rx="3" fill="white" opacity="0.95"/>' +
            '<text x="50" y="48" text-anchor="middle" font-size="22" font-weight="800" font-family="Arial, sans-serif" fill="#0d9488">B</text>' +
            '<text x="50" y="68" text-anchor="middle" font-size="16" font-weight="700" font-family="Arial, sans-serif" fill="#0e7490">PM</text>' +
            '<defs><linearGradient id="loaderGradient" x1="0%" y1="0%" x2="100%" y2="100%">' +
            '<stop offset="0%" style="stop-color:#0d9488;stop-opacity:0.15"/>' +
            '<stop offset="100%" style="stop-color:#0e7490;stop-opacity:0.08"/>' +
            '</linearGradient></defs></svg></div>' +
            '<div class="loader-text"><span class="brand-name-loader">BPM Medicals</span>' +
            '<span class="tagline-loader">Healthcare Solutions</span></div>' +
            '<div class="loading-dots"><span></span><span></span><span></span></div>' +
            '<p class="loading-message">' + (message || 'Loading...') + '</p>' +
            '</div></div>'
        );
        $('body').append(fallbackLoader);
        loaderVisible = true;
    }
}

function hideLoader() {
    console.log('hideLoader called');

    var $preloader = $('#preloader');
    if ($preloader.length) {
        // Clear any existing timer
        if (loaderTimer) {
            clearTimeout(loaderTimer);
            loaderTimer = null;
        }

        // Add hiding class for fade out
        $preloader.addClass('hiding');

        // Wait for minimum display time before hiding
        loaderTimer = setTimeout(function () {
            if (loaderVisible) {
                $preloader.removeClass('active');
                $preloader.css('display', 'none');
                loaderVisible = false;
                console.log('Loader hidden successfully');
            }
            loaderTimer = null;
        }, loaderMinDisplayTime);
    }
}

function hideLoaderImmediately() {
    console.log('hideLoaderImmediately called');

    var $preloader = $('#preloader');
    if ($preloader.length) {
        if (loaderTimer) {
            clearTimeout(loaderTimer);
            loaderTimer = null;
        }
        $preloader.removeClass('active hiding');
        $preloader.css('display', 'none');
        loaderVisible = false;
        console.log('Loader hidden immediately');
    }
}

function showLoaderWithMessage(message) {
    showLoader(message);
}

function isLoaderVisible() {
    return loaderVisible;
}

// For backward compatibility
function showPharmacyLoader() {
    showLoader();
}

function hidePharmacyLoader() {
    hideLoader();
}

function showPharmacyLoaderWithMessage(message) {
    showLoader(message);
}