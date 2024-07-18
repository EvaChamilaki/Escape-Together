// MACROS
#define LOGGER                                      // Enables log print-outs

using System;
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
    [Tooltip("PianoController")]
    public GameObject piano_controller;
    [Tooltip("Subtitles")]
    public GameObject subtitle_go;

    [Header("Frequency params")]
    [Tooltip("Frequency distance from target, where sound changes to show near valid selection")]
    public int near_freq = 20;
    [Tooltip("Frequency distance from target, that indicates a valid selection")]
    public int valid_freq = 5;
    [Tooltip("Frequency step per cursor movement")]
    public int freq_step = 1;
    [Tooltip("First number target frequency")]
    public int first_freq = 55;
    [Tooltip("Second number target frequency")]
    public int second_freq = 160;

    [Header("SFXssss")]
    [Tooltip("Gaussian SFX")]
    public AudioSource gaussian_sfx;
    [Tooltip("Near SFX")]
    public AudioSource near_sfx;
    [Tooltip("First sheet")]
    public AudioSource first_sfx;
    [Tooltip("Second sheet")]
    public AudioSource second_sfx;

    [Header("Stuff")]
    [Tooltip("Radio Camera")]
    public Camera radio_camera;

    private bool focused = false;

    private int selected_freq;
    private int init_freq = 130;

    private Camera main_camera;

    private float init_cursor_x;

    // Start is called before the first frame update
    void Start()
    {
        selected_freq = init_freq;
        init_cursor_x = cursor.transform.position.x;
        focused = false;
        //gaussian_sfx.Play();
        main_camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ignore if not in focus
        if (!focused)
            return;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            SwitchToMainCamera();

            return;
        }

        // Clockwise
        if (Input.GetKeyUp(KeyCode.D))
        {
            //rot_wheel.transform.Rotate(new Vector3(0, 0, 1), rot_speed);

            float new_pox_x = cursor.transform.position.x - cursor_interval;
            new_pox_x = Mathf.Clamp(new_pox_x, right_bound_x, left_bound_x);
            cursor.transform.position = new Vector3(new_pox_x, cursor.transform.position.y, cursor.transform.position.z);

            selected_freq += freq_step;

            UpdateSFX();

#if DEBUG
            Debug.Log("Freq: " + selected_freq);
#endif
        }
        // Counter-clockwise
        else if (Input.GetKeyUp(KeyCode.A))
        {
            //rot_wheel.transform.Rotate(new Vector3(0, 0, 1), -rot_speed);

            float new_pox_x = cursor.transform.position.x + cursor_interval;
            new_pox_x = Mathf.Clamp(new_pox_x, right_bound_x, left_bound_x);
            cursor.transform.position = new Vector3(new_pox_x, cursor.transform.position.y, cursor.transform.position.z);

            selected_freq -= freq_step;

            UpdateSFX();

#if DEBUG
            Debug.Log("Freq: " + selected_freq);
#endif
        }
    }

    /// <summary>
    /// Update the SFX based on updated frequency
    /// </summary>
    private void UpdateSFX()
    {
        int first_dist = Math.Abs(selected_freq - first_freq);
        int second_dist = Math.Abs(selected_freq - second_freq);

        // Valid solutions
        if (first_dist <= valid_freq)
        {
            //gaussian_sfx.Stop();
            near_sfx.Stop();

            if (!first_sfx.isPlaying)
            {
                first_sfx.Play();

                // Enable subtitles
                subtitle_go.GetComponent<SubtitleController>().StartSubtitles(0);
            }

            first_sfx.volume = 0.8f;
#if DEBUG
            Debug.Log("Valid first");
#endif
        }
        else if (second_dist <= valid_freq)
        {
            //gaussian_sfx.Stop();
            near_sfx.Stop();

            if (!second_sfx.isPlaying)
            {
                second_sfx.Play();
            }

            second_sfx.volume = 0.8f;
#if DEBUG
            Debug.Log("Valid second");
#endif
        }
        // Near solution
        else if (first_dist <= near_freq)
        {
            gaussian_sfx.Stop();
            //first_sfx.Stop();
            //second_sfx.Stop();

            if (!near_sfx.isPlaying)
                near_sfx.Play();

            // Adjust volume
            if (!first_sfx.isPlaying)
            {
                first_sfx.Play();

                // Enable subtitles
                subtitle_go.GetComponent<SubtitleController>().StartSubtitles(0);
            }

            first_sfx.volume = 0.8f - 4 * (float)(first_dist / 100.0f);

#if DEBUG
            Debug.Log("Near solution");
#endif
        }
        // Near solution
        else if (second_dist <= near_freq)
        {
            gaussian_sfx.Stop();
            //first_sfx.Stop();
            //second_sfx.Stop();

            if (!near_sfx.isPlaying)
                near_sfx.Play();

            // Adjust volume
            if (!second_sfx.isPlaying)
            {
                second_sfx.Play();

                // Enable subtitles
                subtitle_go.GetComponent<SubtitleController>().StartSubtitles(1);
            }

            second_sfx.volume = 0.8f - 4 * (float)(second_dist / 100.0f);
#if DEBUG
            Debug.Log("Near solution");
#endif
        }
        // Far
        else
        {
            if (!gaussian_sfx.isPlaying)
            {
                near_sfx.Stop();
                first_sfx.Stop();
                second_sfx.Stop();
                gaussian_sfx.Play();

                // Disable subtitles
                subtitle_go.GetComponent<SubtitleController>().StopSubtitles();
            }
        }
    }

    public void SwitchToRadio()
    {
        focused = true;
        gaussian_sfx.Play();
    }

    private void SwitchToMainCamera()
    {
        focused = false;
        piano_controller.GetComponent<PianoController>().focused = true;

        // Disable subtitles
        subtitle_go.GetComponent<SubtitleController>().StopSubtitles();

        gaussian_sfx.Stop();
        near_sfx.Stop();
        first_sfx.Stop();
        second_sfx.Stop();

        selected_freq = init_freq;
        cursor.transform.position = new Vector3(init_cursor_x, cursor.transform.position.y, cursor.transform.position.z);

        radio_camera.enabled = false;
        main_camera.enabled = true;
    }
}
