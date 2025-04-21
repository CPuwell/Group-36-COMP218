using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string id;  // �Ƶı��
    public string cardName;  // �Ƶ�����
    public Sprite frontSprite; // ��������ͼƬ
    public Sprite backSprite;  // ���Ʊ���ͼƬ
    public bool isInsaneCard; // �Ƿ��Ƿ����
    public string saneEffect; // ����״̬Ч��
    public string insaneEffect; // ���״̬Ч��
}
