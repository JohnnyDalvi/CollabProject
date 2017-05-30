using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveForward : MonoBehaviour {

	public float maxSpeed = 5f;
	public float minSpeed = 1f;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().velocity = transform.forward * Random.Range(minSpeed, maxSpeed);		
	}

}
