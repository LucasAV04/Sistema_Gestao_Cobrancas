using Projeto.Domain;

    public class Plano
    {
        public int Id { get;  set; }
        public string Nome { get;  set; }
        public decimal ValorMensal {  get;  set; }
        public int LimiteUsuarios {  get; set; }
        public bool Ativo { get; set; } = true;

        public void  AtualizarDados(string nome,decimal valorMensal,int limiteUsuario)
        {
            if (!Ativo)
                throw new Exception("Plano inativo não pode ser atualizado.");
            Nome = nome;
            ValorMensal = valorMensal;
            LimiteUsuarios = limiteUsuario;
        }

        public void Ativar()
        {
            Ativo = true;
        }

        public void Desativar()
        {
            Ativo = false;
        } 
        public void ValidarValorPlano(decimal valor)
        {
          
        }
    }

