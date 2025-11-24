using Game.Ctrl;
using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class LoginWindow : BaseWindow
    {
        public LoginWindow()
        {
            scenesType=ScenesType.Login;
            resident=false;
            resName = "User/LoginWindow";
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
        InputField AccountInput;
        InputField PwdInput;

        protected override void Awake()
        {
            base.Awake();
            AccountInput = transform.Find("UserBack/AccountInput").GetComponent<InputField>();
            PwdInput = transform.Find("UserBack/PwdInput").GetComponent<InputField>();
        }

        protected override void OnAddListener()
        {
            base.OnAddListener();
            NetEvent.Instance.AddEventListener(1000, HandleUserRegisterS2C);
            NetEvent.Instance.AddEventListener(1000, HandleUserLoginS2C);
        }

        private void HandleUserLoginS2C(BufferEntity p)
        {
            UserLoginS2C s2cMSG = ProtobufHelper.FromBytes<UserLoginS2C>(p.proto);
            switch (s2cMSG.Result)
            {
                case 0:
                    Debug.Log("successfull");
                    if (s2cMSG.RolesInfo != null)
                    {
                        LoginCtrl.Instance.SaveRolesInfo(s2cMSG.RolesInfo);
                    }
                    else
                    {

                    }
                    Close();
                    break;
                case 1:
                    break;
                case 2:
                    Debug.Log("incorrect password");
                    break;
                case 3:
                   
                    break;
                default:
                    break;
            }
        }

        private void HandleUserRegisterS2C(BufferEntity p)
        {
            UserRegisterS2C s2cMSG = ProtobufHelper.FromBytes<UserRegisterS2C>(p.proto);
            switch (s2cMSG.Result)
            {
                case 0:
                    Debug.Log("register successfull");
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    Debug.Log("have existed account");
                    break;
                default:
                    break;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnRemoveListener()
        {
            base.OnRemoveListener();
        }

        protected override void RegisterUIEvent()
        {
            base.RegisterUIEvent();
            for (int i = 0; i < buttonList.Length; i++)
            {
                switch(buttonList[i].name)
                {
                    case "RegisterBtn":
                        buttonList[i].onClick.AddListener(RegisterBtnOnClick);
                        break;
                    case "LoginBtn":
                        buttonList[i].onClick.AddListener(LoginBtnOnClick);
                        break;
                    default:
                        break;

                }
            }
        }

        private void LoginBtnOnClick()
        {
            if (string.IsNullOrEmpty(AccountInput.text))
            {
                Debug.Log("account is empty");
                return;
            }
            if (string.IsNullOrEmpty(PwdInput.text))
            {
                Debug.Log("password is empty");
                return;
            }
            UserLoginC2S c2sMSG = new UserLoginC2S();
            c2sMSG.UserInfo = new UserInfo();
            c2sMSG.UserInfo.Account = AccountInput.text;
            c2sMSG.UserInfo.Password = PwdInput.text;
            BufferFactory.CreateAndSendPackage(1001, c2sMSG);
        }

        private void RegisterBtnOnClick()
        {
            if(string.IsNullOrEmpty(AccountInput.text))
            {
                Debug.Log("account is empty");
                return; 
            }
            if (string.IsNullOrEmpty(PwdInput.text))
            {
                Debug.Log("password is empty");
                return;
            }
            UserRegisterC2S c2sMSG = new UserRegisterC2S();
            c2sMSG.UserInfo=new UserInfo();
            c2sMSG.UserInfo.Account=AccountInput.text;
            c2sMSG.UserInfo.Password=PwdInput.text;
            BufferFactory.CreateAndSendPackage(1000, c2sMSG);
        }
    }

}
