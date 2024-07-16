using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZoomCamera : MonoBehaviour
{
    public GameObject zoomCam;
    public bool zoomedIn = false;
    public TMP_Text inspectText;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !zoomedIn) {
            Inspect();
        }
        else if(Input.GetMouseButtonUp(1) &&  zoomedIn)
        {
            ZoomOut();
        }
    }

    void Inspect() { 
        if(zoomCam != null)
        {
            zoomCam.SetActive(true);
            zoomedIn = true;
            inspectText.text = "Release: Zoom Out";
        }
    }

    void ZoomOut()
    {
        if (zoomCam != null)
        {
            zoomCam.SetActive(false);
            zoomedIn = false;
            inspectText.text = "Hold Right Click: Inspect";
        }
    }
}
