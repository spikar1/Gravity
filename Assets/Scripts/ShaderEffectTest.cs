using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderEffectTest : MonoBehaviour
{

    public AnimationCurve curve;

    void InitiateEffect()
    {
        StartCoroutine(Effect());
    }
    IEnumerator Effect()
    {
        for (float f = 0; f < curve.keys[curve.length - 1].time; f+= Time.deltaTime)
        {

            yield return null;
        }
    }
}
