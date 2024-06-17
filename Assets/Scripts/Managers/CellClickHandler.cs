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
    public static int pickupNum = 0;//取货点总数
    public static int destinationNum = 0;// 目的地总数,设为public，GridManager中需要使用！
    public static int layersNum = 0;// 货架层数总数
    public static int flagNum = 0;

    public static Vector2Int[] pickup; // 取货点数组
    public static Vector2Int[] destination; // 目的地数组
    public static int[] destinationLayers; // 目的地层数数组

    //3D 多智能体数组
    //public PalletrobotMove[] plMoves = new PalletrobotMove[4];

    // 定义一个类表示智能体数据
    public class AgentData
    {
        public Vector2Int pickupPoint;//取货点
        public Vector2Int endPoint;//货物要到达的目的地
        public Vector2Int agentgoPosition; //智能体实际要到达的位置
        public int layer; //货架层数

        public AgentData(Vector2Int pickup, Vector2Int cargo, Vector2Int end, int layer)
        {
            this.pickupPoint = pickup;
            this.endPoint = cargo;
            this.agentgoPosition = new Vector2Int(end.x + 1, end.y); //根据终点位置对应修改
            this.layer = layer;
        }
    }

    // 创建一个列表来存储智能体数据
    public List<AgentData> agentList = new List<AgentData>();


    public static GameObject selectedCell;
    public static Image selectedImage;

    void Start()
    {
        // 初始化取货点数组
        pickup = new Vector2Int[1600]; // 最大1600
        destination = new Vector2Int[1600];
        destinationLayers = new int[1600]; // 初始化目的地层数数组
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
                // 获取格子的行列位置
                Vector2Int cellPosition = gridManager.GetCellPosition(cell);

                // 获取障碍物类型
                int obstacleType = gridManager.GetObstacleType(cellPosition);

                // 只有障碍物类型为1时才能成为pickup点
                if (obstacleType == 1)
                {
                    Image imageToChange = cell.GetComponent<Image>();
                    // 检查是否存在Image组件
                    if (imageToChange != null)
                    {
                        //imageToChange.color = Color.green;
                        // 修改Image组件的图像
                        pickupNum++;
                        switch (pickupNum % 4)
                        {
                            case 1:
                                //imageToChange.sprite = carList[0];
                                //imageToChange.color = new Color(0.0f, 0.5f, 0.5f); // 茶绿色 (Teal)
                                imageToChange.color = Color.white;
                                imageToChange.sprite = flagList[0];
                                break;
                            case 2:
                                //imageToChange.sprite = carList[1];
                                //imageToChange.color = new Color(0.40f, 0.0f, 0.6f); // 深紫色（Deep Purple） 
                                imageToChange.color = Color.white;
                                imageToChange.sprite = flagList[1];
                                break;
                            case 3:
                                //imageToChange.sprite = carList[2];
                                //imageToChange.color = new Color(0.64f, 0.16f, 0.16f); //褐色（Brown）
                                imageToChange.color = Color.white;
                                imageToChange.sprite = flagList[2];
                                break;
                            //case 4:
                            //    //imageToChange.sprite = carList[3];
                            //    imageToChange.color = new Color(0.93f, 0.51f, 0.93f); // 紫罗兰色 (Violet)
                            //    break;
                            case 0:
                                //imageToChange.sprite = carList[4];
                                //imageToChange.color = new Color(1.0f, 0.39f, 0.28f); // 橙红色 (Tomato) 
                                imageToChange.color = Color.white;
                                imageToChange.sprite = flagList[3];
                                break;
                        }
                    }
                    cell.name = "pickupPoint";
                    int current_Index = pickupNum - 1; //需要把前面加1的索引减一
                    // 存储到pickup数组中
                    if (current_Index < pickup.Length)
                    {
                        pickup[current_Index] = cellPosition;
                        // 输出位置到控制台
                        UnityEngine.Debug.Log("!!!Pickup placed at: " + pickup[current_Index] + "Index" + current_Index);
                        //pickupIndex++; //前面已经++过了这里就不用++了
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
                    //// 获取格子的行列位置
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
                //判断当前点不是取货点或者智能体点
                if (!cell.name.Contains("pickupPoint") && !cell.name.Contains("destination"))
                {

                    // 获取格子的行列位置
                    Vector2Int cellPosition1 = gridManager.GetCellPosition(cell);

                    // 获取障碍物类型
                    int obstacleType1 = gridManager.GetObstacleType(cellPosition1);
                    if (obstacleType1 == 2)
                    {
                        Image imageToChange = cell.GetComponent<Image>();
                        // 检查是否存在Image组件
                        if (imageToChange != null)
                        {
                            // 修改Image组件的图像
                            destinationNum++;
                            switch (destinationNum % 4)
                            {
                                case 1:
                                    //imageToChange.sprite = carList[0];
                                    //imageToChange.color = new Color(0.0f, 0.5f, 0.5f); // 茶绿色 (Teal)
                                    imageToChange.color = Color.red;
                                    break;
                                case 2:
                                    //imageToChange.sprite = carList[1];
                                    //imageToChange.color = new Color(0.40f, 0.0f, 0.6f); // 深紫色（Deep Purple） 
                                    imageToChange.color = new Color(0.6235f, 0.3215f, 0.1803f); //深褐色
                                    break;
                                case 3:
                                    //imageToChange.sprite = carList[2];
                                    //imageToChange.color = new Color(0.64f, 0.16f, 0.16f); //褐色（Brown）
                                    imageToChange.color = new Color(0.9254f, 0.349f, 0.5803f); //粉色
                                    break;
                                //case 4:
                                //    //imageToChange.sprite = carList[3];
                                //    imageToChange.color = new Color(0.93f, 0.51f, 0.93f); // 紫罗兰色 (Violet)
                                //break;
                                case 0:
                                    //imageToChange.sprite = carList[4];
                                    //imageToChange.color = new Color(1.0f, 0.39f, 0.28f); // 橙红色 (Tomato) 
                                    imageToChange.color = new Color(0f, 0.4588f, 0.8274f);  //蓝色
                                    break;
                            }
                            cell.name = ("destination" + destinationNum).ToString();


                            // 暂时存储当前点击的cell和Image组件
                            selectedCell = cell;
                            selectedImage = imageToChange;



                        }

                        int current_Index1 = destinationNum - 1;  //需要把前面加1的索引减一
                        // 存储到destination数组中
                        if (current_Index1 < destination.Length)
                        {
                            destination[current_Index1] = cellPosition1;
                            // 输出位置到控制台
                            UnityEngine.Debug.Log("???destination placed at: " + destination[current_Index1] + "Index" + current_Index1);
                            //destinationIndex++; //无需再加1
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
            // 将层数显示在目的地单元格中
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

            // 更新目的地层数数组
            if (layersNum >= 0 && layersNum < destinationLayers.Length)
            {
                destinationLayers[layersNum] = selectedLayer;
                UnityEngine.Debug.Log("???layer: " + destinationLayers[layersNum] + "Index" + layersNum);
            }
            layersNum++;  //加入了新的层数
            UnityEngine.Debug.Log("1111:"+ layersNum+ "1111:" + destinationNum + "1111:" + pickupNum);
            //int destinationIndex = destinationNum - 1;

            // 清空暂存的cell和Image组件
            selectedCell = null;
            selectedImage = null;

            // 当pickup和destination都设置后，创建一个新的AgentData对象并添加到agentList中
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
                    int layer = destinationLayers[i]; // 根据需要设置层数

                    AgentData newAgent = new AgentData(pickupPoint, endPoint, agentgoPosition, layer);
                    agentList.Add(newAgent);

                    // 输出智能体信息到控制台
                    UnityEngine.Debug.Log("New agent created with pickup: " + newAgent.pickupPoint + ", agentgo: " + newAgent.agentgoPosition + ", end: " + newAgent.endPoint + ", layer: " + newAgent.layer);
                    //UnityEngine.Debug.Log("agentList.Count: " + agentList.Count);
                }
                string gameObjectName = this.gameObject.name;
                UnityEngine.Debug.Log("agentList.Count after agentList.Add: " + agentList.Count + " " + "Current GameObject name: " + gameObjectName);
            }
        }
    }
}
