using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public GameObject canvasClick;

    [Header("Respawn Locations")]
    public Transform room_a;
    public Transform room_b;
    public Transform room_c;
    public Transform room_d;

    public bool active;
    private bool hitDoor;

    void OnTriggerEnter(Collider other)
    {
        // Door closing
        if (other.CompareTag("CloseTrigger") && other.gameObject.transform.parent.GetComponentInChildren<Animator>().GetBool("Opened"))
        {
            //if (other.gameObject.transform.parent.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length > other.gameObject.transform.parent.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime)
            //    return;
            other.gameObject.transform.parent.GetComponentInChildren<Animator>().SetTrigger("DoorClose");
            other.gameObject.transform.parent.GetComponentInChildren<Animator>().SetBool("Opened", false);
            other.gameObject.transform.parent.GetComponentsInChildren<AudioSource>()[1].PlayDelayed(0.4f);
        }
        // Player respawing
        else if (other.CompareTag("RespawnTrigger"))
        {
            gameObject.GetComponent<AudioSource>().Play();

            switch (other.gameObject.transform.GetComponent<RespawnTrigger>().room_id)
            {
                case 0:
                {
                    gameObject.transform.position = room_a.position;
                    break;
                }
                case 1:
                {
                    gameObject.transform.position = room_b.position;
                    break;
                }
                case 2:
                {
                    gameObject.transform.position = room_c.position;
                    break;
                }
                case 3:
                {
                    gameObject.transform.position = room_d.position;
                    break;
                }
                default:
                    break;
            }
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

        if (hitDoor && Input.GetMouseButtonDown(0))
        {
            //if (hit.collider.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).length > hit.collider.gameObject.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime)
            //    return;

            if (hit.collider.gameObject.GetComponentInParent<Animator>().GetBool("Opened"))
            {
                hit.collider.gameObject.GetComponentInParent<Animator>().SetTrigger("DoorClose");
                hit.collider.gameObject.GetComponentInParent<Animator>().SetBool("Opened", false);
                hit.collider.gameObject.GetComponents<AudioSource>()[1].PlayDelayed(0.4f);
            }
            else
            {
                hit.collider.gameObject.GetComponentInParent<Animator>().SetTrigger("DoorOpen");
                hit.collider.gameObject.GetComponentInParent<Animator>().SetBool("Opened", true);
                hit.collider.gameObject.GetComponents<AudioSource>()[0].Play();
            }

        }
    }
}
