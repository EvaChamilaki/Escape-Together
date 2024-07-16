// MACROS
#define LOGGER                                      // Enables log print-outs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioController : MonoBehaviour
{
    [Header("Cursor const")]
    [Tooltip("Left bound")]
    public float left_bound_x;
    [Tooltip("Right bound")]
    public float right_bound_x;
    [Tooltip("Rotation speed")]
    public float rot_speed;
    [Tooltip("Cursor interval")]
    public float cursor_interval;

    [Header("Specific GameObjects")]
    [Tooltip("Wheel")]
    public GameObject rot_wheel;
    [Tooltip("Cursor")]
    public GameObject cursor;

    [Header("Frequency params")]
    [Tooltip("Frequency distance from target, where sound changes to show near valid selection")]
    public uint near_freq = 20;
    [Tooltip("Frequency distance from target, that indicates a valid selection")]
    public uint valid_freq = 5;
    [Tooltip("Frequency step per cursor movement")]
    public uint freq_step = 1;
    [Tooltip("First number target frequency")]
    public uint first_freq = 55;
    [Tooltip("Second number target frequency")]
    public uint second_freq = 160;

    private uint selected_freq = 130;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Clockwise
        if (Input.GetKeyUp(KeyCode.D))
        {
            rot_wheel.transform.Rotate(new Vector3(0, 0, 1), rot_speed);

            float new_pox_x = cursor.transform.position.x - cursor_interval;
            new_pox_x = Mathf.Clamp(new_pox_x, right_bound_x, left_bound_x);
            cursor.transform.position = new Vector3(new_pox_x, cursor.transform.position.y, cursor.transform.position.z);

            selected_freq += freq_step;
#if DEBUG
            Debug.Log("Freq: " + selected_freq);
#endif
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            rot_wheel.transform.Rotate(new Vector3(0, 0, 1), -rot_speed);

            float new_pox_x = cursor.transform.position.x + cursor_interval;
            new_pox_x = Mathf.Clamp(new_pox_x, right_bound_x, left_bound_x);
            cursor.transform.position = new Vector3(new_pox_x, cursor.transform.position.y, cursor.transform.position.z);

            selected_freq -= freq_step;
#if DEBUG
            Debug.Log("Freq: " + selected_freq);
#endif
        }    
    }
}
