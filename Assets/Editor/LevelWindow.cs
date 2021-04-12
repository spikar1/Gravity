using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;

public class LevelWindow : EditorWindow {

    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    public int selGridInt = 0;
    public string[] selStrings = new string[] { "Grid 1", "Grid 2", "Grid 3", "Grid 4" };


    [MenuItem("Window/My Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LevelWindow));
    }
    
    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Initiate"))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/test.unity");
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        selGridInt = GUI.SelectionGrid(new Rect(25, 25, 100, 30), selGridInt, selStrings, 2);
        
        GUI.DrawTexture(Rect.MinMaxRect(0, 100, 50, 200), Resources.Load<Texture>("Textures/Map-circle-black.svg"));

        EditorGUILayout.EndHorizontal();
    }
}
