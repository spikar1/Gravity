using UnityEngine;
using UnityEditor;
using System.Collections;
using Sirenix.OdinInspector.Editor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(AdditiveLevelLoader))]
public class AdditiveLevelLoaderEditor : OdinEditor
{
    void OnSceneGUI()
    {
        
        var levelLoader = target as AdditiveLevelLoader;

        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        if (!Event.current.alt)
            return;
        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.MouseDown:
                GUIUtility.hotControl = controlID;
                Debug.Log("MouseDown");
                Event.current.Use();
                break;

            case EventType.MouseUp:

                GUIUtility.hotControl = 0;
                Event.current.Use();

                Vector3 mousePosition = Event.current.mousePosition;
                Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

                var level = levelLoader.GetLevelFromPosition(ray.origin);
                var scene = EditorSceneManager.GetSceneByName(level.name);

                if (scene.isLoaded)
                    EditorSceneManager.CloseScene(scene, true);
                else
                    EditorSceneManager.OpenScene($"Assets/Scenes/AdditiveLoad Levels/{level.name}.unity", OpenSceneMode.Additive);



                break;
        }
    }
}
