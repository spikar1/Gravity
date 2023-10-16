using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using SteffenTools.Extensions;
using System;

[SelectionBase]
public class Doorway : SerializedMonoBehaviour, IInteractable
{
    public enum DoorwayType { Teleporter, SceneChanger, GoToNextBuildIndex }
    [EnumToggleButtons] [HideLabel]
    public DoorwayType doorwayType = DoorwayType.Teleporter;

    #region TeleporterSpecific

    [SerializeField, ShowIf("doorwayType", DoorwayType.Teleporter)]
    bool useDestinationDoorway;

    [SerializeField]
    [ShowIf("@this.doorwayType == DoorwayType.Teleporter && this.useDestinationDoorway == false")]
    bool useOffset = false;

    [SceneObjectsOnly, SerializeField]
    [ShowIf("@this.doorwayType == DoorwayType.Teleporter && this.useDestinationDoorway == false && useOffset == false")]
    [InlineButton("SelectOneWayDestination", label:"Select"  )]
    Transform destinationOneway;
    [SerializeField, ShowIf("@this.doorwayType == DoorwayType.Teleporter && this.useDestinationDoorway == false && useOffset == true")]
    Vector2 teleportOffset = Vector2.right * 4;

    [SceneObjectsOnly, SerializeField]
    [ShowIf("@this.doorwayType == DoorwayType.Teleporter && this.useDestinationDoorway == true")]
    [InlineButton("SelectOtherDoorway", label:"Select"  )]
    Doorway destinationDoorway;

#if UNITY_EDITOR
    void SelectOneWayDestination() { UnityEditor.Selection.activeObject = destinationOneway.gameObject; }
    void SelectOtherDoorway() { UnityEditor.Selection.activeObject = destinationDoorway.gameObject; }
#endif
    #endregion
    #region SceneChangerSpecific
    [SerializeField]
    [ShowIf("@this.doorwayType == DoorwayType.SceneChanger")]
    string linkedDoorIDTo = "insert an ID here";
    [HideInInspector] public string LinkedDoorIDTo => linkedDoorIDTo;

    [SerializeField]
    [ShowIf("@this.doorwayType == DoorwayType.SceneChanger")]
    string sceneName;

    [SerializeField]
    [ShowIf("@this.doorwayType == DoorwayType.SceneChanger")]
    [Tooltip("Leave at -1 to disable")]
    int hubSectionToUnlock = -1;

    [SerializeField]
    [ShowIf("@this.doorwayType == DoorwayType.SceneChanger")]
    bool useLinkedDoor = false;

    [SerializeField]
    [ShowIf("@this.doorwayType == DoorwayType.SceneChanger && this.useLinkedDoor == true")]
    string linkedDoorIDFrom = "";


    #endregion
    #region LevelClearSpecific



    #endregion
    #region Settings Params
    [Space(16), OnValueChanged("SetPromptUI")]
    [SerializeField]
    bool shouldPromptActivation = true;

    [SerializeField]
    bool playPlaybacks = true;
    void SetPromptUI()
    {
        if (promptActivationUI)
            promptActivationUI.SetActive(shouldPromptActivation);
    }

    [SerializeField, ShowIf("shouldPromptActivation")]
    GameObject promptActivationUI;

    #endregion
    #region Debug

    [SerializeField, ToggleGroup("useDebug")]
    bool useDebug;
    [SerializeField, ToggleGroup("useDebug")]
    bool printDebugMessages = true;
    [SerializeField, ToggleGroup("useDebug")]
    Color gizmosColor = Color.white;
    [Button, ToggleGroup("useDebug")]
    void TurnOnAllDebugs() => ToggleDebugOnAllObjects(true);
    [Button, ToggleGroup("useDebug")]
    void TurnOffAllDebugs() => ToggleDebugOnAllObjects(false);
    void ToggleDebugOnAllObjects(bool enabled)  
	{
        foreach (var doorway in FindObjectsOfType<Doorway>())
            doorway.useDebug = enabled;


    }

