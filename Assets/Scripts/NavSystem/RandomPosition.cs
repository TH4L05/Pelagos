/// <author>Marco Eberhardt</author>

using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    [SerializeField]
    float frontMinRange;

    [SerializeField]
    float frontMaxRange;

    [SerializeField]
    float horizontalRange;

    [SerializeField]
    float verticalRange;

    [SerializeField]
    GameObject testObject;

    [SerializeField]
    float speed;

    public GameObject creatur;

    private LayerMask layerMask;

    public float smoothTime;
    private Vector3 velocity = Vector3.zero;

    public PlayerRay playerPoint;

    int test;

    public Vector3 testVector;

    public Quaternion q;

    private void Start()
    {
        transform.LookAt(testObject.transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float y = transform.rotation.y;
            transform.eulerAngles += new Vector3(0f, y - 90, 0f);
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(this.transform.position, testObject.transform.position, speed * Time.deltaTime);

        creatur.transform.position = Vector3.SmoothDamp(creatur.transform.position, this.gameObject.transform.position, ref velocity, smoothTime * Time.deltaTime/*, speed * Time.deltaTime*/);

        creatur.transform.LookAt(this.gameObject.transform.position);
    }


    void SetFirstPoint()
    {
        float x = transform.rotation.x;
        float y = transform.rotation.y;

        ChackPosition(x, y);

        x = transform.rotation.x;
        y = transform.rotation.y;

        SetPoint();

        transform.LookAt(testObject.transform);

        transform.rotation = Quaternion.Euler(0f, y * 100, 0f);
    }

    private void SetPoint()
    {
        float front = Random.RandomRange(frontMinRange, frontMaxRange);

        float horizontal = Random.RandomRange(-horizontalRange, horizontalRange);
        float vertical = Random.RandomRange(-verticalRange, verticalRange);

        Vector3 randomOffset = new Vector3(vertical, horizontal, front);
        var randomOffsetWS = transform.TransformDirection(randomOffset);

        testObject.transform.position = transform.position + randomOffsetWS;

        //transform.rotation = Quaternion.Euler(0f, y, 0f);
    }

    void ChackPosition(float x, float y)
    {
        if (!playerPoint.leftFree && !playerPoint.rightFree)
        {
            transform.eulerAngles += new Vector3(0f, y + 180, 0f);
        }
        else
        {
            if (!playerPoint.leftFree)
            {
                transform.eulerAngles += new Vector3(0f, y + 90, 0f);
            }
            else
            {
                if (!playerPoint.rightFree)
                {
                    transform.eulerAngles += new Vector3(0f, y - 90, 0f);
                }
            }
        }

        if (!playerPoint.topFree && !playerPoint.downFree)
        {
            //Destroy(creatur);
        }
        else
        {
            if (!playerPoint.topFree)
            {
                transform.eulerAngles += new Vector3(x - 90, 0f, 0f);
            }
            else
            {
                if (!playerPoint.downFree)
                {
                    transform.eulerAngles += new Vector3(x + 90, 0f, 0f);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == testObject)
        {
            SetFirstPoint();
        }
    }
    //transform.eulerAngles = testV + new Vector3(x - 90, y, 0f);
}
