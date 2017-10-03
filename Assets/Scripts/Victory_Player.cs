using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory_Player : MonoBehaviour {

    public GameObject target1;
    public GameObject target2;

    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            if (target1.GetComponent<Victory_Box>().boxOnTarget && target2.GetComponent<Victory_Box>().boxOnTarget){
                Debug.Log("you win the game");
            }
        }
    }

}