    #endregion
    #region runtime parameters
    Collider2D lastCollider;
    bool firstOverlapFrame = true;
    private bool isBeingTeleportedTo;
    #endregion

    Vector2 TeleportPosition {
        get {
            if (!useDestinationDoorway)
                if (useOffset)
                    return transform.position + (Vector3)teleportOffset;
                else
                    return destinationOneway.position;
            else
                return destinationDoorway.transform.position + Vector3.down * .24f;
        }
    }

    private void Awake()
    {
        if (promptActivationUI)
            promptActivationUI.SetActive(false);
    }

    private void Update()
    {
        var col = Physics2D.OverlapBox(transform.position, Vector2.one * .1f, 0, LayerMask.GetMask("Player"));
        if (col)
        {
            if (firstOverlapFrame)
            {
                firstOverlapFrame = false;
                OnTriggerEnter2D(col);
            }
            OnTriggerEnter2D(col);
            lastCollider = col;
        }
        else if (firstOverlapFrame == false)
        {
            firstOverlapFrame = true;
            OnTriggerExit2D(lastCollider);
        }
    }
    private void OnDrawGizmos()
    {
        if (!useDebug)
            return;

        Gizmos.color = gizmosColor;
        if(doorwayType == DoorwayType.Teleporter)
        {
            if (!useDestinationDoorway && destinationOneway)
                AdditionalGizmos.DrawArrowLine((Vector2)transform.position + Vector2.up * .2f, (Vector2)destinationOneway.position);
            else if (!useDestinationDoorway && !destinationOneway && useOffset)
            {
                var offsetPosition = (Vector2)transform.position + teleportOffset;
                AdditionalGizmos.DrawArrowLine((Vector2)transform.position + Vector2.up * .2f, offsetPosition);
                AdditionalGizmos.DrawCircle(offsetPosition, .25f, Axis.Z);
            }
            else if (useDestinationDoorway && destinationDoorway)
                AdditionalGizmos.DrawArrowLine((Vector2)transform.position + Vector2.up * .2f, (Vector2)destinationDoorway.transform.position + Vector2.down * .2f);

        }

        Gizmos.DrawWireCube(transform.position + Vector3.up * .5f, new Vector3(1.35f, 2.05f));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector2.one);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryPrint($"{collision.name} has triggered this doorway.");
        if (shouldPromptActivation && promptActivationUI)
            promptActivationUI.SetActive(true);
        if(!shouldPromptActivation && !isBeingTeleportedTo)
        {
            OnInteract(collision.GetComponent<Controller2D>());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryPrint($"{collision.name} is standing in the doorway. ");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TryPrint($"{collision.name} left the doorway. ");
        if (shouldPromptActivation && promptActivationUI)
            promptActivationUI.SetActive(false);
        isBeingTeleportedTo = false;
    }
    void TryPrint(object s)
    {
        if(printDebugMessages)
        print(s);
    }
    /*
    [MenuItem("MyTools/Create Door")]
    static void CreateDoor()
    {
        var newDoor = new GameObject();
        newDoor.AddComponent<Doorway>();
        newDoor.transform.position = SceneView.lastActiveSceneView.pivot;
        Selection.activeObject = newDoor;
    }
    */
    public void OnInteract(Controller2D controller)
    {
        if (doorwayType == DoorwayType.GoToNextBuildIndex)
        {
            if(playPlaybacks)
                LevelManager.Instance.LevelCleared();
            return;
        }
        if(doorwayType == DoorwayType.SceneChanger)
        {
            if(playPlaybacks)
                LevelManager.Instance.LevelCleared();
            else
            {
                if (useLinkedDoor)
                    GameManager.CurrentlyUsedLinkedDoorID = linkedDoorIDFrom;
                else
                    GameManager.CurrentlyUsedLinkedDoorID = "";
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                return;
            }
        }
        if (useDestinationDoorway)
            destinationDoorway.IsTeleportedTo();
        controller.Warp(TeleportPosition);

    }

    private void IsTeleportedTo()
    {
        isBeingTeleportedTo = true;
    }
}
