using MobaServer.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobaServer.GameModule
{
    public class UserModule:GameModuleBase<UserModule>
    {
        public override void AddListener()
        {
            NetEvent.Instance.AddEventListener(1000, HandleUserRegisterC2S);
            NetEvent.Instance.AddEventListener(1000, HandleUserLoginC2S);
        }


        private void HandleUserLoginC2S(BufferEntity request)
            {

            }

            private void HandleUserRegisterC2S(BufferEntity request)
            {
        }

        public override void Init()
        {
            AddListener();
      
        }

     

        public override void Release()
        {
            RemoveListener();
          
        }
       
       
        public override void RemoveListener()
        {
            NetEvent.Instance.RemoveEventListener(1000, HandleUserRegisterC2S);
            NetEvent.Instance.RemoveEventListener(1000, HandleUserLoginC2S);
        }
    }
}
