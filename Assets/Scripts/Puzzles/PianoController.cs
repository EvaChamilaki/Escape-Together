#define LOGGER

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PianoController : MonoBehaviour
{
    [Header("Solution setup")]
    [Tooltip("Solution sequence")]
    public List<short> solution_sequence = new List<short>(16);

    [Header("Custom cursors")]
    public Texture2D point_cursor;
    public Texture2D press_cursor;

    [Header("SFXsssss")]
    public AudioSource solved_sfx;

    [Header("Instruction texts")]
    public TMP_Text instruction_text;

    [Tooltip("Button for next scene")]
    public GameObject switch_scene_button;
    public string next_scene = "";

    public List<GameObject> sheet_list = new List<GameObject>();

    public bool banana;

    [Tooltip("Whether the main camera is active")]
    public bool focused = true;

    GameObject getTarget = null;

    private short note_index = 0;                   // denotes the note that needs to be played in the sequence. 15 is the last note.
    private bool puzzle_solved = false;

    // Cursor properties
    private Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    // Hardcoded (thanks Unity...) instruction texts
    private string main_view_text = "'Esc' or 'P': Pause\nClick on object to interact";
    private string piano_view_text = "'Esc' or 'P': Pause\nClick on piano key to play\n'Spacebar': Exit Piano view";
    private string radio_view_text = "'Esc' or 'P': Pause\n'A' or 'D': Manipulate Radio\n'Spacebar': Exit Radio view";

    // Metrics
    private MasterLog master_log;

    private Camera main_camera;
    private Camera radio_camera;
    private Camera piano_camera;

    private BoxCollider pianoCollider;

    // Start is called before the first frame update
    void Start()
    {
        // Get cameras
        main_camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        radio_camera = GameObject.FindWithTag("RadioCamera").GetComponent<Camera>();
        piano_camera = GameObject.FindWithTag("PianoCamera").GetComponent<Camera>();

        pianoCollider = GameObject.FindWithTag("Piano").GetComponent<BoxCollider>();

        banana = false;
        focused = false;

        // Set initial instruction text
        instruction_text.text = main_view_text;

        // Update cursor
        Cursor.SetCursor(point_cursor, hotSpot, cursorMode);

        // Initialize log
        master_log = new MasterLog();
        master_log.SetupTime();

        switch_scene_button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isGamePaused)
        {
            Cursor.SetCursor(point_cursor, hotSpot, cursorMode);
            return;
        }

        if (piano_camera.enabled && Input.GetKeyUp(KeyCode.Space))
        {
            SwitchToMainCamera();
        }

        if (!banana)
            return;
        RaycastHit hitInfo;

        getTarget = ReturnClickedObject(out hitInfo);
        if (getTarget != null && (getTarget.CompareTag("PianoKeys") || getTarget.CompareTag("Radio") || getTarget.CompareTag("Piano") ))
        {
            // Update cursor
            Cursor.SetCursor(press_cursor, hotSpot, cursorMode);
        }
        else
        {
            // Update cursor
            Cursor.SetCursor(point_cursor, hotSpot, cursorMode);
        }

        //check if the left mouse button has been clicked
        if (Input.GetMouseButtonDown(0))
        {
            // For radio
            if (main_camera.enabled && getTarget != null && getTarget.CompareTag("Radio"))
            {
                // Switch to radio camera
                focused = false;
                main_camera.enabled = false;
                radio_camera.enabled = true;


                getTarget.GetComponent<RadioController>().SwitchToRadio();

                instruction_text.text = radio_view_text;

                return;
            }

            // For piano
            else if (main_camera.enabled && getTarget != null && getTarget.CompareTag("Piano"))
            {
                // Switch to piano camera
                focused = true;
                main_camera.enabled = false;
                piano_camera.enabled = true;

                pianoCollider.enabled = false;

                instruction_text.text = piano_view_text;

                return;
            }

            else if (main_camera.enabled)
                return;
        
            // Only care about keys
            if (piano_camera.enabled && getTarget != null && getTarget.CompareTag("PianoKeys"))
            {
                // Play SFX
                getTarget.GetComponent<AudioSource>().Play();

                // Increment notes hit count
                master_log.IncrementNotes();

                // Check whether the note is correct in the sequence
                short hit_id = getTarget.GetComponent<PianoKey>().id;
#if (LOGGER)
                Debug.Log($"Note id: {hit_id}!");
#endif
                if (!puzzle_solved && hit_id == solution_sequence[note_index])
                {
                    note_index++;

                    // Update shit
                    sheet_list[note_index - 1].SetActive(false);
                    sheet_list[note_index].SetActive(true);

                    // Puzzle solved
                    if (note_index == 16)
                    {
                        // Puzzle solved, do amazing things
                        puzzle_solved = true;
                        solved_sfx.Play();
                        switch_scene_button.SetActive(true);

#if (LOGGER)
                        Debug.Log("Puzzle solved!");
#endif
                        // Log the metrics
                        master_log.WriteLog(SceneManager.GetActiveScene().name);
                    }
                }
                else
                {
                    if (hit_id == solution_sequence[0])
                    {
                        // Update shit
                        sheet_list[note_index].SetActive(false);
                        sheet_list[1].SetActive(true);

                        note_index = 1;  // Reset note index on mistake
                    }
                    else
                    {
                        // Update shit
                        sheet_list[note_index].SetActive(false);
                        sheet_list[0].SetActive(true);

                        note_index = 0;  // Reset note index on mistake
                    }
                }

#if (LOGGER)
                Debug.Log($"Note index: {note_index}!");
#endif
            }
        }
    }

    /// <summary>
    /// Helper function that returns the clicked GameObject
    /// </summary>
    /// <param name="hit">: raycast hit</param>
    /// <returns>: the GameObject that was hit, if it's a piano key</returns>
    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = main_camera.enabled ? main_camera.ScreenPointToRay(Input.mousePosition) : piano_camera.ScreenPointToRay(Input.mousePosition);
        
        //if (Physics.Raycast(ray.origin, ray.direction * 10, out hit) && hit.transform.CompareTag("PianoKeys"))
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit) && (hit.transform.CompareTag("PianoKeys") || hit.transform.CompareTag("Radio") || hit.transform.CompareTag("Piano")))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }


    void SwitchToMainCamera()
    {
        pianoCollider.enabled = true;
        focused = false;
        piano_camera.enabled = false;
        main_camera.enabled = true;

        instruction_text.text = main_view_text;
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(next_scene);
    }
}

