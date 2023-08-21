using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.MagicLeap;

public class VoiceIntents : MonoBehaviour
{
    public PlaneManager PlaneManager;

    // implement singleton
    public GameManager GameManager;

    private readonly MLPermissions.Callbacks permissionCallbacks = new MLPermissions.Callbacks();

    [SerializeField, Tooltip("The text used to display status information for the example.")]
    private Text statusText = null;

    // voice intents configuration instance (needs to be assigned in Inspector)
    public MLVoiceIntentsConfiguration VoiceIntentsConfiguration;

    // subscribe to permission events
    private void Awake()
    {
        permissionCallbacks.OnPermissionGranted += OnPermissionGranted;
        permissionCallbacks.OnPermissionDenied += OnPermissionDenied;
        permissionCallbacks.OnPermissionDeniedAndDontAskAgain += OnPermissionDenied;
    }

    // unsubscribe from permission events
    private void OnDestroy()
    {
        permissionCallbacks.OnPermissionGranted -= OnPermissionGranted;
        permissionCallbacks.OnPermissionDenied -= OnPermissionDenied;
        permissionCallbacks.OnPermissionDeniedAndDontAskAgain -= OnPermissionDenied;
    }

    // request permission for voice input at start
    private void Start()
    {
        MLPermissions.RequestPermission(MLPermission.VoiceInput, permissionCallbacks);
    }

    // on voice permission denied, disable script
    private void OnPermissionDenied(string permission)
    {
        Debug.LogError($"Failed to initialize voice intents due to missing or denied {MLPermission.VoiceInput} permission. Please add to manifest. Disabling script.");
        enabled = false;
    }

    // on voice permission granted, initialize voice input
    private void OnPermissionGranted(string permission)
    {
        if (permission == MLPermission.VoiceInput)
            InitializeVoiceInput();
    }


    // check if voice commands setting is enabled, then set up voice intents
    private void InitializeVoiceInput()
    {
        bool isVoiceEnabled = MLVoice.VoiceEnabled;

        // if voice setting is enabled, try to set up voice intents
        if (isVoiceEnabled)
        {
            Debug.Log("Voice commands setting is enabled");
            var result = MLVoice.SetupVoiceIntents(VoiceIntentsConfiguration);
            if (result.IsOk)
            {
                MLVoice.OnVoiceEvent += MLVoiceOnOnVoiceEvent;
            }
            else
            {
                Debug.LogError("Voice could not initialize:" + result);
            }
        }

        // if voice setting is disabled, open voice settings so user can enable it
        else
        {
            Debug.Log("Voice commands setting is disabled - opening settings");
            UnityEngine.XR.MagicLeap.SettingsIntentsLauncher.LaunchSystemVoiceInputSettings();
            Application.Quit();
        }
    }

    // handle voice events
    private void MLVoiceOnOnVoiceEvent(in bool wasSuccessful, in MLVoice.IntentEvent voiceEvent)
    {
        if (wasSuccessful)
        {
            if (voiceEvent.EventID == 101)
            {
                Debug.Log("Voice Command: Lock");
                statusText.text = "Voice Command: Lock";
                PlaneManager.Lock();
            }
            if (voiceEvent.EventID == 102)
            {
                Debug.Log("Voice Command: Unlock");
                statusText.text = "Voice Command: Unlock";
                PlaneManager.Unlock();
            }
            if (voiceEvent.EventID == 103)
            {
                Debug.Log("Voice Command: Open Window");
                statusText.text = "Voice Command: Open Window";
                PlaneManager.OpenWindow();
            }
            if (voiceEvent.EventID == 104)
            {
                Debug.Log("Voice Command: Close Windows");
                statusText.text = "Voice Command: Close Window";
                PlaneManager.CloseWindow();
            }
            if (voiceEvent.EventID == 105)
            {
                Debug.Log("Voice Command: Quit Application");
                statusText.text = "Voice Command: Quit Application";
                GameManager.QuitApplication();
            }

        }
    }
}
