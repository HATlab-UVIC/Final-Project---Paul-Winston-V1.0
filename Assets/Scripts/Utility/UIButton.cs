using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class UIButton : MonoBehaviour
{
    [SerializeField, Tooltip("Amount of time (seconds) for disappearing animation to be played")]
    private float _dissapearTime = 1f;

    [SerializeField, Tooltip("Scale of the frame at the end of disappeaing animaiton"), Range(0.0f, 1.0f)]
    private float _dissapearScale = 0.5f;

    protected bool _isActive = false;
    protected bool _isHover = false;

    // The last known state before the button was disabled.
    private bool _wasActive = false;
    private ButtonEffect[] _buttonEffects;

    public bool IsActive
    {
        get { return _isActive; }
    }

    private void Start()
    {
        InitializeButtons();
        Default();
    }

    private void OnDisable()
    {
        _wasActive = _isActive;
        Default(true);
    }

    private void OnEnable()
    {
        if (_wasActive)
        {
            Pressed();
        }
    }

    private void Pressed()
    {

    }

    public virtual void Default(bool reset = false)
    {
        if (reset)
        {
            _isActive = false;
        }
        if (IsActiveWithContent())
        {
            return;
        }

        _isHover = false;

        for (int i = 0; i < _buttonEffects.Length; i++)
        {
            if (_buttonEffects[i].Image != null)
            {
                _buttonEffects[i].Image.color = _buttonEffects[i].DefaultColor;
            }

            if (_buttonEffects[i].Text != null)
            {
                _buttonEffects[i].Text.color = _buttonEffects[i].DefaultColor;
            }
        }
    }

    private bool IsActiveWithContent()
    {
        throw new NotImplementedException();
    }

    private void InitializeButtons()
    {

    }

    public class ButtonEffect
    {

    }
}
