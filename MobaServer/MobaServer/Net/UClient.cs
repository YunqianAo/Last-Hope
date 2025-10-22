using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MobaServer.Net
{
    internal class UClient
    {
        private USocket uSocket;
        private IPEndPoint endPoint;
        private int v1;
        private int v2;
        private int session;
        private Action<BufferEntity> dispatchNetEvent;

        public UClient(USocket uSocket, IPEndPoint endPoint, int v1, int v2, int session, Action<BufferEntity> dispatchNetEvent)
        {
            this.uSocket = uSocket;
            this.endPoint = endPoint;
            this.v1 = v1;
            this.v2 = v2;
            this.session = session;
            this.dispatchNetEvent = dispatchNetEvent;
        }

        internal void Close()
        {
            throw new NotImplementedException();
        }

        internal void Handle(BufferEntity bufferEntity)
        {
            throw new NotImplementedException();
        }
    }
}
