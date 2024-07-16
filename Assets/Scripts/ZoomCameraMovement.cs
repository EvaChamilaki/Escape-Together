using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ZoomCameraMovement : MonoBehaviour
{
    private Vector2 turn;

    void Update()
    {
        if (PauseMenu.isGamePaused)
        {
            return;
        }

        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
