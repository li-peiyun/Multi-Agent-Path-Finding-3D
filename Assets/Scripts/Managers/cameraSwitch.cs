using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSwitch : MonoBehaviour
{

    public Camera overallCamera1;
    public Camera overallCamera2;
    public Camera detailedCamer1;
    public Camera detailedCamer2;

    // Start is called before the first frame update
    void Start()
    {
        ShowOverallView1();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    // ���û����¿ո��ʱִ�еĴ���
        //    Debug.Log("�ո񣡣�������");
        //    ShowOverheadView();
        //}

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // ���û��������ּ�1ʱִ�еĴ���
            Debug.Log("2 ����������");
            ShowDetailedView1();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // ���û��������ּ�2ʱִ�еĴ���
            Debug.Log("1 ����������");
            ShowOverallView1();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // ���û��������ּ�2ʱִ�еĴ���
            Debug.Log("3 ����������");
            ShowOverallView2();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // ���û��������ּ�2ʱִ�еĴ���
            Debug.Log("4 ����������");
            ShowDetailedView2();
        }

    }

    public void ShowDetailedView1()
    {
        overallCamera1.enabled = false;
        overallCamera2.enabled = false;
        detailedCamer2.enabled = false;
        detailedCamer1.enabled = true;
    }

    public void ShowDetailedView2()
    {
        overallCamera1.enabled = false;
        overallCamera2.enabled = false;
        detailedCamer2.enabled = true;
        detailedCamer1.enabled = false;
    }

    public void ShowOverallView1()
    {
        detailedCamer1.enabled = false;
        overallCamera2.enabled = false;
        detailedCamer2.enabled = false;
        overallCamera1.enabled = true;
    }

    public void ShowOverallView2()
    {
        detailedCamer1.enabled = false;
        detailedCamer2.enabled = false;
        overallCamera2.enabled = true;
        overallCamera1.enabled = false;
    }
}
