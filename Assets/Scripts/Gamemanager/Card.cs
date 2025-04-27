using UnityEngine;

[System.Serializable]
public class Card
{
    public string cardName;        // ��������
    public string cardId;          // ���Ʊ�ţ�֧���� "1", "1m"��
    public Sprite frontSprite;     // ����ͼƬ
    public Sprite backSprite;      // ����ͼƬ
    public int value;              // ������ֵ�����ڱȽϴ�С��


    public string description;     // �����ı�
    public bool isInsane;          // �Ƿ��Ƿ����

    [System.NonSerialized]
    public GameObject cardObject;  // ��Ӧ��ʵ���� GameObject������ʱ��ֵ��
    public GameObject effectPrefab; // ����Ч��Ԥ���壨�����벻ͬ�ű� prefab��

    public Card(string name, string id, Sprite front, Sprite back, string desc, int value, bool isInsane, GameObject effectPrefab = null)
    {
        cardName = name;
        cardId = id;
        frontSprite = front;
        backSprite = back;
        
        description = desc;
        this.value = value;
        this.isInsane = isInsane;
        this.effectPrefab = effectPrefab;
    }

    

}
