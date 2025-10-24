using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobaServer.Net
{
    public class BufferFactory
    {
        enum MessageType
        {
            ACK = 0,
            Login = 1,
        }
        public static BufferEntity CreateAndSendPackage(UClient uClient,int messageID,IMessage message)
        {
            if (uClient.isConnect)
            {
                BufferEntity bufferEntity=new BufferEntity(uClient.endPoint,uClient.session,0,0,MessageType.Login.GetHashCode(),messageID,ProtobufHelper.ToBytes(message));
                uClient.Send(bufferEntity);
                return bufferEntity;    

            }
            return null;
        }
    }
}
