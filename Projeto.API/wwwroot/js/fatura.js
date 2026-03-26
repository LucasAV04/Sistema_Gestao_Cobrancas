async function GerarFatura() {
    const perfil = localStorage.getItem("perfil");
    const token = localStorage.getItem("token");

    if (perfil === "User") {
        alert("Apenas Administradores podem criar Contratos.");
        return;
    }

    try {
        const response = await fetch("https://localhost:7009/faturas/GerarFatura", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
          
        });

        if (!response.ok) {
            const erro = await response.text();
            alert("Erro ao criar Fatura: " + erro);
            return;
        }

        const mensagem = await response.text();
        alert(mensagem);
        ListarFaturaContrato();

    } catch (error) {
        console.error("Erro:", error);
        alert("Erro de conexão com servidor");
    }
}
async function ListarFaturasEmAberto() {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch("https://localhost:7009/faturas/ListarFaturaEmAberto", {
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
                    Contrato Id: ${l.contratoId} |
                    Mes Referencia: ${l.mesReferencia} |
                    Valor: ${l.valor} |
                    Status: ${l.status}    
                    Data Vencimento: ${l.dataVencimento}    
                </li>
            `;
        });

    } catch (error) {
        console.error("Erro:", error);
    }
}
async function ListarFaturaContrato() {
    const id = document.getElementById("contratoIdBusca").value; 
    if (!id) {
        alert("Informe o ID do contrato");
        return;
    }
    const token = localStorage.getItem("token");
    const perfil = localStorage.getItem("perfil");
   

    try {
        const response = await fetch(`https://localhost:7009/faturas/${id}/ListarFaturaContrato`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}` 
            }
        });

        if (!response.ok) {
            const erro = await response.text();
            alert("Erro: " + erro);
            return;
        }

        const data = await response.json();

        const lista = document.getElementById("lista");
        lista.innerHTML = "";

        data.forEach(l => {
            lista.innerHTML += `
                <li  class="fatura-item">
                 <div class="linha-info">
                    ID: ${l.id} |
                    Contrato Id: ${l.contratoId} |
                    Mês Referência: ${l.mesReferencia} |
                    Valor: ${l.valor} |
                    Status: ${l.status} |
                    Data Vencimento: ${l.dataVencimento}
                     </div>

                     <div class="linha-botoes">
                             <button class="danger"
                                onclick="VencimentoFatura(${l.id})"
                                 ${perfil === "User" ? "disabled" : ""}>
                                  Marcar Fatura com Vencida
                        </div>

                </li>
            `;
        });

    } catch (error) {
        console.error("Erro:", error);
        alert("Erro de conexão com servidor");
    }
}
async function VencimentoFatura(id) {
    const token = localStorage.getItem("token");
    try {
        const response = await fetch(`https://localhost:7009/faturas/${id}/VencimentoFatura`, {
            method: "PUT",
            headers: {
                "Authorization": `Bearer ${token}`
            }

        });

        const mensagem = await response.text();
        alert(mensagem);

        ListarFaturaContrato();

    } catch (error) {
        console.error(error);
    }
}