/// <author>Marco Eberhardt</author>

using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    [Header("Transform")]

    public Transform horizontalRay;

    public Transform leftRay;

    public Transform rightRay;

    [Header("Distance")]

    public int maxDistance;
    public int minDistance;

    [Header("Bools")]

    public bool topFree;
    public float topCurrentDistance;
    public bool downFree;
    public float downCurrentDistance;
    public bool leftFree;
    public float leftCurrentDistance;
    public bool rightFree;
    public float rightCurrentDistance;

    void Update()
    {
        TestDistance();

        Debug.DrawRay(horizontalRay.position, horizontalRay.TransformDirection(Vector3.forward) * maxDistance, Color.black);
    }

    void TestDistance()
    {
        topCurrentDistance = RayTest(horizontalRay, Vector3.up);
        downCurrentDistance = RayTest(horizontalRay, Vector3.down);
        leftCurrentDistance = RayTest(leftRay, Vector3.up);
        rightCurrentDistance = RayTest(rightRay, Vector3.up);

        topFree = SetBools(topCurrentDistance);
        downFree = SetBools(downCurrentDistance);
        leftFree = SetBools(leftCurrentDistance);
        rightFree = SetBools(rightCurrentDistance);
    }

    float RayTest(Transform rayPoint, Vector3 direction)
    {
        float distance = 0;

        RaycastHit hit;

        Debug.DrawRay(rayPoint.position, rayPoint.TransformDirection(direction) * maxDistance, Color.blue);

        if (Physics.Raycast(rayPoint.position, rayPoint.TransformDirection(direction), out hit, maxDistance))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                distance = Vector3.Distance(rayPoint.position, hit.point);
            }
        }
        return distance;
    }

    bool SetBools(float distance)
    {
        bool Bool = false;

        if (distance > minDistance || distance == 0)
        {
            Bool = true;
        }
        else
        {
            Bool = false;
        }

        return Bool;
    }

}
