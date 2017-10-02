using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
	public float speed = 0.1F;
	public Vector2 startPos;
	public Vector2 direction;
	public bool directionChosen;

		void Update()
		{
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				// Get movement of the finger since last frame
	            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition.normalized;

				// Move object across XY plane
	            transform.Translate( -touchDeltaPosition.y * speed, 0,-touchDeltaPosition.x * speed, Space.Self);

	            Debug.Log(touchDeltaPosition); 

	            // obj should be moved according to its own coordination: x & z
			}
		}
	}

//	void Update()
//	{
//		// Track a single touch as a direction control.
//		if (Input.touchCount > 0)
//		{
//			Touch touch = Input.GetTouch(0);

//			// Handle finger movements based on touch phase.
//			switch (touch.phase)
//			{
//				// Record initial touch position.
//				case TouchPhase.Began:
//					startPos = touch.position;
//					directionChosen = false;
//					break;

//				// Determine direction by comparing the current touch position with the initial one.
//				//case TouchPhase.Moved:
//					//direction = touch.position - startPos;
//					//break;

//				// Report that a direction has been chosen when the finger is lifted.
//				case TouchPhase.Ended:
//                    directionChosen = true;
//                    direction = touch.position - startPos;
//                    Debug.Log(direction.normalized);
//					break;
//			}


//		}
//		//if (directionChosen)
//		//{
			
//		//}
//	}
//}