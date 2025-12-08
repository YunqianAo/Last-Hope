using UnityEngine;

/// <summary>
/// 远程玩家：只根据服务器同步过来的位置插值移动
/// Remote player: only interpolates to server-synced position
/// </summary>
public class RemotePlayerController : MonoBehaviour
{
    public int RolesID;

    private Vector3 targetPos;
    private float lerpSpeed = 10f;

    void Start()
    {
        targetPos = transform.position;
    }

    public void SetTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}
