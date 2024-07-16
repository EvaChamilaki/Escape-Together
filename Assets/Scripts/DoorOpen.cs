using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DoorOpen : MonoBehaviour
{
    public GameObject canvasClick;
    public GameObject panel;
    public Animator animator;
    public string next_scene = "";
    public float delay = 1.5f;
    private bool insideCollider = false;
    private bool door_opened = false;

    private float timer_start;

    private MasterLog master_log; // Add a MasterLog instance

    [SerializeField] UnityEvent onTriggerEnter;

    void Start()
    {
        animator = panel.GetComponent<Animator>();
        master_log = new MasterLog(); // Initialize MasterLog
        master_log.SetupTime(); // Set the start time
    }

    void OnTriggerEnter(Collider other)
    {
        canvasClick.SetActive(true);
        insideCollider = true;
        timer_start = Time.time;
        door_opened = true;
    }

    void OnTriggerExit(Collider other)
    {
        canvasClick.SetActive(false);
        insideCollider = false;
    }

    void Update()
    {
        if (insideCollider && Input.GetMouseButtonDown(0))
        {
            onTriggerEnter.Invoke();
            animator.SetTrigger("DoorOpen");
        }

        if (door_opened && Time.time - timer_start > delay)
        {
            // Log the metrics
            master_log.WriteLog(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(next_scene);
        }
    }
}