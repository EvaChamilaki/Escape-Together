using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public GameObject canvasClick;

    public bool active;
    private bool hitDoor;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CloseTrigger") && other.gameObject.transform.parent.GetComponentInChildren<Animator>().GetBool("Opened"))
        {
            //if (other.gameObject.transform.parent.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length > other.gameObject.transform.parent.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime)
            //    return;
            other.gameObject.transform.parent.GetComponentInChildren<Animator>().SetTrigger("DoorClose");
            other.gameObject.transform.parent.GetComponentInChildren<Animator>().SetBool("Opened", false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasClick.SetActive(false);
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;
       
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 4.0f) && hit.transform.CompareTag("Door"))
        {
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
            //if (hit.collider.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).length > hit.collider.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime)
            //    return;

            if (hit.collider.gameObject.GetComponentInParent<Animator>().GetBool("Opened"))
            {
                hit.collider.gameObject.GetComponentInParent<Animator>().SetTrigger("DoorClose");
                hit.collider.gameObject.GetComponentInParent<Animator>().SetBool("Opened", false);
            }
            else
            {
                hit.collider.gameObject.GetComponentInParent<Animator>().SetTrigger("DoorOpen");
                hit.collider.gameObject.GetComponentInParent<Animator>().SetBool("Opened", true);
            }

        }
    }
}
