using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public float moveSpeed = 1f; // 质心移动速度
    private Vector3 targetPosition; // 目标质心位置
    private Vector3 originalPosition; // 目标质心位置
    public float layer;
    
    public bool up = false;//是否上升
    public bool reached = false;//是否到达layer层
    public GameObject carriedObject = null; // 当前绑定的物体

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
            // 检查是否到达目标点
            if (Vector3.Distance(transform.position, targetPosition) <= 0.01f)
            {
                reached = true;
            }
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);

    }
}
