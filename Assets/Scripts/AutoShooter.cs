using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    [SerializeField]
    LayerMask aimableLayers;

    int aimRadius = 4;

    [SerializeField, AssetsOnly]
    Projectile projectilePrefab;

    float lastShotTime = 0;
    float cooldown = .4f;

    private void Update()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, aimRadius, aimableLayers);

        if(colliders.Length > 0 && (Time.time - lastShotTime) >= cooldown)
        {
            Shoot(GetClosestCollider(colliders));
            lastShotTime = Time.time;
        }
    }

    private void Shoot(Collider2D target)
    {
        Projectile projectile = Instantiate(projectilePrefab);
        projectile.transform.position = transform.position + Vector3.up;
        projectile.Init(target.transform);
    }

    private Collider2D GetClosestCollider(Collider2D[] colliders)
    {
        float closestDistance = float.MaxValue;
        Collider2D closestCollider = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            var collider = colliders[i];
            if(Vector2.Distance(collider.transform.position, transform.position) < closestDistance)
            {
                closestCollider = collider;
                closestDistance = Vector2.Distance(collider.transform.position, transform.position);
            }
        }

        return closestCollider;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, aimRadius);
    }
}
