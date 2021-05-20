using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLevel : MonoBehaviour
{
    public AnimationCurve rotationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
    public float timeToTurn = 1;

    Coroutine currentCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentCoroutine == null)
            currentCoroutine = StartCoroutine(Turn(true));
        if (Input.GetKeyDown(KeyCode.Q) && currentCoroutine == null)
            currentCoroutine = StartCoroutine(Turn(false));
    }

    private IEnumerator Turn(bool clockwise)
    {
        print("Startyed");
        var orgRot = transform.rotation;
        var orgScale = transform.localScale;

        Time.timeScale = 0;

        var destinationAngles = clockwise ? -90: 90;
        for (float t = 0; t < 1; t += Time.unscaledDeltaTime / timeToTurn)
        {
            transform.rotation = orgRot * Quaternion.Euler(0,0, destinationAngles * rotationCurve.Evaluate(t));
            transform.localScale = orgScale * scaleCurve.Evaluate(t);
            yield return null;
        }
        transform.localScale = orgScale;
        transform.rotation = orgRot * Quaternion.Euler(0, 0, destinationAngles);

        Time.timeScale = 1;
        currentCoroutine = null;
    }
}
