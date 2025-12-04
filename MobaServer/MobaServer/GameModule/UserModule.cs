using MobaServer.MySql;
using MobaServer.Net;
using ProtoMsg;
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
            UserLoginC2S c2sMSG = ProtobufHelper.FromBytes<UserLoginC2S>(request.proto);
            string sqlCMD = MySqlCMD.Where("Account", c2sMSG.UserInfo.Account) +
                MySqlCMD.And("Password", c2sMSG.UserInfo.Password);

            UserLoginS2C s2cMSG = new UserLoginS2C();
            UserInfo userInfo = DBUserInfo.Instance.Select(sqlCMD);

            if (userInfo != null)
            {
                s2cMSG.UserInfo = userInfo;
                s2cMSG.Result = 0;
                

                RolesInfo rolesInfo = DBRolesInfo.Instance.Select(MySqlCMD.Where("ID", s2cMSG.UserInfo.ID));

                if (rolesInfo != null)
                {
                    s2cMSG.RolesInfo = rolesInfo;
                    
                    
                }

            }
            else
            {
                s2cMSG.Result = 2;
            }
            BufferFactory.CreateAndSendPackage(request, s2cMSG);
        }

        private void HandleUserRegisterC2S(BufferEntity request)
        {
            UserRegisterC2S c2sMSG = ProtobufHelper.FromBytes<UserRegisterC2S>(request.proto);
            UserRegisterS2C s2cMSG = new UserRegisterS2C();
            if (DBUserInfo.Instance.Select(MySqlCMD.Where("Account", c2sMSG.UserInfo.Account)) != null)
            {
                Debug.Log("The account has been registered");
                s2cMSG.Result = 3;
            }
            else
            {
                bool result = DBUserInfo.Instance.Insert(c2sMSG.UserInfo);
                if (result == true)
                {
                    s2cMSG.Result = 0;//注册成功
                }
                else
                {
                    s2cMSG.Result = 4;//未知原因导致的失败
                }
            }

            //返回结果
            BufferFactory.CreateAndSendPackage(request, s2cMSG);
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
