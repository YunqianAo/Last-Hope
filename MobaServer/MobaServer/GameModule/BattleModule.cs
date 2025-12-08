using MobaServer.Net;
using MobaServer.Player;
using MobaServer.Room;
using ProtoMsg;
using Google.Protobuf;

namespace MobaServer.GameModule
{
    /// <summary>
    /// 战斗模块：处理1500战斗输入，并广播给房间内所有玩家
    /// Battle module: handle 1500 input and broadcast to all players in room
    /// </summary>
    class BattleModule : GameModuleBase<BattleModule>
    {
        public override void AddListener()
        {
            base.AddListener();
            // 监听1500 / listen to 1500
            NetEvent.Instance.AddEventListener(1500, HandleBattleUserInputC2S);
        }

        // 处理客户端发来的输入 / handle C2S
        private void HandleBattleUserInputC2S(BufferEntity request)
        {
            // 反序列化 / deserialize
            BattleUserInputC2S c2s = ProtobufHelper.FromBytes<BattleUserInputC2S>(request.proto);

            // 根据session找到玩家 / find player by session
            PlayerEntity player = PlayerManager.GetPlayerEntityFromSession(request.session);
            if (player == null) return;

            // 找到房间 / find room
            RoomEntity room = player.roomEntity;
            if (room == null) return;

            // 构造S2C / build S2C
            BattleUserInputS2C s2c = new BattleUserInputS2C();
            s2c.CMD = c2s;

            // 广播给房间所有玩家 / broadcast to all players in room
            room.Broadcast(1500, s2c);
        }
    }
}
