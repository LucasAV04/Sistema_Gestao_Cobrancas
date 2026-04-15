using Microsoft.Extensions.Configuration;
using Projeto.Application;
using Projeto.Infrastructure.Infrascture.Repositories.Memory;
using Projeto.Infrastructure.Infrascture.Repositories.MySql;
using Microsoft.Extensions.Configuration.Json;

public class ProgramConsole
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var connectionFactory = new MySqlConnectionFactory(configuration);
        var usuRepo = new UsuarioRepositoryMemory();
        var empRepo = new EmpresaClienteMySql(connectionFactory);
        var contRepo = new ContratoMySql(connectionFactory);
        var planoRepo = new PlanoMySql(connectionFactory);
        var fatRepo = new FaturaMySql(connectionFactory);
        var pagaRepo = new PagamentoMySql(connectionFactory);

        var usuService = new AuthService(usuRepo);
        var empService = new EmpresaClienteService(empRepo);
        var contService = new ContratoService(contRepo, empRepo, planoRepo);
        var planoService = new PlanoService(planoRepo);
        var fatService = new FaturaService(fatRepo, contRepo, planoRepo);
        var pagaService = new PagamentoService(pagaRepo, fatRepo);


        Console.WriteLine("=== Sistema de Gestão de Contratos ===");
        Console.Write("Usuário: ");
        string nome = Console.ReadLine();
        Console.Write("Senha: ");
        string senha = LerSenhaOculta();

        var usuario = usuService.Login(nome, senha);

        if (usuario == null)
        {
            Console.WriteLine("Login inválido. Encerrando...");
            return;
        }
        Console.WriteLine($"Login realizado com sucesso! Bem-vindo, {usuario.Nome} ({usuario.Role})");


        while (true)
        {
            Console.WriteLine("1- Empresa Cliente");
            Console.WriteLine("2- Plano");
            Console.WriteLine("3- Contrato");
            Console.WriteLine("4- Fatura");
            Console.WriteLine("5- Pagamento");
            Console.WriteLine("0- Sair");


            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("1- Adicionar Empresa");
                    Console.WriteLine("2- Atualizar Empresa");
                    Console.WriteLine("3- Listar Todos");
                    Console.WriteLine("4- Listar Ativos");
                    Console.WriteLine("5- Ativar");
                    Console.WriteLine("6- Desativar");
                    string choice2 = Console.ReadLine();
                    switch (choice2)
                    {
                        case "1":
                            if (usuario.Role == "Admin")
                                AdicionarEmpresa(empService);
                            else
                                Console.WriteLine("Acesso negado. Apenas Admin.");
                            break;

                        case "2":
                            if (usuario.Role == "Admin")
                                AtualizarEmpresa(empService);
                            else
                                Console.WriteLine("Acesso negado. Apenas Admin.");
                            break;

                        case "3":
                            ListarTodasEmpresa(empService);
                            break;

                        case "4":
                            ListarEmpresasAtivas(empService);
                            break;

                        case "5":
                            if (usuario.Role == "Admin")
                                AtivarEmpresa(empService);
                            else
                                Console.WriteLine("Acesso negado. Apenas Admin.");
                            break;

                        case "6":
                            if (usuario.Role == "Admin")
                                DesativarEmpresa(empService);
                            else
                                Console.WriteLine("Acesso negado. Apenas Admin.");
                            break;
                    }
                    break;

                case "2":
                    Console.WriteLine("1- Adicionar Plano");
                    Console.WriteLine("2- Atualizar Plano");
                    Console.WriteLine("3- Listar Planos");
                    Console.WriteLine("4- Listar Ativos");
                    Console.WriteLine("5- Ativar Plano");
                    Console.WriteLine("6- Desativar Plano");

                    string choice3 = Console.ReadLine();
                    switch (choice3)
                    {
                        case "1":
                            if (usuario.Role == "Admin")
                                AdicionarPlano(planoService);
                            else
                                Console.WriteLine("Acesso negado. Apenas Admin.");
                            break;

                        case "2":
                            if (usuario.Role == "Admin")
                                AtualizarPlano(planoService);
                            else
                                Console.WriteLine("Acesso negado. Apenas Admin.");
                            break;

                        case "3":
                            ListarTodosPlanos(planoService);
                            break;

                        case "4":
                            ListarPlanosAtivos(planoService);
                            break;

                        case "5":
                            if (usuario.Role == "Admin")
                                Ativar(planoService);
                            else
                                Console.WriteLine("Acesso negado. Apenas Admin.");
                            break;

                        case "6":
                            if (usuario.Role == "Admin")
                                Desativar(planoService);
                            else
                                Console.WriteLine("Acesso negado. Apenas Admin.");
                            break;

                    }
                    break;

                case "3":
                    Console.WriteLine("1- Criar Contrato");
                    Console.WriteLine("2- Suspender Contrato");
                    Console.WriteLine("3- Reativar Contrato");
                    Console.WriteLine("4- Cancelar Contrato");
                    Console.WriteLine("5- Listar Contratos");

                    string choice4 = Console.ReadLine();
                    switch (choice4)
                    {
                        case "1":
                            CriarContrato(contService);
                            break;

                        case "2":
                            if (usuario.Role == "Admin")
                                SuspenderContrato(contService);
                            else
                                Console.WriteLine("Acesso negado. Apenas Admin.");
                            break;

                        case "3":
                            if (usuario.Role == "Admin")
                                ReativarContrato(contService);
                            else
                                Console.WriteLine("Acesso negado. Apenas Admin.");
                            break;

                        case "4":
                            if (usuario.Role == "Admin")
                                CancelarContrato(contService);
                            else
                                Console.WriteLine("Acesso negado. Apenas Admin.");
                            break;

                        case "5":
                            ListarContrato(contService);
                            break;

                    }
                    break;

                case "4":
                    Console.WriteLine("1- Gerar Faturas Mensais");
                    Console.WriteLine("2- Vencimento da Fatura");
                    Console.WriteLine("3- Listar Faturas por Contrato");
                    Console.WriteLine("4- Listar Faturas em Aberto");

                    string choice5 = Console.ReadLine();
                    switch (choice5)
                    {
                        case "1":
                            fatService.GerarFaturasMensais();
                            break;

                        case "2":
                            MarcarFaturaComoVencida(fatService);
                            break;

                        case "3":
                            ListarFaturasContrato(fatService);
                            break;

                        case "4":
                            ListarFaturasAbertas(fatService);
                            break;

                    }
                    break;

                case "5":
                    Console.WriteLine("1- Registrar Pagamento");
                    Console.WriteLine("2- Listar");
                    Console.WriteLine("3- Baixar Fatura");



                    string choice6 = Console.ReadLine();
                    switch (choice6)
                    {
                        case "1":
                            RegistrarPagamento(pagaService);
                            break;

                        case "2":
                            ListarPagamento(pagaService);
                            break;

                        case "3":
                            BaixarFatura(pagaService);
                            break;
                    }
                    break;

                case "0":
                    return;
                    break;

                default:
                    Console.WriteLine("Opção Inválida");
                    break;
            }
        }
    }
    //=======================================Empresa Cliente=============================
    static void AdicionarEmpresa(EmpresaClienteService service)
    {
        Console.WriteLine("Digite a Razão Social da Empresa: ");
        string razaoSocial = Console.ReadLine();
        Console.WriteLine("Digite o Cnpj da Empresa: ");
        string cnpj = Console.ReadLine();
        Console.WriteLine("Digite o Email da Empresa");
        string email = Console.ReadLine();
        Console.WriteLine("A Empresa foi Adicionada com sucesso");
        service.Adicionar(razaoSocial, cnpj, email);
    }
    static void AtualizarEmpresa(EmpresaClienteService service)
    {
        Console.WriteLine("Digite o Id da Empresa: ");
        int id_empresa = int.Parse(Console.ReadLine());
        Console.WriteLine("Digite a Razão Social da Empresa: ");
        string razaoSocial = Console.ReadLine();
        Console.WriteLine("Digite o Cnpj da Empresa: ");
        string cnpj = Console.ReadLine();
        Console.WriteLine("Digite o Email da Empresa");
        string email = Console.ReadLine();
        Console.WriteLine("Empresa Atualizada com Sucesso");
        service.AtualizarEmpresa(id_empresa, razaoSocial, cnpj, email);
    }
    static void ListarTodasEmpresa(EmpresaClienteService service)
    {
        var listar_Empresa = service.ListarTodos();
        if (listar_Empresa.Count == 0)
            throw new Exception("Ñão tem Empresas Cadastradas");
        foreach (var l in listar_Empresa)
        {
            Console.WriteLine("ID: " + l.Id + "|" + " Razão Social: " + l.RazaoSocial + "|" + " Cnpj: " + l.Cnpj +
                "|" + " Email: " + l.Email + " Ativo: " + l.Ativo + "|" + " Data de Cadastro: " + l.DataCadastro);
        }
    }
    static void ListarEmpresasAtivas(EmpresaClienteService service)
    {
        var listar_ativos = service.ListarAtivos();
        if (listar_ativos.Count == 0)
            throw new Exception("Não tem Empresas Ativas Cadastradas");
        foreach (var l in listar_ativos)
        {
            Console.WriteLine("ID: " + l.Id + "|" + " Razão Social: " + l.RazaoSocial + "|" + " Cnpj: " + l.Cnpj +
                "|" + " Email: " + l.Email + " Ativo: " + l.Ativo + "|" + " Data de Cadastro: " + l.DataCadastro);
        }
    }
    static void AtivarEmpresa(EmpresaClienteService service)
    {
        Console.WriteLine("Digite o Id da Empresa: ");
        int id_ativar = int.Parse(Console.ReadLine());
        Console.WriteLine("Empresa Ativada com Sucesso");
        service.Ativar(id_ativar);
    }
    static void DesativarEmpresa(EmpresaClienteService service)
    {
        Console.WriteLine("Digite o Id da Empresa: ");
        int id_desativar = int.Parse(Console.ReadLine());
        Console.WriteLine("Empresa Desativada com Sucesso");
        service.Desativar(id_desativar);
    }
    static void RemoverEmpresa(EmpresaClienteService service)
    {
        Console.WriteLine("Digite o Id da Empresa: ");
        int id = int.Parse(Console.ReadLine());
        service.RemoverEmpresaCliente(id);
        Console.WriteLine("Empresa Removida com Sucesso");
    }
    //=====================================================Plano====================================================
    static void AdicionarPlano(PlanoService service)
    {
        Console.WriteLine("Digite o Nome do Plano: ");
        string nome = Console.ReadLine();
        Console.WriteLine("Digite o Valor Mensal do Plano: ");
        decimal valor = decimal.Parse(Console.ReadLine());
        Console.WriteLine("Digite o Número Limite de Usuários: ");
        int limite = int.Parse(Console.ReadLine());
        Console.WriteLine("Plano Adicionado com Sucesso");
        service.Adicionar(nome, valor, limite);
    }
    static void AtualizarPlano(PlanoService service)
    {
        Console.WriteLine("Digite o Id do Plano: ");
        int id = int.Parse(Console.ReadLine());
        Console.WriteLine("Digite o Nome do Plano: ");
        string nome = Console.ReadLine();
        Console.WriteLine("Digite o Valor Mensal do Plano: ");
        decimal valor = decimal.Parse(Console.ReadLine());
        Console.WriteLine("Digite o Número Limite de Usuários: ");
        int limite = int.Parse(Console.ReadLine());
        Console.WriteLine("Plano Atualizado com Sucesso");
        service.AtualizarPlano(id, nome, valor, limite);
    }
    static void ListarTodosPlanos(PlanoService service)
    {
        var listar_plano = service.ListarTudo();
        if (listar_plano.Count == 0)
            throw new Exception("Não Tem Planos Cadastrados");
        foreach (var a in listar_plano)
        {
            Console.WriteLine("Id: " + a.Id + "|" + " Nome: " + a.Nome + "|" + " Valor Mensal: " + a.ValorMensal + "|" +
               " Limite de Usuários: " + a.LimiteUsuarios + "|" + " Ativo: " + a.Ativo);
        }
    }
    static void ListarPlanosAtivos(PlanoService service)
    {
        var listar_ativos = service.ListarAtivos();
        if (listar_ativos.Count == 0)
            throw new Exception("Não tem Planos Ativos Cadastradas");
        foreach (var a in listar_ativos)
        {
            Console.WriteLine("Id: " + a.Id + "|" + " Nome: " + a.Nome + "|" + " Valor Mensal: " + a.ValorMensal + "|" +
               " Limite de Usuários: " + a.LimiteUsuarios + "|" + " Ativo: " + a.Ativo);
        }
    }
    static void Ativar(PlanoService service)
    {
        Console.WriteLine("Digite o Id do Plano");
        int id = int.Parse(Console.ReadLine());
        Console.WriteLine("Plano Ativado com Sucesso");
        service.Ativar(id);
    }
    static void Desativar(PlanoService service)
    {
        Console.WriteLine("Digite o Id do Plano");
        int id = int.Parse(Console.ReadLine());
        Console.WriteLine("Plano Desativado com Sucesso");
        service.Desativar(id);
    }
    static void RemoverPlano(PlanoService service)
    {
        Console.WriteLine("Digite o Id do Plano");
        int id = int.Parse(Console.ReadLine());
        service.RemoverPlano(id);
        Console.WriteLine("Plano Removido com Sucesso");
    }
    //=====================================================Contrato================================================
    static void CriarContrato(ContratoService service)
    {
        Console.WriteLine("Digite o Id da Empresa: ");
        int id_empresa = int.Parse(Console.ReadLine());
        Console.WriteLine("Digite o Id do Plano: ");
        int id_plano = int.Parse(Console.ReadLine());
        Console.WriteLine("Digite o Dia do Vencimento do Contrato: ");
        int dia = int.Parse(Console.ReadLine());
        Console.WriteLine("Contrato Criado com Sucesso");
        service.CriarContrato(id_empresa, id_plano, dia);
    }
    static void SuspenderContrato(ContratoService service)
    {
        Console.WriteLine("Digite o Id do Contrato: ");
        int id = int.Parse(Console.ReadLine());
        service.SuspenderContrato(id);
        Console.WriteLine("Contrato Suspenso");
    }
    static void ReativarContrato(ContratoService service)
    {
        Console.WriteLine("Digite o Id do Contrato: ");
        int id = int.Parse(Console.ReadLine());
        service.ReativarContrato(id);
    }
    static void CancelarContrato(ContratoService service)
    {
        Console.WriteLine("Digite o Id do Contrato: ");
        int id = int.Parse(Console.ReadLine());
        service.CancelarContrato(id);
    }
    static void ListarContrato(ContratoService service)
    {
        var listar_contrato = service.ListarContrato();
        if (listar_contrato.Count == 0)
            throw new Exception("Não Tem Contratos Cadastrados");
        foreach (var l in listar_contrato)
        {
            Console.WriteLine("Id: " + l.Id + "|" + " Empresa Cliente Id: " + l.EmpresaClienteId +
                "|" + " Plano Id: " + l.PlanoId + "|" + " Data de Inicio: " + l.DataInicio +
                "|" + " Status: " + l.Status);
        }
    }
    static void RemoverContrato(ContratoService service)
    {
        Console.WriteLine("Digite o Id do Contrato: ");
        int id = int.Parse(Console.ReadLine());
        service.RemoverContrato(id);
        Console.WriteLine("Contrato Removido");
    }
    //===================================================Faturas========================================================
    static void MarcarFaturaComoVencida(FaturaService service)
    {
        Console.WriteLine("Digite o Id da Fatura: ");
        int id = int.Parse(Console.ReadLine());
        Console.WriteLine("Fatura está Vencida");
        service.MarcarFaturaComoVencida(id);
    }
    static void ListarFaturasContrato(FaturaService service)
    {
        Console.WriteLine("Digite o Id do Contrato");
        int id = int.Parse(Console.ReadLine());
        var listar_faturas = service.ListarFaturasPorContrato(id);
        if (listar_faturas.Count == 0)
            throw new Exception("Não Tem Faturas Cadastradas");
        foreach (var l in listar_faturas)
        {
            Console.WriteLine("Id: " + l.Id + "|" + " Contrato Id: " + l.ContratoId + "|" + " Mês Referencia: " + l.MesReferencia +
                "|" + " Valor: " + l.Valor + "|" + " Status: " + l.Status + "|" + " Data Vencimento: " + l.DataVencimento);
        }
    }
    static void ListarFaturasAbertas(FaturaService service)
    {
        var listar_ativas = service.ListarFaturasEmAberto();
        if (listar_ativas.Count == 0)
            throw new Exception("Não Tem Faturas em Aberto");
        foreach (var l in listar_ativas)
        {
            Console.WriteLine("Id: " + l.Id + "|" + " Contrato Id: " + l.ContratoId + "|" + " Mês Referencia: " + l.MesReferencia +
                "|" + " Valor: " + l.Valor + "|" + " Status: " + l.Status + "|" + " Data Vencimento: " + l.DataVencimento);
        }
    }
    static void RemoverFatura(FaturaService service)
    {
        Console.WriteLine("Digite o Id da Fatura: ");
        int id = int.Parse(Console.ReadLine());
        service.RemoverFatura(id);
    }
    //====================================================Pagamento========================================================
    static void RegistrarPagamento(PagamentoService service)
    {
        Console.WriteLine("Digite o Id da Fatura: ");
        int id = int.Parse(Console.ReadLine());
        Console.WriteLine("Digite o Valor: ");
        decimal valor = decimal.Parse(Console.ReadLine());
        Console.WriteLine("Digite a Forma de Pagamento");
        string forma = Console.ReadLine();
        Console.WriteLine("Pagamento Registrado");
        service.RegistrarPagamento(id, valor, forma);

    }
    static void ListarPagamento(PagamentoService service)
    {
        var listar_pagamento = service.Listar();
        if (listar_pagamento == null)
            throw new Exception("Não Tem Pagamentos Cadastrados");
        foreach (var l in listar_pagamento)
        {
            Console.WriteLine("Id: " + l.Id + "|" + " Fatura Id: " + l.FaturaId + "|" + " Valor Pago: " + l.ValorPago +
                "|" + "Data de Pagamento: " + l.DataPagamento + "|" + " Forma de Pagamento: " + l.FormaPagamento);
        }
    }
    static void BaixarFatura(PagamentoService service)
    {
        Console.WriteLine("Digite o Id da Fatura: ");
        int id = int.Parse(Console.ReadLine());
        service.BaixarFatura(id);
    }
    static void RemoverPagamento(PagamentoService service)
    {
        Console.WriteLine("Digite o Id do Pagamento: ");
        int id = int.Parse(Console.ReadLine());
        service.RemoverPagamento(id);
        Console.WriteLine("Pagamento Removido");
    }

    public static string LerSenhaOculta()
    {
        string senha = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true); // true = não mostra a tecla no console

            if (key.Key == ConsoleKey.Backspace && senha.Length > 0)
            {
                // Remove último caractere se usuário apertar Backspace
                senha = senha.Substring(0, senha.Length - 1);
                Console.Write("\b \b");
            }
            else if (key.Key != ConsoleKey.Enter)
            {
                // Adiciona caractere digitado
                senha += key.KeyChar;
                Console.Write("*");

            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine(); // pula linha depois do Enter
        return senha;
    }

}