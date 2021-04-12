using UnityEngine;
using System.Collections;

public class followObject : MonoBehaviour {

    public Transform objToFollow;
	
	// Update is called once per frame
	void Update () {
        objToFollow = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = Vector3.Lerp(transform.position, new Vector3(objToFollow.position.x, objToFollow.position.y, transform.position.z), 5f * Time.deltaTime);
    }
}
