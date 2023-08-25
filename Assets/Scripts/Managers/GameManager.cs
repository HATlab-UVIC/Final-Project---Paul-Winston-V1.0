using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // implement singleton

    public void QuitApplication()
    {
        Application.Quit();
    }

    private void OnScneneLoaded(Scene scene, LoadSceneMode mode)
    {
        // VoiceIntents.Instance.InitializeVoiceInput();
    }
}
