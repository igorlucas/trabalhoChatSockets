using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Cliente";
            Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint connect = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2000);
            listen.Connect(connect);

            string dados = "";
            while (dados.ToLower() != "sair")
            {
                Console.WriteLine("Digite alguma coisa!");
                dados = Console.ReadLine();
                byte[] mensagemEnviada = new byte[100];
                mensagemEnviada = Encoding.Default.GetBytes(dados);
                listen.Send(mensagemEnviada);
            }




            //Console.ReadKey();
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
