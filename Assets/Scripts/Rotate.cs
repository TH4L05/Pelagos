/// <author>Thoams Krahl</author>

using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 20f;

    void FixedUpdate()
    {
        RotateAround();
    }

    public void RotateAround()
    {
        transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * rotationSpeed);
    }
}
