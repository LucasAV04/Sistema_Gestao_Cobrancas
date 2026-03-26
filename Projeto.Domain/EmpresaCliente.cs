using Projeto.Domain;

    public class EmpresaCliente
    {
        public int Id { get; set; }
        public string RazaoSocial {  get; set; }
        public string Cnpj {  get; set; }
        public string Email {  get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro {  get; set; }

        public void AtualizarDados(string razaoSocial, string cnpj, string email)
        {
            if (!Ativo)
                throw new Exception("Empresa Inativa não pode ser Atualizada");
            
            RazaoSocial = razaoSocial;
            Cnpj = cnpj;
            Email = email;
        }
        public void Ativar()
        {
            Ativo = true;
        }
        public void Desativar()
        {
            Ativo= false;
        }
    }

