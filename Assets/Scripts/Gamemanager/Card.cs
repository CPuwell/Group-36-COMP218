using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;        // ��������
    public int cardId;             // ���Ʊ�ţ�����ʶ��������Դ��
    public Sprite frontSprite;     // ����ͼƬ
    public Sprite backSprite;      // ����ͼƬ��һ�㶼һ����
    public int value;          // �������

    public CardType cardType;      // �������ͣ����繥��������������ȣ�
    public string description;     // ����������Ч���ı�

    public Card(string name, int id, Sprite front, Sprite back, CardType type, string desc)
    {
        cardName = name;
        cardId = id;
        frontSprite = front;
        backSprite = back;
        cardType = type;
        description = desc;
    }

    public void PlayCard()
    {
        Debug.Log($"{cardName} effect played.");
        // TODO: ʵ�ֿ���Ч���߼�
    }

}

public enum CardType
{
    Attack,
    Defense,
    Special,
    Neutral
}
