using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour {

    // Darren, I'm just doing this script in order to test the tower animation, you can freely edit it as you want

    public GameObject energyBall;
    public float verticalMovimentLimit;
    public float verticalMovimentSpeed;
    public float rotationSpeed;
    float initialPosition;
    bool goingUp;
    void Start()
    {
        initialPosition = energyBall.transform.position.y;
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
