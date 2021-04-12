using UnityEngine;
using System.Collections;


public class GridSnap : MonoBehaviour {

    public float gridsize = 1;
    public float modX, modY;


    //Vector3 size = Vector3.one;
    void OnDrawGizmosSelected()
    {
        if (GetComponent<Collider2D>())
        {
            //size = GetComponent<Collider2D>().bounds.size + Vector3.forward;
        }

        modX = transform.position.x % gridsize;
        modY = transform.position.y % gridsize;
        if (modX != 0)
        {
            Vector3 newXpos = Vector3.zero;
            if (Mathf.Abs(modX) > gridsize * .5f)
            {
                newXpos = new Vector3((transform.position.x - (modX)) + (gridsize * Mathf.Sign(transform.position.x)), transform.position.y, transform.position.z);
                transform.position = newXpos;
            }

            if (Mathf.Abs(modX) < gridsize * .5f)
            {
                newXpos = new Vector3(transform.position.x - (modX), transform.position.y, transform.position.z);
                transform.position = newXpos;
            }
        }
        if (modY != 0)
        {
            Vector3 newYpos = Vector3.zero;
            if (Mathf.Abs(modY) > gridsize * .5f)
            {

                newYpos = new Vector3(transform.position.x, (transform.position.y - (modY)) + (gridsize * Mathf.Sign(transform.position.y)), transform.position.z);
                transform.position = newYpos;
            }

            if (Mathf.Abs(modY) < gridsize * .5f)
            {
                newYpos = new Vector3(transform.position.x, transform.position.y - (modY), transform.position.z);
                transform.position = newYpos;
            }
        }
    }
}
