using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoMsg;
using Game.Net;

public class ProtoTest : MonoBehaviour
{
    USocket uSocket;
    // Start is called before the first frame update
    void Start()
    {
        uSocket=new USocket(DispatchNetEvent);
        UserInfo userInfo = new UserInfo();
        userInfo.Account = "11111";
        userInfo.Password = "password";

        UserRegisterC2S userRegisterC2S = new UserRegisterC2S();
        userRegisterC2S.UserInfo = userInfo;
        BufferEntity bufferEntity=BufferFactory.CreateAndSendPackage(1001, userInfo);
        //UserRegisterC2S userRegisterC2S1 = ProtobufHelper.FromBytes<UserRegisterC2S>(bufferEntity.proto);
    }

    // Update is called once per frame
    void Update()
    {
        if (uSocket != null)
        {
            uSocket.Handle();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            
            UserInfo userInfo = new UserInfo();
            userInfo.Account = "11111";
            userInfo.Password = "password";

            UserRegisterC2S userRegisterC2S = new UserRegisterC2S();
            userRegisterC2S.UserInfo = userInfo;
            BufferEntity bufferEntity = BufferFactory.CreateAndSendPackage(1001, userInfo);
            //UserRegisterC2S userRegisterC2S1 = ProtobufHelper.FromBytes<UserRegisterC2S>(bufferEntity.proto);
        }
    }
    void DispatchNetEvent(BufferEntity buffer)
    {



    }
}
