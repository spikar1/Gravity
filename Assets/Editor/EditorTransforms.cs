using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorTransforms : ScriptableWizard
{
    void PlaceAllInCircle()
    {



        
    }

    public float radius = 1;
    public Vector3 centroid;

    public GameObject[] objs = new GameObject[0];

    [MenuItem("Tools/SteffenTools/Place Objects in circle")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<EditorTransforms>("EditorTransforms", "Done", "Update selection of Game Objects");
        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
    }

    void OnWizardCreate()
    {
    }

    void OnWizardUpdate()
    {
        helpString = "";
        if (objs.Length <= 0)
            return;
        centroid = objs[0].transform.position;
        for (int i = 1; i < objs.Length; i++)
        {
            var obj = objs[i];
            centroid += obj.transform.position;
        }
        centroid = centroid / (float)objs.Length;
        SteffenTools.Extensions.DebugDrawers.DrawCircle(centroid, radius, SteffenTools.Extensions.Axis.Z, Color.white, 0, objs.Length);
        for (int i = 0; i < objs.Length; i++)
        {
            var obj = objs[i];
            Undo.RecordObject(obj.transform, "Placed objects in a circle");
            var delta = (float)i / objs.Length * Mathf.PI * 2;
            float x = Mathf.Sin(delta) * radius;
            float y = Mathf.Cos(delta) * radius;
            var pos = new Vector3(x, y, centroid.z);
            obj.transform.position = pos + centroid;
        }
    }

    void OnWizardOtherButton()
    {
        objs = Selection.gameObjects;
        
    }

    
}
