using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
        void Close() { 
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
        
    }
}
