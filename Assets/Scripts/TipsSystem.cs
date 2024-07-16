using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipsSystem : MonoBehaviour
{
    [Header("Communication tips")]
    [Tooltip("Why are you looking for a tip??? Shouldn't it be obvious?!")]
    public List<string> tips = new List<string>(4);

    [Header("Related things")]
    [Tooltip("Robot GameObject")]
    public GameObject robot_go;
    [Tooltip("Text object")]
    public TMP_Text text_go;
    [Tooltip("Tip timer delay")]
    public float delay = 10.0f;

    [Header("Animation name")]
    public string anim0 = "RobotIngress";

    private short tip_index = 0;
    //private bool tips_enabled = false;
    private bool tips_enabled = true;

    // Timer stuff
    private float timer_start = 0.0f;
    private bool timer_on = false;

    // Start is called before the first frame update
    void Start()
    {
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!tips_enabled)
            return;

        if (CheckTimer())
        {
            // Play animation
            robot_go.GetComponent<Animator>().Play(anim0);

            // Setup text
            text_go.text = tips[tip_index];
            tip_index++;
            if (tip_index >= tips.Count)
                tip_index = 0;
        }
    }

    public void EnableTips()
    {
        tips_enabled = true;
        StartTimer();
    }

    public void DisableTips()
    {
        tips_enabled = false;
        timer_on = false;
    }

    private void StartTimer()
    {
        timer_start = Time.time;
        timer_on = true;
    }

    private bool CheckTimer()
    {
        if (!timer_on || Time.time - timer_start > delay)
        {
            StartTimer();
            return true;
        }
        
        return false;
    }
}
