using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            RunUdpClient();
            Process.GetCurrentProcess().WaitForExit();
        }

        
        private static void RunUdpClient()
        {
            Console.Title = "Cliente UDP";
            //Criando um cliente UDP
            string clientIp = "127.0.0.10";
            int clientPort = 2001;
            IPEndPoint connectionClient = new IPEndPoint(IPAddress.Parse(clientIp), clientPort);
            UdpClient socketClientUdp = new UdpClient(connectionClient);

            //Conexão do host que será enviado a mensagem
            string remoteIp = "127.0.0.1";
            int remotePort = 2000;
            IPEndPoint connectionServer = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);

            //váriavel que recebera os dados digitados
            string dados = "";
            byte[] mensagemEnviada;

            try
            {
                //Estabelece conexão de um cliente com um servidor
                socketClientUdp.Connect(connectionServer);
                while (dados.ToLower() != "sair")//condição de saída do canal
                {
                    Console.WriteLine("Digite alguma coisa!");
                    dados = Console.ReadLine();
                    mensagemEnviada = Encoding.ASCII.GetBytes(dados);
                    socketClientUdp.Send(mensagemEnviada, mensagemEnviada.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private static void RunTcpClient()
        {
            Console.Title = "Cliente";
            Socket socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint connectionServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2000);
            socketClient.Connect(connectionServer);

            string dados = "";
            while (dados.ToLower() != "sair")
            {
                Console.WriteLine("Digite alguma coisa!");
                dados = Console.ReadLine();
                byte[] mensagemEnviada = new byte[100];
                mensagemEnviada = Encoding.Default.GetBytes(dados);
                socketClient.Send(mensagemEnviada);
            }
        }
    }
}
