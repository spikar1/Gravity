using System;
using UnityEngine;

internal class Projectile : MonoBehaviour
{
    Transform target;

    float speed = 0;
    float acceleration = 3;

    public void Init(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        transform.right = target.transform.position - transform.position;

        speed += acceleration * Time.deltaTime;

        if (Vector2.Distance(transform.position, target.position) < 0.01f)
            DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}