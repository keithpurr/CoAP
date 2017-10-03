using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public float speed = 2.0f;
    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;
    private bool interactable = true;

	private float startTime;
	private Vector3 startPosition;
	private Vector3 targetPosition;
    private float journeyLength = 1.0f;


    void Start(){
        
        startPosition = transform.position;
        targetPosition = startPosition;
        startTime = Time.time;

    }


	void Update(){

        // move to targetPosition set by buttons
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPosition, targetPosition, fracJourney);

		// if last operation is not done yet (move 1 in given direction), button not interactive 
		if (targetPosition != transform.position){
            interactable = false;
        }else{
            interactable = true;
        }
		upButton.interactable = interactable;
		downButton.interactable = interactable;
		leftButton.interactable = interactable;
		rightButton.interactable = interactable;

		}

    // when collide into (invisible) walls, lock movement in one direction from crashing into the wall
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wallx"){
            Debug.Log("hitwallx");
            // for a wall on the left, keep x the same will prevent Player run into the wall
            targetPosition.x = transform.position.x;
        }
        if (collision.gameObject.tag == "Wallz"){
            Debug.Log("hitwallz");
            targetPosition.z = transform.position.z;
        }
        //if(collision.gameObject.tag == "Box"){
        //    Debug.Log("hit box");
        //    collision.gameObject.transform.parent = transform;
        //}
    }

    // called when according button is pressed
    public void MoveUpWard(){

        startTime = Time.time;
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.forward;
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
