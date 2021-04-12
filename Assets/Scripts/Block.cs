using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    public LayerMask obstacles;

    //public bool hitBelow, hitAbove, hitLeft, hitRight;

    //public int bitwise;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*int sum = 0;

        if (Above()) sum += 1;
        if (Left()) sum += 2;
        if (Right()) sum += 4;
        if (Below()) sum += 8;
        
        bitwise = sum;

        //GetComponent<SpriteRenderer>().sprite 

        hitBelow = Below();
        hitAbove = Above();
        hitLeft = Left();
        hitRight = Right();*/
	}

    public int GetBit()
    {
        int sum = 0;
        if (North_West() && North() && West())  { sum += 1; }
        if (North())                            { sum += 2; }
        if (North_East() && North() && East())  { sum += 4; }
        if (West())                             { sum += 8; }
        if (East())                             { sum += 16; }
        if (South_West() && South() && West())  { sum += 32; }
        if (South())                            { sum += 64; }
        if (South_East() && South() && East())  { sum += 128; }
        return sum;
    }

    public bool North_West()
    {
        return Physics2D.OverlapCircle(transform.position + Vector3.up + Vector3.left, .4f, obstacles);
    }
    public bool North()
    {
        return Physics2D.OverlapCircle(transform.position + Vector3.up, .4f, obstacles);
    }
    public bool North_East()
    {
        return Physics2D.OverlapCircle(transform.position + Vector3.up + Vector3.right, .4f, obstacles);
    }
    public bool West()
    {
        return Physics2D.OverlapCircle(transform.position + Vector3.left, .4f, obstacles);
    }
    public bool East()
    {
        return Physics2D.OverlapCircle(transform.position + Vector3.right, .4f, obstacles);
    }
    public bool South_West()
    {
        return Physics2D.OverlapCircle(transform.position + Vector3.down + Vector3.left, .4f, obstacles);
    }
    public bool South()
    {
        return Physics2D.OverlapCircle(transform.position + Vector3.down, .4f, obstacles);
    }
    public bool South_East()
    {
        return Physics2D.OverlapCircle(transform.position + Vector3.down + Vector3.right, .4f, obstacles);
    }


}

