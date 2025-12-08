using UnityEngine;
using Game.Net;

public class SceneGameManager : MonoBehaviour
{
    public static SceneGameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateLocalPlayer();
        NotifyServerEnterScene();
    }

    void CreateLocalPlayer()
    {
        GameObject prefab = Resources.Load<GameObject>("Player/Player");
        GameObject obj = Instantiate(prefab);

        obj.name = "LocalPlayer";
        obj.GetComponent<NetworkPlayer>().playerID = UClient.Instance.sessionID;

        Transform spawn = GameObject.Find("SpawnPoint").transform;
        obj.transform.position = spawn.position;
    }

    void NotifyServerEnterScene()
    {
        // 你已有 UClient.Send()，这里只需要发送即可
        UClient.Instance.SendEnterScene();
    }

    public void CreateRemotePlayer(int id, Vector3 pos)
    {
        GameObject prefab = Resources.Load<GameObject>("Player/Player");
        GameObject obj = Instantiate(prefab);

        obj.name = "RemotePlayer_" + id;

        NetworkPlayer np = obj.GetComponent<NetworkPlayer>();
        np.playerID = id;

        obj.transform.position = pos;
    }
}
