window.onload = function () {

    const token = localStorage.getItem("token");

    if (!token) {
        window.location.href = "login.html";
        return;
    }

    const payload = JSON.parse(atob(token.split('.')[1]));
    const role = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

    // Se não for Admin, esconder itens admin
    if (role !== "Admin") {
        const adminItems = document.querySelectorAll(".admin-only");
        adminItems.forEach(item => item.style.display = "none");
    }
};

function irPara(pagina) {
    window.location.href = pagina;
}

function logout() {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}