async function ListarPlano() {
    const perfil = localStorage.getItem("perfil");
    const token = localStorage.getItem("token");
    fetch("https://localhost:7009/planos/ListarPlanos", {
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
                    <li class="plano-item">

                        <div class="linha-info">
                            ID: ${l.id} |
                            Nome: ${l.nome} |
                            Valor Mensal: ${l.valorMensal} |
                            Limite de Usuarios: ${l.limiteUsuarios} |
                            Ativo: ${l.ativo}                          
                        </div>

                        <div class="linha-botoes">
                            <button class="danger" onclick="DesativarPlano(${l.id})"
                             ${perfil === "User" ? "disabled" : ""}>
                            Desativar
                            </button>
                            <button class="success" onclick="AtivarPlano(${l.id})"
                            ${perfil === "User" ? "disabled" : ""}>
                            Ativar</button>
                             <button class="primary"
                                onclick="AbrirEdicao(${l.id}, '${l.nome}', '${l.valorMensal}', '${l.limiteUsuarios}')"
                                 ${perfil === "User" ? "disabled" : ""}>
                                  Atualizar
                        </div>

                    </li>
                `;
            });

        })
        .catch(error => console.error("Erro:", error));
}
async function AdicionarPlano() {
    const perfil = localStorage.getItem("perfil");
    const token = localStorage.getItem("token");

    if (perfil === "User") {
        alert("Apenas Administradores podem criar Planos.");
        return;
    }
    const plano = {
        nome: document.getElementById("nome").value,
        valorMensal: document.getElementById("valorMensal").value,
        limiteUsuario: document.getElementById("limiteUsuarios").value,
        ativo : true
    };
    try {
        const response = await fetch("https://localhost:7009/planos/Adicionar", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(plano)
        });
        if (!response.ok) {
            throw new Error("Erro ao criar Plano");
        }
        alert("Plano Criado com sucesso");
        ListarPlano();
    }
    catch (error) {
        console.error(error);
    }
}
async function AtualizarPlano(id,nome, valorMensal,limiteUsuario) {
    const token = localStorage.getItem("token");

    try {
        const response = await fetch(`https://localhost:7009/planos/${id}/Atualizar`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({
                nome: nome,
                valorMensal: valorMensal,
                limiteUsuarios: limiteUsuarios
            })
        });

        if (!response.ok) {
            const erro = await response.text();
            alert("Erro ao atualizar Plano: " + erro);
            return;
        }

        const mensagem = await response.text();
        alert(mensagem);


        ListarPlano();

    } catch (error) {
        console.error("Erro:", error);
        alert("Erro de conexão com servidor");
    }
}
async function ListarAtivos() {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch("https://localhost:7009/planos/ListarAtivos", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            }

        });

        if (!response.ok) {
            const erro = await response.text();
            alert(erro);
            return;
        }

        const data = await response.json();

        const lista = document.getElementById("lista");
        lista.innerHTML = "";

        data.forEach(l => {
            lista.innerHTML += `
                <li>
                    ID: ${l.id} |
                    Razão Social: ${l.nome} |
                    CNPJ: ${l.valorMensal} |
                    Email: ${l.limiteUsuarios} |
                    Ativo: ${l.ativo}    
                </li>
            `;
        });

    } catch (error) {
        console.error("Erro:", error);
    }
}
async function AtivarPlano(id) {
    const token = localStorage.getItem("token");

    try {
        const response = await fetch(`https://localhost:7009/planos/${id}/Ativar`, {
            method: "PUT",
             headers: {
                "Authorization": `Bearer ${token}`
            }

        });

        const mensagem = await response.text();
        alert(mensagem);

        ListarPlano();

    } catch (error) {
        console.error(error);
    }
}
async function DesativarPlano(id) {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch(`https://localhost:7009/planos/${id}/Desativar`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${token}`
            }

        });

        const mensagem = await response.text();
        alert(mensagem);

        ListarPlano();

    } catch (error) {
        console.error(error);
    }
}
function AbrirEdicao(id, nome, valorMensal, limiteUsuarios) {

    const novoNome = prompt("Novo nome:", nome);
    if (nome === null) return;

    const novoValorMensal = prompt("Novo valor mensal:", valorMensal);
    if (valorMensal === null) return;

    const novoLimiteUsuarios = prompt("Novo limite de Usuarios:", limiteUsuarios);
    if (limiteUsuarios === null) return;

    AtualizarPlano(id, novoNome, novoValorMensal, novoLimiteUsuarios);

}
 