using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour {

	// Darren, I'm just doing this script in order to test the tower animation, you can freely edit it as you want, 
	//nope, looks just dandy the way it is :)

    public GameObject energyBall;
    public Transform[] lightningPositions;
    public float verticalMovimentLimit;
    public float verticalMovimentSpeed;
    public float rotationSpeed;
    public GameObject lightningParticle;
    float initialPosition;
    bool goingUp;
    void Start()
    {
        initialPosition = energyBall.transform.position.y;
        InvokeRepeating("LightningBall", 0, 0.2f);
    }

    void LightningBall()
    {
        foreach(Transform lightning in lightningPositions)
        {
            float roll = Random.Range(0, 1f);
            if (roll >= 0.9f)
                Instantiate(lightningParticle, lightning);
        }
    }

    void Update ()
    {
        energyBall.transform.Rotate(Vector3.forward * rotationSpeed *Time.deltaTime);

        if (goingUp)
        {
            energyBall.transform.position += (Vector3.up * verticalMovimentSpeed * Time.deltaTime);
            if (energyBall.transform.position.y - initialPosition > verticalMovimentLimit)
            {
                goingUp = false;
            }
        }
        else
        {
            energyBall.transform.position += (Vector3.down * verticalMovimentSpeed * Time.deltaTime);
            if (energyBall.transform.position.y-initialPosition < -verticalMovimentLimit)
            {
                goingUp = true;
            }
        }
        	
	}
}
