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
    public void SetCard(Card card,Hand hand)
    {
        Debug.Log($"���ÿ��ƣ�{card.cardName}");
        cardData = card;
        owningHand = hand; // ����������
        

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
        Debug.Log($"Clicked on card: {(cardData != null ? cardData.cardName : "null")}");

        if (!IsMyTurn())
        {
            Debug.LogWarning("���ڲ�����Ļغϣ��޷����ƣ�");
            return;
        }

        if (owningHand != null && cardData != null)
        {
            owningHand.SelectCard(cardData); // ֻ�ڲ�Ϊ null ʱ����
        }
        else
        {
            Debug.LogWarning("CardUI ���ʱ owningHand �� cardData Ϊ null��");
        }
    }


    //elegate �¼��������Ʊ�ѡ��ʱ����
    public void SetSelected(bool selected)
    {
        GetComponent<Image>().color = selected ? Color.yellow : Color.white;
    }

    private bool IsMyTurn()
    {
        if (GameManager.Instance == null) return false;

        Player currentPlayer = GameManager.Instance.GetCurrentPlayer();

        return currentPlayer != null && currentPlayer.isHuman;
    }

}
