// MACROS
#define LOGGER                                      // Enables log print-outs

using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Describes the interactable book gameobjects
/// </summary>
public class DragDrop : MonoBehaviour
{
    [Header("Drag&Drop utility properties")]
    [Tooltip("Minimum distance from ghost to perform snap action.")]
    public float snap_distance = 0.05f;

    [Header("Custom cursors")]
    public Texture2D point_cursor;
    public Texture2D hold_cursor;

    [Header("SFXsssssss")]
    [Tooltip("Grab SFX")]
    public AudioClip grab_sfx;
    [Tooltip("Place SFX")]
    public AudioClip place_sfx;
    [Tooltip("Puzzled solved SFX")]
    public AudioClip solved_sfx;
    [Tooltip("Audio Source for solved SFX")]
    public AudioSource solved_src;
    [Tooltip("Surprise left SFX")]
    public AudioSource surprise_left_sfx;
    [Tooltip("Surprise right SFX")]
    public AudioSource surprise_rght_sfx;

    [Header("VFXsssssss")]
    [Tooltip("Suprise left VFX")]
    public ParticleSystem ps_left;
    [Tooltip("Suprise right VFX")]
    public ParticleSystem ps_right;

    [Header("Instruction texts")]
    public TMP_Text instruction_text;

    public GameObject GlowyBookOutline;

    [Tooltip("Button for next scene")]
    public GameObject switch_scene_button;

    public string next_scene = "";

    public bool banana = false;

    private AudioSource camera_sfx_src;
    private bool grab_sfx_played = false;

    private bool mouseDragging = false;

    private GameObject[] books;
    private GameObject[] ghosts;
    private Transform[] books_positions;
    private Vector3[] new_books_positions;

    private GameObject[] correctBooks;

    Vector3 positionOfScreen;

    GameObject getTarget =  null;

    private List<Vector3> positionList;
    private List<Vector3> originalPositionList;

    private float posX = 0.0f, posY = 0.0f, posZ = 0.0f;

    private short puzzle_status;
    private short puzzle_solved_status;
    private short slot_status = 0;                      // Denotes the status of each slot (filled or empty)

    private bool solved_sfx_played = false;

    private List<GameObject> solutionBooks;             //store the books that are part of the solution
    private List<Vector3> targetPositionLists;          //store the "pushed back" positions of the books

    // Hardcoded (thanks Unity...) instruction texts
    private string main_view_text = "Hover cursor over book and Hold 'Left-Click' to Drag";
    private string drag_view_text = "Release 'Left-Click' to Drop book";

    // Cursor properties
    private Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    // Metrics
    private MasterLog master_log;

