using UnityEngine;
using System.Collections;

public class GravityLock : MonoBehaviour
{
    public bool lockUp = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {

        print("gravity locked");
        collision.collider.GetComponent<Player>().gravity = 100;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        print("is triggered");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        collision.GetComponent<Player>().gravityLock = true;
        if (lockUp)
        {
            collision.GetComponent<Player>().gravity = -.6f;
        }
        else
        {
            collision.GetComponent<Player>().gravity = .6f;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<Player>().gravityLock = false;
    }
}
