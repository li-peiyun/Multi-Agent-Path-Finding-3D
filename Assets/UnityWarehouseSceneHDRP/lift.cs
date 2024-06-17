using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public float moveSpeed = 1f; // �����ƶ��ٶ�
    private Vector3 targetPosition; // Ŀ������λ��
    private Vector3 originalPosition; // Ŀ������λ��
    public float layer;
    
    public bool up = false;//�Ƿ�����
    public bool reached = false;//�Ƿ񵽴�layer��
    public GameObject carriedObject = null; // ��ǰ�󶨵�����

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition;
        targetPosition.y += 1.5f*layer;

    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = transform.position;
        targetPosition.y = originalPosition.y+layer;
        originalPosition = transform.position;
        originalPosition.y = 0.15f;
        //Debug.Log("up:"+ up);
        //Debug.Log("reached:" + reached);
        if (up)
        {
            //Debug.Log("layer:" + layer);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            // ����Ƿ񵽴�Ŀ���
            if (Vector3.Distance(transform.position, targetPosition) <= 0.01f)
            {
                reached = true;
            }
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);

    }
}
