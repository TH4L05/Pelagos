/// <author>Thoams Krahl</author>

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PelagosProject.User.Input
{
    public class PlayerInput : MonoBehaviour
    {
        #region Events

        public static Action TakeScreenshot;
        public static Action<bool> ToggleIngameMenu;
        public static Action<bool> SpeedlinkJoystickDeviceChanged;
        public static Action SetupFinished;

        #endregion

        #region PrivateFields

        private Controls controls;
        private List<InputAction> movementInputActions;
        private List<InputAction> baseInputActions;
        private List<InputAction> uiInputActions;
        private List<InputAction> extraInputActions;
        private List<InputAction> uiOptionsInputActions;
        private bool speedLinkJoystickConnected = false;
        private int connectedJoystickVersion = -1;

        private Vector2 moveVector = Vector2.zero;
        private Vector2 moveDetailVector = Vector2.zero;
        private float rotationYglobalValue;
        private float powerInputValue;
        private bool powerInputPlusIsPressed = false;
        private bool powerInputMinusIsPressed = false;
        private bool takePhotoInputIsPressed = false;
        private bool scanInputIsPressed = false;
        private bool toogleFlashlightInputStatus = false;

        public bool MovementActionsAreActive;
        public bool BaseActionsAreActive;
        public bool UIActionsAreActive;
        public bool UIOptionsActionsAreActive;
        public bool ExtraActionsAreActive;

        #endregion

        #region PublicFields

        public Vector2 MoveVector => moveVector;
        public Vector2 MoveExtraVector => moveDetailVector;
        public bool PowerInputMinusIsPressed => powerInputMinusIsPressed;
        public bool PowerInputPlusIsPressed => powerInputPlusIsPressed;
        public bool TakePhotoInputIsPressed => takePhotoInputIsPressed;
        public float PowerInputValue => powerInputValue;
        public bool ScanInputIsPressed => scanInputIsPressed;
        public float RotationYWorld => rotationYglobalValue;
        public bool SpeedLinkJoystickConnected => speedLinkJoystickConnected;
        public int ConnectedJoystickVersion => connectedJoystickVersion;
        public bool ToogleFlashlightInputStatus => toogleFlashlightInputStatus;
        public bool IngameMenuIsActive { get; set; }

        public bool DirectionSwitchActive { get; set; }
        public bool UINavigatUpActive { get; set; }
        public bool UINavigatDownActive { get; set; }
        public bool UINavigatLeftActive { get; set; }
        public bool UINavigatRightActive { get; set; }
        public bool UINavigateNextActive { get; set; }
        public bool UINavigatePreviousActive { get; set; }
        public bool MovementInputPaused { get; set; }
        public bool UIInputPaused { get; set; }
        public bool PhotoGalleryDeletePictureInputActive { get; set; }
        //public bool UIMenuBackActive { get; set; }

        public bool UIoptionsNavigatLeftActive { get; set; }
        public bool UIoptionsNavigatRightActive { get; set; }
        public bool UIoptionsNavigatBackActive { get; set; }

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            controls = new Controls();
            movementInputActions = new List<InputAction>();
            baseInputActions = new List<InputAction>();
            uiInputActions = new List<InputAction>();
            extraInputActions = new List<InputAction>();
            uiOptionsInputActions = new List<InputAction>();
            powerInputValue = -1f;
            InputSystem.onDeviceChange += OnDeviceChange;           
        }

        private void Start()
        {        
            SetBaseInputActions();
            SetMovementInputActions();
            SetUIInputActions();
            SetExtraInputActions();
            SetUIoptionsInputActions();

            JoystickChanged(false, -1, null, InputDeviceChange.UsageChanged);
            if (Joystick.all.Count < 1)
            {
                SetupFinished?.Invoke();
                return;
            }

            if (Joystick.current.description.product == "PS3/PC Gamepad")
            {
                //Debug.Log(Joystick.current.device);
                //Debug.Log(Joystick.current.deviceId);
                //Debug.Log(Joystick.current.description);
                //Debug.Log(Joystick.current.description.manufacturer);
                //Debug.Log(Joystick.current.description.product);

                JoystickChanged(true, 0, null, InputDeviceChange.HardReset);
                Debug.Log("A Speedlink Joystick is Connected");
            }
            else if (Joystick.current.description.product == "Generic   USB  Joystick  ")
            {
                JoystickChanged(true, 1, null, InputDeviceChange.HardReset);
                Debug.Log("Another? Speedlink Joystick is Connected");
            }

            Debug.Log(Joystick.current.description.product + " / " + Joystick.current.description.version);
            SetupFinished?.Invoke();
        }

        private void OnDestroy()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        private void OnDeviceChange(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
        {
            switch (inputDeviceChange)
            {
                case InputDeviceChange.Added:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(true, 0, inputDevice, inputDeviceChange);
                    }
                    else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    {
                        JoystickChanged(true, 1, inputDevice, inputDeviceChange);
                    }
                    break;

                case InputDeviceChange.Removed:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(false, -1, inputDevice, inputDeviceChange);

                    }
                    else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    {
                        JoystickChanged(false, -1, inputDevice, inputDeviceChange);
                    }
                    break;

                case InputDeviceChange.Disconnected:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(false, -1, inputDevice, inputDeviceChange);

                    }
                    else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    {
                        JoystickChanged(false, -1, inputDevice, inputDeviceChange);
                    }
                    break;

                case InputDeviceChange.Reconnected:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(true, 0, inputDevice, inputDeviceChange);
                    }
                    else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    {
                        JoystickChanged(true, 1, inputDevice, inputDeviceChange);
                    }
                    break;

                case InputDeviceChange.Enabled:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(true, 0, inputDevice, inputDeviceChange);
                    }
                    else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    {
                        JoystickChanged(true, 1, inputDevice, inputDeviceChange);
                    }
                    break;

                case InputDeviceChange.Disabled:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(false, -1, inputDevice, inputDeviceChange);

                    }
                    else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    {
                        JoystickChanged(false, -1, inputDevice, inputDeviceChange);
                    }
                    break;

                case InputDeviceChange.UsageChanged:
                    break;

                case InputDeviceChange.ConfigurationChanged:
                    break;

                case InputDeviceChange.SoftReset:
                    break;

                case InputDeviceChange.HardReset:
                    break;
                default:
                    break;
            }
        }

        private void JoystickChanged(bool connected, int versionindex, InputDevice inputDevice, InputDeviceChange inputDeviceChange)
        {
            speedLinkJoystickConnected = connected;
            connectedJoystickVersion = versionindex;
            SpeedlinkJoystickDeviceChanged?.Invoke(speedLinkJoystickConnected);

            if (inputDevice == null) return;        
            Debug.Log($"Speedlink Joystick  -> \"{inputDevice.displayName}\" = {inputDeviceChange}");
        }

        #endregion

        #region SetInputs

        private void SetMovementInputActions()
        {
            var movementInputs = controls.Movement;

            movementInputs.Move.performed += Move_performed;
            movementInputActions.Add(movementInputs.Move);

            movementInputs.Strafe.performed += MoveExtra_performed;
            movementInputActions.Add(movementInputs.Strafe);

            movementInputs.PowerIncreaseKeyboard.performed += PowerInputIncreaseIsPressed;
            movementInputs.PowerIncreaseKeyboard.canceled += PowerInputIncreaseIsPressed;
            movementInputActions.Add(movementInputs.PowerIncreaseKeyboard);

            movementInputs.PowerDecreaseKeyboard.performed += PowerInputDecreaseIsPressed;
            movementInputs.PowerDecreaseKeyboard.canceled += PowerInputDecreaseIsPressed;
            movementInputActions.Add(movementInputs.PowerDecreaseKeyboard);

            movementInputs.SpeedSlider.performed += SpeedTest;
            movementInputActions.Add(movementInputs.SpeedSlider);

            movementInputs.DirectionSwitch.performed += DirectionSwitchIsPressed;
            movementInputActions.Add(movementInputs.DirectionSwitch);

            movementInputs.BaseRotation.performed += BaseRotationValueChanged;
            movementInputActions.Add(movementInputs.BaseRotation);

            SetMovementInputActionsStatus(true);
        }

        private void SetBaseInputActions()
        {
            var baseInputs = controls.Base;

            baseInputs.TakePhoto.performed += TakePhotoInput;
            baseInputs.TakePhoto.canceled += TakePhotoInput;
            baseInputActions.Add(baseInputs.TakePhoto);

            baseInputs.ScanMode.performed += ScanInputChanged;
            baseInputs.ScanMode.canceled += ScanInputChanged;
            baseInputActions.Add(baseInputs.ScanMode);

            baseInputs.ToggleFlashlights.performed += ToggleFlashLightStatus;
            baseInputActions.Add(baseInputs.ToggleFlashlights);

            SetBaseInputActionsStatus(true);
        }

        private void SetExtraInputActions()
        {
            var extraInputs = controls.Extra;
            extraInputs.ToggleIngameMenu.performed += ToggleIngameMenuStatus;
            extraInputActions.Add(extraInputs.ToggleIngameMenu);
            SetExtraInputActionsStatus(true);
        }

        private void SetUIInputActions()
        {
            var uiInputs = controls.UI;
            uiInputs.NavigateUp.performed += NavigateUpIsPressed;
            uiInputs.NavigateUp.canceled += NavigateUpIsCanceled;
            uiInputActions.Add(uiInputs.NavigateUp);

            uiInputs.NavigateDown.performed += NavigateDownPressed;
            uiInputs.NavigateDown.canceled += NavigateDownCanceled;
            uiInputActions.Add(uiInputs.NavigateDown);

            uiInputs.NavigateLeft.performed += NavigateLeftPressed;
            uiInputs.NavigateLeft.canceled += NavigateLeftCanceled;
            uiInputActions.Add(uiInputs.NavigateLeft);

            uiInputs.NavigateRight.performed += NavigateRightPressed;
            uiInputs.NavigateRight.canceled += NavigateRightCanceled;
            uiInputActions.Add(uiInputs.NavigateRight);

            uiInputs.NavigateNext.performed += NavigateNext_performed;
            uiInputActions.Add(uiInputs.NavigateNext);

            uiInputs.NavigatePrevious.performed += NavigatePrevious_performed;
            uiInputActions.Add(uiInputs.NavigatePrevious);

            uiInputs.PhotoGalleryDeletePicture.performed += PhotoGalleryDeletePicture_performed;
            uiInputs.PhotoGalleryDeletePicture.canceled += PhotoGalleryDeletePicture_canceled;
            uiInputActions.Add(uiInputs.PhotoGalleryDeletePicture);


            //uiInputs.MenuBack.performed += MenuBack_performed;
            //uiInputs.MenuBack.canceled += MenuBack_canceled;
            //uiInputActions.Add(uiInputs.MenuBack);

            //SetUIInputActionsStatus(true);
        }

        private void SetUIoptionsInputActions()
        {
            var uiOptionsInputs = controls.UIoptions;

            uiOptionsInputs.OptionsSubSectionBack.performed += OptionsSubSectionBack_performed;
            uiOptionsInputActions.Add(uiOptionsInputs.OptionsSubSectionBack);

            uiOptionsInputs.OptionsSubSectionNavigateRight.performed += OptionsSubSectionNavigateRight_performed;
            uiOptionsInputs.OptionsSubSectionNavigateRight.canceled += OptionsSubSectionNavigateRight_canceled;
            uiOptionsInputActions.Add(uiOptionsInputs.OptionsSubSectionNavigateRight);

            uiOptionsInputs.OptionsSubSectionNavigateLeft.performed += OptionsSubSectionNavigateLeft_performed;
            uiOptionsInputs.OptionsSubSectionNavigateLeft.canceled += OptionsSubSectionNavigateLeft_canceled;
            uiOptionsInputActions.Add(uiOptionsInputs.OptionsSubSectionNavigateLeft);
        }

        private void OptionsSubSectionNavigateLeft_canceled(InputAction.CallbackContext obj)
        {
            UIoptionsNavigatLeftActive = false;
        }

        private void OptionsSubSectionNavigateRight_canceled(InputAction.CallbackContext obj)
        {
            UIoptionsNavigatRightActive = false;
        }

        private void OptionsSubSectionNavigateLeft_performed(InputAction.CallbackContext obj)
        {
            UIoptionsNavigatLeftActive = true;
        }

        private void OptionsSubSectionNavigateRight_performed(InputAction.CallbackContext obj)
        {
            UIoptionsNavigatRightActive = true;
        }

        private void OptionsSubSectionBack_performed(InputAction.CallbackContext obj)
        {
            UIoptionsNavigatBackActive = true;
        }

        #endregion

        #region Enable/DisableInputActions

        public void SetMovementInputActionsStatus(bool enable)
        {
            MovementActionsAreActive = enable;
            if (enable)
            {
                foreach (var inputAction in movementInputActions)
                {
                    inputAction.Enable();
                }
            }
            else
            {
                foreach (var inputAction in movementInputActions)
                {
                    inputAction.Disable();
                }
            }
        }

        public void SetBaseInputActionsStatus(bool enable)
        {
            BaseActionsAreActive = enable;
            if (enable)
            {
                foreach (var inputAction in baseInputActions)
                {
                    inputAction.Enable();
                }
            }
            else
            {
                foreach (var inputAction in baseInputActions)
                {
                    inputAction.Disable();
                }
            }
        }

        public void SetExtraInputActionsStatus(bool enable)
        {       
            ExtraActionsAreActive = enable;
            if (enable)
            {
                foreach (var inputAction in extraInputActions)
                {
                    inputAction.Enable();
                }
            }
            else
            {
                foreach (var inputAction in extraInputActions)
                {
                    inputAction.Disable();
                }
            }
        }

        public void SetUIInputActionsStatus(bool enable)
        {
            UIActionsAreActive = enable;
            if (enable)
            {
                foreach (var inputAction in uiInputActions)
                {
                    inputAction.Enable();
                }
            }
            else
            {
                foreach (var inputAction in uiInputActions)
                {
                    inputAction.Disable();
                }
            }
        }

        public void SetUiOptionsInputActionsStatus(bool enable)
        {
            UIOptionsActionsAreActive = enable;
            if (enable)
            {
                foreach (var inputAction in uiOptionsInputActions)
                {
                    inputAction.Enable();
                }
            }
            else
            {
                foreach (var inputAction in uiOptionsInputActions)
                {
                    inputAction.Disable();
                }
            }
        }

        public void ChangeAllInputActionListStatus(bool enable)
        {
            SetMovementInputActionsStatus(enable);
            SetBaseInputActionsStatus(enable);
            SetExtraInputActionsStatus(enable);
            SetUIInputActionsStatus(enable);
            SetUiOptionsInputActionsStatus(enable);
        }

        #endregion

        #region MovementInput

        private void PowerInputIncreaseIsPressed(InputAction.CallbackContext context)
        {
            powerInputPlusIsPressed = !powerInputPlusIsPressed;
        }

        private void PowerInputDecreaseIsPressed(InputAction.CallbackContext context)
        {
            powerInputMinusIsPressed = !powerInputMinusIsPressed;
        }

        private void Move_performed(InputAction.CallbackContext context)
        {
            moveVector = context.ReadValue<Vector2>();
        }
        private void MoveExtra_performed(InputAction.CallbackContext context)
        {
            moveDetailVector = context.ReadValue<Vector2>();
        }

        private void SpeedTest(InputAction.CallbackContext context)
        {
            powerInputValue = context.ReadValue<float>();
        }

        private void DirectionSwitchIsPressed(InputAction.CallbackContext context)
        {
            DirectionSwitchActive = true;
        }

        private void BaseRotationValueChanged(InputAction.CallbackContext context)
        {
            rotationYglobalValue = context.ReadValue<float>();
        }

        #endregion

        #region OtherInput

        private void TakePhotoInput(InputAction.CallbackContext context)
        {

            takePhotoInputIsPressed = !takePhotoInputIsPressed;
            TakeScreenshot?.Invoke();
        }

        private void ScanInputChanged(InputAction.CallbackContext context)
        {
            scanInputIsPressed = !scanInputIsPressed;
        }

        private void ToggleIngameMenuStatus(InputAction.CallbackContext context)
        {
            IngameMenuIsActive = !IngameMenuIsActive;
            //ToggleIngameMenu?.Invoke(ingameMenuIsActive);
        }
      
        private void ToggleFlashLightStatus(InputAction.CallbackContext context)
        {
            toogleFlashlightInputStatus = !toogleFlashlightInputStatus;
        }

        #endregion

        #region UIInput

        private void NavigateRightPressed(InputAction.CallbackContext context)
        {
            UINavigatRightActive = true;
        }

        private void NavigateRightCanceled(InputAction.CallbackContext context)
        {
            UINavigatRightActive = false;
        }

        private void NavigateLeftPressed(InputAction.CallbackContext context)
        {
            UINavigatLeftActive = true;
        }

        private void NavigateLeftCanceled(InputAction.CallbackContext context)
        {
            UINavigatLeftActive = false;
        }

        private void NavigateDownPressed(InputAction.CallbackContext context)
        {         
            UINavigatDownActive = true;
        }

        private void NavigateDownCanceled(InputAction.CallbackContext context)
        {
            UINavigatDownActive = false;
        }

        private void NavigateUpIsPressed(InputAction.CallbackContext context)
        {
            UINavigatUpActive = true;
        }

        private void NavigateUpIsCanceled(InputAction.CallbackContext context)
        {
            UINavigatUpActive = false;
        }

        private void NavigatePrevious_performed(InputAction.CallbackContext obj)
        {
            UINavigatePreviousActive = true;
        }

        private void NavigateNext_performed(InputAction.CallbackContext obj)
        {
            UINavigateNextActive = true;
        }

        private void PhotoGalleryDeletePicture_performed(InputAction.CallbackContext obj)
        {
            PhotoGalleryDeletePictureInputActive = true;
        }
        private void PhotoGalleryDeletePicture_canceled(InputAction.CallbackContext obj)
        {
            PhotoGalleryDeletePictureInputActive = false;
        }

        //private void MenuBack_canceled(InputAction.CallbackContext obj)
        //{
        //    UIMenuBackActive = true;
        //}

        //private void MenuBack_performed(InputAction.CallbackContext obj)
        //{
        //    UIMenuBackActive = false;
        //}

        #endregion
    }
}

