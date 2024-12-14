const togglePass = document.getElementById("togglePassword");
const passInput = document.getElementById("password");

togglePass.addEventListener("click", function () {
    this.classList.toggle("fa-eye");
    this.classList.toggle("fa-eye-slash");

    // Toggle the type attribute
    const type = passInput.getAttribute("type") === "password" ? "text" : "password";
    passInput.setAttribute("type", type);
});