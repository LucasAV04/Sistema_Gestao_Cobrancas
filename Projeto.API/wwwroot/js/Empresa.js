
async function listarEmpresas() {
    const perfil = localStorage.getItem("perfil");
    const token = localStorage.getItem("token");
    fetch("https://localhost:7009/empresas/ListarTodos", {
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
                    <li class="empresa-item">

                        <div class="linha-info">
                            ID: ${l.id} |
                            Razão Social: ${l.razaoSocial} |
                            CNPJ: ${l.cnpj} |
                            Email: ${l.email} |
                            Ativo: ${l.ativo} |
                            Data Cadastro: ${l.dataCadastro}
                        </div>

                        <div class="linha-botoes">
                            <button class="danger" onclick="desativarEmpresa(${l.id})"
                             ${perfil === "User" ? "disabled" : ""}>
                            Desativar
                            </button>
                            <button class="success" onclick="ativarEmpresa(${l.id})"
                            ${perfil === "User" ? "disabled" : ""}>
                            Ativar</button>
                             <button class="primary"
                                onclick="abrirEdicao(${l.id}, '${l.razaoSocial}', '${l.cnpj}', '${l.email}')"
                                 ${perfil === "User" ? "disabled" : ""}>
                                  Atualizar
                        </div>

                    </li>
                `;
            });

        })
        .catch(error => console.error("Erro:", error));
}
async function criarEmpresa() {
    const perfil = localStorage.getItem("perfil");
    const token = localStorage.getItem("token");
    // 🚫 Bloqueia se for User
    if (perfil === "User") {
        alert("Apenas Administradores podem criar empresas.");
        return;
    }

    const empresa = {
        razaoSocial: document.getElementById("razaoSocial").value,
        cnpj: document.getElementById("cnpj").value,
        email: document.getElementById("email").value,
        ativo: true
    };
    
    try {
        const response = await fetch("https://localhost:7009/empresas/Adicionar", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                 "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(empresa)
        });

        if (!response.ok) {
            throw new Error("Erro ao criar empresa");
        }

        alert("Empresa criada com sucesso!");
        listarEmpresas();
    } catch (error) {
        console.error(error);
    }
}
async function listarAtivos() {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch("https://localhost:7009/empresa/ListarAtivos", {
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
                        Razão Social: ${l.razaoSocial} |
                        CNPJ: ${l.cnpj} |
                        Email: ${l.email} |
                        Ativo: ${l.ativo} |
                        Data Cadastro: ${l.dataCadastro}
                </li>
            `;
        });

    } catch (error) {
        console.error("Erro:", error);
    }
}
async function desativarEmpresa(id) {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch(`https://localhost:7009/empresas/${id}/Desativar`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        const mensagem = await response.text();
        alert(mensagem);

        listarEmpresas();

    } catch (error) {
        console.error(error);
    }
}
async function ativarEmpresa(id) {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch(`https://localhost:7009/empresas/${id}/Ativar`, {
            method: "PUT",
             headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        const mensagem = await response.text();
        alert(mensagem);

        listarEmpresas();

    } catch (error) {
        console.error(error);
    }
}
async function buscarPorId() {
    const id = document.getElementById("empresaIdBusca").value;
    const token = localStorage.getItem("token");

    try {
        const response = await fetch(`https://localhost:7009/empresas/${id}/BuscarPorID`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}` // 🔑 envia o token
            }
        });

        if (!response.ok) {
            const erro = await response.text();
            alert("Erro: " + erro);
            return;
        }

        const l = await response.json();

        const lista = document.getElementById("lista");
        lista.innerHTML = `
            <li>
                ID: ${l.id} |
                Razão Social: ${l.razaoSocial} |
                CNPJ: ${l.cnpj} |
                Email: ${l.email} |
                Ativo: ${l.ativo}
            </li>
        `;

    } catch (error) {
        console.error("Erro:", error);
        alert("Erro de conexão com servidor");
    }
}

async function atualizarEmpresa(id, razaoSocial, cnpj, email) {
    const token = localStorage.getItem("token"); 

    try {
        const response = await fetch(`https://localhost:7009/empresas/${id}/Atualizar`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}` 
            },
            body: JSON.stringify({
                razaoSocial: razaoSocial,
                cnpj: cnpj,
                email: email
            })
        });

        if (!response.ok) {
            const erro = await response.text();
            alert("Erro ao atualizar empresa: " + erro);
            return;
        }

        const mensagem = await response.text();
        alert(mensagem); 

       
        listarEmpresas();

    } catch (error) {
        console.error("Erro:", error);
        alert("Erro de conexão com servidor");
    }
}
function abrirEdicao(id, razaoSocial, cnpj, email) {

    const novaRazao = prompt("Nova Razão Social:", razaoSocial);
    if (novaRazao === null) return;

    const novoCnpj = prompt("Novo CNPJ:", cnpj);
    if (novoCnpj === null) return;

    const novoEmail = prompt("Novo Email:", email);
    if (novoEmail === null) return;

    atualizarEmpresa(id, novaRazao, novoCnpj, novoEmail);
}