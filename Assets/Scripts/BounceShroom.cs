/// <author>Thoams Krahl</author>

using PelagosProject.User.Movement;
using UnityEngine;

public class BounceShroom : MonoBehaviour
{
    [SerializeField] private Animator shroomAnim;
    [SerializeField] private float bounceMultiply = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        shroomAnim.SetTrigger("shrink");
        var playerMovement = other.GetComponent<PlayerMovement>();
        float[] values = playerMovement.GetCurrentPowerValues();
        playerMovement.InstantStop();
        Vector3 direction = transform.position - other.transform.position;
        other.transform.position += -direction * values[0] * bounceMultiply * Time.deltaTime;
    }
}
