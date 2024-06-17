using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.IO;
using System;
using System.Text.RegularExpressions;
using UnityEditor;
using SlimUI.ModernMenu;
using System.Collections;

public class GridManager : MonoBehaviour
{
    public GameObject gridCellPrefab; // 方格预制体
    //public Color highlightColor; // 高亮颜色
    public string status;
    public Material[] newMaterial; // 材质数组

    private GameObject[,] gridCells; // 方格数组
    public int[,] obstacles; //障碍物数组
    private Vector2[] carPositions; // 小车位置数组

    public Sprite[] carList; //小车材质

    public UIMenuManager uiMenuManager;
    public CellClickHandler cellClickHandler;


    //使用单例模式传递地图参数
    public static GridManager Instance;

    void Start()
    {
        // 初始化方格数组
        gridCells = new GameObject[41, 41];
        obstacles = new int[41, 41];
        carPositions = new Vector2[2];

        status = "obstacle";

        // 生成障碍物和小车的位置
        InitializeObstaclesAndCars("init.yaml");
        // 创建方格
        CreateGrid();
    }

    private void Awake()
    {
        //绑定一下该单例
        Instance = this;
    }

    //初始化障碍物和小车位置
    private void InitializeObstaclesAndCars(string fileName)
    {
        // 获取StreamingAssets文件夹的路径
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        List<Vector2> usedPositions = new List<Vector2>();

        if (File.Exists(filePath))
        {
            Debug.Log($"!!!!成功读取文件");
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yamlInput = File.ReadAllText(filePath);

            var yamlObject = deserializer.Deserialize<Dictionary<string, object>>(yamlInput);

            // 获取地图尺寸
            var map = yamlObject["map"] as Dictionary<object, object>;
            var dimensions = map["dimensions"] as List<object>;
            int width = Convert.ToInt32(dimensions[0]);
            int height = Convert.ToInt32(dimensions[1]);
            Debug.Log($"!!!!width" + width);

            // 初始化障碍物数组
            obstacles = new int[width, height];

            // 提取simple_obstacles
            var simpleObstaclesList = map["simple_obstacles"] as List<object>;
            // 打印simple_obstacles列表中的内容
            foreach (var obstacle in simpleObstaclesList)
            {
                var obstacleData = obstacle as Dictionary<object, object>;
                var coordinate = obstacleData["coordinate"] as List<object>;
                // 提取坐标并存储到obstacles列表中
                int x = Convert.ToInt32(coordinate[0]);
                int y = Convert.ToInt32(coordinate[1]);
                obstacles[x, y] = 3; // 3代表simple_obstacles
                //Debug.Log("Obstacle_Coordinate: (" + x + ", " + y + ")");
            }

            // 提取goods
            var goodsList = map["goods"] as List<object>;
            foreach (var goods in goodsList)
            {
                var goodsData = goods as Dictionary<object, object>;
                var coordinate = goodsData["coordinate"] as List<object>;
                int x = Convert.ToInt32(coordinate[0]);
                int y = Convert.ToInt32(coordinate[1]);
                obstacles[x, y] = 1; // 1代表goods
                //Debug.Log("Goods_Coordinate: (" + x + ", " + y + ")");
            }

            // 提取shelf
            var shelfList = map["shelf"] as List<object>;
            foreach (var shelf in shelfList)
            {
                var shelfData = shelf as Dictionary<object, object>;
                var coordinate = shelfData["coordinate"] as List<object>;
                int x = Convert.ToInt32(coordinate[0]);
                int y = Convert.ToInt32(coordinate[1]);
                obstacles[x, y] = 2; // 2代表shelf
                //Debug.Log("Shelf_Coordinate: (" + x + ", " + y + ")");
            }

            // 提取小车位置
            var agents = yamlObject["agents"] as List<object>;
            carPositions = new Vector2[agents.Count];
            for (int i = 0; i < agents.Count; i++)
            {
                var agent = agents[i] as Dictionary<object, object>;
                var start = agent["start"] as List<object>;
                float x = Convert.ToSingle(start[0]);
                float y = Convert.ToSingle(start[1]);
                carPositions[i] = new Vector2(x, y);
            }

            // 输出小车位置以检查结果
            for (int i = 0; i < carPositions.Length; i++)
            {
                //Debug.Log($"carPositions[{i}] = {carPositions[i]}");
            }
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
        }



        ////随机生成障碍物位置
        //for (int i = 0; i < 20; i++)
        //{
        //    Vector2 pos;
        //    do
        //    {
        //        pos = new Vector2(UnityEngine.Random.Range(0, 40), UnityEngine.Random.Range(0, 40));
        //    } while (usedPositions.Contains(pos));

        //    usedPositions.Add(pos);
        //    obstacles[(int)pos.x, (int)pos.y] = UnityEngine.Random.Range(1, 4); // 三种障碍物类型，编号1, 2, 3
        //}

        ////随机生成小车位置
        //for (int i = 0; i < 2; i++)
        //{
        //    Vector2 pos;
        //    do
        //    {
        //        pos = new Vector2(UnityEngine.Random.Range(0, 40), UnityEngine.Random.Range(0, 40));
        //    } while (usedPositions.Contains(pos));

        //    usedPositions.Add(pos);
        //    carPositions[i] = pos;
        //}
    }

