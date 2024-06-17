using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretch : MonoBehaviour
{
    public float stretchAmount = 2.0f; // ������
    public float stretchSpeed = 1.0f; // �����ٶ�
    public float moveSpeed = 0.5f; // �����ƶ��ٶ�

    private Vector3 targetScale; // Ŀ������ֵ
    private Vector3 originalScale; // ԭʼ����λ��
    public float layer;
    public bool stretch = false;

    void Start()
    {
        // �ڿ�ʼʱ����ԭʼ������ֵ
        originalScale = new Vector3(0.1f, 0, 0.1f);
    }

    void Update()
    {
        targetScale = new Vector3(0.1f, layer, 0.1f);
        if (stretch)
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, stretchSpeed * Time.deltaTime);
        else
            transform.localScale = Vector3.MoveTowards(transform.localScale, originalScale, stretchSpeed * Time.deltaTime);

    }
}
