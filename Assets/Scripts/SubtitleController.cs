using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleController : MonoBehaviour
{
    [Header("Captions")]
    [Tooltip("Caption texts")]
    public List<string> captions;
    [Tooltip("Delay for captions (between them and loop)")]
    public List<float> delays;

    public bool focused = false;

    private int sub_index = 0;
    private bool timer_on = false;
    private float timer_start;

    private bool playing;

    private int sheet_number = 0;

    // Start is called before the first frame update
    void Start()
    {
        focused = false;
        playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!focused)
        {
            timer_on = false;
            GetComponent<TMP_Text>().enabled = false;
            return;
        }

        if (timer_on && Time.time - timer_start > delays[sub_index + sheet_number * 2])
        {
            sub_index++;
            if (sub_index == 2)
                sub_index = 0;

            timer_start = Time.time;

            GetComponent<TMP_Text>().text = captions[sub_index + sheet_number * 2];
        }
    }

    public void StartSubtitles(int sheet)
    {
        sheet_number = sheet;
        focused = true;
        GetComponent<TMP_Text>().text = captions[0 + sheet_number * 2];
        timer_on = true;
        timer_start = Time.time;
        //GetComponent<TMP_Text>().enabled = true;
        //playing = true;
    }

    public void StopSubtitles()
    {
        focused = false;
        GetComponent<TMP_Text>().enabled = false;
        sub_index = 0;
        playing = false;
    }

    public void ShowSubtitles()
    { playing = true; GetComponent<TMP_Text>().enabled = true; }

    public void HideSubtitles()
    { playing = false; GetComponent<TMP_Text>().enabled = false; }

    public bool ArePlaying()
    { return playing; }
}
