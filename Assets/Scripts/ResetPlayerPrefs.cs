using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ResetPlayerPrefs : EditorWindow
{

    [MenuItem("Tools/ResetPlayerPrefs")]
    private static void ShowWindow()
    {
        PlayerPrefs.DeleteAll();
        UnityEngine.Debug.Log("You reset PlayerPrefs!");
    }

}