using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobaServer.Net
{
    public class UClient
    {
        private USocket uSocket;
        public IPEndPoint endPoint;
        private int sendSN;
        private int handleSN;
        public int session;
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
        public bool isConnect=true;
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

        //internal void Handle(BufferEntity buffer)
        //{
        //  int sn=buffer.sn;
        //    switch(buffer.messageType)
        //    {
        //        case 0:
        //            BufferEntity buffer1;
        //            if(sendPackage.TryRemove(buffer.sn,out buffer1))
        //            {
        //                Debug.Log($"buffer has been acknowladged{buffer.sn}");
        //            }
        //            else
        //            {
        //                Debug.Log($"acknowladged buffer has been removed {buffer.sn}");
        //            }
        //            break;
        //            case 1:
        //            if (buffer.sn != 1)
        //            {
        //                return;
        //            }
        //            BufferEntity ackPackage=new BufferEntity(buffer);
        //            uSocket.SendACK(ackPackage,endPoint);
        //            Debug.Log("receive business message");
        //            HandleLoginPackage(buffer);
        //            break;
        //            default:
        //            break;
        //    }
        //}
        // 处理从客户端收到的一条完整消息
        // Handle one complete message from client
        internal void Handle(BufferEntity buffer)
        {
            // messageType = 0 表示对方发来的 ACK，只用来移除重传队列
            // messageType = 0 means ACK from client, only used to remove resend queue
            switch (buffer.messageType)
            {
                case 0: // ACK
                    BufferEntity removed;
                    if (sendPackage.TryRemove(buffer.sn, out removed))
                    {
                        Debug.Log($"[Server ACK] sn={buffer.sn} removed, id={removed.messageID}");
                    }
                    else
                    {
                        Debug.Log($"[Server ACK] sn={buffer.sn} not found in sendPackage");
                    }
                    break;

                case 1: // 业务消息（需要可靠传输） / business message (reliable)
                        // 先回 ACK，再按顺序处理
                        // first send ACK, then process in order
                    BufferEntity ackPackage = new BufferEntity(buffer);
                    uSocket.SendACK(ackPackage, endPoint);

                    Debug.Log($"[Server Recv] business msg sn={buffer.sn}, id={buffer.messageID}");
                    HandleLoginPackage(buffer);
                    break;

                default:
                    Debug.Log($"[Server] unknown messageType={buffer.messageType}");
                    break;
            }
        }

        ConcurrentDictionary<int,BufferEntity> waitHandle = new ConcurrentDictionary<int,BufferEntity>();
        //private void HandleLoginPackage(BufferEntity buffer)
        //{
        //    if(buffer.sn<=handleSN)
        //    {
        //        Debug.Log($"message has been handled{buffer.sn}");
        //        return;
        //    }
        //    if(buffer.sn-handleSN>1)
        //    {
        //        if (waitHandle.TryAdd(buffer.sn, buffer))
        //        {
        //            Debug.Log($"incorrect order buffer{buffer.sn}"); 
        //        }
        //        return;
        //    }
        //    handleSN = buffer.sn;
        //    if(dispatchNetEvent!=null)
        //    {
        //        Debug.Log("dispatch message for game");
        //        dispatchNetEvent(buffer);
        //    }
        //    BufferEntity nextBuffer;
        //    if(waitHandle.TryRemove(handleSN+1, out nextBuffer))
        //    {
        //        HandleLoginPackage(nextBuffer);
        //    }
        //}
        // 按顺序处理业务消息，保证不乱序
        // Handle business messages in order, ensure no out-of-order
        private void HandleLoginPackage(BufferEntity buffer)
        {
            // 1. 如果 sn 已经处理过，直接丢弃（可能是重复包）
            // 1. If sn already handled, drop it (maybe duplicated)
            if (buffer.sn <= handleSN)
            {
                Debug.Log($"[Server] message already handled sn={buffer.sn}");
                return;
            }

            // 2. 如果这条消息的 sn 大于期待的 sn+1，说明前面有包没到，先缓存起来
            // 2. If sn > handleSN + 1, means some previous packet missing, cache it first
            if (buffer.sn > handleSN + 1)
            {
                if (waitHandle.TryAdd(buffer.sn, buffer))
                {
                    Debug.Log($"[Server] out of order buffer sn={buffer.sn}, wait for sn={handleSN + 1}");
                }
                return;
            }

            // 3. 正常顺序的下一条包（sn == handleSN + 1）
            //    更新 handleSN，并分发给上层游戏逻辑
            // 3. Normal next packet (sn == handleSN + 1), update handleSN and dispatch
            handleSN = buffer.sn;

            if (dispatchNetEvent != null)
            {
                Debug.Log($"[Server] dispatch message sn={buffer.sn}, id={buffer.messageID} to GameManager");
                dispatchNetEvent(buffer);
            }

            // 4. 检查是否有“下一号”的包已经在 waitHandle 里，如果有就递归处理
            // 4. Check if next sn already cached in waitHandle, handle it recursively
            BufferEntity nextBuffer;
            if (waitHandle.TryRemove(handleSN + 1, out nextBuffer))
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
