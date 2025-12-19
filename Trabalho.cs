using System;
using System.IO;
public class Trabalho
{
    // Vetor que armazena os códigos dos voos
    static string[] voo = new string[3];

    // Vetor que armazena os destinos de cada voo
    static string[] destino = new string[3];

    // Vetor que guarda quantidade de assentos disponíveis por voo
    static int[] assentos = new int[3];

    // Matriz de reservas: cada linha é um voo e cada coluna um assento
    static string[,] reservas = new string[3, 50];


    // ===============================
    // IMPORTAR DADOS DO ARQUIVO
    // ===============================
    public static void Importar()
    {
        StreamReader sr = new StreamReader("C:\\Users\\eduar\\Documents\\trabalho de atp\\Dados.txt");

        string line;
        int p = 0;

        while ((line = sr.ReadLine()) != null)
        {
            // Evita problemas se existir linha vazia no arquivo
            if (line.Trim() == "")
                continue;

            // Evita ultrapassar o tamanho do vetor
            if (p >= voo.Length)
                break;

            // Separa o conteúdo usando ponto e vírgula
            string[] aux = line.Split(';');

            // Preenche os vetores com os dados da linha
            voo[p] = aux[0];
            destino[p] = aux[1];
            assentos[p] = int.Parse(aux[2]);

            p++;
        }

        sr.Close();

        Console.WriteLine("Dados importados!");
    }


    // ===============================
    // MENU DO SISTEMA
    // ===============================
    public static int Menu()
    {
        Console.WriteLine("===== SISTEMA DE RESERVA DE VOOS =====");
        Console.WriteLine("\n===== MENU =====");
        Console.WriteLine("1 - Importar Dados");
        Console.WriteLine("2 - Realizar reserva");
        Console.WriteLine("3 - Cancelar reserva");
        Console.WriteLine("4 - Consultar assentos disponíveis");
        Console.WriteLine("5 - Relatório de ocupação");
        Console.WriteLine("6 - Sair");
        Console.Write("Opção: ");

        // Retorna a opção escolhida pelo usuário
        return int.Parse(Console.ReadLine());
    }


    // ===============================
    // CONTROLE DE EXECUÇÃO DO PROGRAMA
    // ===============================
    public static void Rodar()
    {
        int op;

        // Mantém o programa executando até o usuário escolher sair
        do
        {
            op = Menu();

            switch (op)
            {
                case 1:
                    Importar();
                    break;

                case 2:
                    RealizarReserva();
                    break;

                case 3:
                    CancelarReserva();
                    break;

                case 4:
                    MostrarAssentosDisponiveis();
                    break;

                case 5:
                    RelatorioOcupacao();
                    break;
            }

            Console.WriteLine();

        } while (op != 6);
    }


    // ===============================
    // PROCURA INDICE DE VOO PELO CÓDIGO
    // ===============================
    public static int ProcurarVoo(string codigoVoo)
    {
        // Percorre vetor de voos procurando coincidência
        for (int i = 0; i < voo.Length; i++)
        {
            if (voo[i] == codigoVoo)
            {
                return i; // Se encontrou, retorna posição
            }
        }

        return -1; // Se não existir, retorna -1
    }


    // ===============================
    // REALIZA RESERVA DE ASSENTO
    // ===============================
    public static void RealizarReserva()
    {
        Console.Write("Digite o código do voo: ");
        string codigoVoo = Console.ReadLine();

        // Descobre qual linha da matriz pertence ao voo digitado
        int indiceVoo = ProcurarVoo(codigoVoo);

        if (indiceVoo == -1)
        {
            Console.WriteLine("O voo não foi encontrado.");
            return;
        }

        Console.Write("Digite o número do assento (1 a 50): ");
        int numeroAssento = int.Parse(Console.ReadLine());

        // Verifica se número de assento é válido
        if (numeroAssento < 1 || numeroAssento > 50)
        {
            Console.WriteLine("Número de assento inválido.");
            return;
        }

        // Verifica se o assento já está reservado
        if (reservas[indiceVoo, numeroAssento - 1] != null)
        {
            Console.WriteLine("Assento já reservado.");
            return;
        }

        Console.Write("Nome do passageiro: ");
        string nomePassageiro = Console.ReadLine();

        // Marca assento na matriz
        reservas[indiceVoo, numeroAssento - 1] = nomePassageiro;

        assentos[indiceVoo]--; // reduz lugares disponíveis

        Console.WriteLine("Reserva realizada com sucesso.");
    }


    // ===============================
    // CANCELA UMA RESERVA EXISTENTE
    // ===============================
    public static void CancelarReserva()
    {
        Console.Write("Digite o código do voo: ");
        string codigoVoo = Console.ReadLine();

        int indiceVoo = ProcurarVoo(codigoVoo);

        if (indiceVoo == -1)
        {
            Console.WriteLine("O voo não foi encontrado.");
            return;
        }

        Console.Write("Digite o número do assento (1 a 50): ");
        int numeroAssento = int.Parse(Console.ReadLine());

        // Se o assento estiver vazio, não existe reserva para cancelar
        if (reservas[indiceVoo, numeroAssento - 1] == null)
        {
            Console.WriteLine("Assento disponível! Não há reserva para cancelar.");
            return;
        }

        // Libera o assento
        reservas[indiceVoo, numeroAssento - 1] = null;

        assentos[indiceVoo]++; // devolve assento ao total disponível

        Console.WriteLine("Reserva cancelada com sucesso.");
    }


    // ===============================
    // MOSTRA TODOS OS ASSENTOS DISPONÍVEIS POR VOO
    // ===============================
    public static void MostrarAssentosDisponiveis()
    {
        Console.Write("Digite o código do voo: ");
        string codigoVoo = Console.ReadLine();

        int indiceVoo = ProcurarVoo(codigoVoo);

        if (indiceVoo == -1)
        {
            Console.WriteLine("O voo não foi encontrado.");
            return;
        }

        Console.WriteLine($"Assentos disponíveis para o voo {codigoVoo}:");

        // Percorre todas as colunas do voo
        for (int i = 0; i < 50; i++)
        {
            if (reservas[indiceVoo, i] == null)
            {
                Console.Write($"{i + 1} ");
            }
        }

        Console.WriteLine();
    }


    // ===============================
    // RELATÓRIO DE OCUPAÇÃO DO VOO
    // ===============================
    public static void RelatorioOcupacao()
    {
        Console.Write("Código do voo: ");
        string codigoVoo = Console.ReadLine();

        int posicaoVoo = ProcurarVoo(codigoVoo);

        if (posicaoVoo == -1)
        {
            Console.WriteLine("O voo não foi encontrado.");
            return;
        }

        Console.WriteLine($"\nVoo {codigoVoo} - Destino: {destino[posicaoVoo]}");

        // lista nome de cada passageiro ou marca como disponível
        for (int i = 0; i < 50; i++)
        {
            if (reservas[posicaoVoo, i] == null)
            {
                Console.WriteLine($"{i + 1}: Disponível");
            }
            else
            {
                Console.WriteLine($"{i + 1}: Reservado por {reservas[posicaoVoo, i]}");
            }
        }
    }
}
