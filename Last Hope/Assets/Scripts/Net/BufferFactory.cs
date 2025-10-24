using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Net
{
    public class BufferFactory 
    {
        enum MessageType{
            ACK=0,
            Login=1,
        }
        public static BufferEntity CreateAndSendPackage(int messageID, IMessage message)
        {
            Debug.Log($"protoID{messageID}\n{JsonHelper.SerializeObject(message)}");
            BufferEntity buffer=new BufferEntity(USocket.local.endPoint,USocket.local.sessionID,0,0,MessageType.Login.GetHashCode(),messageID,ProtobufHelper.ToBytes(message));
            USocket.local.Send(buffer); 
            return buffer;
        }
    }
}
