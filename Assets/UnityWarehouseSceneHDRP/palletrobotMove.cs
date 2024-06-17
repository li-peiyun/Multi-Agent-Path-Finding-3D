using UnityEngine;
using System.Text.RegularExpressions;

public class PalletrobotMove : MonoBehaviour
{
    public Vector3[] movePoints = new Vector3[0]; // ��ʼ��������
    public float moveSpeed = 1f; // �ƶ��ٶ�
    public int currentIndex = 0; // ��ǰ�ƶ����ĵ������
    public float turnSpeed = 6f; // תͷ�ٶ�

    private int robotNumber;//��ǰС�����

    private GameObject carriedObject = null; // ��ǰ�󶨵�����
    public Vector3 trueTargetPosition; // ������ʵж����

    public static int arrivedNum = 0;
    public static int agentNum = 4; //����������
    public bool haveArrived = false;

    //����װ��
    public Lift[] lift = new Lift[1];
    public Stretch[] stretchers = new Stretch[4];

    //�����Ƿ�ƽ�Ƶ�������
    private bool carriedObjectMove = false;

    // ȫ����ͣ��־
    public static bool isPaused = false;

    private bool load = false;//��װ����־
    private bool unload = false;//��ж����־

    private void Start()
    {
        // ��ȡ��ǰ���������
        string objectName = gameObject.name;
        // ʹ��������ʽ��ȡ����ĩβ������
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
        // �������λ�Ƶ�δ�ƶ���
        if (movePoints.Length>0 && currentIndex < movePoints.Length)
        {
            //if(robotNumber==4)
            //    UnityEngine.Debug.Log("robotNumber-" + robotNumber + ",currentIndex-" + currentIndex+",should-" + movePoints[currentIndex]+ "position-"+ transform.position);
            // ���㵱ǰλ�õ�Ŀ���ľ���
            float distance = Vector3.Distance(transform.position, movePoints[currentIndex]);

            // �������С��ĳ����ֵ����ʾ�Ѿ�����Ŀ���
            if (distance < 0.01f)
            {
                //UnityEngine.Debug.Log("robotNumber-" + robotNumber + "-arrivedNum: " + arrivedNum);
                //if (!haveArrived)
                //{
                //    arrivedNum++;
                //}
                // ����Ƿ�Ϊ���һ��Ŀ���
                if (!unload && currentIndex == movePoints.Length-1)
                {
                    HandleObjectAtPoint();
                    PyRun.Instance.AddAgentArrived(robotNumber);
                    //PyRun.Instance.PauseAllAgents(robotNumber);
                    // ����Ƿ���������Ҫ�󶨻���
                }
                currentIndex++;

            }
            else
            {
                // ��Ŀ����ƶ�
                transform.position = Vector3.MoveTowards(transform.position, movePoints[currentIndex], moveSpeed * Time.deltaTime);

                // ����Ŀ�귽��
                Vector3 targetDirection = movePoints[currentIndex] - transform.position;
                if (targetDirection != Vector3.zero)
                {
                    // ����Ŀ����ת
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    // ��ֵ��ת��Ŀ�귽��
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
        // ��鵱ǰλ���Ƿ�Ϊ���һ��Ŀ���
        if (movePoints.Length>0 && currentIndex == movePoints.Length - 1)
        {
            //if (robotNumber == 3)
            //    for (int i = 0; i < movePoints.Length; i++)
            //    {
            //        Debug.Log("movePoints[" + i + "]: " + movePoints[i]);
            //    }
            // ����Ѿ��������壬�����
            if (carriedObject != null)
            {
                carriedObject.transform.parent = lift[0].transform;
                //Debug.Log("robotNum-"+robotNumber+"-trueTargetPosition:"+ trueTargetPosition);
                lift[0].layer = trueTargetPosition.y;
                lift[0].up = true;
                Debug.Log("���robotNum-" + robotNumber + "-currentIndex:" + currentIndex + "-movepoints.Length.:" + movePoints.Length + "-shouldbe"+movePoints[currentIndex]+"-position"+transform.position);
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
                // ��鵱ǰλ���Ƿ�������
                Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f); // ���뾶0.5�����η�Χ�ڵ�����
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject != this.gameObject) // �ų�����
                    {
                        Debug.Log("��robotNum-"+robotNumber+"collider object: " + collider.gameObject.name+ "-currentIndex:" + currentIndex + "-" + "-" + transform.position+"-"+ collider.gameObject.transform.position);
                        
                        GameObject detectedObject = collider.gameObject;

                        // �����⵽�������и���������Ѱ��ֱ���ҵ���߼��ĸ�����
                        while (detectedObject.transform.parent != null)
                        {
                            detectedObject = detectedObject.transform.parent.gameObject;
                            //Debug.Log("Detected object: " + detectedObject.name);
                            // �����߼��ĸ������Ƿ�ΪBox
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

