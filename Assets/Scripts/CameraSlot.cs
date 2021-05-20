using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlot : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(20, 20, 0));
    }
}
