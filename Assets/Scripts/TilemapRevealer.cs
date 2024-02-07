using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TilemapRevealer : MonoBehaviour
{
    bool shouldReveal = false;
    Tilemap tilemap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
            shouldReveal = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
            shouldReveal = false;
    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (shouldReveal)
            tilemap.color = Color.Lerp(tilemap.color, new Color(1, 1, 1, 0), Time.deltaTime * 15f);
        else
            tilemap.color = Color.Lerp(tilemap.color, Color.white, Time.deltaTime * 15f);
    }
}
