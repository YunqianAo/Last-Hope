using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MobaServer.Net
{
    internal class USocket
    {
        UdpClient socket;
        string ip="192.168.0.144";
        int port = 8899;
        Action<BufferEntity> dispatchNetEvent;
        public USocket(Action<BufferEntity> dispatchNetEvent) { 
             this.dispatchNetEvent = dispatchNetEvent;
            socket = new UdpClient(8899);
            Receive();
            Task.Run(Handle,ct.Token);
        }
        public async void Send(byte[] data,IPEndPoint endPoint)
        {
            if(socket != null)
            {
                try
                {
                    int length = await socket.SendAsync(data, data.Length, endPoint);
                    if (data.Length == length)
                    {

                    }
                }
                catch (Exception e)
                {
                    Close();
                }
            }
        }
        public void SendACK(BufferEntity ackPackage,IPEndPoint endPoint)
        {
            Send(ackPackage.buffer, endPoint);
        }
        ConcurrentQueue<UdpReceiveResult> awaitHandle = new ConcurrentQueue<UdpReceiveResult>();
        public async void Receive()
        {
            if (socket != null)
            {
                try
                {
                    UdpReceiveResult result = await socket.ReceiveAsync();
                    awaitHandle.Enqueue(result);
                    Receive();  
                }
                catch(Exception e) { 
                Close();
                }
            }
        }
        CancellationTokenSource ct= new CancellationTokenSource();  
        int sessionID = 1000;
        async Task Handle()
        {
            while (!ct.IsCancellationRequested)
            {
                if (awaitHandle.Count > 0)
                {
                    UdpReceiveResult data;
                    if (awaitHandle.TryDequeue(out data))
                    {
                        BufferEntity bufferEntity = new BufferEntity(data.RemoteEndPoint, data.Buffer);
                        if (bufferEntity.isFull)
                        {
                            if (bufferEntity.session == 0)
                            {
                                sessionID += 1;
                                bufferEntity.session = sessionID;
                            }
                            UClient targetClient;
                            if (clients.TryGetValue(bufferEntity.session, out targetClient))
                            {
                                targetClient.Handle(bufferEntity);
                            }
                        }

                    }
                }
            }
            

        }
        void Close() { 
            ct.Cancel();
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }
            if (dispatchNetEvent != null)
            {
                dispatchNetEvent=null;
            }
        }
        ConcurrentDictionary<int,UClient> clients= new ConcurrentDictionary<int,UClient>();
        void CreateUClient(BufferEntity buffer)
        {
            UClient client;
            if(!clients.TryGetValue(buffer.session, out client))
            {
                client = new UClient(this,buffer.endPoint,0,0,buffer.session,dispatchNetEvent);
                clients.TryAdd(buffer.session, client);
            }
        }
        public void RemoveClient(int sessionID)
        {
            UClient client;
            if(clients.TryRemove(sessionID, out client))
            {
                client.Close();
                client=null;
            }
        }
        public UClient GetClient(int sessionID)
        {
            UClient client;
            if (clients.TryGetValue(sessionID, out client))
            {
                return client;
            }
            return null;
        }
    }
}
