using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Dev_Tools
{
    public class MazeLightController : ScriptableObject
    {
        [MenuItem("Tools/Light Controller/Switch to Point Lights")]
        static void SwitchToPoints()
        {
            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (go.hideFlags != HideFlags.None)
                    continue;

                if (PrefabUtility.GetPrefabType(go) == PrefabType.Prefab || PrefabUtility.GetPrefabType(go) == PrefabType.ModelPrefab)
                    continue;

                if (go.CompareTag("DirectionalLight"))
                    go.SetActive(false);
                else if (go.CompareTag("MazeLight"))
                    go.SetActive(true);
            }

            EditorUtility.DisplayDialog("Light Controller", "Switched to point lights!", "OK", "");
        }
        [MenuItem("Tools/Light Controller/Switch to Directional Light")]
        static void SwitchToDirectional()
        {
            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (go.hideFlags != HideFlags.None)
                    continue;

                if (PrefabUtility.GetPrefabType(go) == PrefabType.Prefab || PrefabUtility.GetPrefabType(go) == PrefabType.ModelPrefab)
                    continue;

                if (go.CompareTag("DirectionalLight"))
                    go.SetActive(true);
                else if (go.CompareTag("MazeLight"))
                    go.SetActive(false);
            }

            EditorUtility.DisplayDialog("Light Controller", "Switched to directional!", "OK", "");
        }
    }
}