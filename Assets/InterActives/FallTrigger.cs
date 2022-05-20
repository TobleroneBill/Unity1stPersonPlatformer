using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Debug.Log(other.gameObject);
            player.GetComponent<Controller>().respawn = true;
        }
    }

}
