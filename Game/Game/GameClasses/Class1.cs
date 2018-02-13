using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameClasses
{

    
    [Serializable]
    public class Position
    {
        public int PosX { set; get; }
        public int PosY { set; get; }

        public Position(int posX,int posY)
        {
            this.PosX = posX;
            this.PosY = posY;
        }

    }
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

}
