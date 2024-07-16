using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chooseThis : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
                }
            }
        }
    }
}
