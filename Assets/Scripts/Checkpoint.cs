using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, ITriggerable
{
    public CheckpointChallenge challenge;

    public void OnTrigger(Player player)
    {
    }

    public void OnTriggerArrive(Player player)
    {
        print("nbciods");
        challenge.CheckpointReached(this);
    }

    public void OnTriggerLeave(Player player)
    {
        print("leave");
    }
}
