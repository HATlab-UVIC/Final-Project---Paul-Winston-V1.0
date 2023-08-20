using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.XR.Interaction;
using System.Threading;
using UnityEngine.Events;

public class UserInterface : MonoBehaviour
{
    [SerializeField, Tooltip("Amount of time (seconds) for disappearing animation to be played")]
    private float _dissapearTime = 0.5f;

    [Tooltip("Add buttons in the Main Menu")]
    public GameObject[] MainMenuUI;
    [Tooltip("Add buttons in the Practioner Menu")]
    public GameObject[] PractionerMenuUI;
    [Tooltip("Add buttons in the Patient Menu")]
    public GameObject[] PatientMenuUI;

    private CurrentMenu _currentMenu = CurrentMenu.MainMenu;

    public static UnityEvent UIUpdated;

    private void Awake()
    {
        UIUpdated = new UnityEvent();
        UIUpdated.AddListener(_OnUIChanged);
    }

    private void Start()
    {
        UIUpdated.Invoke();
    }

    public enum CurrentMenu
    {
        MainMenu = 0,
        PractionerMenu = 1,
        PatientMenu = 2,
        Empty = 3,
    }

    public void OnClick(String name)
    {
        switch (_currentMenu)
        {
            case CurrentMenu.MainMenu:
                StartCoroutine(MainMenuSelected(name));
                return;
            case CurrentMenu.PractionerMenu:
                StartCoroutine(PractitionerMenuSelected(name));
                return;
            case CurrentMenu.PatientMenu:
                StartCoroutine(PatientMenuSelected(name));
                return;
        }
    }

    private IEnumerator PractitionerMenuSelected(string name)
    {
        for (int i = 0; i < PractionerMenuUI.Length; i++)
        {
            Animator anim = PractionerMenuUI[i].GetComponent<Animator>();
            if (name != PractionerMenuUI[i].name)
            {
                anim.Play("Disabled", -1, _dissapearTime);
            }
        }
        yield return new WaitForSeconds(_dissapearTime);

        // back button selected
        if (name == PractionerMenuUI[0].name)
        {
            _currentMenu = CurrentMenu.MainMenu;
            UIUpdated.Invoke();
        }
        // button not defined yet
        else if (name == PractionerMenuUI[1].name)
        {
            _currentMenu = CurrentMenu.PractionerMenu;
            UIUpdated.Invoke();
        }
    }

    private IEnumerator PatientMenuSelected(string name)
    {
        for (int i = 0; i < PatientMenuUI.Length; i++)
        {
            Animator anim = PatientMenuUI[i].GetComponent<Animator>();
            if (name != PatientMenuUI[i].name)
            {
                anim.Play("Disabled", -1, _dissapearTime);
            }
        }
        yield return new WaitForSeconds(_dissapearTime);
        
        // back button selected
        if (name == PatientMenuUI[0].name)
        {
            _currentMenu = CurrentMenu.MainMenu;
            UIUpdated.Invoke();
        }
        // other button selected
        else
        {
            _currentMenu = CurrentMenu.Empty;
            UIUpdated.Invoke();
        }
    }

    private IEnumerator MainMenuSelected(String name)
    {
        for (int i = 0; i < MainMenuUI.Length; i++)
        {
            Animator anim = MainMenuUI[i].GetComponent<Animator>();
            if (name != MainMenuUI[i].name)
            {
                anim.Play("Disabled", -1, _dissapearTime);
            }
        }
        yield return new WaitForSeconds(_dissapearTime);
        
        // practioner button selected
        if (name == MainMenuUI[0].name)
        {
            Debug.Log("practioner button selected");
            _currentMenu = CurrentMenu.PractionerMenu;
            UIUpdated.Invoke();
        }
        // patient button selected
        else if (name == MainMenuUI[1].name)
        {
            Debug.Log("patient button selected");
            _currentMenu = CurrentMenu.PatientMenu;
            UIUpdated.Invoke();
        }
    }

    private void _OnUIChanged()
    {
        Debug.Log($"_OnUIChanged _currentMenu: {_currentMenu}");
        switch (_currentMenu)
        {
            case CurrentMenu.MainMenu:
                MainMenuUI[0].transform.parent.gameObject.SetActive(true);
                PractionerMenuUI[0].transform.parent.gameObject.SetActive(false);
                PatientMenuUI[0].transform.parent.gameObject.SetActive(false);
                return;
            case CurrentMenu.PractionerMenu:
                MainMenuUI[0].transform.parent.gameObject.SetActive(false);
                PractionerMenuUI[0].transform.parent.gameObject.SetActive(true);
                PatientMenuUI[0].transform.parent.gameObject.SetActive(false);
                return;
            case CurrentMenu.PatientMenu:
                MainMenuUI[0].transform.parent.gameObject.SetActive(false);
                PractionerMenuUI[0].transform.parent.gameObject.SetActive(false);
                PatientMenuUI[0].transform.parent.gameObject.SetActive(true);
                return;
            case CurrentMenu.Empty:
                MainMenuUI[0].transform.parent.gameObject.SetActive(false);
                PractionerMenuUI[0].transform.parent.gameObject.SetActive(false);
                PatientMenuUI[0].transform.parent.gameObject.SetActive(false);
                return;
        }
    }

    public void ResetMainMenu()
    {
        for (int i = 0; i < MainMenuUI.Length; i++)
        {
            MainMenuUI[i].SetActive(true);
        }
        _currentMenu = CurrentMenu.MainMenu;
        UIUpdated.Invoke();
    }
}
