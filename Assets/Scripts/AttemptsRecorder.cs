using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class AttemptsRecorder : MonoBehaviour
{


    public SpriteRenderer rendererToRecord;
    private float snapshotInterval = .01f;

    List<Attempt> attempts = new List<Attempt>();

    List<GameObject> spawnedReplayObjects = new List<GameObject>();

    int tryCount = 0;

    SpriteRenderer[] replayRenderers;
    Polyline[] replayPolyLines;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Playback();
    }

    IEnumerator Start()
    {
        rendererToRecord = rendererToRecord ? rendererToRecord : FindObjectOfType<Player>().GetComponent<SpriteRenderer>();
        if (!rendererToRecord)
            yield break;
        LevelManager.Instance.restartLevelDelegate += OnPlayerDeath;
        LevelManager.Instance.levelClearedDelegate += Playback;
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
        replayPolyLines = new Polyline[attempts.Count];
        var frame = 0;
        var numberOfCompletedAttempts = 0;
        while (true)
        {

            for (int i = 0; i < attempts.Count; i++)
            {
                if (replayRenderers[i] == null)
                {
                    replayRenderers[i] = new GameObject().AddComponent<SpriteRenderer>();
                    spawnedReplayObjects.Add(replayRenderers[i].gameObject);
                }
                var sr = replayRenderers[i];
                var attempt = attempts[i];

                UnityEngine.Random.InitState(i);
                if(frame == 0)
                {
                    var newPolyLine = new GameObject().AddComponent<Polyline>();
                    newPolyLine.points.Clear();
                    newPolyLine.Closed = false;
                    newPolyLine.Joins = PolylineJoins.Round;
                    newPolyLine.BlendMode = ShapesBlendMode.Screen;
                    newPolyLine.meshOutOfDate = true;

                    replayPolyLines[i] = newPolyLine;
                    var col = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);
                    col.a = .6f;
                    newPolyLine.Color = col;
                }
                if (attempt.positionArr.Count <= frame  )
                {
                    numberOfCompletedAttempts++;
                    continue;
                }

                if (frame % 5 == 0 )
                {
                    var polyLine = replayPolyLines[i];
                    polyLine.AddPoint(attempt.positionArr[frame]);
                }



                sr.transform.position = attempt.positionArr[frame];
                sr.sprite = attempt.spriteArr[frame];
                sr.flipX = attempt.isFlippedXArr[frame];
                sr.flipY = attempt.isFlippedYArr[frame];
                sr.material.color = attempt.colorArr[frame];


                continue;
                //If using spriteRederers for path
                if(frame % 5 == 0)
                {
                    UnityEngine.Random.InitState(i);
                    var ghost = new GameObject().AddComponent<SpriteRenderer>();
                    spawnedReplayObjects.Add(ghost.gameObject);
                    ghost.transform.position = attempt.positionArr[frame];
                    ghost.sprite = attempt.spriteArr[frame];
                    ghost.flipX = attempt.isFlippedXArr[frame];
                    ghost.flipY = attempt.isFlippedYArr[frame];
                    var col = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);
                    col.a = .2f;
                    ghost.material.color = col;
                }

            }
            if (numberOfCompletedAttempts >= attempts.Count)
                break;
            else
                numberOfCompletedAttempts = 0;
            yield return new WaitForSeconds(snapshotInterval);
            frame++;
        }
        yield return new WaitForSeconds(1);
        ClearPlaybacks();
        Playback();
        print("Is this at the end of all anims?");
    }

    private void ClearPlaybacks()
    {
        for (int i = spawnedReplayObjects.Count - 1; i >= 0; i--)
        {
            Destroy(spawnedReplayObjects[i].gameObject, UnityEngine.Random.Range(0, .4f));
        }
        for (int i = replayPolyLines.Length- 1; i >= 0; i--)
        {
            Destroy(replayPolyLines[i]);
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