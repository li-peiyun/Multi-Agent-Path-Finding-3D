using UnityEngine;

public class boxMove : MonoBehaviour
{
    public Vector3[] movePoints = new Vector3[]
    {
        new Vector3(28.5f, 0, 33.5f),    // ��һ��Ŀ����λ��
        new Vector3(28.5f, 0, 35.5f),    // �ڶ���Ŀ����λ��
        new Vector3(26.5f, 0, 35.5f),    // ������Ŀ����λ��
        //new Vector3(31.5f, 0, 36),    // ���ĸ�Ŀ����λ��
    }; // ��ʼ��������
    public float moveSpeed = 2f; // �ƶ��ٶ�
    private int currentIndex = 0; // ��ǰ�ƶ����ĵ������
    public float turnSpeed = 2f; // תͷ�ٶ�
    private GameObject carriedObject = null; // ��ǰ�󶨵�����
    public Vector3 tureTargetPosition = new Vector3(25.5f,3f,33.5f); // ������ʵж����

    //����װ��
    public Lift[] lift = new Lift[1];
    public Stretch[] stretchers = new Stretch[4];

    private bool carriedObjectMove = false;

    // ȫ����ͣ��־
    //public static bool isPaused = false;

    void Update()
    {
        // �����ͣ��־Ϊtrue���򲻽����κβ���
        //if (isPaused)
        //{
        //    return;
        //}

        // �������λ�Ƶ�δ�ƶ���
        if (currentIndex < movePoints.Length)
        {
            // ���㵱ǰλ�õ�Ŀ���ľ���
            float distance = Vector3.Distance(transform.position, movePoints[currentIndex]);

            // �������С��ĳ����ֵ����ʾ�Ѿ�����Ŀ���
            if (distance < 0.01f)
            {
                //Debug.Log(currentIndex);
                // ����Ƿ���������Ҫ�󶨻���
                HandleObjectAtPoint();

                // �л�����һ��Ŀ���
                currentIndex++;

                // ����Ƿ�Ϊ���һ��Ŀ���
                //if (currentIndex == movePoints.Length)
                //{
                //    // ��ͣ����������
                //    PyRun.Instance.PauseAllAgents(2);
                //}
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
            // ��鵱ǰλ���Ƿ�������
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f); // ���뾶1�����η�Χ�ڵ�����
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != this.gameObject) // �ų�����
                {
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

