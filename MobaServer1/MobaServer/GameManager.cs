using MobaServer.GameModule;
using MobaServer.Net;
using System;

namespace MobaServer
{
    internal class GameManager
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            GameModuleInit();
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

        NetEvent.Instance.Dispatch(buffer.messageID, buffer);
        
        }
        static void GameModuleInit()
        {
            UserModule.Instance.Init();
        }
    }
}
