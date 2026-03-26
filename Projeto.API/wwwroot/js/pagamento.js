async function RegistrarPagamento() {
    const perfil = localStorage.getItem("perfil");
    const token = localStorage.getItem("token");

    if (!token) {
        alert("Sessão expirada. Faça login novamente.");
        return;
    }

    // Se quiser bloquear no front-end também
    if (perfil === "User") {
        alert("Apenas Administradores podem registrar pagamentos.");
        return;
    }

    const id = document.getElementById("faturaId").value;
    const valor = parseFloat(document.getElementById("valorPagamento").value);
    const forma = document.getElementById("formaPagamento").value;

    const pagamento = {
        valor: valor,
        forma: forma
    };

    try {
        const response = await fetch(`https://localhost:7009/pagamento/${id}/RegistrarPagamento`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(pagamento)
        });

        if (!response.ok) {
            const erro = await response.text();
            alert("Erro ao registrar pagamento: " + erro);
            return;
        }

        const mensagem = await response.text();
        alert(mensagem); 

        // Se quiser atualizar a lista de faturas depois
        ListarFaturaContrato();

    } catch (error) {
        console.error("Erro:", error);
        alert("Erro de conexão com servidor");
    }
}
async function ListarPagamento() {
    const perfil = localStorage.getItem("perfil");
    const token = localStorage.getItem("token");
    fetch("https://localhost:7009/pagamento/ListarPagamento", {
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
                    <li class="pagamento-item">

                        <div class="linha-info">
                            ID: ${l.id} |
                            Fatura Id: ${l.faturaId} |
                            Valor Pago: ${l.valorPago} |
                            Data de Pagamento: ${l.dataPagamento} |
                            Forma de Pagamento: ${l.formaPagamento}                          
                        </div>

                        <div class="linha-botoes">
                            <button class="primary" onclick="BaixarFatura(${l.id})"
                            Baixar Fatura
                            </button>
                        </div>

                    </li>
                `;
            });

        })
        .catch(error => console.error("Erro:", error));
}
async function BaixarFatura(id) {
    const token = localStorage.getItem("token");

    try {
        const response = await fetch(`https://localhost:7009/pagamento/${id}/BaixarFatura`, {
            method: "GET", // ou PUT, conforme seu endpoint
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) {
            const erro = await response.text();
            alert("Erro ao baixar fatura: " + erro);
            return;
        }

        // ✅ Tratar como arquivo (blob)
        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);

        const a = document.createElement("a");
        a.href = url;
        a.download = `fatura_${id}.pdf`; // nome do arquivo
        document.body.appendChild(a);
        a.click();
        a.remove();

        window.URL.revokeObjectURL(url);

    } catch (error) {
        console.error("Erro:", error);
        alert("Erro de conexão com servidor");
    }
}
