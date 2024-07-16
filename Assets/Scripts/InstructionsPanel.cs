using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsPanel : MonoBehaviour
{
    public GameObject instructions;
    public GameObject introduction;
    public GameObject cameraGO;
    private GameObject player;
    //public GameObject tempDisableObj;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
    public void OnStartButton()
    {
        GetComponent<AudioSource>().Stop();
        player.GetComponent<FirstPersonMovement>().stop_flag = false;
        instructions.SetActive(true);
        introduction.SetActive(false);
//         if (cameraGO != null)
//         {
//             cameraGO.GetComponent<FirstPersonLook>().enabled = true;
//         }

//         if (tempDisableObj != null)
//         { 
//             tempDisableObj.SetActive(true);
//         }

//         if (Cursor.lockState == CursorLockMode.None && SceneManager.GetActiveScene().name == "PlayerControllerTestScene") 

        cameraGO.GetComponent<FirstPersonLook>().enabled = true;

        if (Cursor.lockState == CursorLockMode.None)
        { 
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void OnContinueButton()
    {
        gameObject.SetActive(false);
        introduction.SetActive(true);
    }

}
