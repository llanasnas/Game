using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        private static IPAddress ServerIP;
        private static int ClientPort;
        private static NetworkStream ClientNS;
        private static TcpClient Client = new TcpClient();

        static void Main(string[] args)
        {
                ClientPort = 50000;
                ServerIP = IPAddress.Parse("127.0.0.1");
                Thread rebredades = new Thread(rebreDades);    
                Client.Connect(ServerIP, ClientPort);
            if (Client.Connected)
            {
                rebredades.Start();
                Thread enviardades = new Thread(enviarDades);
                enviardades.Start();
            }
            else {
                Console.WriteLine("No se puede establecer la conexion con el servidor");
            }
                

        }

        public static void rebreDades() {

            while (true)
            {
                byte[] BufferLocal = new byte[256];
                try
                {
                    int BytesRebuts = ClientNS.Read(BufferLocal, 0, BufferLocal.Length);
                    string s = "";
                    //Passem de bytes a string
                    s = Encoding.UTF8.GetString(BufferLocal, 0, BytesRebuts);

                    Console.WriteLine("{0}", s);
                    
                }
                catch (System.NullReferenceException) {
                }
                
            }
        }

        public static void enviarDades() {

            while (true)
            {

                ClientNS = Client.GetStream();

                Console.WriteLine("Escriu un missatge:");
                string frase = Console.ReadLine();

                //Passem de string a bytes
                byte[] fraseBytes = Encoding.UTF8.GetBytes(frase);

                //Enviem al servidor
                ClientNS.Write(fraseBytes, 0, fraseBytes.Length);
            }
        }

    }
}

