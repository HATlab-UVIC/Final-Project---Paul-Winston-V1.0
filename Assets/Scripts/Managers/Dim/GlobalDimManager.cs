using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class GlobalDimManager : MonoBehaviour
{
    public SceneInfo sceneInfo;

    private float GlobalDimmingValue = 0f;
    private bool GlobalDimmingIsOn = false;
    private float dimmerDelta;

    [SerializeField, Tooltip("Amount of time for Global Dimming to take an effect"), Range(0f, 5f)]
    private float duration = 1.0f;

    [SerializeField, Tooltip("The text used to display status information for the example.")]
    private Text statusText = null;

    public static UnityEvent<bool> dimVoiceCommand;

    private void Awake()
    {
        dimVoiceCommand = new UnityEvent<bool>();
        dimVoiceCommand.AddListener(_OnGlobalDimVoiceCommand);
        dimmerDelta = duration * Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        this.GlobalDimmingIsOn = sceneInfo.GlobalDimmingIsOn;
    }

    private void _OnGlobalDimVoiceCommand(bool globalDimIsON)
    {
        if (globalDimIsON)
        {
            StartCoroutine(OnGlobalDimOn());
        }
        else
        {
            StartCoroutine(OnGlobalDimOff());
        }
    }

    private IEnumerator OnGlobalDimOn()
    {
        if (!GlobalDimmingIsOn)
        {
            for (GlobalDimmingValue = 0f; GlobalDimmingValue <= 1.0f; GlobalDimmingValue += dimmerDelta)
            {
                MLGlobalDimmer.SetValue(GlobalDimmingValue);
                yield return new WaitForFixedUpdate();
            }
            GlobalDimmingIsOn = true;
            sceneInfo.GlobalDimmingIsOn = true;
            statusText.text += "\nGlobal Dimming is ON";
        }
        else
        {
            statusText.text += "\nGlobal Dimming is already ON. No change occured";
        }
        
    }

    private IEnumerator OnGlobalDimOff()
    {
        if (GlobalDimmingIsOn)
        {
            for (GlobalDimmingValue = 1.0f; GlobalDimmingValue >= 0f; GlobalDimmingValue -= dimmerDelta)
            {
                MLGlobalDimmer.SetValue(GlobalDimmingValue);
                yield return new WaitForFixedUpdate();
            }
            GlobalDimmingIsOn = false;
            sceneInfo.GlobalDimmingIsOn = false;
            statusText.text += "\nGlobal Dimming is OFF";
        }
        else
        {
            statusText.text += "\nGlobal Dimming is already OFF. No change occured";
        }
        
    }

    private void FixedUpdate()
    {

    }
}
