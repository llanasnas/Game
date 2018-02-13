using GameClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Game
{
    public class Ball
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public BallGraphics BallDraw;

        public Ball (int posX, int posY, int width, int height, Color color)
        {
            PosX = posX;
            PosY = posY;

            BallDraw = new BallGraphics(width, height, color);
        }
    }

    public class BallGraphics
    {
        public Ellipse ShapeBall { get; set; }
        public Color Color { get; set; }

        public BallGraphics(int width, int height, Color color)
        {
            ShapeBall = new Ellipse();
            ShapeBall.Width = width;
            ShapeBall.Height = height;
            Color = color;
            ShapeBall.Fill = new SolidColorBrush(Color);

        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dTimer = new DispatcherTimer();
        int NumBalls = 0;
        List<Ball> Balls = new List<Ball>();
        private static IPAddress ServerIP;
        private static int ClientPort;
        private static NetworkStream ClientNS;
        private static TcpClient Client = new TcpClient();        

        public MainWindow()
        {
            InitializeComponent(); 
            ClientPort = 50000;
            ServerIP = IPAddress.Parse("127.0.0.1");
            Client.Connect(ServerIP, ClientPort);
            CreateBall();
            Loop();
        }

        public void CreateBall()
        {
            //Creem una bola
            Ball ball = new Game.Ball(200, 325, 50, 50, Colors.Red);

            //Dibuixem la bola al taulell
            CanvasBalls.Children.Add(ball.BallDraw.ShapeBall);
            DrawBall(ball);
            
            //Guardem informació de la bola
            Balls.Add(ball);
            NumBalls++;
        }
        public void CreateBall2()
        {
            //Creem una bola
            Ball ball = new Game.Ball(100, 325, 50, 50, Colors.Black);

            //Dibuixem la bola al taulell
            CanvasBalls.Children.Add(ball.BallDraw.ShapeBall);
            DrawBall(ball);

            //Guardem informació de la bola
            Balls.Add(ball);
            NumBalls++;
        }

        public void Loop()
        {
            dTimer.Interval = TimeSpan.FromMilliseconds(30);
            dTimer.Tick += Timer_Tick;
            dTimer.Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            DrawBall(Balls[0]);
        }

        public void DrawBall(Ball infoBall)
        {
            Canvas.SetLeft(infoBall.BallDraw.ShapeBall, infoBall.PosX);
            Canvas.SetTop(infoBall.BallDraw.ShapeBall, infoBall.PosY);
        }
        private static byte[] Serialize(object obj)
        {
            byte[] bytesPos;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                bytesPos = ms.ToArray();
            }

            return bytesPos;
        }

        void CanvasKeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.D:
                    Balls[0].PosX= Balls[0].PosX + 10;
                    break;
                case Key.A:
                    Balls[0].PosX = Balls[0].PosX - 10;
                    break;
                case Key.S:
                    Balls[0].PosY = Balls[0].PosY + 10;
                    break;
                case Key.W:
                    Balls[0].PosY = Balls[0].PosY - 10;
                    break;
                default:                    
                    break;
            }
            Position position = new Position(Balls[0].PosX,Balls[0].PosY);            

            ClientNS = Client.GetStream();            
            //Enviem al servidor
            ClientNS.Write(Serialize(position), 0, Serialize(position).Length);
        }

    }
}
