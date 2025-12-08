using System.Collections.Generic;
using UnityEngine;
using ProtoMsg;
using Game.Net;   // BufferEntity / NetEvent 的命名空间

/// <summary>
/// 战斗场景管理器：生成玩家，处理1500同步消息
/// Battle scene manager: spawn players, handle 1500 sync messages
/// </summary>
public class BattleSceneManager : MonoBehaviour
{
    public static BattleSceneManager Instance;

    public GameObject playerPrefab;

    // RolesID -> 远程玩家控制 / remote players
    private Dictionary<int, RemotePlayerController> remotePlayers
        = new Dictionary<int, RemotePlayerController>();

    void Awake()
    {
        Instance = this;
        // 注册1500消息 / listen to 1500 messages
        NetEvent.Instance.AddEventListener(1500, HandleBattleUserInputS2C);
    }

    void OnDestroy()
    {
        if (NetEvent.Instance != null)
            NetEvent.Instance.RemoveEventListener(1500, HandleBattleUserInputS2C);
    }

    void Start()
    {
        if (playerPrefab == null)
        {
            playerPrefab = Resources.Load<GameObject>("Player/Player");
        }

        SpawnAllPlayers();
    }

    void SpawnAllPlayers()
    {
        // 简单出生点数组 / simple spawn points
        Vector3[] spawnPos =
        {
            new Vector3(-3,1,0),
            new Vector3(3,1,0),
            new Vector3(-5,1,0),
            new Vector3(5,1,0),
            new Vector3(-7,1,0),
            new Vector3(7,1,0),
        };

        int i = 0;
        foreach (var roles in BattleData.AllPlayers)
        {
            Vector3 pos = spawnPos[i % spawnPos.Length];
            i++;

            GameObject obj = Instantiate(playerPrefab, pos, Quaternion.identity);

            if (roles.RolesID == BattleData.LocalRolesID)
            {
                // 本地玩家 / local player
                var local = obj.AddComponent<LocalPlayerController>();
                local.RolesID = roles.RolesID;
                local.RoomID = BattleData.RoomID;
            }
            else
            {
                // 远程玩家 / remote player
                var remote = obj.AddComponent<RemotePlayerController>();
                remote.RolesID = roles.RolesID;
                remotePlayers[roles.RolesID] = remote;
            }
        }
    }

    // 收到1500的S2C时调用 / called when 1500 S2C is received
    void HandleBattleUserInputS2C(BufferEntity buffer)
    {
        BattleUserInputS2C s2c = ProtobufHelper.FromBytes<BattleUserInputS2C>(buffer.proto);
        var cmd = s2c.CMD;

        // 我们只用Key=1的位置同步 / we only use Key=1 as position sync
        if (cmd.Key != 1) return;

        int rid = cmd.RolesID;
        var p = cmd.MousePosition;
        Vector3 pos = new Vector3(p.X, p.Y, p.Z);

        // 自己的消息本地已经移动了，不需要再用 / ignore self
        if (rid == BattleData.LocalRolesID) return;

        RemotePlayerController ctrl;
        if (remotePlayers.TryGetValue(rid, out ctrl))
        {
            ctrl.SetTargetPosition(pos);
        }
    }
}
