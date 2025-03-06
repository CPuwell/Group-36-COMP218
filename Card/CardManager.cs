using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public GameObject canvasPrefab; // 预制体：CardCanvas（包含 CardPanel）
    public Transform cardParent; // 存放所有卡牌的父物体
    public List<CardData> allCards; // 存储所有卡牌数据
    private List<GameObject> instantiatedCards = new List<GameObject>(); // 存储生成的卡牌对象

    void Start()
    {
        GenerateCards(); // 生成卡牌
        ShuffleCards();  // ✅ 生成卡牌后立即洗牌
    }

    void GenerateCards()
    {
        foreach (CardData cardData in allCards) // 遍历所有卡牌数据
        {
            // 1⃣ 直接复制 CanvasPrefab，保持所有组件的属性
            GameObject newCanvas = Instantiate(canvasPrefab, cardParent);
            newCanvas.name = "Canvas_" + cardData.cardName; // 设置名称
            instantiatedCards.Add(newCanvas); // ✅ 记录生成的卡牌对象，方便后续洗牌

            // 2⃣ 获取 CardPanel
            Transform cardPanel = newCanvas.transform.Find("CardPanel");
            if (cardPanel == null)
            {
                Debug.LogError("CardCanvas 中找不到 CardPanel，请检查预制体结构！", newCanvas);
                continue;
            }

            // 3⃣ 获取卡牌的前后图片 & 目标检测点
            Transform frontTransform = cardPanel.Find("CardFront");
            Transform backTransform = cardPanel.Find("CardBack");
            Transform targetPoint = cardPanel.Find("TargetPoint");

            if (frontTransform == null || backTransform == null || targetPoint == null)
            {
                Debug.LogError("CardPanel 内找不到 CardFront、CardBack 或 TargetPoint，请检查预制体！", cardPanel);
                continue;
            }

            Image frontImage = frontTransform.GetComponent<Image>();
            Image backImage = backTransform.GetComponent<Image>();

            if (frontImage == null || backImage == null)
            {
                Debug.LogError("CardFront 或 CardBack 没有 Image 组件！");
                continue;
            }

                        // 4⃣ 设置卡牌图片（仅更新 Sprite，不修改其他属性）
            frontImage.sprite = cardData.frontImage;
            backImage.sprite = cardData.backImage;

            // ✅ 5⃣ 确保 BoxCollider 存在（完全继承 CanvasPrefab 设定）
            BoxCollider collider = cardPanel.gameObject.GetComponent<BoxCollider>();
            if (collider == null)
            {
                Debug.LogError("CardPanel 缺少 BoxCollider，请确保预制体设置正确！");
                continue;
            }

            // ✅ 6⃣ 确保 CardRotation 脚本存在，并绑定参数
            CardRotation rotationScript = cardPanel.gameObject.GetComponent<CardRotation>();
            if (rotationScript == null)
            {
                Debug.LogError("CardPanel 缺少 CardRotation 组件，请检查预制体！");
                continue;
            }

            rotationScript.cardFront = frontTransform;
            rotationScript.cardBack = backTransform;
            rotationScript.targetFacePoint = targetPoint;
            rotationScript.col = collider;
        }

        Debug.Log("所有卡牌已成功生成！（保持与模板一致）");
    }

    /// <summary>
    /// ✅ Fisher-Yates 洗牌算法：随机打乱卡牌顺序
    /// </summary>
    void ShuffleCards()
    {
        int n = instantiatedCards.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            // ✅ 交换 GameObject 在 Hierarchy 的顺序
            instantiatedCards[i].transform.SetSiblingIndex(j);
        }

        Debug.Log("卡牌已成功洗牌！");
    }
}