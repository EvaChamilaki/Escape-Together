using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    // Flag To Stop Controller During Interaction
    public bool stop_flag = false;

    Vector2 velocity;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    void FixedUpdate()
    {
        // Move.
        IsRunning = canRun && Input.GetKey(runningKey);
        float movingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
            movingSpeed = speedOverrides[speedOverrides.Count - 1]();

        // Check if Interaction is Happening
        if (!stop_flag)
        {
            velocity.y = Input.GetAxis("Vertical") * movingSpeed * Time.deltaTime;
            velocity.x = Input.GetAxis("Horizontal") * movingSpeed * Time.deltaTime;
            transform.Translate(velocity.x, 0, velocity.y);
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().Sleep();
        }
    }
}