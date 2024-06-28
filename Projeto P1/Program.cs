using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOCADORA
{
    public class Program
    {
        static void Main(String[] args)
        {
            Functions functions = new Functions();
            List<Cliente> clientes = new List<Cliente>();
            clientes = functions.CarregaListaClientes();
            List<Filme> filmes = new List<Filme>();
            filmes = functions.CarregaListaFilmes();
            List<Locacao> locacoes = new List<Locacao>();
            locacoes = functions.CarregaListaLocacoes();

            Thread.Sleep(3000);
            Console.Clear();
            int opt = 0;
            int novoId;
            while (opt != 5)
            {
                Console.WriteLine("******************************SISTEMA LOCAÇÂO DE FILMES******************************");
                Console.WriteLine("*                                                                                   *");
                Console.WriteLine("*        1. Cadastrar Cliente                                                       *");
                Console.WriteLine("*        2. Cadastrar Título                                                        *");
                Console.WriteLine("*        3. Registrar Locação                                                       *");
                Console.WriteLine("*        4. Registrar Devolução                                                     *");
                Console.WriteLine("*        5. Sair                                                                    *");
                Console.WriteLine("*                                                                                   *");
                Console.WriteLine("*************************************************************************************");
                Console.Write("         Selecione uma das opções: ");
                opt = int.Parse(Console.ReadLine());
                switch (opt)
                {
                    case 1:
                        Console.Clear();
                        novoId = clientes.Count > 0 ? clientes.Select(x => x.Id).Max() + 1 : 1;
                        functions.CadCliente(novoId);
                        Thread.Sleep(2000);
                        clientes = functions.CarregaListaClientes();
                        Console.Clear();
                        break;
                    case 2:
                        Console.Clear();
                        novoId = filmes.Count > 0 ? filmes.Select(x => x.Id).Max() + 1 : 1 ;
                        functions.CadFilme(novoId);
                        Thread.Sleep(2000);
                        filmes = functions.CarregaListaFilmes();
                        Console.Clear();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine($"Título 1: {clientes.Where(x => x.Id == 1).Select(y => y.Titulo1).FirstOrDefault()} e Título 2: {clientes.Where(x => x.Id == 1).Select(y => y.Titulo2).FirstOrDefault()}");
                        functions.CadLocacao(clientes, filmes);
                        Thread.Sleep(2000);
                        locacoes = functions.CarregaListaLocacoes();
                        Console.Clear();
                        break;
                    case 4:
                        Console.Clear();
                        functions.CadDevolucao(locacoes, filmes, clientes);
                        Thread.Sleep(2000);
                        locacoes = functions.CarregaListaLocacoes();
                        Console.Clear();
                        break;
                    case 5:
                        Console.WriteLine("Finalizando o sistema!");
                        Thread.Sleep(1000);
                        break;
                    default:
                        Console.WriteLine("");
                        break;
                }
            }
        }
    }

    public class Functions
    {
        public string montaLinha(string campo, string dado)
        {
            int countCaracteres = 9 + campo.Length + dado.Length + 1;
            string linha = "*                                                                                   *";
            var partLinha = linha.Substring(countCaracteres);
            linha = $"*        {campo}:{dado}" + partLinha;
            return linha;
        }


        public bool verificaCliente(List<Cliente> clientes, int id, string titulo)
        {
            var cliente = clientes.Where(x => x.Id == id).FirstOrDefault();
            if (cliente == null)
            {
                return false;
            }
            else
            {
                if((cliente.Titulo1 != "n\\a") && (cliente.Titulo2 != "n\\a"))
                {
                    return false;
                }
               
                if(cliente.Titulo1 == "n\\a")
                {
                    cliente.Titulo1 = titulo;
                    AtualizarArquivo("clientes", AlteraClientes(clientes));
                    return true;
                }
                cliente.Titulo2 = titulo;
                AtualizarArquivo("clientes", AlteraClientes(clientes));
                return true;
            }
        }

       
        public bool verificaFilme(List<Filme> filmes, int id)
        {
            var filme = filmes.Where(x => x.Id == id).FirstOrDefault();
            if (filme != null && filme.Unidades > 0)
            {
                filme.Unidades = filme.Unidades - 1;
                AtualizarArquivo("filmes", AlteraFilmes(filmes));
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool verificaLocacao(List<Locacao> locacoes, int idCLiente, int idFilme)
        {
            var locacao = locacoes.Where(x=>x.CodigoCliente == idCLiente && x.CodigoFilme == idFilme).FirstOrDefault();
            
            if (locacao != null && locacao.Entregue == "N")
            {
                locacao.Entregue = "S";
                AtualizarArquivo("locacoes", AlteraLocacoes(locacoes));
                return true;
            }
            else
            {
                return false;
            }

        }


        public List<string> AlteraFilmes(List<Filme> filmes)
        {
            List<string> strings = new List<string>();
            foreach (var item in filmes)
            {
                strings.Add($"{item.Id},{item.Titulo},{item.Genero},{item.Unidades}");
            }
            return strings;
        }
        
        public List<string> AlteraClientes(List<Cliente> clientes)
        {
            List<string> strings = new List<string>();
            foreach (var cliente in clientes)
            {
                strings.Add($"{cliente.Id},{cliente.Nome},{cliente.Sobrenome},{cliente.Logradouro},{cliente.Bairro},{cliente.CidadeEstado},{cliente.CEP},{cliente.Documento},{cliente.Titulo1},{cliente.Titulo2}");
            }
            return strings;
        }
      
        public List<string> AlteraLocacoes(List<Locacao> locacoes)
        {
            List<string> strings = new List<string>();
            foreach (var locacao in locacoes)
            {
                strings.Add($"{locacao.CodigoCliente},{locacao.Nome},{locacao.CodigoFilme},{locacao.Titulo},{locacao.DataLocacao},{locacao.DataPrazoEntrega},{locacao.Entregue}"); ;
            }
            return strings;
        }



        public void AtualizarArquivo(string nome, List<string> linhas)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(nome+".csv"))
                {
                    foreach (string linha in linhas)
                    {
                        writer.WriteLine(linha);
                    }
                }

                Console.WriteLine($"{nome}.csv criado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao criar o arquivo CSV: " + ex.Message);
            }
        }


        public void PreencheTelaCliente(Cliente cliente)
        {
            Console.WriteLine("******************************SISTEMA LOCAÇÂO DE FILMES******************************");
            Console.WriteLine("*                                                                                   *");
            Console.WriteLine(montaLinha("Id", cliente.Id.ToString()));
            Console.WriteLine(montaLinha("Nome", cliente.Nome == null ? "" : cliente.Nome));
            Console.WriteLine(montaLinha("Sobrenome", cliente.Sobrenome == null ? "" : cliente.Sobrenome));
            Console.WriteLine(montaLinha("Logradouro", cliente.Logradouro == null ? "" : cliente.Logradouro));
            Console.WriteLine(montaLinha("Bairro", cliente.Bairro == null ? "" : cliente.Bairro));
            Console.WriteLine(montaLinha("Cidade/Estado", cliente.CidadeEstado == null ? "" : cliente.CidadeEstado));
            Console.WriteLine(montaLinha("CEP", cliente.CEP == null ? "" : cliente.CEP));
            Console.WriteLine(montaLinha("Documento", cliente.Documento == null ? "" : cliente.Documento));
            Console.WriteLine("*                                                                                   *");
            Console.WriteLine("*************************************************************************************");
        }
        public void PreencheTelaFilme(Filme filme)
        {
            Console.WriteLine("******************************SISTEMA LOCAÇÂO DE FILMES******************************");
            Console.WriteLine("*                                                                                   *");
            Console.WriteLine(montaLinha("Id", filme.Id.ToString()));
            Console.WriteLine(montaLinha("Título", filme.Titulo == null ? "" : filme.Titulo));
            Console.WriteLine(montaLinha("Gênero", filme.Genero == null ? "" : filme.Genero));
            Console.WriteLine(montaLinha("Unidades", filme.Unidades.ToString()));
            Console.WriteLine("*                                                                                   *");
            Console.WriteLine("*************************************************************************************");
        }
        public void PreencheTelaLocacao(Locacao locacao)
        {
            Console.WriteLine("******************************SISTEMA LOCAÇÂO DE FILMES******************************");
            Console.WriteLine("*                                                                                   *");
            Console.WriteLine(montaLinha("Código do Cliente", locacao.CodigoCliente.ToString()));
            Console.WriteLine(montaLinha("Nome", locacao.Nome == null ? "" : locacao.Nome));
            Console.WriteLine(montaLinha("Código do Filme", locacao.CodigoCliente.ToString()));
            Console.WriteLine(montaLinha("Título", locacao.Titulo == null ? "" : locacao.Titulo));
            Console.WriteLine(montaLinha("Data Locação", locacao.DataLocacao.ToString()));
            Console.WriteLine(montaLinha("Data Prazo de Entrega", locacao.DataPrazoEntrega.ToString()));
            Console.WriteLine("*                                                                                   *");
            Console.WriteLine("*************************************************************************************");
        }


        public List<Cliente> CarregaListaClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            try
            {
                if(File.Exists("clientes.csv"))
                {
                    using (StreamReader reader = new StreamReader("clientes.csv"))
                    {
                        string linha;
                        while ((linha = reader.ReadLine()) != null)
                        {
                            Cliente cliente = new();
                            string[] dados = linha.Split(',');
                            if (dados.Length == 10)
                            {
                                cliente.Id = int.Parse(dados[0]);
                                cliente.Nome = dados[1];
                                cliente.Sobrenome = dados[2];
                                cliente.Logradouro = dados[3];
                                cliente.Bairro = dados[4];
                                cliente.CidadeEstado = dados[5];
                                cliente.CEP = dados[6];
                                cliente.Documento = dados[7];
                                cliente.Titulo1 = dados[8];
                                cliente.Titulo2 = dados[9];

                                clientes.Add(cliente);
                            }
                            else
                            {
                                Console.WriteLine("Ignorando linha inválida: " + linha);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Cadastro de cliente não encontrado!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
            return clientes;
        }

        public List<Filme> CarregaListaFilmes()
        {
            List<Filme> filmes = new List<Filme>();
            
            try
            {
                if(File.Exists("filmes.csv"))
                {
                    using (StreamReader reader = new StreamReader("filmes.csv"))
                    {
                        string linha;
                        while ((linha = reader.ReadLine()) != null)
                        {
                            Filme filme = new();
                            string[] dados = linha.Split(',');
                            if (dados.Length == 4)
                            {
                                filme.Id = int.Parse(dados[0]);
                                filme.Titulo = dados[1];
                                filme.Genero = dados[2];
                                filme.Unidades = int.Parse(dados[3]);

                                filmes.Add(filme);
                            }
                            else
                            {
                                Console.WriteLine("Ignorando linha inválida: " + linha);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Cadastro de filmes não encontrado!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
            return filmes;
        }

        public List<Locacao> CarregaListaLocacoes()
        {
            List<Locacao> locacoes = new List<Locacao>();
            
            try
            {
                if(File.Exists("locacoes.csv"))
                {
                    using (StreamReader reader = new StreamReader("locacoes.csv"))
                    {
                        string linha;
                        while ((linha = reader.ReadLine()) != null)
                        {
                            Locacao locacao = new();
                            string[] dados = linha.Split(',');
                            if (dados.Length == 7)
                            {
                                locacao.CodigoCliente = int.Parse(dados[0]);
                                locacao.Nome = dados[1];
                                locacao.CodigoFilme = int.Parse(dados[2]);
                                locacao.Titulo = dados[3];
                                locacao.DataLocacao = DateTime.Parse(dados[4]);
                                locacao.DataPrazoEntrega = DateTime.Parse(dados[5]);
                                locacao.Entregue = dados[6];

                                locacoes.Add(locacao);
                            }
                            else
                            {
                                Console.WriteLine("Ignorando linha inválida: " + linha);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Cadastro de locações não encontrado!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
            return locacoes;
        }


        public void CadCliente(int novoId)
        {
            Cliente cliente = new Cliente();
            cliente.Id = novoId;
            PreencheTelaCliente(cliente);
            Console.Write("Digite o Nome do cliente: ");
            cliente.Nome = Console.ReadLine();
            Console.Clear();
            PreencheTelaCliente(cliente);
            Console.Write("Digite o Sobrenome do cliente: ");
            cliente.Sobrenome = Console.ReadLine();
            Console.Clear();
            PreencheTelaCliente(cliente);
            Console.Write("Digite o Logradouro do cliente: ");
            cliente.Logradouro = Console.ReadLine();
            Console.Clear();
            PreencheTelaCliente(cliente);
            Console.Write("Digite o Bairro do cliente: ");
            cliente.Bairro = Console.ReadLine();
            Console.Clear();
            PreencheTelaCliente(cliente);
            Console.Write("Digite a Cidade/Estado do cliente: ");
            cliente.CidadeEstado = Console.ReadLine();
            Console.Clear();
            PreencheTelaCliente(cliente);
            Console.Write("Digite o CEP do cliente: ");
            cliente.CEP = Console.ReadLine();
            Console.Clear();
            PreencheTelaCliente(cliente);
            Console.Write("Digite a Documento do cliente: ");
            cliente.Documento = Console.ReadLine();

            try
            {
                using (StreamWriter writer = File.Exists("clientes.csv") ? File.AppendText("clientes.csv") : new StreamWriter("clientes.csv"))
                {
                    writer.WriteLine($"{cliente.Id},{cliente.Nome},{cliente.Sobrenome},{cliente.Logradouro},{cliente.Bairro},{cliente.CidadeEstado},{cliente.CEP},{cliente.Documento},n\\a,n\\a");
                    Console.WriteLine("Cliente cadastrado com sucesso!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }

        public void CadFilme(int novoId)
        {
            Filme filme = new Filme();
            filme.Id = novoId;
            PreencheTelaFilme(filme);
            Console.Write("Digite o Título do filme: ");
            filme.Titulo = Console.ReadLine();
            Console.Clear();
            PreencheTelaFilme(filme);
            Console.Write("Digite o Gênero do filme: ");
            filme.Genero = Console.ReadLine();
            Console.Clear();
            PreencheTelaFilme(filme);
            Console.Write("Digite a quantidade de unidades do filme: ");
            filme.Unidades = int.Parse(Console.ReadLine());
            Console.Clear();

            try
            {
                using (StreamWriter writer = File.Exists("filmes.csv") ? File.AppendText("filmes.csv") : new StreamWriter("filmes.csv"))
                {
                    writer.WriteLine($"{filme.Id},{filme.Titulo},{filme.Genero},{filme.Unidades}");
                    Console.WriteLine("Filme cadastrado com sucesso!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }

        public void CadLocacao(List<Cliente> clientes, List<Filme> filmes)
        {
            Locacao locacao = new Locacao();
            locacao.DataLocacao = DateTime.Now;
            locacao.DataPrazoEntrega = DateTime.Now.AddDays(3);
            locacao.Entregue = "N";
            PreencheTelaLocacao(locacao);
            Console.Write("Digite o código do cliente: ");
            locacao.CodigoCliente = int.Parse(Console.ReadLine());
            locacao.Nome = clientes.Where(x => x.Id == locacao.CodigoCliente).Select(y => y.Nome).FirstOrDefault();
            if (verificaCliente(clientes, locacao.CodigoCliente, "n\\a"))
            {
                Console.Clear();
                PreencheTelaLocacao(locacao);
                Console.Write("Digite o código do filme: ");
                locacao.CodigoFilme = int.Parse(Console.ReadLine());
                locacao.Titulo = filmes.Where(x => x.Id == locacao.CodigoFilme).Select(y => y.Titulo).FirstOrDefault();
                if(verificaFilme(filmes, locacao.CodigoFilme))
                {
                    Console.Clear();
                    PreencheTelaLocacao(locacao);
                    verificaCliente(clientes, locacao.CodigoCliente, locacao.Titulo);
                    try
                    {
                    
                        using (StreamWriter writer = File.Exists("locacoes.csv") ? File.AppendText("locacoes.csv") : new StreamWriter( "locacoes.csv"))
                        {
                            writer.WriteLine($"{locacao.CodigoCliente},{locacao.Nome},{locacao.CodigoFilme},{locacao.Titulo},{locacao.DataLocacao},{locacao.DataPrazoEntrega},{locacao.Entregue}");
                            Console.WriteLine("Locação cadastrada com sucesso!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Não há mais cópias disponíveis!");
                }
            }
            else
            {
                Console.WriteLine("Cliente excedeu o número de 2 Títulos locados ou não foi encontrado!");
            }
            
        }


        public void CadDevolucao(List<Locacao> locacoes, List<Filme> filmes, List<Cliente> clientes)
        {
            Console.Write("Digite o código do cliente: ");
            int idCliente = int.Parse(Console.ReadLine());
            var filtroLoc = locacoes.Where(x => x.CodigoCliente == idCliente && x.Entregue == "N").ToList();

            if (filtroLoc != null)
            {
                foreach (var locacao in filtroLoc)
                {
                    PreencheTelaLocacao(locacao);
                    Console.WriteLine("\r\n");
                }
                Console.Write("Digite o código do filme a ser devolvido: ");
                int CodigoFilme = int.Parse(Console.ReadLine());
                try
                {
                    if(verificaLocacao(locacoes, idCliente, CodigoFilme))
                    {

                        var filme = filmes.Where(z => z.Id == CodigoFilme).FirstOrDefault();
                        filme.Unidades += 1;
                        AtualizarArquivo("filmes", AlteraFilmes(filmes));
                        var cliente = clientes.Where(x=>x.Id == idCliente).FirstOrDefault();
                        if(cliente.Titulo1 == filme.Titulo)
                        {
                            cliente.Titulo1 = "n\\a";
                        }
                        else if(cliente.Titulo2 == filme.Titulo)
                        {
                            cliente.Titulo2 = "n\\a";
                        }
                        AtualizarArquivo("clientes", AlteraClientes(clientes));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Não existem locações cadastradas para este cliente!");
            }

        }
    }


    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string CidadeEstado { get; set; }
        public string CEP { get; set; }
        public string Documento { get; set; }
        public string Titulo1 { get; set; }
        public string Titulo2 { get; set; }
    }

    public class Filme
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public int Unidades { get; set; }
    }

    public class Locacao
    {
        public int CodigoCliente { get; set; }
        public string Nome {  set; get; }
        public int CodigoFilme { get; set; }
        public string Titulo { get; set; }
        public DateTime DataLocacao { get; set; }
        public DateTime DataPrazoEntrega { get; set; }
        public string Entregue { get; set; }
    }
}
