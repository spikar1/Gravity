using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectOnLevelReset : MonoBehaviour
{
    private Vector3 startPosition;



    void Start()
    {
        LevelManager.Instance.restartLevelDelegate += ResetObject;
        startPosition = transform.position;
    }

    internal void DisableObject()
    {
        transform.position += new Vector3(1000, 1000, 0);
    }

    private void ResetObject()
    {
        transform.position = startPosition;
        gameObject.SetActive(true);

        gameObject.BroadcastMessage("OnObjectReset", SendMessageOptions.DontRequireReceiver);

    }

    private void OnDisable()
    {
        LevelManager.Instance.restartLevelDelegate -= ResetObject;
    }
}
