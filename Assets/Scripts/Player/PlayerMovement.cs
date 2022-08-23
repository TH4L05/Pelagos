/// <author>Thoams Krahl</author>

using UnityEngine;
using PelagosProject.User.Input;
using PelagosProject.Audio;


namespace PelagosProject.User.Movement
{
    public enum MovementDirection
    {
        Forward,
        Backward
    }

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField] private MovementData movementData;
        [SerializeField] private bool useRigidbodyMovement = false;

        [Header("Audio")]
        [SerializeField] private AudioEventList shipAudioList;

        //[SerializeField] private EventReference engineAudioEvent;

        [Header("VFX")]
        [SerializeField] private GameObject dustVFX;
       
        #endregion

        #region PrivateFields

        private PlayerInput input;
        private Rigidbody rb;
        private Vector3 rotation = Vector3.zero;
        private Vector3 detailMovement = Vector3.zero;
        private Vector3 rotationMomentum = Vector3.zero;
        private Vector3 movementMomentum = Vector3.zero;
        private FMOD.Studio.EventInstance audioEventInstance;

        private float inputPower;
        private float currentPower = 0f;
        private float currenMaxPower;
        private float rbVelocityMag;
        private float rotX;
        private float rotZ;

        private MovementDirection movementDirection;
        private MovementDirection lastMovementDirection;
        private bool movementDirectionChanged;
        private bool canPerformDetailMovement = true;
        private bool pauseMovement;
        private bool isBouncing;

        #endregion

        public bool MovementDirectionChanged => movementDirectionChanged;

        #region UnityFunctions

        private void Start()
        {
            Setup();
        }

        private void Update()
        {
            if (movementData == null) return;
            if (pauseMovement) return;

            UpdatePower();
            UpdateRotation();
            GlobalRotationY();
        }

        private void FixedUpdate()
        {
            if (movementData == null) return;
            if (pauseMovement) return;
            if (isBouncing) return;

            CheckMovementDirection();
            UpdatePosition();
            UpdateDetailMovement();
        }

        private void OnDestroy()
        {
            audioEventInstance.release();          
        }

        #endregion

        private void Setup()
        {
            input = Game.Instance.Input;
            rb = GetComponent<Rigidbody>();
            //rb.drag = movementData.PowerDrag;
            movementDirection = MovementDirection.Forward;

            
            shipAudioList.CreateEvent("ShipEngine");
        }

        #region Rotation

        private void GlobalRotationY()
        {
            float rotationValue = input.RotationYWorld * movementData.RotationSensitivityGlobalY * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationValue);
        }

        private void UpdateRotation()
        {
            float moveX = input.MoveVector.y * movementData.RotationSensitivityX;
            float moveY = input.MoveVector.x * movementData.RotationSensitivityY;
            float moveZ = input.MoveVector.x * movementData.RotationSensitivityZ * Time.deltaTime;
 
            rotZ -= moveZ;

            //rotX = Mathf.Clamp(rotX, -85f, 85f);
            //rotY = Mathf.Clamp(rotY, -85f, 85f);
            rotZ = Mathf.Clamp(rotZ, -35f, 35f);

            rotation = new Vector3(moveX, moveY, 0f);
            rotation *= Time.deltaTime;

            rotationMomentum += new Vector3(rotation.x, rotation.y, 0f);
            rotationMomentum.z = rotZ;
            transform.Rotate(rotation);

            Vector3 targetRotation = transform.eulerAngles;
             targetRotation.z = rotZ;
            transform.eulerAngles = targetRotation;
       

            if (rotationMomentum.magnitude > 0.1f)
            {
                rotationMomentum -= Time.deltaTime * movementData.DragRotation * rotationMomentum;
                transform.Rotate(Time.deltaTime * new Vector3(rotationMomentum.x, rotationMomentum.y, 0f));
                rotZ = rotationMomentum.z;

                targetRotation = transform.eulerAngles;
                targetRotation.z = rotZ;
                transform.eulerAngles = targetRotation;
            }

            if (rotationMomentum.magnitude < 0.1f)
            {
                rotationMomentum = Vector3.zero;
                rotZ = 0f;
            }


            //Clamp X Axis Rotation
            rotX = targetRotation.x;

            if (rotX > 0f && rotX < 90f)
            {
                rotX = Mathf.Clamp(rotX, 0f, 75f);
            }
            else if (rotX > 270f && rotX < 360f)
            {
                rotX = Mathf.Clamp(rotX, 295f, 360);
            }
            targetRotation.x = rotX;
            transform.eulerAngles = targetRotation;


        }

        #endregion

        #region Power

        private void UpdatePower()
        {
            if (movementDirectionChanged) return;

            if (input.SpeedLinkJoystickConnected)
            {
                UpdatePowerJoystick();
            }
            else
            {
                UpdatePowerKeyboard();
            }

            var audioParam = currentPower / movementData.PowerSpeedMax;
            var eventInstance = shipAudioList.GetInstance("ShipEngine");
            eventInstance.setParameterByName("ShipPower", audioParam);

            if (currenMaxPower < currentPower)
            {
                currentPower -= Time.deltaTime * movementData.PowerDrag ;

                if (currentPower <= currenMaxPower)
                {
                    currentPower = currenMaxPower;
                }
            }
            else
            {
                if (currenMaxPower > 0f)
                {
                    //audioEventInstance.start();
                    currentPower += Time.deltaTime * movementData.Accerlation;
                    canPerformDetailMovement = false;

                    if (currentPower >= currenMaxPower)
                    {
                        currentPower = currenMaxPower;
                    }
                }

                else
                {
                    currentPower -= Time.deltaTime * movementData.PowerDrag;

                    if (currentPower <= 0.1f)
                    {
                        currentPower = 0f;
                        canPerformDetailMovement = true;
                    }
                }
            }

            rbVelocityMag = rb.velocity.magnitude;
        }

        private void UpdatePowerKeyboard()
        {
            if (input.PowerInputPlusIsPressed)
            {
                inputPower += Time.deltaTime / 6f;

                if (inputPower > 2f)
                {
                    inputPower = 2f;
                }

            }

            if (input.PowerInputMinusIsPressed)
            {
                inputPower -= Time.deltaTime / 3f;

                if (inputPower < 0.1f)
                {
                    inputPower = 0;
                }

            }
             
            currenMaxPower = (inputPower * movementData.PowerSpeedMax) / 2;        
        }

        private void UpdatePowerJoystick()
        {
            inputPower = (-1 - input.PowerInputValue) * -1;
            //inputPower = (input.PowerInputValue * -1) + 1;                       
            currenMaxPower = (inputPower * movementData.PowerSpeedMax) / 2;
        }

        #endregion

        #region MovementDirection

        private void CheckMovementDirection()
        {
            if (input.DirectionSwitchActive)
            {
                switch (movementDirection)
                {
                    case MovementDirection.Forward:
                        movementDirection = MovementDirection.Backward;
                        break;
                    case MovementDirection.Backward:
                        movementDirection = MovementDirection.Forward;
                        break;
                    default:
                        break;
                }
                input.DirectionSwitchActive = false;
            }

            if (lastMovementDirection != movementDirection && movementDirectionChanged == false) movementDirectionChanged = true;
            lastMovementDirection = movementDirection;

            MovementDirectionIsChanged();
        }

        private void MovementDirectionIsChanged()
        {
            if (movementDirectionChanged)
            {
                if (useRigidbodyMovement)
                {
                    switch (movementDirection)
                    {
                        case MovementDirection.Forward:
                            rb.velocity = currentPower  * -transform.forward;
                            break;
                        case MovementDirection.Backward:
                            rb.velocity =  currentPower * transform.forward;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (movementDirection)
                    {
                        case MovementDirection.Forward:
                            transform.position += Time.deltaTime * currentPower * -transform.forward;
                            break;
                        case MovementDirection.Backward:
                            transform.position += Time.deltaTime * currentPower * transform.forward;
                            break;
                        default:
                            break;
                    }
                }

                if (currenMaxPower >= 0)
                {
                    currentPower -= Time.deltaTime * movementData.PowerDrag;

                    if (currentPower <= 0.1f)
                    {
                        movementDirectionChanged = false;
                        currentPower = 0f;
                    }
                }
            }
        }

        #endregion

        #region Movement

        private void UpdateDetailMovement()
        {
            if (!canPerformDetailMovement)
            {
                detailMovement = Vector3.zero;
                return;
            }

            Vector2 detailMove = input.MoveExtraVector;
            detailMovement = transform.right * detailMove.x + transform.up * detailMove.y;
            transform.Translate(Time.deltaTime * movementData.ExtraMovementSpeed * detailMovement);

            movementMomentum += detailMovement;

            if (movementMomentum.magnitude > 0.1f)
            {
                transform.position += Time.deltaTime * movementMomentum;
                movementMomentum -= Time.deltaTime * movementData.DragMove * 3f * movementMomentum;
            }

            if (movementMomentum.magnitude < 0.2f)
            {
                movementMomentum = Vector3.zero;
            }
        }

        private void UpdatePosition()
        {
            if (movementDirectionChanged) return;
            if (isBouncing) return;

            if (currentPower != 0)
            {
                switch (movementDirection)
                {
                    case MovementDirection.Forward:
                        ForwardMove();
                        break;

                    case MovementDirection.Backward:
                        BackwardMove();
                        break;

                    default:
                        break;
                }
            }
        }

        private void ForwardMove()
        {
            if (useRigidbodyMovement)
            {
                //rb.AddForce(transform.forward * currentPower * rbforceMultiplier, ForceMode.Force);
                rb.velocity = currentPower * transform.forward;
            }
            else
            {
                transform.position += Time.deltaTime * currentPower * transform.forward;
            }
        }

        private void BackwardMove()
        {
            if (useRigidbodyMovement)
            {
                //rb.AddForce(-transform.forward * currentPower * rbforceMultiplier, ForceMode.Force);
                rb.velocity = currentPower * -transform.forward;
            }
            else
            {
                transform.position += Time.deltaTime * currentPower * -transform.forward;
            }
        }



        #endregion

        public float[] GetCurrentPowerValues()
        {
            float[] powerValues = new float[] { currentPower, movementData.PowerSpeedMax };
            return powerValues;
        }

        public Vector3 GetCurrentRotationVector()
        {
            return rotation;
        }

        public MovementDirection GetCurrentMovementDirection()
        {
            return lastMovementDirection;
        }

        public void InstantStop()
        {
            currentPower = 0f;
            currenMaxPower = 0f;
            rotationMomentum = Vector3.zero;

        }

        public void PauseMovement(bool pause)
        {
            pauseMovement = pause;
        }


        #region Collision

        private void OnCollisionEnter(Collision collision)
        {

            //UnityEngine.Debug.Log("Collision");

            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Instantiate(dustVFX, collision.contacts[0].point, Quaternion.identity);
            }

            if (isBouncing) return;           
            if (collision.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
            if (collision.gameObject.layer != LayerMask.NameToLayer("Environment")) return;
            StartBounce(collision);         
        }


        private void StartBounce(Collision collision)
        {
            isBouncing = true;
            float bounce = rb.velocity.magnitude * currentPower;
            //rb.AddForce(collision.contacts[0].normal * bounce);

            Vector3 direction = collision.transform.position - transform.position;
            direction.y = 0f;
            direction = direction.normalized;
            //rb.AddForce(-direction * bounce);

            rb.AddExplosionForce(bounce, collision.contacts[0].point, 50f);
            rb.velocity = Vector3.zero;
            Invoke("StopBounce", 0.3f);
        }

        private void StopBounce()
        {
            isBouncing = false;
        }

        #endregion

        #region VFX



        #endregion
    }

}

