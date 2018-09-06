using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            RunUdpServer();
            Process.GetCurrentProcess().WaitForExit();
        }

        private static void RunUdpServer()
        {
            Console.Title = "SERVIDOR UDP";
            //Criando um servidor UDP
            string serverIp = "127.0.0.1";
            int serverPort = 2000;
            IPEndPoint connectionServer = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            UdpClient socketServerUdp = new UdpClient(connectionServer);


            //Exibindo informações
            Console.WriteLine($"Servidor UDP escutando no endereço: {serverIp}:{serverPort}");
            Console.WriteLine("-------------------------------------------------------------\n");

            //Inicia recebimento assincrono de dados
            socketServerUdp.BeginReceive(DataReceived, socketServerUdp);
        }

        private static void RunTcpServer()
        {
            //Criando um host servidor
            Console.Title = "Servidor";
            Socket connectionServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket connectionRemote;
            IPEndPoint ip_porta = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2000);

            //Lingando o socket servidor a um endereço porta ip
            connectionServer.Bind(ip_porta);
            //Definindo o numero máximo de conexões
            connectionServer.Listen(10);
            Console.WriteLine("Servidor escutando");

            connectionRemote = connectionServer.Accept();
            Console.WriteLine("Um novo usuário acaba de entrar");

            int array_size = 0;
            byte[] mensagemRecebida = new byte[1000];
            array_size = connectionRemote.Receive(mensagemRecebida, 0, mensagemRecebida.Length, 0);
            while (connectionRemote.Connected)
            {
                var dados = Encoding.Default.GetString(mensagemRecebida);
                Console.WriteLine("A informação recebida é: {0}", dados);
            }
        }

        private static void DataReceived(IAsyncResult result)
        {
            UdpClient socketClientUdp = (UdpClient)result.AsyncState;
            IPEndPoint connectionClient = new IPEndPoint(IPAddress.Any, 0);
            byte[] mensagemRecebida = socketClientUdp.EndReceive(result, ref connectionClient);

            //Converte o array de buffer em string  
            string dados = ASCIIEncoding.ASCII.GetString(mensagemRecebida);
            Console.Write(connectionClient + " disse: " + dados + Environment.NewLine);

            //Reinicia recebimento de dados
            socketClientUdp.BeginReceive(DataReceived, result.AsyncState);
        }

    }
}
