async function login(event) {
    event.preventDefault(); // 🚨 impede o reload da página

    const tipo = document.getElementById("tipoUsuario").value;
    const senha = document.getElementById("senha").value;
    const erro = document.getElementById("erroLogin");

    erro.innerText = "";

    if (!tipo) {
        erro.innerText = "Selecione o tipo de usuário";
        return;
    }

    let usuario;

    if (tipo === "User") {
        usuario = "andre";
        if (senha !== "1234") {
            erro.innerText = "Senha incorreta para User";
            return;
        }
    }

    if (tipo === "Admin") {
        usuario = "Lucas";
        if (senha !== "3214") {
            erro.innerText = "Senha incorreta para Admin";
            return;
        }
    }

    try {
        const response = await fetch("https://localhost:7009/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                usuario: usuario,
                senha: senha
            })
        });

        if (!response.ok) {
            erro.innerText = "Falha na autenticação";
            return;
        }

        const data = await response.json();

        // 🔑 Aqui você salva o token e o perfil
        localStorage.setItem("token", data.token);
        localStorage.setItem("perfil", tipo);

        // Redireciona para o menu
        window.location.href = "menu.html";

    } catch (error) {
        console.error(error);
        erro.innerText = "Erro ao conectar com servidor";
    }
}
