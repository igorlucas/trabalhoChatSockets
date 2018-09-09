using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketServer   
{

    //Dados recebidos de um cliente.  
    public static string data = null;

    public static void StartListening(Chat chat)
    {
        string IP = "127.0.0.1";
        int Port = 2000;
        int MaxConnections = 10;


        //Buffer para receber dados.  
        byte[] ArrayBytes = new Byte[1024];

        //Definindo endereço local para o socket.  
        IPAddress ipAddress = IPAddress.Parse(IP);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Port);

        //Criando um socket TCP/IP.  
        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        //Ligando o socket ao endereço local e aguardando entrada de conexões   
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(MaxConnections);
            Console.WriteLine($"Servidor rodando no endereço: {listener.LocalEndPoint}");
            //Inicia escuta de conexões.  
            while (true)
            {
                Console.WriteLine("Aguardando por uma conexão...");

                Socket handler = listener.Accept();//O programa é suspenso enquanto aguarda por uma conexão de entrada.
                Console.WriteLine($"Uma conexão realizada pelo endereço: {handler.RemoteEndPoint}");

                string command = "";
                //Processando entradas  da conexão.
                while (true)
                {
                    data = null;

                    int bytesRec = handler.Receive(ArrayBytes);//O programa fica suspenso aguardando dados de uma conexão
                    data += Encoding.ASCII.GetString(ArrayBytes, 0, bytesRec);

                    if (command == "CadastrarUsuario")
                    {
                        //data vem com os dados de registro de usuario
                        var dataUser = data.Split(':');
                        //verificar se já existe um cadastro de usuario com os mesmos parametros ##PENDENTE)##
                        var user = new User(dataUser[0], dataUser[1], dataUser[2]);
                        chat.users.Add(user);
                        command = "UsuarioCadastrado";
                    }
                    else if (command == "LogarUsuario")
                    {
                        //data vem com os dados de login de usuario
                        var dataUser = data.Split(':');
                        //verificar se existem usuarios cadastrados
                        if (chat.users.Count > 0)
                        {
                            //validar login
                            foreach (var user in chat.users)
                            {
                                if ((user.Name == dataUser[0]) && (user.Email == dataUser[1]))
                                {
                                    command = "UsuarioAutenticado";
                                }
                                else { command = "UsuarioNaoAutenticado"; }
                            }
                        }
                        else
                        {
                            command = "SemUsuarioCadastrado";
                        }
                    }
                    //Menu inicio
                    //Login
                    //Cadastro



                    if (data == "sair")//(data.IndexOf("<EOF>") > -1)
                        break;


                    //Exibindo dados no console.  
                    Console.WriteLine("Dados recebidos : {0}", data);



                    if (data == "2")
                    {
                        command = "CadastrarUsuario";
                    }
                    else if (data == "1")
                    {
                        command = "LogarUsuario";
                    }

                    //Retornado dados ao cliente.
                    byte[] msg = Encoding.ASCII.GetBytes(command);
                    handler.Send(msg);

                }


                //Liberando recursos
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nDigite ENTER para continuar...");
        Console.Read();

    }

    public static int Main(String[] args)
    {
        Chat chat = new Chat();
        StartListening(chat);
        return 0;
    }
}
