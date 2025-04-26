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

    public void PlayCard()
    {
        Debug.Log($"{cardName} effect played.");

        if (effectPrefab != null)
        {
            Debug.Log($"Instantiating effect prefab for {cardName}.");
            GameObject effectInstance = GameObject.Instantiate(effectPrefab);

            // ���Ի�ȡ������UIЧ�����ͣ�������Show
            if (effectInstance.TryGetComponent<UICardEffectNotice>(out var notice))
            {
                notice.Show(description, () =>
                {
                    Debug.Log($"{cardName} ����ȷ����ɣ�");
                });
            }
            else if (effectInstance.TryGetComponent<UIGuess>(out var guess))
            {
                // �����߼�����Ҫ��Ŀ������б�ͻص���
                guess.Show(GameManager.Instance.GetAvailableTargets(GameManager.Instance.GetCurrentPlayer()), (target, guessNumber) =>
                {
                    Debug.Log($"�²���ɣ�Ŀ�꣺{target.playerName}�����֣�{guessNumber}");
                });
            }
            else if (effectInstance.TryGetComponent<UIPlayerSelect>(out var select))
            {
                select.Show(GameManager.Instance.GetAvailableTargets(GameManager.Instance.GetCurrentPlayer()), (target) =>
                {
                    Debug.Log($"ѡ������ң�{target.playerName}");
                });
            }
            else
            {
                Debug.LogWarning($"δ֪����Ч��Prefab��δ��ʶ��ű����ͣ�");
            }
        }
        else
        {
            Debug.LogWarning($"���� {cardName} û�й���Ч��Prefab��");
        }
    }

}
