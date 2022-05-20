using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustPad : MonoBehaviour
{
    public float thrustPower = 10f;
    public float thrustTime = 10f;
    float thrusttimer;
    public bool thrusting = false;
    public Controller controller;
    public GameObject player = null;
    public CharacterController charController = null;
    // Start is called before the first frame update

    private void Start()
    {
        thrusttimer = thrustTime;
    }

    private void Update()
    {
        if (thrusting) {
            Thrust();
        }
    }

    private void LateUpdate()
    {
        //player.AddComponent<Controller>().velocityY = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("THRUST ENTER");
        if (other == charController)
        {
            thrusting = true;
        }
    }

    void Thrust() {
        if (thrusttimer > 0)
        {
            charController.Move(transform.right * thrustPower);
            thrusttimer -= 1;
        }
        else {
            thrusttimer = thrustTime;
            controller.velocityY = 10f;
            thrusting = false;
        }
        
    }
}
