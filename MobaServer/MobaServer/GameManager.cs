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
            // 用户模块：注册 1000 / 1001（注册、登录）
            // User module: handle 1000 / 1001 (register / login)
            UserModule.Instance.Init();

            // 角色模块：注册 1201（创建角色等）
            // Roles module: handle 1201 (create role, etc.)
            RolesModule.Instance.Init();

            // 大厅模块：注册 1300 / 1302（进入匹配、退出匹配）
            // Lobby module: handle 1300 / 1302 (enter / quit match)
            LobbyModule.Instance.Init();

            // 房间模块：注册 1400+（选英雄、进战斗等房间消息）
            // Room module: handle 1400+ (room / hero select / battle)
            RoomModule.Instance.Init();
            // 新增战斗模块 / add battle module
            BattleModule.Instance.Init();
        }
    }
}
