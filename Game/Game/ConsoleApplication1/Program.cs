using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("Iniciar client S/N?");
                String opcio = Console.ReadLine();
                if (opcio.Equals("S"))
                {
                    Process proceso = new Process();
                    proceso.StartInfo.FileName = @"C:\Users\Usuari1\Documents\Visual Studio 2015\Game\Game\Game\Client\bin\Debug\Client.exe";
                    proceso.Start();
                }
            }

        }
    }
}
