using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretch : MonoBehaviour
{
    public float stretchAmount = 2.0f; // 伸缩量
    public float stretchSpeed = 1.0f; // 伸缩速度
    public float moveSpeed = 0.5f; // 质心移动速度

    private Vector3 targetScale; // 目标缩放值
    private Vector3 originalScale; // 原始质心位置
    public float layer;
    public bool stretch = false;

    void Start()
    {
        // 在开始时保存原始的缩放值
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
