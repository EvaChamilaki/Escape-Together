using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsPanelNoFPC : MonoBehaviour
{
    public GameObject instructions;
    public GameObject introduction;
    public GameObject controller_go;

    public bool first_scene = false;

    public void OnStartButton()
    {
        GetComponent<AudioSource>().Stop();
        if (!first_scene)
            controller_go.GetComponent<PianoController>().banana = true;
        else
            controller_go.GetComponent<DragDrop>().banana = true;
        instructions.SetActive(true);
        introduction.SetActive(false);
    }

    public void OnContinueButton()
    {
        gameObject.SetActive(false);
        introduction.SetActive(true);
    }

}
