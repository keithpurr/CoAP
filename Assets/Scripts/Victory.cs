using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Box")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log("You win");
        }
    }
}
