using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Diagnostics;
using System.Reflection; // This is needed to use UnityEditor.EditorApplication.isPlaying
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject PauseUI;
    public GameObject instructions;
    private string MainMenuScene = "MainMenu";
    public Button resumeButton;
    public bool cursorCheck = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if(cursorCheck)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        instructions.SetActive(true);
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        instructions.SetActive(false);
    }
}
