using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using JetBrains.Annotations;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using SlimUI.ModernMenu;
using System.Runtime.CompilerServices;
using static UnityEngine.Rendering.DebugUI.Table;
using static SlimUI.ModernMenu.UIMenuManager;
using System.Collections.Generic;
using System.Diagnostics;

public class CellClickHandler : MonoBehaviour, IPointerClickHandler
{
    public GameObject cell;
    public Material newMaterial;
    public GridManager gridManager;
    public Sprite[] carList;
    public Sprite[] flagList;
    public UIMenuManager uiMenuManager;
    public static int pickupNum = 0;//ȡ��������
    public static int destinationNum = 0;// Ŀ�ĵ�����,��Ϊpublic��GridManager����Ҫʹ�ã�
    public static int layersNum = 0;// ���ܲ�������
    public static int flagNum = 0;

    public static Vector2Int[] pickup; // ȡ��������
    public static Vector2Int[] destination; // Ŀ�ĵ�����
    public static int[] destinationLayers; // Ŀ�ĵز�������

    //3D ������������
    //public PalletrobotMove[] plMoves = new PalletrobotMove[4];

    // ����һ�����ʾ����������
    public class AgentData
    {
        public Vector2Int pickupPoint;//ȡ����
        public Vector2Int endPoint;//����Ҫ�����Ŀ�ĵ�
        public Vector2Int agentgoPosition; //������ʵ��Ҫ�����λ��
        public int layer; //���ܲ���

        public AgentData(Vector2Int pickup, Vector2Int cargo, Vector2Int end, int layer)
        {
            this.pickupPoint = pickup;
            this.endPoint = cargo;
            this.agentgoPosition = new Vector2Int(end.x + 1, end.y); //�����յ�λ�ö�Ӧ�޸�
            this.layer = layer;
        }
    }

    // ����һ���б����洢����������
    public List<AgentData> agentList = new List<AgentData>();


    public static GameObject selectedCell;
    public static Image selectedImage;

    void Start()
    {
        // ��ʼ��ȡ��������
        pickup = new Vector2Int[1600]; // ���1600
        destination = new Vector2Int[1600];
        destinationLayers = new int[1600]; // ��ʼ��Ŀ�ĵز�������
    }

