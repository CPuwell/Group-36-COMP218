using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public GameObject canvasPrefab; // Ԥ���壺CardCanvas������ CardPanel��
    public Transform cardParent; // ������п��Ƶĸ�����
    public List<Card> allCards; // �洢���п�������

    void Start()
    {
        GenerateCards(); // ���ɿ���
    }

    void GenerateCards()
    {
        foreach (Card cardData in allCards) // �������п�������
        {
            // 1?? ֱ�Ӹ��� CanvasPrefab�������������������
            GameObject newCanvas = Instantiate(canvasPrefab, cardParent);
            newCanvas.name = "Canvas_" + cardData.cardName; // ��������

            // 2?? ��ȡ CardPanel
            Transform cardPanel = newCanvas.transform.Find("CardPanel");
            if (cardPanel == null)
            {
                Debug.LogError("CardCanvas ���Ҳ��� CardPanel������Ԥ����ṹ��", newCanvas);
                continue;
            }

            // 3?? ��ȡ���Ƶ�ǰ��ͼƬ & Ŀ�����
            Transform frontTransform = cardPanel.Find("CardFront");
            Transform backTransform = cardPanel.Find("CardBack");
            Transform targetPoint = cardPanel.Find("TargetPoint");

            if (frontTransform == null || backTransform == null || targetPoint == null)
            {
                Debug.LogError("CardPanel ���Ҳ��� CardFront��CardBack �� TargetPoint������Ԥ���壡", cardPanel);
                continue;
            }

            Image frontImage = frontTransform.GetComponent<Image>();
            Image backImage = backTransform.GetComponent<Image>();

            if (frontImage == null || backImage == null)
            {
                Debug.LogError("CardFront �� CardBack û�� Image �����");
                continue;
            }

            // 4?? ���ÿ���ͼƬ�������� Sprite�����޸��������ԣ�
            frontImage.sprite = cardData.frontSprite;
            backImage.sprite = cardData.backSprite;

            // ? **BoxCollider �Ĵ�С��λ�á���ת��ȫ���� CanvasPrefab ���趨**
            BoxCollider collider = cardPanel.gameObject.GetComponent<BoxCollider>();
            if (collider == null)
            {
                Debug.LogError("CardPanel ȱ�� BoxCollider����ȷ��Ԥ����������ȷ��");
                continue;
            }

            // 5?? **ȷ�� CardRotation �ű����ڣ����󶨲���**
            CardRotation rotationScript = cardPanel.gameObject.GetComponent<CardRotation>();
            if (rotationScript == null)
            {
                Debug.LogError("CardPanel ȱ�� CardRotation ���������Ԥ���壡");
                continue;
            }

            rotationScript.cardFront = frontTransform;
            rotationScript.cardBack = backTransform;
            rotationScript.targetFacePoint = targetPoint;
            rotationScript.col = collider;
        }

        Debug.Log("���п����ѳɹ����ɣ���������ģ��һ�£�");
    }
}
