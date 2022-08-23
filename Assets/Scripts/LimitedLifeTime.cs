/// <author>Thoams Krahl</author>

using UnityEngine;

public class LimitedLifeTime : MonoBehaviour
{
    [SerializeField , Range(0.1f, 60f)] private float lifeTime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
