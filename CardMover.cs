using UnityEngine;

public class CardMover : MonoBehaviour
{
    public Vector3 targetPosition; // 目标位置（卡牌要移动到的位置）
    public float moveSpeed = 5f;   // 移动速度

    private bool shouldMove = false;

    void Start()
    {
        // 设置目标位置为屏幕中央（假设是 (0, 0, 0)）
        targetPosition = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (shouldMove)
        {
            // 平滑移动卡牌到目标位置
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 当卡牌接近目标位置时停止移动
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;
                shouldMove = false;
            }
        }
    }

    public void MoveCard()
    {
        shouldMove = true; // 开始移动
    }
}
