using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveForward : MonoBehaviour {

	public float speed = 5f;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().velocity = transform.forward * speed;		
	}

}
