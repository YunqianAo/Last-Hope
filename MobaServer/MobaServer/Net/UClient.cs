using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Net
{
    internal class UClient
    {
        private USocket uSocket;
        private IPEndPoint endPoint;
        private int sendSN;
        private int handleSN;
        private int session;
        private Action<BufferEntity> dispatchNetEvent;

        public UClient(USocket uSocket, IPEndPoint endPoint, int sendSN, int handleSN, int session, Action<BufferEntity> dispatchNetEvent)
        {
            this.uSocket = uSocket;
            this.endPoint = endPoint;
            this.sendSN = sendSN;
            this.handleSN = handleSN;
            this.session = session;
            this.dispatchNetEvent = dispatchNetEvent;
            CheckOutTime();
        }
        bool isConnect=true;
        int overtime = 150;
        private async void CheckOutTime() { 
        
            await Task.Delay(overtime);
            foreach(var package in sendPackage.Values)
            {
                if (TimeHelper.Now() - package.time >= overtime * 10)
                {
                    Debug.LogError($"repeat over 10 times but fail{package.messageID}");
                    uSocket.RemoveClient(session);
                    return;
                }
                if(TimeHelper.Now()-package.time >= overtime * (package.recurCount + 1))
                {
                    package.recurCount += 1;
                    Debug.Log($"overtime and repeat to send{package.sn}");
                    uSocket.Send(package.buffer, endPoint);
                }
            }
            CheckOutTime();
        }
        internal void Close()
        {
            isConnect = false;
        }

        internal void Handle(BufferEntity buffer)
        {
          int sn=buffer.sn;
            switch(buffer.messageType)
            {
                case 0:
                    BufferEntity buffer1;
                    if(sendPackage.TryRemove(buffer.sn,out buffer1))
                    {
                        Debug.Log($"buffer has been acknowladged{buffer.sn}");
                    }
                    else
                    {
                        Debug.Log($"acknowladged buffer has been removed {buffer.sn}");
                    }
                    break;
                    case 1:
                    BufferEntity ackPackage=new BufferEntity(buffer);
                    uSocket.SendACK(ackPackage,endPoint);
                    HandleLoginPackage(buffer);
                    break;
                    default:
                    break;
            }
        }
        ConcurrentDictionary<int,BufferEntity> waitHandle = new ConcurrentDictionary<int,BufferEntity>();
        private void HandleLoginPackage(BufferEntity buffer)
        {
            if(buffer.sn<=handleSN)
            {
                Debug.Log($"message has been handled{buffer.sn}");
                return;
            }
            if(buffer.sn-handleSN>1)
            {
                if (waitHandle.TryAdd(buffer.sn, buffer))
                {
                    Debug.Log($"incorrect order buffer{buffer.sn}"); 
                }
                return;
            }
            handleSN = buffer.sn;
            if(dispatchNetEvent!=null)
            {
                dispatchNetEvent(buffer);
            }
            BufferEntity nextBuffer;
            if(waitHandle.TryRemove(handleSN+1, out nextBuffer))
            {
                HandleLoginPackage(nextBuffer);
            }
        }

        ConcurrentDictionary<int,BufferEntity> sendPackage=new ConcurrentDictionary<int,BufferEntity>();
        public void Send(BufferEntity package)
        {
            if (isConnect==false)
            {
                return;
            }
            package.time = TimeHelper.Now();
            sendSN += 1;
            package.sn = sendSN;
            package.Encoder(false);
            uSocket.Send(package.buffer,endPoint);
            if(session!=0)
            {
                sendPackage.TryAdd(package.sn, package);
            }
        }
    }
}
