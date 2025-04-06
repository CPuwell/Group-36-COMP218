using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] // 允许在编辑器模式下执行该脚本
public class CardRotation : MonoBehaviour
{
    public Transform cardFront; // 卡牌正面的 Transform
    public Transform cardBack;  // 卡牌背面的 Transform
    public Transform targetFacePoint; // 目标检测点（通常设定在卡牌的正面中心）
    public Collider col; // 卡牌的碰撞体（用于射线检测）

    private bool showingBack = false; // 标记当前是否显示卡牌背面

    private void Start()
    {
        col = GetComponent<Collider>(); // 获取当前物体的 Collider
    }

    private void Update()
    {
        // 存储射线检测到的所有碰撞信息
        RaycastHit[] hits;
        hits = Physics.RaycastAll(
            Camera.main.transform.position, // 射线起点（摄像机位置）
            (-Camera.main.transform.position + targetFacePoint.position).normalized, // 射线方向（指向 targetFacePoint）
            (-Camera.main.transform.position + targetFacePoint.position).magnitude // 射线长度（到 targetFacePoint 的距离）
        );

        bool passedThrough = false; // 记录射线是否穿过了卡牌

        // 遍历所有射线检测到的碰撞体
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col) // 如果射线击中了当前卡牌
                passedThrough = true;
        }

        // 如果检测到的状态与当前状态不同，则进行切换
        if (passedThrough != showingBack)
        {
            showingBack = passedThrough; // 更新当前状态

            if (showingBack)
            {
                cardFront.gameObject.SetActive(false); // 隐藏正面
                cardBack.gameObject.SetActive(true);  // 显示背面
            }
            else
            {
                cardFront.gameObject.SetActive(true); // 显示正面
                cardBack.gameObject.SetActive(false); // 隐藏背面
            }
        }
    }
}
