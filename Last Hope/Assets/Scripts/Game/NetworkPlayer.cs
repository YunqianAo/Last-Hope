using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    public int playerID;

    public void ApplyServerPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
