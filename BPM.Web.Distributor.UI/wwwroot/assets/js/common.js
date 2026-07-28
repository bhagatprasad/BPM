function togglePassword(id, eyeId) {

    var input = document.getElementById(id);
    var eye = document.getElementById(eyeId);

    if (!input || !eye)
        return;

    if (input.type === "password") {
        input.type = "text";
        eye.className = "bi bi-eye-slash";
    }
    else {
        input.type = "password";
        eye.className = "bi bi-eye";
    }
}