    void Start()
    {
        // Initialize log
        master_log = new MasterLog();
        master_log.SetupTime();

        // Update cursor
        Cursor.SetCursor(point_cursor, hotSpot, cursorMode);

        // Point to camera audio source
        camera_sfx_src = GameObject.FindWithTag("MainCamera").transform.GetChild(0).GetComponent<AudioSource>();

        switch_scene_button.SetActive(false);

        if (camera_sfx_src == null)
            Debug.LogError("Camera SFX source not found!");

        camera_sfx_src.loop = false;

        mouseDragging = false;
        books = GameObject.FindGameObjectsWithTag("Books"); //find all books
        ghosts = GameObject.FindGameObjectsWithTag("Ghosts"); //find all ghosts
        positionList = new List<Vector3>();
        originalPositionList = new List<Vector3>();
        solutionBooks = new List<GameObject>();
        targetPositionLists = new List<Vector3>();

        //save ghost positions in a list
        foreach(GameObject GO in ghosts)
        {
            positionList.Add(GO.transform.position);
        }

        foreach(GameObject GO in books)
        {
           originalPositionList.Add(GO.transform.position);
        }

        // TODO: Consider adding check for duplicate ids in book GameObjects!

        GhostsShowing(false);

        instruction_text.text = main_view_text;                     // Start with main instructions

        // Calculate puzzle status on solution
        puzzle_solved_status = (short)(Mathf.Pow(2, ghosts.Length) - 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isGamePaused) {
            Cursor.SetCursor(point_cursor, hotSpot, cursorMode);
            return;
        }
        RaycastHit hitInfo;

        if (!mouseDragging)
        {
            getTarget = ReturnClickedObject(out hitInfo);
            if (getTarget != null && getTarget.CompareTag("Books"))
            {
                // Update cursor
                Cursor.SetCursor(hold_cursor, hotSpot, cursorMode);
            }
            else
            {
                // Update cursor
                Cursor.SetCursor(point_cursor, hotSpot, cursorMode);
            }
        }

        //check if the left mouse button has been clicked
        if (Input.GetMouseButtonDown(0))
        {
            if(getTarget != null)
            {
                mouseDragging = true;
                GhostsShowing(true);

                instruction_text.text = drag_view_text;             // Set instructions to dragging
            }

            // Only care about interactable books
            if (getTarget != null && getTarget.CompareTag("Books"))
            {
                if (!grab_sfx_played)
                {
                    // Play SFX
                    camera_sfx_src.clip = grab_sfx;
                    camera_sfx_src.Play();

                    grab_sfx_played = true;
                }

                // Remove book if already placed
                if (getTarget.GetComponent<InteractableBook>().IsPlaced())
                {
                    short slot = getTarget.GetComponent<InteractableBook>().GetSlotId();
                    slot_status = WriteIntoBits(slot_status, slot, 0);
#if (LOGGER)
                    Debug.Log($"Removed book {getTarget.GetComponent<InteractableBook>().id} from slot {slot}!");
                    Debug.Log($"Slot status: {slot_status}");
#endif
                    getTarget.GetComponent<InteractableBook>().SetPlaced(false, 0xff);
                    puzzle_status = WriteIntoBits(puzzle_status, slot, 0);

#if (LOGGER)
                    Debug.Log($"Updated puzzle status to {puzzle_status}!");
#endif
                }

                positionOfScreen = Camera.main.WorldToScreenPoint(getTarget.transform.position);
                posX = Input.mousePosition.x - positionOfScreen.x;
                posY = Input.mousePosition.y - positionOfScreen.y;
                posZ = Input.mousePosition.z - positionOfScreen.z;
            }
        }

        //check if the left mouse button has been released
        if(Input.GetMouseButtonUp(0) && mouseDragging)
        {
            grab_sfx_played = false;

            // Update cursor
            Cursor.SetCursor(point_cursor, hotSpot, cursorMode);

            mouseDragging = false;
            GhostsShowing(false);
            SnapToPosition();

            instruction_text.text = main_view_text;             // Set instructions to main
        }

        //while the left mouse button remains clicked, do....
        if(mouseDragging)
        {
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z); //tracking mouse position

            float disX = Input.mousePosition.x - posX;
            float disY = Input.mousePosition.y - posY;
            float disZ = Input.mousePosition.z - posZ;
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(new Vector3(disX, disY, disZ));
            getTarget.transform.position = currentPosition;
        }

