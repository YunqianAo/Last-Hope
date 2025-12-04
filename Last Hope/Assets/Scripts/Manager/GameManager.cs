using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Net;

public class GameManager : MonoBehaviour
{
    public static USocket uSocket;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        uSocket = new USocket(DispatchNetEvent);
        WindowManager.Instance.OpenWindow(WindowType.LoginWindow);
    }

    // Update is called once per frame
    void Update()
    {
        if (uSocket != null)
        {
            uSocket.Handle();
        }
    }
    void DispatchNetEvent(BufferEntity buffer)
    {
        NetEvent.Instance.Dispatch(buffer.messageID, buffer);
    }
}
