using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtemptsRecorder : MonoBehaviour
{


    public SpriteRenderer rendererToRecord;
    private float snapshotInterval = .01f;

    List<Attempt> attempts = new List<Attempt>();

    int tryCount = 0;

    SpriteRenderer[] replayRenderers;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Playback();
    }

    IEnumerator Start()
    {
        if (!rendererToRecord)
            yield break;
        GameManager.Instance.restartLevelDelegate += OnPlayerDeath;
        GameManager.Instance.levelClearedDelegate += Playback;
        while (true)
        {
            if (attempts.Count <= tryCount)
                attempts.Add(new Attempt());

            var attempt = attempts[tryCount];
            attempt.positionArr.Add(rendererToRecord.transform.position);
            attempt.spriteArr.Add(rendererToRecord.sprite);
            attempt.isFlippedXArr.Add(rendererToRecord.flipX);
            attempt.isFlippedYArr.Add(rendererToRecord.flipY);
            attempt.colorArr.Add(rendererToRecord.material.color);
            yield return new WaitForSeconds(snapshotInterval);
        }
    }

    [ContextMenu("Playback")]
    void Playback()
    {
        StopAllCoroutines();
        StartCoroutine(PlaybackRoutine());
    }

    private IEnumerator PlaybackRoutine()
    {

        replayRenderers = new SpriteRenderer[attempts.Count];
        var frame = 0;
        while (true)
        {
            for (int i = 0; i < attempts.Count; i++)
            {
                if (replayRenderers[i] == null)
                    replayRenderers[i] = new GameObject().AddComponent<SpriteRenderer>();
                var sr = replayRenderers[i];
                var attempt = attempts[i];

                if (attempt.positionArr.Count <= frame  )
                {
                    continue;
                }

                sr.transform.position = attempt.positionArr[frame];
                sr.sprite = attempt.spriteArr[frame];
                sr.flipX = attempt.isFlippedXArr[frame];
                sr.flipY = attempt.isFlippedYArr[frame];
                sr.material.color = attempt.colorArr[frame];
                if(frame % 5 == 0)
                {
                    UnityEngine.Random.InitState(i);
                    var ghost = new GameObject().AddComponent<SpriteRenderer>();
                    ghost.transform.position = attempt.positionArr[frame];
                    ghost.sprite = attempt.spriteArr[frame];
                    ghost.flipX = attempt.isFlippedXArr[frame];
                    ghost.flipY = attempt.isFlippedYArr[frame];
                    var col = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);
                    col.a = .3f;
                    ghost.material.color = col;
                }

            }
            yield return new WaitForSeconds(snapshotInterval);
            frame++;
        }


    }

    void OnPlayerDeath()
    {
        tryCount++;

    }
}

class Attempt
{
    public List<Vector3> positionArr = new List<Vector3>();
    public List<Sprite> spriteArr = new List<Sprite>(); //todo: Change to some kind of state or string something
    public List<bool> isFlippedXArr = new List<bool>();
    public List<bool> isFlippedYArr = new List<bool>();
    public List<Color> colorArr = new List<Color>();
}