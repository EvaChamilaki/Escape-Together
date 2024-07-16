using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PianoKey : MonoBehaviour
{
    [Header("Key properties")]
    [Tooltip("ID used to find key that was hit")]
    public short id = 0;
}