    //void Update()
    //{
    //    string gameObjectName = this.gameObject.name;
    //    UnityEngine.Debug.Log("agentList from cell-update: " + agentList.Count+"\n"+ "Current GameObject name: " + gameObjectName);
    //}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (cell != null)
        {
            if (uiMenuManager.status == "obstacle")
            {
                // ��ȡ���ӵ�����λ��
                Vector2Int cellPosition = gridManager.GetCellPosition(cell);

                // ��ȡ�ϰ�������
                int obstacleType = gridManager.GetObstacleType(cellPosition);

                // ֻ���ϰ�������Ϊ1ʱ���ܳ�Ϊpickup��
                if (obstacleType == 1)
                {
                    Image imageToChange = cell.GetComponent<Image>();
                    // ����Ƿ����Image���
                    if (imageToChange != null)
                    {
                        //imageToChange.color = Color.green;
                        // �޸�Image�����ͼ��
                        pickupNum++;
                        switch (pickupNum % 4)
                        {
                            case 1:
                                //imageToChange.sprite = carList[0];
                                //imageToChange.color = new Color(0.0f, 0.5f, 0.5f); // ����ɫ (Teal)
                                imageToChange.color = Color.white;
                                imageToChange.sprite = flagList[0];
                                break;
                            case 2:
                                //imageToChange.sprite = carList[1];
                                //imageToChange.color = new Color(0.40f, 0.0f, 0.6f); // ����ɫ��Deep Purple�� 
                                imageToChange.color = Color.white;
                                imageToChange.sprite = flagList[1];
                                break;
                            case 3:
                                //imageToChange.sprite = carList[2];
                                //imageToChange.color = new Color(0.64f, 0.16f, 0.16f); //��ɫ��Brown��
                                imageToChange.color = Color.white;
                                imageToChange.sprite = flagList[2];
                                break;
                            //case 4:
                            //    //imageToChange.sprite = carList[3];
                            //    imageToChange.color = new Color(0.93f, 0.51f, 0.93f); // ������ɫ (Violet)
                            //    break;
                            case 0:
                                //imageToChange.sprite = carList[4];
                                //imageToChange.color = new Color(1.0f, 0.39f, 0.28f); // �Ⱥ�ɫ (Tomato) 
                                imageToChange.color = Color.white;
                                imageToChange.sprite = flagList[3];
                                break;
                        }
                    }
                    cell.name = "pickupPoint";
                    int current_Index = pickupNum - 1; //��Ҫ��ǰ���1��������һ
                    // �洢��pickup������
                    if (current_Index < pickup.Length)
                    {
                        pickup[current_Index] = cellPosition;
                        // ���λ�õ�����̨
                        UnityEngine.Debug.Log("!!!Pickup placed at: " + pickup[current_Index] + "Index" + current_Index);
                        //pickupIndex++; //ǰ���Ѿ�++��������Ͳ���++��
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning("Pickup array is full.");
                    }
                }

            }
            else if (uiMenuManager.status == "agent")
            {
                if (cell.name.Contains("destination"))
                {
                    //// ��ȡ���ӵ�����λ��
                    //Vector2Int cellPosition_tmp = gridManager.GetCellPosition(cell);
                    //UnityEngine.Debug.Log("...... " + cellPosition_tmp + "!!!" + destination[destinationNum - 1]);
                    //if (cellPosition_tmp == destination[destinationNum - 1])
                    //{
                    //    if (uiMenuManager.layerlevel >= 1 && uiMenuManager.layerlevel <= 5)
                    //    {
                    //        OnLayerSelected(uiMenuManager.layerlevel);
                    //    }
                    //}
                }
                //�жϵ�ǰ�㲻��ȡ��������������
                if (!cell.name.Contains("pickupPoint") && !cell.name.Contains("destination"))
                {

                    // ��ȡ���ӵ�����λ��
                    Vector2Int cellPosition1 = gridManager.GetCellPosition(cell);

                    // ��ȡ�ϰ�������
                    int obstacleType1 = gridManager.GetObstacleType(cellPosition1);
                    if (obstacleType1 == 2)
                    {
                        Image imageToChange = cell.GetComponent<Image>();
                        // ����Ƿ����Image���
                        if (imageToChange != null)
                        {
                            // �޸�Image�����ͼ��
                            destinationNum++;
                            switch (destinationNum % 4)
                            {
                                case 1:
                                    //imageToChange.sprite = carList[0];
                                    //imageToChange.color = new Color(0.0f, 0.5f, 0.5f); // ����ɫ (Teal)
                                    imageToChange.color = Color.red;
                                    break;
                                case 2:
                                    //imageToChange.sprite = carList[1];
                                    //imageToChange.color = new Color(0.40f, 0.0f, 0.6f); // ����ɫ��Deep Purple�� 
                                    imageToChange.color = new Color(0.6235f, 0.3215f, 0.1803f); //���ɫ
                                    break;
                                case 3:
                                    //imageToChange.sprite = carList[2];
                                    //imageToChange.color = new Color(0.64f, 0.16f, 0.16f); //��ɫ��Brown��
                                    imageToChange.color = new Color(0.9254f, 0.349f, 0.5803f); //��ɫ
                                    break;
                                //case 4:
                                //    //imageToChange.sprite = carList[3];
                                //    imageToChange.color = new Color(0.93f, 0.51f, 0.93f); // ������ɫ (Violet)
                                //break;
                                case 0:
                                    //imageToChange.sprite = carList[4];
                                    //imageToChange.color = new Color(1.0f, 0.39f, 0.28f); // �Ⱥ�ɫ (Tomato) 
                                    imageToChange.color = new Color(0f, 0.4588f, 0.8274f);  //��ɫ
                                    break;
                            }
                            cell.name = ("destination" + destinationNum).ToString();


                            // ��ʱ�洢��ǰ�����cell��Image���
                            selectedCell = cell;
                            selectedImage = imageToChange;



                        }

                        int current_Index1 = destinationNum - 1;  //��Ҫ��ǰ���1��������һ
                        // �洢��destination������
                        if (current_Index1 < destination.Length)
                        {
                            destination[current_Index1] = cellPosition1;
                            // ���λ�õ�����̨
                            UnityEngine.Debug.Log("???destination placed at: " + destination[current_Index1] + "Index" + current_Index1);
                            //destinationIndex++; //�����ټ�1
                        }
                        else
                        {
                            UnityEngine.Debug.LogWarning("Destination array is full.");
                        }
                    }

                }
            }

           
        }
    }

    public void OnLayerSelected(int selectedLayer)
    {
        UnityEngine.Debug.Log(".." );
        if (selectedCell != null && selectedImage != null)
        {
            UnityEngine.Debug.Log("/////");
            // ��������ʾ��Ŀ�ĵص�Ԫ����
            Text layerText = selectedCell.GetComponentInChildren<Text>();
            if (layerText == null)
            {
                GameObject textObject = new GameObject("LayerText");
                textObject.transform.SetParent(selectedCell.transform, false);
                layerText = textObject.AddComponent<Text>();
                layerText.alignment = TextAnchor.MiddleCenter;
                layerText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                layerText.color = Color.white;
            }
            layerText.text = selectedLayer.ToString();

            // ����Ŀ�ĵز�������
            if (layersNum >= 0 && layersNum < destinationLayers.Length)
            {
                destinationLayers[layersNum] = selectedLayer;
                UnityEngine.Debug.Log("???layer: " + destinationLayers[layersNum] + "Index" + layersNum);
            }
            layersNum++;  //�������µĲ���
            UnityEngine.Debug.Log("1111:"+ layersNum+ "1111:" + destinationNum + "1111:" + pickupNum);
            //int destinationIndex = destinationNum - 1;

            // ����ݴ��cell��Image���
            selectedCell = null;
            selectedImage = null;

            // ��pickup��destination�����ú󣬴���һ���µ�AgentData������ӵ�agentList��
            if (uiMenuManager.status == "agent" && pickupNum == destinationNum)
            {
                UnityEngine.Debug.Log("New agent created with pickup: " + pickupNum + " de" + destinationNum);
                UnityEngine.Debug.Log("Printing pickup array:");
                for (int i = 0; i < pickupNum; i++)
                {
                    UnityEngine.Debug.Log("pickup[" + i + "] = " + pickup[i]);
                }
                for (int i = 0; i < destinationNum; i++)
                {
                    UnityEngine.Debug.Log("des[" + i + "] = " + destination[i]);
                }
                for (int i = 0; i < pickupNum; i++)
                {
                    Vector2Int pickupPoint = pickup[i];
                    Vector2Int endPoint = destination[i];
                    Vector2Int agentgoPosition = new Vector2Int(endPoint.x, endPoint.y);
                    int layer = destinationLayers[i]; // ������Ҫ���ò���

                    AgentData newAgent = new AgentData(pickupPoint, endPoint, agentgoPosition, layer);
                    agentList.Add(newAgent);

                    // �����������Ϣ������̨
                    UnityEngine.Debug.Log("New agent created with pickup: " + newAgent.pickupPoint + ", agentgo: " + newAgent.agentgoPosition + ", end: " + newAgent.endPoint + ", layer: " + newAgent.layer);
                    //UnityEngine.Debug.Log("agentList.Count: " + agentList.Count);
                }
                string gameObjectName = this.gameObject.name;
                UnityEngine.Debug.Log("agentList.Count after agentList.Add: " + agentList.Count + " " + "Current GameObject name: " + gameObjectName);
            }
        }
    }
}