        // Check for solution
        if (puzzle_status == puzzle_solved_status)
        {
            // TODO: Do something great!

            // Play SFX & VFX and fill solution books position list
            if (!solved_sfx_played)
            {
                ps_left.Play();
                ps_right.Play();
                //surprise_left_sfx.Play();
                //surprise_rght_sfx.Play();

                solved_sfx_played = !solved_sfx_played;
                solved_src.Play();
                SolutionTransform();

#if (LOGGER)
                Debug.Log("Solved!");
#endif

                master_log.WriteLog(SceneManager.GetActiveScene().name);
                GlowyBookOutline.SetActive(false);
            }

            StartCoroutine(PushBooksBack()); //push books a bit to the back before deactivating them
        }
    }

    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray.origin, ray.direction *10, out hit) && hit.transform.tag == "Books")
        {
            target= hit.collider.gameObject;
        }

        return target;
    }

    /// <summary>
    /// Snaps book to a slot if close enough
    /// </summary>
    void SnapToPosition()
    {
        if(getTarget != null)
        {
            Vector3 currentPosition = getTarget.transform.position;
            Vector3 closestPosition = currentPosition;
            short tgt_id_of_slot = 0;
            short id_of_slot = 0;

            float minDistance = Mathf.Infinity;
            //foreach (Vector3 ghostPosition in positionList)
            foreach (GameObject ghostGO in ghosts)
            {
                float distance = Vector3.Distance(currentPosition, ghostGO.transform.position);

                if(distance < minDistance)
                {
                    minDistance = distance;
                    closestPosition = ghostGO.transform.position;
                    tgt_id_of_slot = ghostGO.GetComponent<GhostBook>().tgt_id;
                    id_of_slot = ghostGO.GetComponent<GhostBook>().id;
                }
            }

            // Snap to slot
            if(minDistance < snap_distance)
            {
                getTarget.transform.position = closestPosition;

                // Handle already filled slot
                if (ReadFromBits(slot_status, id_of_slot) == 1)
                {
                    int bookIndex = System.Array.IndexOf(books, getTarget);
                    getTarget.transform.position = originalPositionList[bookIndex];
                    return;
                }

                // Update log
                master_log.UpdateBooks();

                // Play SFX
                camera_sfx_src.clip = place_sfx;
                camera_sfx_src.Play();

                // Set book slot
                getTarget.GetComponent<InteractableBook>().SetPlaced(true, id_of_slot);

                // Update slot status
                slot_status = WriteIntoBits(slot_status, id_of_slot, 1);

                // Check whether book was placed in the correct slot
                short placed_book_id = getTarget.GetComponent<InteractableBook>().id;
#if (LOGGER)
                Debug.Log($"Placed book {placed_book_id} into slot {tgt_id_of_slot}!");
                Debug.Log($"Slot status: {slot_status}");
#endif

                if (tgt_id_of_slot == placed_book_id)
                {
                    puzzle_status = WriteIntoBits(puzzle_status, id_of_slot, 1);
#if (LOGGER)
                    Debug.Log($"Updated puzzle status to {puzzle_status}!");
#endif
                }
            }
            // Reset to original position
            else
            {
                // Play SFX
                camera_sfx_src.clip = place_sfx;
                camera_sfx_src.Play();

                int bookIndex = System.Array.IndexOf(books, getTarget);
                getTarget.transform.position = originalPositionList[bookIndex];
            }
        }
    }

    /// <summary>
    /// Toggles slot visibility
    /// </summary>
    /// <param name="isActive">: flag</param>
    void GhostsShowing(bool isActive)
    {
        foreach(GameObject ghost in ghosts)
            ghost.GetComponent<Renderer>().enabled = isActive;
    }

    /// <summary>
    /// Stores positions of books and the desired target positions
    /// </summary>
    void SolutionTransform()
    {
        int solutionLayer = LayerMask.NameToLayer("Solution"); //get all the books from the right bookcase
        GameObject [] solutionOjectsArray = GameObject.FindObjectsOfType<GameObject>();

        foreach( GameObject GO in solutionOjectsArray)
        {
            if(GO.layer == solutionLayer && !solutionBooks.Contains(GO))
            {
                solutionBooks.Add(GO);                                                  //collect all the books that are part of the solution
                targetPositionLists.Add(GO.transform.position - new Vector3(0,0,0.1f)); //store the target positions for each GO
            }
        }
    }

    /// <summary>
    /// Coroutine to handle push-back animation
    /// </summary>
    /// <returns></returns>
    IEnumerator PushBooksBack()
    {
        //const float duration = 1f;
        const float duration = 4f;
        float elapsedTime = 0f;
        //const float transitionSpeed = 0.5f;
        const float transitionSpeed = 0.05f;

        while (elapsedTime < duration)
        {
            for (int i = 0; i < solutionBooks.Count; i++)   //push all books that are part of the solution back
            {
                GameObject GO = solutionBooks[i];
                Vector3 targetPosition = targetPositionLists[i];

                GO.transform.position = Vector3.Lerp(GO.transform.position, targetPosition, transitionSpeed*Time.deltaTime);
            }

            elapsedTime += Time.deltaTime;
            yield return null;

        }

        switch_scene_button.SetActive(true);

        gameObject.SetActive(false);                        //deactivate gameObjects after the puzzle has been solved

    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(next_scene);
    }

    // **************************************************
    // Utility functions
    // **************************************************

    /// <summary>
    /// Bit magic
    /// </summary>
    /// <param name="a">: the short to manipulate</param>
    /// <param name="bit_index">: the index of the bit</param>
    /// <param name="b">: the bit value to write</param>
    /// <returns>: the updated short</returns>
    short WriteIntoBits(short a, short bit_index, int b)
    {
        if (b != 0 && b != 1 && bit_index >= sizeof(short) * 8)
            return a;

        short mask = 0;
        //if (bit_index != 0)
        //    mask = (short)(~(1 << bit_index));
        mask = (short)(~(1 << bit_index));

        a = (short)((a & mask) | b << bit_index);

        return a;
    }

    /// <summary>
    /// More bit magic
    /// </summary>
    /// <param name="source">: the source short</param>
    /// <param name="bit_index">: the index of the bit</param>
    /// <returns>: the read bit</returns>
    short ReadFromBits(short source, short bit_index)
    {
        short mask = (short)(1 << bit_index);
        return (short)((source & mask) >> bit_index);
    }
}
