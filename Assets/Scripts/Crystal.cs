using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

[SelectionBase]
public class Crystal : MonoBehaviour, ITriggerable
{
    [SerializeField]
    ParticleSystem particles;
    bool disabled = false;

    float randomOffset;
    Vector3 originalPos;

    [SerializeField]
    float freq = 1, amp = .1f;

    MMFeedbacks feedbacks;

    private void Awake()
    {
        randomOffset = Random.value * Mathf.PI;
        originalPos = transform.position;

        feedbacks = GetComponent<MMFeedbacks>();
    }



    private void Update()
    {
        if (disabled)
            return;

        transform.position = new Vector3(
            originalPos.x,
            originalPos.y + Mathf.Sin(randomOffset + Time.time* freq * Mathf.PI) * amp,
            originalPos.z);
    }

    public void OnTrigger(Player player)
    {
        if (disabled)
            return;
        
        particles.transform.right = player.velocity + ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
        particles.Play();
        var main = particles.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(Mathf.Clamp(player.velocity.magnitude/2, 0.6f, 5), Mathf.Clamp(player.velocity.magnitude / 2, 5, 13));
        player.ChargePowerAmount(5);
        GetComponent<SpriteRenderer>().enabled = false;
        feedbacks.PlayFeedbacks();
        disabled = true;

    }

    void OnObjectReset()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        disabled = false;
    }

    public void OnTriggerLeave(Player player)
    {
    }

    public void OnTriggerArrive(Player player)
    {
    }
}
