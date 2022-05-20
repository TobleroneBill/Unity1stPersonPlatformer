using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpHeight = 10f;
    public CharacterController player;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == player) {
            Debug.Log("Hello?");
            player.GetComponent<Controller>().velocityY = jumpHeight;
        }
        
    }

}
