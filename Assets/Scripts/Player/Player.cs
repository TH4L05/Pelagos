/// <author>Thoams Krahl</author>

using System;
using UnityEngine;
using PelagosProject.User.Input;
using PelagosProject.User.Movement;
using UnityEngine.Rendering.HighDefinition;

namespace PelagosProject.User
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : MonoBehaviour
    {
        public static Action<float[]> ShipInformationsChanged;

        [SerializeField] private Transform seaLevel;
        [SerializeField] private GameObject[] shipLights;
        [SerializeField] private Light directionalLight;
        [SerializeField] private HDAdditionalLightData additionalLightData;
        [SerializeField] private float maxIntensity = 20000f;
        [SerializeField] private ParticleSystem speedParticleForward;
        [SerializeField] private ParticleSystem speedParticleBackward;

        private Scanner scanner;
        private PlayerMovement playerMovement;
        private PlayerInput input;
        private float currentDepth;
        private float currentPressure;
        //private GameObject hitGameObject;

        private void Start()
        {
            GetComponents();
        }

        private void Update()
        {
            ToggleFlashLight();
            UpdateMovementParticle();
            //TestRayCast();
        }

        private void LateUpdate()
        {
            GetCurrentDepth();
            GetCurrentPressure();
            UpdateShipInformations();
            UpdateDirectionalLightIntensity();
        }

        private void GetComponents()
        {
            input = Game.Instance.Input;
            scanner = GetComponent<Scanner>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void ToggleFlashLight()
        {
            bool active = input.ToogleFlashlightInputStatus;
            foreach (var light in shipLights)
            {
                light.SetActive(active);
            }
        }

        private void UpdateMovementParticle()
        {
            if (playerMovement.MovementDirectionChanged) return;

            var movementDirection = playerMovement.GetCurrentMovementDirection();
            float[] speedValues = playerMovement.GetCurrentPowerValues();
            Vector3 rotation = playerMovement.GetCurrentRotationVector();

            ParticleSystem.MainModule mainForward = speedParticleForward.main;
            ParticleSystem.MainModule mainBackward = speedParticleBackward.main;

            ParticleSystem.EmissionModule emissionForward = speedParticleForward.emission;
            ParticleSystem.EmissionModule emissionBackward = speedParticleBackward.emission;

            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeForward = speedParticleForward.velocityOverLifetime;
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeBackward = speedParticleBackward.velocityOverLifetime;

            switch (movementDirection)
            {   
                case MovementDirection.Forward:

                    if (speedParticleForward == null) return;

                    mainBackward.startSpeed = new ParticleSystem.MinMaxCurve(0f);
                    emissionBackward.rateOverTime = new ParticleSystem.MinMaxCurve(5f);

                    mainForward.startSpeed = new ParticleSystem.MinMaxCurve(speedValues[0] / 50f);

                    if (speedValues[0] == 0)
                    {
                        emissionForward.rateOverTime = new ParticleSystem.MinMaxCurve(5);
                    }
                    else
                    {
                        emissionForward.rateOverTime = new ParticleSystem.MinMaxCurve((int)(speedValues[0] * 3f));
                    }

                    //velocityOverLifetimeForward.x = new ParticleSystem.MinMaxCurve(rotation.x / 3);
                    //velocityOverLifetimeForward.y = new ParticleSystem.MinMaxCurve(rotation.y / 3);

                    break;


                case MovementDirection.Backward:

                    if (speedParticleBackward == null) return;
                    mainForward.startSpeed = new ParticleSystem.MinMaxCurve(0f);
                    emissionForward.rateOverTime = new ParticleSystem.MinMaxCurve(5);

                    mainBackward.startSpeed = new ParticleSystem.MinMaxCurve(speedValues[0] / 50f);

                    if (speedValues[0] == 0)
                    {
                        emissionBackward.rateOverTime = new ParticleSystem.MinMaxCurve(5);
                    }
                    else
                    {
                        emissionBackward.rateOverTime = new ParticleSystem.MinMaxCurve((int)(speedValues[0] * 3f));
                    }

                    //velocityOverLifetimeBackward.x = new ParticleSystem.MinMaxCurve(rotation.x / 3);
                    //velocityOverLifetimeBackward.y = new ParticleSystem.MinMaxCurve(rotation.y / 3);
                    
                    break;


                default:
                    break;
            }


            
            

            
        }


        //TEST
        /*private void TestRayCast()
        {
            Vector3 rayOrigin = Camera.main.transform.position;
            Vector3 rayDirection = Camera.main.transform.forward;

            Ray ray = new Ray(rayOrigin, rayDirection);
            RaycastHit hit;
            float distance = 999f;
            Color rayColor = Color.magenta;


            if (Physics.Raycast(ray, out hit, 999f))
            {
                hitGameObject = hit.collider.gameObject;
                distance = Vector3.Distance(transform.position, hit.point);
                rayColor = Color.blue;

            }
            else
            {
               
            }

            Debug.DrawRay(ray.origin, ray.direction * distance, rayColor);
        }*/

        #region ShipInfoValues

        private void GetCurrentDepth()
        {
            if (seaLevel == null) return;
            currentDepth = seaLevel.position.y - transform.position.y;
        }

        private void GetCurrentPressure()
        {
            if (seaLevel == null) return;
            currentPressure = currentDepth * 0.1f + 1.0f;
        }

        private void UpdateShipInformations()
        {
            float[] powerValues = playerMovement.GetCurrentPowerValues();
            float[] infoValues = new float[] {currentDepth * -1, currentPressure, powerValues[0], powerValues[1] };
            ShipInformationsChanged?.Invoke(infoValues);
        }

        #endregion

        private void UpdateDirectionalLightIntensity()
        {
            //var intensity = maxIntensity / currentDepth;
            float t = 0.97f;
            float a = Mathf.Pow(t, currentDepth);
            float c = maxIntensity;

            float intensity = c * a;

            //directionalLight.intensity = intensity;
            if(additionalLightData != null) additionalLightData.SetIntensity(intensity);
        }
    }
}

