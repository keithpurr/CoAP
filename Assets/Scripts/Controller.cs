using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    public float speed = 2.0f;
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

			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fracJourney);

		}

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
