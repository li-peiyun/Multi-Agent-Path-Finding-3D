using UnityEngine;
using System.Text.RegularExpressions;

public class PalletrobotMove : MonoBehaviour
{
    public Vector3[] movePoints = new Vector3[0]; // 初始化空数组
    public float moveSpeed = 1f; // 移动速度
    public int currentIndex = 0; // 当前移动到的点的索引
    public float turnSpeed = 6f; // 转头速度

    private int robotNumber;//当前小车序号

    private GameObject carriedObject = null; // 当前绑定的物体
    public Vector3 trueTargetPosition; // 物体真实卸货点

    public static int arrivedNum = 0;
    public static int agentNum = 4; //智能体总数
    public bool haveArrived = false;

    //升降装置
    public Lift[] lift = new Lift[1];
    public Stretch[] stretchers = new Stretch[4];

    //货物是否平移到货架上
    private bool carriedObjectMove = false;

    // 全局暂停标志
    public static bool isPaused = false;

    private bool load = false;//已装货标志
    private bool unload = false;//已卸货标志

    private void Start()
    {
        // 获取当前对象的名称
        string objectName = gameObject.name;
        // 使用正则表达式提取名称末尾的数字
        Match match = Regex.Match(objectName, @"\d+$");
        if (match.Success)
        {
            robotNumber = int.Parse(match.Value);
        }
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }
        // 如果还有位移点未移动到
        if (movePoints.Length>0 && currentIndex < movePoints.Length)
        {
            //if(robotNumber==4)
            //    UnityEngine.Debug.Log("robotNumber-" + robotNumber + ",currentIndex-" + currentIndex+",should-" + movePoints[currentIndex]+ "position-"+ transform.position);
            // 计算当前位置到目标点的距离
            float distance = Vector3.Distance(transform.position, movePoints[currentIndex]);

            // 如果距离小于某个阈值，表示已经到达目标点
            if (distance < 0.01f)
            {
                //UnityEngine.Debug.Log("robotNumber-" + robotNumber + "-arrivedNum: " + arrivedNum);
                //if (!haveArrived)
                //{
                //    arrivedNum++;
                //}
                // 检查是否为最后一个目标点
                if (!unload && currentIndex == movePoints.Length-1)
                {
                    HandleObjectAtPoint();
                    PyRun.Instance.AddAgentArrived(robotNumber);
                    //PyRun.Instance.PauseAllAgents(robotNumber);
                    // 检查是否有物体需要绑定或解绑
                }
                currentIndex++;

            }
            else
            {
                // 向目标点移动
                transform.position = Vector3.MoveTowards(transform.position, movePoints[currentIndex], moveSpeed * Time.deltaTime);

                // 计算目标方向
                Vector3 targetDirection = movePoints[currentIndex] - transform.position;
                if (targetDirection != Vector3.zero)
                {
                    // 计算目标旋转
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    // 插值旋转到目标方向
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                }
            }
        }

        if (lift[0].reached)
        {
            carriedObject.transform.parent = null;
            carriedObjectMove = true;
            lift[0].reached = false;
        }
        if (carriedObjectMove)
        {
            carriedObject.transform.position = Vector3.MoveTowards(carriedObject.transform.position, trueTargetPosition, 0.5f * Time.deltaTime);
            if (Vector3.Distance(carriedObject.transform.position, trueTargetPosition) <= 0.01f)
            {
                lift[0].up = false;
                for(int i=0;i<4;i++)
                    stretchers[i].stretch = false;
                carriedObject = null;
                carriedObjectMove = false;
            }
        }
    }

    private void HandleObjectAtPoint()
    {
        // 检查当前位置是否为最后一个目标点
        if (movePoints.Length>0 && currentIndex == movePoints.Length - 1)
        {
            //if (robotNumber == 3)
            //    for (int i = 0; i < movePoints.Length; i++)
            //    {
            //        Debug.Log("movePoints[" + i + "]: " + movePoints[i]);
            //    }
            // 如果已经绑定了物体，解除绑定
            if (carriedObject != null)
            {
                carriedObject.transform.parent = lift[0].transform;
                //Debug.Log("robotNum-"+robotNumber+"-trueTargetPosition:"+ trueTargetPosition);
                lift[0].layer = trueTargetPosition.y;
                lift[0].up = true;
                Debug.Log("解绑robotNum-" + robotNumber + "-currentIndex:" + currentIndex + "-movepoints.Length.:" + movePoints.Length + "-shouldbe"+movePoints[currentIndex]+"-position"+transform.position);
                for (int i = 0; i < 4; i++)
                {
                    stretchers[i].layer = trueTargetPosition.y;
                    stretchers[i].stretch = true;
                }
                unload = true;
            }
            else
            {
                if (load)
                    unload = true;
                load = true;
                // 检查当前位置是否有物体
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f); // 检测半径0.5的球形范围内的物体
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject != this.gameObject) // 排除自身
                    {
                        Debug.Log("绑定robotNum-"+robotNumber+"collider object: " + collider.gameObject.name+ "-currentIndex:" + currentIndex + "-" + "-" + transform.position+"-"+ collider.gameObject.transform.position);
                        
                        GameObject detectedObject = collider.gameObject;

                        // 如果检测到的物体有父对象，向上寻找直到找到最高级的父对象
                        while (detectedObject.transform.parent != null)
                        {
                            detectedObject = detectedObject.transform.parent.gameObject;
                            //Debug.Log("Detected object: " + detectedObject.name);
                            // 检查最高级的父对象是否为Box
                            if (detectedObject.tag == "Box")
                            {
                                carriedObject = detectedObject;
                                carriedObject.transform.parent = this.transform;
                                Debug.Log("Detected object: " + carriedObject.name+ "- "+ carriedObject.transform.position);
                                break;
                            }
                        }
                        if (carriedObject != null)
                            break;

                    }
                }
                //load = true;
            }
        }
    }
}

