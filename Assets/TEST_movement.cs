using UnityEngine;
using System.Collections;

public class TEST_movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.K))
        transform.Translate(Vector2.down);
        if (Input.GetKeyDown(KeyCode.L))
            transform.Translate(Vector2.up);
    }
}