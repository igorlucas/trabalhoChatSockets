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
            Console.Title = "Servidor";
            Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket conexao;
            IPEndPoint ip_porta = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2000);

            listen.Bind(ip_porta);
            listen.Listen(10);
            Console.WriteLine("Servidor escutando");

            conexao = listen.Accept();
            Console.WriteLine("Conexão aceita");


            
            int array_size = 0;
            byte[] mensagemRecebida = new byte[1000];

            while (conexao.Connected)
            {
                array_size = conexao.Receive(mensagemRecebida, 0, mensagemRecebida.Length, 0);
                Array.Resize(ref mensagemRecebida, array_size);
                var dados = Encoding.Default.GetString(mensagemRecebida);
                Console.WriteLine("A informação recebida é: {0}", dados);
            }


            //Console.ReadKey();
            Process.GetCurrentProcess().WaitForExit();
        }
    }
}
