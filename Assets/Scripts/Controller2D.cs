using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

    public LayerMask collisionMask;

    const float skinWidth = .0015f;
    public float horizontalRayCount;
    public float verticalRayCount;

    [HideInInspector]
    public BoxCollider2D col;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    private float coyoteLandingLeeway = .1f;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    //todo: Flytte velocity hit?
    public void Move (Vector2 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        

        if(velocity.x!= 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if(velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }


        transform.position += velocity.toVector3();
    }

    void HorizontalCollisions(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (i * horizontalRaySpacing);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            if (hit)
            {
                if ((LayerMask.LayerToName(hit.collider.gameObject.layer) == "Platform" && (directionY != Mathf.Sign(Player.gravity) || directionY == 0)))
                    return;

                if (i == 0 && CheckCoyoteLanding(ref velocity))
                {
                    transform.position += (Vector3)(Vector3.right * Mathf.Sign(velocity.x) * hit.distance * 1.3f);
                    return;
                }

                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
                hit.collider.SendMessage("OnCollisionEnter", SendMessageOptions.DontRequireReceiver);
                return;
            }
        }
    }

    private bool CheckCoyoteLanding(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
        rayOrigin += Vector2.up * coyoteLandingLeeway;

        Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.magenta, 10);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

        return hit ? false : true;
    }

    void VerticalCollisions( ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (i * verticalRaySpacing + velocity.x);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            if (hit)
            {
                if ((LayerMask.LayerToName(hit.collider.gameObject.layer) == "Platform" && (directionY != Mathf.Sign(Player.gravity) || directionY == 0)))
                    return;

                velocity.y = (hit.distance - skinWidth) * directionY;
                collisions.above = directionY == +1;
                collisions.below = directionY == -1;
                hit.collider.SendMessage("OnCollisionEnter", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }

    internal void Warp(Vector3 newPosition)
    {
        transform.position = newPosition;
        
    }
}
