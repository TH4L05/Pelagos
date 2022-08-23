/// <author>Thoams Krahl</author>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmSpawner : MonoBehaviour
{
    [SerializeField] private bool activate = true;
    [SerializeField] private GameObject[] creatureTemplates;
    [SerializeField] [Range(1, 999)] private int amount = 1;
    [SerializeField] [Range(1.0f, 10.0f)] private float maxSwarmDimensionX = 5f;
    [SerializeField] [Range(1.0f, 10.0f)] private float maxSwarmDimensionY = 5f;
    [SerializeField] [Range(1.0f, 10.0f)] private float maxSwarmDimensionZ = 5f;
    [SerializeField] private Vector3 spawnRotation = Vector3.zero;

    [Header("DEV")]
    [SerializeField] private Color gizmoColor = Color.cyan;

    public Vector3 goalPos = Vector3.zero;

    private GameObject[] spawnedCreatures;

    private void Awake()
    {
        if(activate) SpawnSwarm();
    }

    private void Update()
    {
        if (Random.Range(0, 10000) < 50)
        {
            goalPos = RandomPosition();
        }
    }

    private void SpawnSwarm()
    {
        spawnedCreatures = new GameObject[amount];
        for (int i = 0; i < amount; i++)
        {
            Vector3 position = RandomPosition();
            int index = RandomCreaturePrefabsIndex();
            spawnedCreatures[i] = Instantiate(creatureTemplates[index], position, Quaternion.identity);
            spawnedCreatures[i].transform.Rotate(Vector3.up, spawnRotation.y);
        }
    }

    private Vector3 RandomPosition()
    {
        float posX = UnityEngine.Random.Range(-maxSwarmDimensionX, maxSwarmDimensionX);
        float posY = UnityEngine.Random.Range(-maxSwarmDimensionY, maxSwarmDimensionY);
        float posZ = UnityEngine.Random.Range(-maxSwarmDimensionZ, maxSwarmDimensionZ);

        Vector3 newPosition = new Vector3(posX + transform.position.x,
                                          posY + transform.position.y,
                                          posZ + transform.position.z
                                         );

        return newPosition;
    }

    private int RandomCreaturePrefabsIndex()
    {
        if (creatureTemplates.Length == 1) return 0;

        int index = UnityEngine.Random.Range(0, creatureTemplates.Length);

        return index;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
