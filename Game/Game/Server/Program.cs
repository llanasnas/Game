using GameClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Player
    {
        public Player(int idPlayer, NetworkStream networkPlayer)
        {
            this.idPlayer = idPlayer;
            this.networkPlayer = networkPlayer;
        }

        public int idPlayer { get; set; }
        public NetworkStream networkPlayer { get; set; }
    }

    class Program
    {
        private static IPAddress ServerIP = IPAddress.Parse("127.0.0.1");
        private static int PortIP;
        private static IPEndPoint ServerEndPoint;
        private static int ServerPort = 50000;
        private static TcpListener Server = new TcpListener(ServerIP, ServerPort);
        private static List <Player> playerList = new List<Player>();

        static void Main(string[] args)
        {
            Console.WriteLine("Servidor creat");
            Server.Start();
            Console.WriteLine("Servidor iniciat");
            Thread auto = new Thread(autoAccept);
            auto.Start();

            Console.ReadLine();
        }
        static void autoAccept()
        {
            while (true)
            {
                TcpClient Client = Server.AcceptTcpClient();
                Console.WriteLine("Client connectat");
                Thread resp = new Thread(response);
                resp.Start(Client);
            }
        }
        private static object Deserialize(byte[] param)
        {
            object obj = null;
            using (MemoryStream ms = new MemoryStream(param))
            {
                IFormatter br = new BinaryFormatter();
                obj = (br.Deserialize(ms) as Position);
            }

            return obj;
        }
        static void response(Object objecte)
        {
            //Rebem dades del client
            TcpClient Client = (TcpClient)objecte;
            NetworkStream ServerNS = Client.GetStream();

            Player player = new Player(playerList.Count(), ServerNS);
            playerList.Add(player);
            
            try
            {
                while (true)
                {
                    byte[] BufferLocal = new byte[256];
                    int BytesRebuts = ServerNS.Read(BufferLocal, 0, BufferLocal.Length);

                    //Passem de bytes a string
                    string s = "Usuario "+player.idPlayer+":";
                    s += Encoding.UTF8.GetString(BufferLocal, 0, BytesRebuts);

                    Position position;
                    position = (Position)Deserialize(BufferLocal);
                    //Passem de string a bytes
                    byte[] fraseBytes = Encoding.UTF8.GetBytes(s);
                    Console.WriteLine("posicio Y: " + position.PosY + " Posicio X:" + position.PosX);
                    //Enviem resposta al client
                   /* foreach (Player p in playerList)
                    {

                        p.networkPlayer.Write(fraseBytes, 0, fraseBytes.Length);
                        
                    }
                    */
                }             


            }
            catch (System.IO.IOException)
            {

            }
            finally
            {
                ServerNS.Close();
                Client.Close();
            }
        }
    }
}
