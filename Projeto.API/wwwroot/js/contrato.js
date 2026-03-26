async function ListarContrato() {
    const perfil = localStorage.getItem("perfil");
    const token = localStorage.getItem("token");
    fetch("https://localhost:7009/contratos/ListarContratos", {
        method: "GET",
        headers: {
            "Content-type": "application/json",
            "Authorization": `Bearer ${token}`
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Erro: ${response.status}`);
            }
            return response.json();
        })

        .then(data => {

            const lista = document.getElementById("lista");
            lista.innerHTML = "";

            data.forEach(l => {

                lista.innerHTML += `
                    <li class="contrato-item">

                        <div class="linha-info">
                            ID: ${l.id} |
                            Empresa Cliente Id: ${l.empresaClienteId} |
                            Plano Id: ${l.planoId} |
                            Data de Inicio: ${l.dataInicio} |
                            Status: ${l.status}                          
                        </div>

                        <div class="linha-botoes">
                            <button class="primary" onclick="SuspenderContrato(${l.id})"
                             ${perfil === "User" ? "disabled" : ""}>
                            Suspender
                            </button>
                            <button class="success" onclick="ReativarContrato(${l.id})"
                            ${perfil === "User" ? "disabled" : ""}>
                            Reativar Contrato</button>
                             <button class="danger"
                                onclick="CancelarContrato(${l.id})"
                                 ${perfil === "User" ? "disabled" : ""}>
                                  Cancelar Contrato
                        </div>

                    </li>
                `;
            });

        })
        .catch(error => console.error("Erro:", error));
}
async function CriarContrato() {
    const perfil = localStorage.getItem("perfil");
    const token = localStorage.getItem("token");

    if (perfil === "User") {
        alert("Apenas Administradores podem criar Contratos.");
        return;
    }

    const contrato = {
        empresaId: parseInt(document.getElementById("empresaId").value), // corrigido
        planoId: parseInt(document.getElementById("planoId").value),     // corrigido
        diaVencimento: parseInt(document.getElementById("diaVencimento").value) // corrigido
    };

    try {
        const response = await fetch("https://localhost:7009/contratos/CriarContrato", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(contrato)
        });

        if (!response.ok) {
            const erro = await response.text();
            alert("Erro ao criar Contrato: " + erro);
            return;
        }

        const mensagem = await response.text();
        alert(mensagem); // "Contrato Criado com sucesso"
        ListarContrato();

    } catch (error) {
        console.error("Erro:", error);
        alert("Erro de conexão com servidor");
    }
}
async function SuspenderContrato(id) {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch(`https://localhost:7009/contratos/${id}/SuspenderContrato`, {
            method: "PUT",
            headers: {
                "Authorization": `Bearer ${token}`
            }

        });

        const mensagem = await response.text();
        alert(mensagem);

        ListarContrato();

    } catch (error) {
        console.error(error);
    }
}
async function ReativarContrato(id) {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch(`https://localhost:7009/contratos/${id}/ReativarContrato`, {
            method: "PUT",
            headers: {
                "Authorization": `Bearer ${token}`
            }

        });

        const mensagem = await response.text();
        alert(mensagem);

        ListarContrato();

    } catch (error) {
        console.error(error);
    }
}
async function CancelarContrato(id) {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch(`https://localhost:7009/contratos/${id}/CancelarContrato`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${token}`
            }

        });

        const mensagem = await response.text();
        alert(mensagem);

        ListarContrato();

    } catch (error) {
        console.error(error);
    }
}