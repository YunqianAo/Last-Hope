using UnityEngine;
using ProtoMsg;      // 你的 proto 命名空间
using Game.Net;     // 你 UClient / BufferFactory 的命名空间

/// <summary>
/// 本地玩家：在 PlayerMovement 的基础上，定时把自己位置发给服务器
/// Local player: extends PlayerMovement, periodically send position to server
/// </summary>
public class LocalPlayerController : PlayerMovement
{
    public int RolesID;  // 当前角色ID / current role ID
    public int RoomID;   // 当前房间ID / current room ID

    private float sendInterval = 0.1f; // 每0.1秒发送一次 / send every 0.1s
    private float timer = 0f;

    protected override void Update()
    {
        // 先做本地移动 / local movement first
        base.Update();

        // 再定时同步位置 / then sync position periodically
        timer += Time.deltaTime;
        if (timer >= sendInterval)
        {
            timer = 0f;
            SendPositionToServer();
        }
    }

    void SendPositionToServer()
    {
        BattleUserInputC2S c2s = new BattleUserInputC2S();
        c2s.RolesID = RolesID;
        c2s.RoomID = RoomID;

        // Key=1 表示“位置同步” / Key=1 means "position sync"
        c2s.Key = 1;

        // 用 MousePosition 字段存位置 / reuse MousePosition as position
        c2s.MousePosition = new V3Info
        {
            X = (int)transform.position.x,
            Y = (int)transform.position.y,
            Z = (int)transform.position.z
        };

        // 1500 已经在 PBConfig 里映射为 BattleUserInputC2S/S2C
        BufferFactory.CreateAndSendPackage(1500, c2s);
    }
}