    public void CreateGrid()
    {
        // 计算方格大小
        float cellSize = GetComponent<RectTransform>().rect.width / 40;
        // 输出大小
        //Debug.Log("Cell Size: " + cellSize);

        // 生成方格
        for (int row = 0; row < 40; row++)
        {
            for (int col = 0; col < 40; col++)
            {
                // 实例化方格
                GameObject cell = Instantiate(gridCellPrefab, transform);
                // 设置方格位置和大小
                RectTransform rt = cell.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(cellSize, cellSize);
                // 将方格添加到数组中
                gridCells[row, col] = cell;
                Image imageSet = cell.GetComponent<Image>();


                //imageSet.color = Color.red;
                //imageSet.material = newMaterial[0];
                int obstacleType = obstacles[row, col];
                if (obstacleType == 1)
                {
                    //imageSet.material = newMaterial[0];
                    imageSet.color = new Color(1.0f, 0.39f, 0.28f); ;
                }
                else if (obstacleType == 2)
                {
                    imageSet.color = Color.yellow;
                }
                else if (obstacleType == 3)
                {
                    imageSet.color = Color.gray;
                }

                // 设置小车
                foreach (Vector2 carPos in carPositions)
                {
                    if (carPos.x == row && carPos.y == col)
                    {
                        //imageSet.color = new Color(0.93f, 0.51f, 0.93f); 
                        imageSet.color = Color.white;
                        imageSet.sprite = carList[0];
                    }
                }

            }
        }
    }

    // 获取格子在网格中的行和列
    public Vector2Int GetCellPosition(GameObject cell)
    {
        for (int row = 0; row < 40; row++)
        {
            for (int col = 0; col < 40; col++)
            {
                if (gridCells[row, col] == cell)
                {
                    return new Vector2Int(row, col);
                }
            }
        }
        // 返回一个无效的位置（假设-1, -1表示无效）
        return new Vector2Int(-1, -1);
    }

    // 根据位置获取当前格子障碍物类型
    public int GetObstacleType(Vector2Int position)
    {
        if (position.x >= 0 && position.x < obstacles.GetLength(0) && position.y >= 0 && position.y < obstacles.GetLength(1))
        {
            return obstacles[position.x, position.y];
        }
        return -1; // Return an invalid type if out of bounds
    }


    public void GotoAgentMode()
    {
        status = "agent";
    }

    public void GotoObstacleMode()
    {
        status = "obstacle";
    }
    public void GotoGoalMode()
    {
        status = "goal";
    }

    //加载新场景并传递数组
    public void SwitchTo3DScene(string sceneName)
    {
        // 找到最新的名为的"destination"GameObject
        //GameObject destinationGameObject = GameObject.Find("destination" + CellClickHandler.destinationNum);
        //GameObject destinationGameObject = GameObject.Find("cell" + CellClickHandler.destinationNum);
        //if (destinationGameObject != null)
        //{
        //    // 获取该GameObject上的CellClickHandler组件
        //    cellClickHandler = destinationGameObject.GetComponent<CellClickHandler>();
        //    if (cellClickHandler != null)
        //    {
        //        Debug.Log("Current GameObject name: " + cellClickHandler);
        //    }
        //    else
        //    {
        //        Debug.LogError("CellClickHandler component not found on the GameObject named 'destination1'");
        //    }
        //}
        //else
        //{
        //    Debug.LogError("GameObject named 'destination1' not found in the scene.");
        //}
        Debug.Log("Current GameObject name: " + cellClickHandler);

        // 获取input信息和运算方式
        Debug.Log("uiMenuManager.method: " + uiMenuManager.method);
        int method = uiMenuManager.method;
        List<CellClickHandler.AgentData> agentList = cellClickHandler.agentList;
        Debug.Log("agentList.Count: " + agentList.Count);

        // 加载新场景并传递参数
        SceneManager.LoadScene(sceneName);
        Debug.Log("sceneName: " + sceneName);

        // 运行python的cbs或primal程序
        PyRun.Instance.startPathFinding(method, agentList);
    }
}
