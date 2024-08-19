#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MazePlayerPositionController : ScriptableObject
{
    [MenuItem("Tools/Player Position Tool/Set Player at Room A")]
    static void setAtRoomA()
    {
        Vector3 target_position = new Vector3(-157.729996f, 5.03999996f, -7.42999983f);

        GameObject player_object = GameObject.FindWithTag("Player");                                // Get Player GameObject

        player_object.transform.position = target_position;                                          // Transform Player

        EditorUtility.DisplayDialog("Player Position Tool", "Player Set at Room A!", "OK", "");      // Display Result
    }

    [MenuItem("Tools/Player Position Tool/Set Player at Room B")]
    static void setAtRoomB()
    {
        Vector3 target_position = new Vector3(-105.389999f, 5.03999996f, -17.9500008f);

        GameObject player_object = GameObject.FindWithTag("Player");                                // Get Player GameObject

        player_object.transform.position = target_position;                                         // Transform Player

        EditorUtility.DisplayDialog("Player Position Tool", "Player Set at Room B!", "OK", "");    // Display Result
    }

    [MenuItem("Tools/Player Position Tool/Set Player at Room C")]
    static void setAtRoomC()
    {
        Vector3 target_position = new Vector3(-62.2700005f, 5.04f, 13.8999996f);

        GameObject player_object = GameObject.FindWithTag("Player");                                // Get Player GameObject

        player_object.transform.position = target_position;                                         // Transform Player

        EditorUtility.DisplayDialog("Player Position Tool", "Player Set at Room C!", "OK", "");   // Display Result
    }

    [MenuItem("Tools/Player Position Tool/Set Player at Room D")]
    static void setAtRoomD()
    {
        Vector3 target_position = new Vector3(-42.7599983f, 5.04f, -33.3899994f);

        GameObject player_object = GameObject.FindWithTag("Player");                                // Get Player GameObject

        player_object.transform.position = target_position;                                         // Transform Player

        EditorUtility.DisplayDialog("Player Position Tool", "Player Set at Room D!", "OK", "");   // Display Result
    }
}
#endif