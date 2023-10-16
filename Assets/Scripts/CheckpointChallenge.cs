using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointChallenge : MonoBehaviour, ITriggerable
{
    private bool playerHasEntered;

    [SerializeField]
    List<CheckpointData> checkpoints = new List<CheckpointData>();

    int checkpointIndex = 0;
    [SerializeField, AssetsOnly]
    Checkpoint checkpointPrefab;
    private float challengeStartTime;

    public void OnTrigger(Player player)
    {
    }


    public void OnTriggerArrive(Player player)
    {
        foreach (var checkpoint in checkpoints)
            

        playerHasEntered = true;
        checkpointIndex = 0;

    }

    public void OnTriggerLeave(Player player)
    {
        if (playerHasEntered)
            StartChallenge();
    }

    private void StartChallenge()
    {
        challengeStartTime = Time.time;
        SpawnNextCheckpoint();
    }

    internal void CheckpointReached(Checkpoint checkpoint)
    {
        Destroy(checkpoint.gameObject);
        checkpointIndex++;
        if (checkpointIndex >= checkpoints.Count)
            ChallengeComplete();
        else
            SpawnNextCheckpoint();
    }

    private void ChallengeComplete()
    {
        print($"You win! Time: {Time.time - challengeStartTime}");
    }

    private void SpawnNextCheckpoint()
    {
        var nextCheckpoint = Instantiate(checkpointPrefab);
        nextCheckpoint.transform.position = checkpoints[checkpointIndex].position;
        nextCheckpoint.challenge = this;
    }

    private void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        foreach (var checkpoint in checkpoints)
        {
            Gizmos.color = Color.yellow;
            Vector2 size = Vector3.one;
            size.x = checkpoint.vertical ? 1 : checkpoint.size;
            size.y = checkpoint.vertical ? checkpoint.size : 1;
            Vector2 offset;
            offset.x = checkpoint.vertical ? 0 : (float)checkpoint.size / 2 - 0.5f;
            offset.y = checkpoint.vertical ? (float)checkpoint.size / 2 - 0.5f : 0;

            Gizmos.DrawWireCube(checkpoint.position+ offset, size);
        }
    }

    [Serializable]
    struct CheckpointData
    {
        public Vector2 position;
        public bool vertical;
        [Range(1f, 99f)]
        public int size;
    }
}
