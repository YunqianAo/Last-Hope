using MobaServer.Net;
using System;

namespace MobaServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            NetSystemInit();

            Console.ReadLine(); 
        }
        public static USocket uSocket;
        static void NetSystemInit()
        {
            uSocket = new USocket(DispatchNetEvent);
            Debug.Log("Net System Initial Complete");
        }
        static void DispatchNetEvent(BufferEntity buffer) { 

        
        
        }
    }
}
