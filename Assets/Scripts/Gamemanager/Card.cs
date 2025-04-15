using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;        // ��������
    public string cardId;          // ���Ʊ�ţ�֧���� "1", "1m"��
    public Sprite frontSprite;     // ����ͼƬ
    public Sprite backSprite;      // ����ͼƬ
    public int value;              // ������ֵ�����ڱȽϴ�С��

    public CardType cardType;      // ��������
    public string description;     // �����ı�

    public bool isInsane;      // �Ƿ��Ƿ����

    public Card(string name, string id, Sprite front, Sprite back, CardType type, string desc, int value, bool isInsane)
    {
        cardName = name;
        cardId = id;
        frontSprite = front;
        backSprite = back;
        cardType = type;
        description = desc;
        this.value = value;
        this.isInsane = isInsane;
    }

    public void PlayCard()
    {
        Debug.Log($"{cardName} effect played.");
        // TODO: �������ؽű��Ķ�ӦЧ���߼�
    }
}

public enum CardType
{
    Attack,
    Defense,
    Special,
    Neutral
}
