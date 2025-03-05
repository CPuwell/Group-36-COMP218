using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] // �����ڱ༭��ģʽ��ִ�иýű�
public class CardRotation : MonoBehaviour
{
    public Transform cardFront; // ��������� Transform
    public Transform cardBack;  // ���Ʊ���� Transform
    public Transform targetFacePoint; // Ŀ����㣨ͨ���趨�ڿ��Ƶ��������ģ�
    public Collider col; // ���Ƶ���ײ�壨�������߼�⣩

    private bool showingBack = false; // ��ǵ�ǰ�Ƿ���ʾ���Ʊ���

    private void Start()
    {
        col = GetComponent<Collider>(); // ��ȡ��ǰ����� Collider
    }

    private void Update()
    {
        // �洢���߼�⵽��������ײ��Ϣ
        RaycastHit[] hits;
        hits = Physics.RaycastAll(
            Camera.main.transform.position, // ������㣨�����λ�ã�
            (-Camera.main.transform.position + targetFacePoint.position).normalized, // ���߷���ָ�� targetFacePoint��
            (-Camera.main.transform.position + targetFacePoint.position).magnitude // ���߳��ȣ��� targetFacePoint �ľ��룩
        );

        bool passedThrough = false; // ��¼�����Ƿ񴩹��˿���

        // �����������߼�⵽����ײ��
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col) // ������߻����˵�ǰ����
                passedThrough = true;
        }

        // �����⵽��״̬�뵱ǰ״̬��ͬ��������л�
        if (passedThrough != showingBack)
        {
            showingBack = passedThrough; // ���µ�ǰ״̬

            if (showingBack)
            {
                cardFront.gameObject.SetActive(false); // ��������
                cardBack.gameObject.SetActive(true);  // ��ʾ����
            }
            else
            {
                cardFront.gameObject.SetActive(true); // ��ʾ����
                cardBack.gameObject.SetActive(false); // ���ر���
            }
        }
    }
}
