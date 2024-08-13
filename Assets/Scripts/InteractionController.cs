using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    public GameObject canvasClick;

    [Header("Respawn Locations")]
    public Transform room_a;
    public Transform room_b;
    public Transform room_c;
    public Transform room_d;

    [Header("Respawn Fader Animation")]
    public GameObject respawn_image;
    public float alpha_interval = 0.01f;

    public bool active;
    private bool hitDoor;

    // Animation stuff
    private bool fade_in = true;
    private bool respawn_animation_playing = false;
    private bool exit_animation_playing = false;
    private Vector3 respawn_tgt;

    private GameObject player_object;
    private GameObject camera_object;

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
                    //gameObject.transform.position = room_a.position;
                    respawn_tgt = room_a.position;
                    respawn_animation_playing = true;
                    respawn_image.SetActive(true);
                    break;
                }
                case 1:
                {
                    //gameObject.transform.position = room_b.position;
                    respawn_tgt = room_b.position;
                    respawn_animation_playing = true;
                    respawn_image.SetActive(true);
                    break;
                }
                case 2:
                {
                    //gameObject.transform.position = room_c.position;
                    respawn_tgt = room_c.position;
                    respawn_animation_playing = true;
                    respawn_image.SetActive(true);
                    break;
                }
                case 3:
                {
                    //gameObject.transform.position = room_d.position;
                    respawn_tgt = room_d.position;
                    respawn_animation_playing = true;
                    respawn_image.SetActive(true);
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

        player_object = GameObject.FindWithTag("Player");
        camera_object = GameObject.FindWithTag("MainCamera");

        if (player_object == null || camera_object == null)
            Debug.LogError("PLAYER OR CAMERA NOT FOUND");
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        // Respawn animation logic
        if (respawn_animation_playing)
        {
            float new_alpha = (fade_in) ? Mathf.Clamp01(respawn_image.GetComponent<Image>().color.a + alpha_interval) :
                Mathf.Clamp01(respawn_image.GetComponent<Image>().color.a - alpha_interval);

            respawn_image.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, new_alpha);

            // Teleport
            if (fade_in && new_alpha == 1.0f)
            {
                // Freeze player
                player_object.GetComponent<FirstPersonMovement>().stop_flag = true;
                camera_object.GetComponent<FirstPersonLook>().stop_flag= true;

                gameObject.transform.position = respawn_tgt;
                fade_in = false;

                return;
            }

            if (!fade_in && new_alpha == 0.0f)
            {
                // Unfreeze player
                player_object.GetComponent<FirstPersonMovement>().stop_flag = false;
                camera_object.GetComponent<FirstPersonLook>().stop_flag = false;

                fade_in = true;
                respawn_image.SetActive(false);

                respawn_animation_playing = false;
            }

            return;
        }

        //// Exit Animation logic
        //if (exit_animation_playing)
        //{

        //}
       
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
