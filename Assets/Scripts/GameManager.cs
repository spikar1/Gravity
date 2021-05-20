using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    string currentlyUsedLinkedDoorID;

    public static GameManager Instance => instance;

    public List<int> hubAreasUnlocked = new List<int>();

    public static string CurrentlyUsedLinkedDoorID {
        get => instance.currentlyUsedLinkedDoorID; 
        set => instance.currentlyUsedLinkedDoorID = value; 
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);    
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(currentlyUsedLinkedDoorID != "")
        {
            foreach (var doorway in FindObjectsOfType<Doorway>())
            {
                if (doorway.doorwayType == Doorway.DoorwayType.SceneChanger && doorway.LinkedDoorIDTo == currentlyUsedLinkedDoorID)
                {
                    GameObject.Find("Player").GetComponent<Controller2D>().Warp(doorway.transform.position);
                    BroadcastAll("Unhide", "");
                }
            }
        }
    }

    public static void BroadcastAll(string fun, System.Object msg)
    {
        GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in gos)
        {
            if (go && go.transform.parent == null)
            {
                go.gameObject.BroadcastMessage(fun, msg, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}