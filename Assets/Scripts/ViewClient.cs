using System;
using System.Net;
using UnityEngine;
using Com.AugustCellars.CoAP;
using UnityToolbag;
using Com.AugustCellars.CoAP.Net;

public class ViewClient: MonoBehaviour
{
    public GameObject playerCube;

    private CoapClient client;
    private Controller playerController;
    private string move;

	// Use this for initialization
	void Start()
	{
        IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 0);
        Debug.Log("\nAddress Family :" + localEp.AddressFamily);
        Debug.Log("\nIPEndPoint information:" + localEp.ToString());
        CoAPEndPoint ep = new CoAPEndPoint(localEp);

        client = new CoapClient(){EndPoint=ep};
        ep.Start();
        //client = new CoapClient();
        playerController = playerCube.GetComponent<Controller>();
	}

    //public void StartClient(){
       
    //    //client.Uri = new Uri("coap://californium.eclipse.org:5683/");
    //    client.Uri = new Uri("coap://Qis-iPhone:5683/");

    //    //what about errorhandling? How to use CON?
    //    client.UseCONs();

    //}

    // all 3 available resources including cam & cube color
    public void ObserveResources(){

        Debug.Log("observe button hit");

        client.Uri = new Uri("coap://192.168.0.19:5683");
        // Can't resolve Qis-iPhone
        //client.Uri = new Uri("coap://Qis-iPhone:5683/");

        //what about errorhandling? How to use CON?
        client.UseCONs();

        client.UriPath = "/player_move";
        //client.UriPath = "/obs";
        client.Observe(MediaType.ApplicationJson, Notify);
        //Response response =  client.Get();
        //Console.WriteLine(response);
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
