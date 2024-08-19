using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    [Header("Hands")]
    public GameObject min_hand;
    public GameObject hour_hand;

    [Header("Options")]
    [Tooltip("Continuous hour hand movement")]
    public bool continuous_hour = false;
    [Tooltip("Continuous minute hand movement")]
    public bool continuous_min = false;

    private System.DateTime time;

    // To properly apply the new rotation each time
    private float prev_min_rot = 0.0f;
    private float prev_hour_rot = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Get starting time
        time = System.DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        // Update time
        time = System.DateTime.Now;

        // Update rotations

        //float min_rot = time.Minute * 6;
        float min_rot = (continuous_min) ? time.Minute * 6 + time.Second / 10 : time.Minute * 6;

        min_hand.transform.Rotate(0.0f, 0.0f, -prev_min_rot);
        min_hand.transform.Rotate(0.0f, 0.0f, min_rot);

        int nice_hour = (time.Hour >= 12) ? time.Hour - 12 : time.Hour;
        //float hour_rot = nice_hour * 30;
        float hour_rot = (continuous_hour) ? nice_hour * 30 + time.Minute / 2 : nice_hour * 30;
        hour_hand.transform.Rotate(0.0f, 0.0f, -prev_hour_rot);
        hour_hand.transform.Rotate(0.0f, 0.0f, hour_rot);

        prev_min_rot = min_rot;
        prev_hour_rot = hour_rot;
    }
}
