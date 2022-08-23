using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewBoidData", menuName = "PelagosProject/Data/BoidData")]
public class BoidData : ScriptableObject
{
    [Header("General Settings")]
    public Vector2 behavioralCh = new Vector2(2.0f, 6.0f);

    [Header("Flock Settings")]
    [Range(1, 150)] public int flockNum = 2;
    [Range(0, 5000)] public int fragmentedFlock = 30;
    [Range(0, 1)] public float fragmentedFlockYLimit = 0.5f;
    [Range(0, 1.0f)] public float migrationFrequency = 0.1f;
    [Range(0, 1.0f)] public float posChangeFrequency = 0.5f;
    [Range(0, 100)] public float smoothChFrequency = 0.5f;

    [Header("Bird Settings")]
    public GameObject birdPref;
    [Range(1, 9999)] public int birdsNum = 10;
    [Range(0, 150)] public float birdSpeed = 1;
    [Range(0, 100)] public int fragmentedBirds = 10;
    [Range(0, 1)] public float fragmentedBirdsYLimit = 1;
    [Range(0, 10)] public float soaring = 0.5f;
    [Range(0.01f, 500)] public float verticalWawe = 20;
    public bool rotationClamp = false;
    [Range(0, 360)] public float rotationClampValue = 50;
    //public Vector2 scaleRandom = new Vector2(1.0f, 1.5f);

    [Header("Danger Settings (one flock)")]
    
    public float dangerRadius = 15;
    public float dangerSpeed = 1.5f;
    public float dangerSoaring = 0.5f;
    public LayerMask dangerLayer;
}
