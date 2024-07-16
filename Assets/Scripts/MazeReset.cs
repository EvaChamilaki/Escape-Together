using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeReset : MonoBehaviour
{
    public GameObject emptyGO;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) 
        {
            gameObject.transform.position = emptyGO.transform.position;
            gameObject.transform.rotation = emptyGO.transform.rotation;
        }
    }
}
