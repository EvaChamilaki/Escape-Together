using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Main Menu Panel
    [Tooltip("Main Menu Panel")]
    [SerializeField]
    private GameObject mainMenu;

    // Instructions Menu Panel
    [Tooltip("Instructions Menu Panel")]
    [SerializeField]
    private GameObject instructionsMenu;

    // Settings Panel
    [Tooltip("Settings Panel")]
    [SerializeField]
    private GameObject settingsMenu;



   public void OnStartButton()
   {
    SceneManager.LoadScene(1);
   }

   public void OnExitButton()
   {
    Application.Quit();
   }

   public void OnInstructionsButton()
   {
    mainMenu.SetActive(false);
    instructionsMenu.SetActive(true);
   }

   public void OnSettingsButton()
   {
    mainMenu.SetActive(false);
    settingsMenu.SetActive(true);
   }

   public void OnBackButton()
   {
    instructionsMenu.SetActive(false);
    settingsMenu.SetActive(false);
    mainMenu.SetActive(true);
   }


}
