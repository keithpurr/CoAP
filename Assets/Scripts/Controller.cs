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

    private bool ifCollide = false;
    private Vector3 positionDiff;
    //[SerializeField]
    //private Transform[] boxInContact =  new Transform[2];
    private Transform boxInContact;
    private Vector3 moveTranslate;



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
        // everytime stand next to a box, calculate there position difference for judging
        // if player going to push the box
        if(collision.gameObject.tag == "Box"){
            Debug.Log("hit box");
            ifCollide = true;
            positionDiff = collision.transform.position - transform.position;
            // what about multiple boxes in contact
            boxInContact = collision.transform;
            //boxInContact.SetValue(collision.gameObject.transform,0);
            //collision.gameObject.transform.parent = transform;
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Box"){
            ifCollide = false;

        }
    }

    // called when according button is pressed
    public void MoveUpWard()
    {

        startTime = Time.time;
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.forward;

        CarryBoxOrPutDown(Vector3.forward);

        //if (ifCollide && positionDiff == Vector3.forward)
        //{
        //    Debug.Log("about to push the box forward");
        //    boxInContact.SetParent(transform);
        //    //what about leaving
        //}
        //if (ifCollide && positionDiff != Vector3.forward){
        //    Debug.Log("Leaving");
        //    transform.DetachChildren();
        //}
    }

	public void MoveDownWard()
	{

		startTime = Time.time;
		startPosition = transform.position;
        targetPosition = startPosition + Vector3.back;

		CarryBoxOrPutDown(Vector3.back);

	}

    public void MoveLeftWard()
	{

		startTime = Time.time;
		startPosition = transform.position;
        targetPosition = startPosition + Vector3.left;

		CarryBoxOrPutDown(Vector3.left);

	}
	public void MoveRightWard()
	{

		startTime = Time.time;
		startPosition = transform.position;
        targetPosition = startPosition + Vector3.right;

		CarryBoxOrPutDown(Vector3.right);

	}

    private void CarryBoxOrPutDown (Vector3 direction){
        
        if (ifCollide && positionDiff == direction)
		{
			Debug.Log("about to push the box forward");
			boxInContact.SetParent(transform);
			//what about leaving
		}
        if (ifCollide && positionDiff != direction && boxInContact.parent == transform)
		{
			Debug.Log("Leaving");
			transform.DetachChildren();
		}
    }

    public void MoveByController(string direction){

        Debug.Log("moveplayerbycontroller is called");

        switch(direction){
            case "left":
                moveTranslate = Vector3.left;
                break;
            case "right":
                moveTranslate = Vector3.right;
                break;
            case "up":
                moveTranslate = Vector3.forward;
                break;
            case "down":
                moveTranslate = Vector3.back;
                break;
            default:
                moveTranslate = Vector3.zero;
                break;

        }
        
		startTime = Time.time;
		startPosition = transform.position;
        targetPosition = startPosition + moveTranslate;

        CarryBoxOrPutDown(moveTranslate);

        Debug.Log("player moved");
    }

}
