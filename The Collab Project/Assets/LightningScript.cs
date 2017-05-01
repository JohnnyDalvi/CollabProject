using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningScript : MonoBehaviour {

    ParticleSystem lightning;
    void Start ()
    {
        lightning = transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(gameObject, lightning.duration + lightning.startLifetime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
