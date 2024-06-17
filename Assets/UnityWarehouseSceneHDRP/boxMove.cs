using UnityEngine;

public class boxMove : MonoBehaviour
{
    public Vector3[] movePoints = new Vector3[]
    {
        new Vector3(28.5f, 0, 33.5f),    // 第一个目标点的位置
        new Vector3(28.5f, 0, 35.5f),    // 第二个目标点的位置
        new Vector3(26.5f, 0, 35.5f),    // 第三个目标点的位置
        //new Vector3(31.5f, 0, 36),    // 第四个目标点的位置
    }; // 初始化空数组
    public float moveSpeed = 2f; // 移动速度
    private int currentIndex = 0; // 当前移动到的点的索引
    public float turnSpeed = 2f; // 转头速度
    private GameObject carriedObject = null; // 当前绑定的物体
    public Vector3 tureTargetPosition = new Vector3(25.5f,3f,33.5f); // 物体真实卸货点

    //升降装置
    public Lift[] lift = new Lift[1];
    public Stretch[] stretchers = new Stretch[4];

    private bool carriedObjectMove = false;

    // 全局暂停标志
    //public static bool isPaused = false;

    void Update()
    {
        // 如果暂停标志为true，则不进行任何操作
        //if (isPaused)
        //{
        //    return;
        //}

        // 如果还有位移点未移动到
        if (currentIndex < movePoints.Length)
        {
            // 计算当前位置到目标点的距离
            float distance = Vector3.Distance(transform.position, movePoints[currentIndex]);

            // 如果距离小于某个阈值，表示已经到达目标点
            if (distance < 0.01f)
            {
                //Debug.Log(currentIndex);
                // 检查是否有物体需要绑定或解绑
                HandleObjectAtPoint();

                // 切换到下一个目标点
                currentIndex++;

                // 检查是否为最后一个目标点
                //if (currentIndex == movePoints.Length)
                //{
                //    // 暂停所有智能体
                //    PyRun.Instance.PauseAllAgents(2);
                //}
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
        if(carriedObjectMove)
        {
            carriedObject.transform.position = Vector3.MoveTowards(carriedObject.transform.position, tureTargetPosition, 0.5f * Time.deltaTime);
            if (Vector3.Distance(carriedObject.transform.position, tureTargetPosition) <= 0.01f)
            {
                lift[0].up = false;
                for (int i = 0; i < 4; i++)
                    stretchers[i].stretch = false;
                carriedObject = null;
                carriedObjectMove = false;
            }
        }
    }

    private void HandleObjectAtPoint()
    {
        //Debug.Log("carried object: " + carriedObject);
        if (carriedObject != null)
        {
            carriedObject.transform.parent = lift[0].transform;
            //Debug.Log("transfered parent");
            tureTargetPosition = new Vector3(this.transform.position.x - 1, 3f, this.transform.position.z);
            //Debug.Log("Lift.up:"+ Lift.up);
            lift[0].up = true;
            for (int i = 0; i < 4; i++)
                stretchers[i].stretch = true;
            //Debug.Log("start");
            //Debug.Log("start");
            //while (!Lift.reached) { }
            //Debug.Log("finish");
            //Lift.reached = false;
            //carriedObject.transform.parent = null;
            //carriedObjectMove = true;
            //carriedObject.transform.position = Vector3.MoveTowards(carriedObject.transform.position, tureTargetPosition, 0.5f * Time.deltaTime);

            //Lift.up = false;
            //Stretch.stretch = false;
            //carriedObject = null;
        }
        else
        {
            // 检查当前位置是否有物体
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f); // 检测半径1的球形范围内的物体
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != this.gameObject) // 排除自身
                {
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
                            //Debug.Log("Detected object: " + carriedObject.name);
                            break;
                        }
                    }
                if (carriedObject != null)
                    break;
                        
                }
            }
        }

    }
}

