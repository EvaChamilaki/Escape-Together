using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public GameObject canvasClick;

    private bool hitDoor;

    // Start is called before the first frame update
    void Start()
    {
        canvasClick.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 7.0f) && hit.transform.CompareTag("Door"))
        {
            if (hit.collider.gameObject.GetComponentInParent<Animator>().GetBool("Opened"))
                return;
            hitDoor = true;
            canvasClick.SetActive(true);
        }
        else
        {
            hitDoor = false;
            canvasClick.SetActive(false);
        }

        if(hitDoor && Input.GetMouseButtonDown(0))
        {
            hit.collider.gameObject.GetComponentInParent<Animator>().SetTrigger("DoorOpen");
            hit.collider.gameObject.GetComponentInParent<Animator>().SetBool("Opened", true);


        }



    }
}
