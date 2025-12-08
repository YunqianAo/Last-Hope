using System.Collections.Generic;
using ProtoMsg;

/// <summary>
/// 战斗前从房间界面带到战斗场景的数据
/// Data passed from RoomWindow to battle scene
/// </summary>
public static class BattleData
{
    public static int RoomID;
    public static List<RolesInfo> AllPlayers = new List<RolesInfo>();
    public static int LocalRolesID;
}
