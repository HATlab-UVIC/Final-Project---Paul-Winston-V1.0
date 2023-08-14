using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.XR.Interaction;
using System.Threading;

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

    public enum CurrentMenu
    {
        MainMenu = 0,
        PractionerMenu = 1,
        PatientMenu = 2
    }

    public void OnClick(String name)
    {
        switch (_currentMenu)
        {
            case CurrentMenu.MainMenu:
                StartCoroutine(MainMenuSelected(name));
                return;
            case CurrentMenu.PractionerMenu:
                return;
            case CurrentMenu.PatientMenu:
                return;
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
        for (int i = 0; i < MainMenuUI.Length; i++)
        {
            MainMenuUI[i].SetActive(false);
        }
    }

    public void ResetMainMenu()
    {
        for (int i = 0; i < MainMenuUI.Length; i++)
        {
            MainMenuUI[i].SetActive(true);
        }
        _currentMenu = CurrentMenu.MainMenu;
    }
}
