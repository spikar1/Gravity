using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Crystal : MonoBehaviour, ITriggerable
{
    [SerializeField]
    ParticleSystem particles;
    bool disabled = false;

    public void OnTrigger(Player player)
    {
        if (disabled)
            return;
        disabled = true;
        particles.transform.right = player.velocity;
        particles.Play();
        player.power += 5;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 2);
    }
}
