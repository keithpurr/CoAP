using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    public float speed = 2.0f;
    public float powerMagnitude = 10.0f;

	private float startTime;
	private Vector3 startPosition;
	private Vector3 targetPosition;
    private float journeyLength = 1.0f;

    //private Rigidbody _rigidbody;
    //private Vector3 force;
    //private bool buttonPushed = false;

    void Start(){
        
        startPosition = transform.position;
        targetPosition = startPosition;
        startTime = Time.time;

        //_rigidbody = GetComponent<Rigidbody>();

    }


	void Update(){

        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPosition, targetPosition, fracJourney);
        //if (buttonPushed)
        //{
        //    _rigidbody.AddForce(force * powerMagnitude);
        //    buttonPushed = false;
        //}

		}

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall"){
            Debug.Log("hitwall");
            // for a wall on the left, keep x the same will prevent Player run into the wall
            targetPosition.x = transform.position.x; 
        }
    }

    public void MoveUpWard(){

        startTime = Time.time;
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.forward;

        //force = Vector3.forward;
        //buttonPushed = true;
        }

	public void MoveDownWard()
	{

		startTime = Time.time;
		startPosition = transform.position;
        targetPosition = startPosition + Vector3.back;

	}

    public void MoveLeftWard()
	{

		startTime = Time.time;
		startPosition = transform.position;
        targetPosition = startPosition + Vector3.left;

	}
	public void MoveRightWard()
	{

		startTime = Time.time;
		startPosition = transform.position;
        targetPosition = startPosition + Vector3.right;

	}

}
