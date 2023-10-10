using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.MagicLeap;
using UnityEngine.XR;
using UnityEngine.UI;
using static MagicLeapInputs;
using InputDevice = UnityEngine.XR.InputDevice;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;

public class EyeRayInteractor : MonoBehaviour
{
    public Text statusText;

    [SerializeField, Tooltip("Transform of the origin of the ray")]
    private Transform rayOriginTransform;
    [SerializeField, Tooltip("Transform of the main camera")]
    private Transform mainCamera;

    public InputActionProperty eyeInputProperty;

    // Used to get ml inputs.
    private MagicLeapInputs mlInputs;

    // Used to get other eye data
    private InputDevice eyesDevice;

    // Used to get eyes action data.
    [SerializeField, Tooltip("Used to get eyes action data.")]
    private MagicLeapInputs.EyesActions eyesActions;

    //reference to gamebojcet in scene
    public EyeTrackingManager EyeTrackingManager;
    private bool permissionGranted;

    public GameObject fixationPoint;
    private Vector3 rayRotationFocusPointPosition;

    public XRRayInteractor XRRayInteractor;

    void Update()
    {
        permissionGranted = EyeTrackingManager.permissionGranted;
        if (permissionGranted)
        {
            OnPermissionGranted();
            Debug.Log("Permission is granted");
        }
        else
        {
            Debug.Log("Permission not granted");
            return;
        }

        if (!eyesDevice.isValid)
        {
            this.eyesDevice = InputSubsystem.Utils.FindMagicLeapDevice(InputDeviceCharacteristics.EyeTracking | InputDeviceCharacteristics.TrackedDevice);
            return;
        }

        UnityEngine.InputSystem.XR.Eyes temp = eyeInputProperty.action.ReadValue<UnityEngine.InputSystem.XR.Eyes>();
        fixationPoint.transform.position = Vector3.Lerp(fixationPoint.transform.position, temp.fixationPoint, 0.1f);

        rayRotationFocusPointPosition = Vector3.Lerp(rayRotationFocusPointPosition, temp.fixationPoint, 0.1f);
        rayOriginTransform.position = mainCamera.position;
        rayOriginTransform.LookAt(rayRotationFocusPointPosition);

    }
    private void OnPermissionGranted()
    {
        eyesActions = new MagicLeapInputs.EyesActions(mlInputs);
    }

    public void OnClickByEye()
    {
        statusText.text = "On Click By Eye triggered by Eye Controller in XR Rig";
        XRRayInteractor xrRayInteractorEyeTracking = XRRayInteractor.GetComponent<XRRayInteractor>();

        if (xrRayInteractorEyeTracking.interactablesSelected.Count != 0)
        {
            if (xrRayInteractorEyeTracking.interactablesSelected[0].transform.gameObject.GetComponent<Button>() != null)
            {
                xrRayInteractorEyeTracking.interactablesSelected[0].transform.gameObject.GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}
