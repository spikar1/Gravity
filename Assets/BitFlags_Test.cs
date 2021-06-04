using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitFlags_Test : MonoBehaviour
{
    
    [System.Flags]
    public enum Test
    {
        a = 1 << 1,
        b = 1 << 2,
        c = 1 << 3,
        d = 1 << 4,
        e = 1 << 5
    }
    public Test test;
}
