using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.MagicLeap;
using static MagicLeapInputs;

public class EyeTrackingManager : MonoBehaviour
{
    // private Transform attachTranform;
    private Transform rayOriginTransform;
    private Transform leftEyeTranformTrackedPoseDriver;
    private Transform rightEyeTranformTrackedPoseDriver;
    private Vector3 leftEyePositionInputSystem;
    private Quaternion leftEyeRotationInputSystem;
    private Vector3 rightEyePositionInputSystem;
    private Quaternion rightEyeRotationInputSystem;

    [SerializeField, Tooltip("Eye Controller in the XR Rig to enable and disable Ray Interactor")]
    private GameObject EyesController;

    [SerializeField, Tooltip("Main Camera Transform data for ray origin")]
    private Transform mainCameraTransform;
    private MagicLeapInputs playerInputs;

    public Text statusText;

    // Used to get other eye data
    private InputDevice eyesDevice;

    // Used to get eyes action data.
    private MagicLeapInputs.EyesActions eyesActions;

    // Used to get ml inputs.
    private MagicLeapInputs mlInputs;

    // Was EyeTracking permission granted by user
    [HideInInspector]
    public bool permissionGranted;
    private readonly MLPermissions.Callbacks permissionCallbacks = new MLPermissions.Callbacks();

    private void Awake()
    {
        playerInputs = new MagicLeapInputs();
        permissionGranted = false;

        permissionCallbacks.OnPermissionGranted += OnPermissionGranted;
        permissionCallbacks.OnPermissionDenied += OnPermissionDenied;
        permissionCallbacks.OnPermissionDeniedAndDontAskAgain += OnPermissionDenied;
    }

    private void Start()
    {
        mlInputs = new MagicLeapInputs();
        mlInputs.Enable();
        MLPermissions.RequestPermission(MLPermission.EyeTracking, permissionCallbacks);
        if (permissionGranted)
        {
            rayOriginTransform = mainCameraTransform;
            leftEyeTranformTrackedPoseDriver = this.transform.GetChild(0).transform;
            rightEyeTranformTrackedPoseDriver = this.transform.GetChild(1).transform;
            statusText.text += "Eye Tracking is active";
        }
        else
        {
            statusText.text += "Eye Tracking is not active";
        }
    }

    private void Update()
    {
        if (!permissionGranted)
        {
            return;
        }
        if (!eyesDevice.isValid)
        {
            this.eyesDevice = InputSubsystem.Utils.FindMagicLeapDevice(InputDeviceCharacteristics.EyeTracking | InputDeviceCharacteristics.TrackedDevice);
            return;
        }
    }

    private void OnDestroy()
    {
        permissionCallbacks.OnPermissionGranted -= OnPermissionGranted;
        permissionCallbacks.OnPermissionDenied -= OnPermissionDenied;
        permissionCallbacks.OnPermissionDeniedAndDontAskAgain -= OnPermissionDenied;

        mlInputs.Disable();
        mlInputs.Dispose();

        InputSubsystem.Extensions.MLEyes.StopTracking();
    }

    private void OnPermissionDenied(string permission)
    {
        MLPluginLog.Error($"{permission} denied, example won't function.");
        statusText.text += $"Failed to create Planes Subsystem due to missing or denied {MLPermission.EyeTracking} permission. Please add to manifest. Disabling script.";
    }

    private void OnPermissionGranted(string permission)
    {
        InputSubsystem.Extensions.MLEyes.StartTracking();
        eyesActions = new MagicLeapInputs.EyesActions(mlInputs);
        permissionGranted = true;
    }

    public void OnStopEyeTrackingByVoiceIntent()
    {
        InputSubsystem.Extensions.MLEyes.StopTracking();
        try
        {
            foreach (Transform child in this.transform) {
                child.gameObject.SetActive(false);
            }
            EyesController.SetActive(false);
        }
        catch (Exception e)
        {
            statusText.text += e.ToString();
        }
        statusText.text += "Eye tracking disabled by voice intent";
    }
    
    public void OnStartEyeTrackingByVoiceIntent()
    {
        InputSubsystem.Extensions.MLEyes.StartTracking();
        try
        {
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(true);
            }
            EyesController.SetActive(true); ;
        }
        catch (Exception e)
        {
            statusText.text += e.ToString();
        }
        statusText.text += "Eye tracking enabled by voice intent";
    }
}
