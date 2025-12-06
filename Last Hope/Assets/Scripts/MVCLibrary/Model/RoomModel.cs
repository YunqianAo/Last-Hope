using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoMsg;
using Google.Protobuf.Collections;

namespace Game.Model
{
    /// <summary>
    /// 保存房间里面的数据
    /// </summary>
    public class RoomModel : Singleton<RoomModel>
    {
       public RepeatedField<PlayerInfo> playerInfos;
     }
}
