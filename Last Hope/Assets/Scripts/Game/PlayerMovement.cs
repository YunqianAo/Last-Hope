using UnityEngine;

/// <summary>
/// 本地玩家移动（只管自己）
/// Local player movement (only for local player)
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // 移动速度 / move speed
    public float rotateSpeed = 120f;  // 转向速度 / rotation speed

    protected CharacterController controller;

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    protected virtual void Update()
    {
        // WSAD / 方向键前后 / forward & backward
        float v = Input.GetAxis("Vertical");
        // AD 左右转向 / left & right rotation
        float h = Input.GetAxis("Horizontal");

        Vector3 move = transform.forward * v * moveSpeed;
        controller.SimpleMove(move);

        transform.Rotate(Vector3.up * h * rotateSpeed * Time.deltaTime);
    }
}
