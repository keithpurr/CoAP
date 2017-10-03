using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory_Box : MonoBehaviour {

    public bool boxOnTarget = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Box")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            boxOnTarget = true;

            Debug.Log("box on target"+ gameObject.name);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Box"){
            boxOnTarget = false;
        }
    }
}
