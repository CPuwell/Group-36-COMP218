using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardUI : MonoBehaviour, IPointerClickHandler
{
    public Image frontImage;
    public Image backImage;

    private Card cardData;              // ��ǰ�󶨵Ŀ�������
    private bool isFaceUp = false;
    private Hand owningHand;           // ��ǰ���ƣ��߼��㣩����

    // ���ÿ���ͼ��������
    public void SetCard(Card card, Hand hand)
    {
        cardData = card;
        owningHand = hand;

        frontImage.sprite = card.frontSprite;
        backImage.sprite = card.backSprite;

        Flip(false); // ��ʼ�Ǹ��ŵ�
    }

    // ���ƣ�true ��ʾ���棬false ��ʾ����
    public void Flip(bool faceUp)
    {
        isFaceUp = faceUp;
        frontImage.gameObject.SetActive(faceUp);
        backImage.gameObject.SetActive(!faceUp);
    }

    // ����¼�����˫�����ƣ�
    public void OnPointerClick(PointerEventData eventData)
    {
        owningHand.SelectCard(cardData);
    }

    //elegate �¼��������Ʊ�ѡ��ʱ����
    public void SetSelected(bool selected)
    {
        GetComponent<Image>().color = selected ? Color.yellow : Color.white;
    }

}
