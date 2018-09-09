using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketClient
{
    public static void StartClient()
    {
        //Buffer de dados para ser enviados.  
        byte[] bytes = new byte[1024];

        //Conectando a um dispositivo remoto.  
        try
        {
            //Definindo um endpoint remoto para o socket.  
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2000);

            //Criando um socket TCP/IP.  
            Socket sender = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            //Conectando o socket ao endereço remoto e capturando exceções.  
            try
            {
                sender.Connect(remoteEP);
                Console.WriteLine("Socket conectado ao endpoint {0}",
                    sender.RemoteEndPoint.ToString());


                string response = "";
                string request;
                while (response != "sair")
                {
                    request = "";

                    if (response == "UsuarioNaoAutenticado" || response == "" || response == "MENU" || response == "UsuarioCadastrado" || response == "SemUsuarioCadastrado")
                    {

                        request = InitMenu(response);
                    }
                    else if (response == "CadastrarUsuario")
                    {
                        request = Register();
                    }
                    else if (response == "LogarUsuario")
                    {
                        request = Login();
                    }
                    else if (response== "UsuarioAutenticado")
                    {
                        request = ChatMenu();
                    }

                    //Enviando dados ao dispositivo remoto
                    //Convertendo os dados de uma string em um array de bytes.  
                    byte[] msg = Encoding.ASCII.GetBytes(request);
                    //Enviando dados pelo socket.  
                    int bytesSent = sender.Send(msg);

                    //Recebendo a resposta do dispositivo remoto.  
                    int bytesRec = sender.Receive(bytes);
                    response = Encoding.ASCII.GetString(bytes, 0, bytesRec);



                    //Retorno do servidor
                    //Console.WriteLine("Você disse: {0}",
                    //    data);



                }
                //Liberando o socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                Console.WriteLine("Desconectou");
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine(ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static string ChatMenu()
    {
        string request;
        //mensagem de usuario atenticado temporaria
        Console.Clear();
        Console.WriteLine("     Menu Chat");
        Console.WriteLine("1- Listar salas");
        Console.WriteLine("2- Criar uma sala");
        Console.WriteLine("3- Editar uma sala");
        Console.WriteLine("4- Entrar em uma sala");
        Console.WriteLine("0- SAIR");
        Console.WriteLine("Escolha uma opção:");
        request = Console.ReadLine();
        return request;
    }

    private static string Login()
    {
        string request;
        Console.Clear();
        Console.WriteLine(" Login\n");
        Console.WriteLine("Digite seu nome:");
        var name = Console.ReadLine();
        Console.WriteLine("Digite seu email:");
        var email = Console.ReadLine();
        request = name + ":" + email;
        return request;
    }

    private static string Register()
    {
        string data;
        Console.Clear();
        Console.WriteLine("     Cadastrar usuário");
        Console.WriteLine("Digite seu nome:");
        var name = Console.ReadLine();
        Console.WriteLine("Digite seu e-mail:");
        var email = Console.ReadLine();
        Console.WriteLine("Digite sua idade:");
        var age = Console.ReadLine();
        data = name + ":" + email + ":" + age;
        return data;
    }

    private static string InitMenu(string response)
    {
        string data;
        Console.Clear();
        if (response == "UsuarioCadastrado")
            Console.WriteLine("Usuário cadastrado com sucesso!\n");
        else if (response == "SemUsuarioCadastrado")
        {
            Console.WriteLine("Ainda não existem usuário cadastrados no sistema\n");
        }else if (response== "UsuarioNaoAutenticado")
        {
            Console.WriteLine("Não existe usuário com esses dados cadastrados no sistema!\n");
        }

        Console.WriteLine("     MENU");
        Console.WriteLine("1- Login");
        Console.WriteLine("2- Cadastrar usuário");
        Console.WriteLine("0- SAIR\n");
        Console.WriteLine("Escolha uma opção:");
        data = Console.ReadLine();
        return data;
    }

    public static int Main(String[] args)
    {
        StartClient();
        Console.ReadKey();
        return 0;
    }
}