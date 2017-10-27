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
       
        //client.Uri = new Uri("coap://californium.eclipse.org:5683/");
        client.Uri = new Uri("coap://Qis-iPhone:5683/");

        //what about errorhandling? How to use CON?
        client.UseCONs();

    }

    public void ObserveResources(){

        client.UriPath = "/player_move";
        //client.Observe(MediaType.ApplicationJson, Notify);
        Response response =  client.Get();
        Console.WriteLine(response);
    }

    void Notify(Response response){
        //Debug.Log(response);
        //move = Convert.ToBase64String(response.Payload);
        Debug.Log("received payload: " + response.PayloadString);   
        Dispatcher.InvokeAsync(() =>
        {
            // this code is executed in main thread
            //playerController.MoveByController(move);
            move = response.PayloadString;
            playerController.MoveByController(move);
        });
    }

    public void CancelObservation(){

        // if the only way?
        Request request = new Request(Method.GET, true);
        request.MarkObserveCancel();
        client.Send(request);
    }





}
