using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PasswordManager : MonoBehaviour
{
    public string pass0;
    public string pass1;
    public string first_phase_puzzle;
    public string second_phase_puzzle;

    private TMP_InputField in_f;

    // Start is called before the first frame update
    void Start()
    {
        in_f = GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (in_f.text == pass0)
                SceneManager.LoadScene(first_phase_puzzle);
            else if (in_f.text == pass1)
                SceneManager.LoadScene(second_phase_puzzle);
            else
                in_f.text = "";
        }
    }
}
