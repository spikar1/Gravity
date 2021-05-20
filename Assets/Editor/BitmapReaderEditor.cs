using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BitmapReader))]
public class BitmapReaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BitmapReader myScript = (BitmapReader)target;
        if (GUILayout.Button("BuildLevel"))
        {
            myScript.BuildLevel();
        }
    }
}
