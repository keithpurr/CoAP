using System;
using UnityEngine;
using Com.AugustCellars.CoAP;
using UnityToolbag;

public class ViewClient: MonoBehaviour
{
    public GameObject playerCube;

    private CoapClient client;
    private Controller playerController;
    private string move;

	// Use this for initialization
	void Start()
	{
        client = new CoapClient();
        playerController = playerCube.GetComponent<Controller>();
	}

    public void StartClient(){
        client.UseCONs();
        //1. observe to 3 resources
        client.Uri = new Uri("coap://californium.eclipse.org:5683/obs");
        //what about error? How to use CON?
        client.Observe(MediaType.ApplicationJson, Notify);

    }

    void Notify(Response response){
        //Debug.Log(response);
        move = Convert.ToBase64String(response.Payload);
        Debug.Log(move);   
        Dispatcher.InvokeAsync(() =>
        {
            // this code is executed in main thread
            //playerController.MoveByController(move);
            playerController.MoveByController("up");
        });
    }

    void CancelNotification(){

        // if the only way?
        Request request = new Request(Method.GET, true);
        request.MarkObserveCancel();
        client.Send(request);
    }





}
