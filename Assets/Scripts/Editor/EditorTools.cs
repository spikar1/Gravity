using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;

public class EditorTools
{
    static Vector2 sceneViewMousePos;

    [MenuItem("SteffenTools/MovePlayerToCursor &P")]
    static void MovePlayer()
    {
        var player = GameObject.FindObjectOfType<Player>();
        if (player == null)
            throw new System.Exception("No player is found");

        SceneView.duringSceneGui += OnSceneGUI;

        GameObject.FindObjectOfType<Player>().transform.position = sceneViewMousePos;
        Undo.RegisterCompleteObjectUndo(player.transform, "Player moved to Mouse position");
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Vector3 mousePosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

        sceneViewMousePos = ray.origin;
    }
}
