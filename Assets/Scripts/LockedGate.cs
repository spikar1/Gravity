using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedGate : MonoBehaviour
{

    public int gateLockID = 0;

    void OnCollisionEnter()
    {
        if (FindObjectOfType<Player>().keyIDs.Contains(gateLockID))
            StartCoroutine(OpenGate());
    }

    public IEnumerator OpenGate()
    {
        yield return new WaitForSeconds(.05f);
        foreach (var col in Physics2D.OverlapBoxAll(transform.position, Vector2.one * 1.1f, 0))
        {
            var otherGate = col.GetComponent<LockedGate>();
            if (otherGate && otherGate.gateLockID == gateLockID)
                otherGate.StartCoroutine(otherGate.OpenGate());
        }
        GetComponent<ResetObjectOnLevelReset>().DisableObject();

    }

    void OnObjectReset()
    {
        StopAllCoroutines();
    }

    internal void Unlock()
    {
        //Destroy(gameObject);
    }
}
