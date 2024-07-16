using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InteractableBook : MonoBehaviour
{
    [Header("Book properties")]
    [Tooltip("The id of the book (used for solve check)")]
    public short id;
    [Tooltip("The maximum valid id of a book")]
    public short max_id = 10;
    [Tooltip("The symbol associated with the book (for dev purposes only)")]
    public string symbol;

    private short slot_id;          // id of the slot the book is placed in (0xff means not placed)
    // TODO: Bool can be removed if we use 0xff for non-placed!
    private bool placed_flag;       // whether the book is placed in a slot

    // Start is called before the first frame update
    void Start()
    {
        if (max_id == 0)
            Debug.LogError("max_id has not been initialized!");

        // Valid id check
        Assert.IsTrue(id <= max_id);
    }

    public bool IsPlaced()
    { return placed_flag; }

    public short GetSlotId()
    { return slot_id; }

    public void SetPlaced(bool placed, short slot)
    {
        placed_flag = placed;
        slot_id = slot;
    }
}
