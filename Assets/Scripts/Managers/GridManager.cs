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
    public GameObject gridCellPrefab; // ����Ԥ����
    //public Color highlightColor; // ������ɫ
    public string status;
    public Material[] newMaterial; // ��������

    private GameObject[,] gridCells; // ��������
    public int[,] obstacles; //�ϰ�������
    private Vector2[] carPositions; // С��λ������

    public Sprite[] carList; //С������

    public UIMenuManager uiMenuManager;
    public CellClickHandler cellClickHandler;


    //ʹ�õ���ģʽ���ݵ�ͼ����
    public static GridManager Instance;

    void Start()
    {
        // ��ʼ����������
        gridCells = new GameObject[41, 41];
        obstacles = new int[41, 41];
        carPositions = new Vector2[2];

        status = "obstacle";

        // �����ϰ����С����λ��
        InitializeObstaclesAndCars("init.yaml");
        // ��������
        CreateGrid();
    }

    private void Awake()
    {
        //��һ�¸õ���
        Instance = this;
    }

    //��ʼ���ϰ����С��λ��
    private void InitializeObstaclesAndCars(string fileName)
    {
        // ��ȡStreamingAssets�ļ��е�·��
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        List<Vector2> usedPositions = new List<Vector2>();

        if (File.Exists(filePath))
        {
            Debug.Log($"!!!!�ɹ���ȡ�ļ�");
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yamlInput = File.ReadAllText(filePath);

            var yamlObject = deserializer.Deserialize<Dictionary<string, object>>(yamlInput);

            // ��ȡ��ͼ�ߴ�
            var map = yamlObject["map"] as Dictionary<object, object>;
            var dimensions = map["dimensions"] as List<object>;
            int width = Convert.ToInt32(dimensions[0]);
            int height = Convert.ToInt32(dimensions[1]);
            Debug.Log($"!!!!width" + width);

            // ��ʼ���ϰ�������
            obstacles = new int[width, height];

            // ��ȡsimple_obstacles
            var simpleObstaclesList = map["simple_obstacles"] as List<object>;
            // ��ӡsimple_obstacles�б��е�����
            foreach (var obstacle in simpleObstaclesList)
            {
                var obstacleData = obstacle as Dictionary<object, object>;
                var coordinate = obstacleData["coordinate"] as List<object>;
                // ��ȡ���겢�洢��obstacles�б���
                int x = Convert.ToInt32(coordinate[0]);
                int y = Convert.ToInt32(coordinate[1]);
                obstacles[x, y] = 3; // 3����simple_obstacles
                //Debug.Log("Obstacle_Coordinate: (" + x + ", " + y + ")");
            }

            // ��ȡgoods
            var goodsList = map["goods"] as List<object>;
            foreach (var goods in goodsList)
            {
                var goodsData = goods as Dictionary<object, object>;
                var coordinate = goodsData["coordinate"] as List<object>;
                int x = Convert.ToInt32(coordinate[0]);
                int y = Convert.ToInt32(coordinate[1]);
                obstacles[x, y] = 1; // 1����goods
                //Debug.Log("Goods_Coordinate: (" + x + ", " + y + ")");
            }

            // ��ȡshelf
            var shelfList = map["shelf"] as List<object>;
            foreach (var shelf in shelfList)
            {
                var shelfData = shelf as Dictionary<object, object>;
                var coordinate = shelfData["coordinate"] as List<object>;
                int x = Convert.ToInt32(coordinate[0]);
                int y = Convert.ToInt32(coordinate[1]);
                obstacles[x, y] = 2; // 2����shelf
                //Debug.Log("Shelf_Coordinate: (" + x + ", " + y + ")");
            }

            // ��ȡС��λ��
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

            // ���С��λ���Լ����
            for (int i = 0; i < carPositions.Length; i++)
            {
                //Debug.Log($"carPositions[{i}] = {carPositions[i]}");
            }
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
        }



        ////��������ϰ���λ��
        //for (int i = 0; i < 20; i++)
        //{
        //    Vector2 pos;
        //    do
        //    {
        //        pos = new Vector2(UnityEngine.Random.Range(0, 40), UnityEngine.Random.Range(0, 40));
        //    } while (usedPositions.Contains(pos));

        //    usedPositions.Add(pos);
        //    obstacles[(int)pos.x, (int)pos.y] = UnityEngine.Random.Range(1, 4); // �����ϰ������ͣ����1, 2, 3
        //}

        ////�������С��λ��
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
        // ���㷽���С
        float cellSize = GetComponent<RectTransform>().rect.width / 40;
        // �����С
        //Debug.Log("Cell Size: " + cellSize);

        // ���ɷ���
        for (int row = 0; row < 40; row++)
        {
            for (int col = 0; col < 40; col++)
            {
                // ʵ��������
                GameObject cell = Instantiate(gridCellPrefab, transform);
                // ���÷���λ�úʹ�С
                RectTransform rt = cell.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(cellSize, cellSize);
                // ��������ӵ�������
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

                // ����С��
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

    // ��ȡ�����������е��к���
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
        // ����һ����Ч��λ�ã�����-1, -1��ʾ��Ч��
        return new Vector2Int(-1, -1);
    }

    // ����λ�û�ȡ��ǰ�����ϰ�������
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

    //�����³�������������
    public void SwitchTo3DScene(string sceneName)
    {
        // �ҵ����µ���Ϊ��"destination"GameObject
        //GameObject destinationGameObject = GameObject.Find("destination" + CellClickHandler.destinationNum);
        //GameObject destinationGameObject = GameObject.Find("cell" + CellClickHandler.destinationNum);
        //if (destinationGameObject != null)
        //{
        //    // ��ȡ��GameObject�ϵ�CellClickHandler���
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

        // ��ȡinput��Ϣ�����㷽ʽ
        Debug.Log("uiMenuManager.method: " + uiMenuManager.method);
        int method = uiMenuManager.method;
        List<CellClickHandler.AgentData> agentList = cellClickHandler.agentList;
        Debug.Log("agentList.Count: " + agentList.Count);

        // �����³��������ݲ���
        SceneManager.LoadScene(sceneName);
        Debug.Log("sceneName: " + sceneName);

        // ����python��cbs��primal����
        PyRun.Instance.startPathFinding(method, agentList);
    }
}
