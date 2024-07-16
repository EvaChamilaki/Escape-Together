using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Describes the ghost book (slots) gameobjects
/// </summary>
public class GhostBook : MonoBehaviour
{
    [Header("Slot properties")]
    [Tooltip("The id of the slot (used for solve check)")]
    public short id;
    [Tooltip("The expected id of the book to be placed in this slot")]
    public short tgt_id;
    [Tooltip("The maximum valid id of a slot")]
    public short max_id = 0;

    private Renderer objectRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (max_id == 0)
        {
            Debug.LogError("max_id has not been initialized!");
        }

        // Valid id check
        Assert.IsTrue(id <= max_id);

        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError("Renderer component not found on the GameObject!");
        }
        else
        {
            // Start blinking for the first 5 seconds
            // StartCoroutine(BlinkEverySecondForDuration(5f));
        }
    }

    private IEnumerator BlinkEverySecondForDuration(float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            objectRenderer.enabled = !objectRenderer.enabled;
            yield return new WaitForSeconds(1f); // Toggle every 1 second
        }
        // Ensure the object is invisible at the end
        objectRenderer.enabled = false;
    }
}
