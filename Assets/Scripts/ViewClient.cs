using System;
using System.Net;
//using System.Drawing;
using UnityEngine;
using Com.AugustCellars.CoAP;
using UnityToolbag;
using Com.AugustCellars.CoAP.Net;

public class ViewClient: MonoBehaviour
{
    public GameObject playerCube;

    private CoapClient client;

    private Controller playerController;
    private Renderer playerRenderer;

    private string move;
    private string color;
    // defined in UnityEngine.Color
    private Color updateColor = Color.white;
    private string view;

	// Use this for initialization
	void Start()
	{
        IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 0);
        Debug.Log("\nAddress Family :" + localEp.AddressFamily);
        Debug.Log("\nIPEndPoint information:" + localEp.ToString());
        CoAPEndPoint ep = new CoAPEndPoint(localEp);

        client = new CoapClient(){EndPoint=ep};
        ep.Start();

        playerController = playerCube.GetComponent<Controller>();
        playerRenderer = playerCube.GetComponent<Renderer>();
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
        client.Observe(MediaType.ApplicationJson, NotifyMove);

        client.UriPath = "/player_color";
        client.Observe(MediaType.ApplicationJson, NotifyColor);

        //client.UriPath = "/camera_view";
        //client.Observe(MediaType.ApplicationJson, NotifyView);
    }

    void NotifyMove(Response response){

        Debug.Log("received payload: " + response.PayloadString);   
        Dispatcher.InvokeAsync(() =>
        {
            // this code is executed in main thread
            //playerController.MoveByController(move);
            move = response.PayloadString;
            playerController.MoveByController(move);
        });
    }

    void NotifyColor(Response response)
    {

        Debug.Log("received payload: " + response.PayloadString);
        Dispatcher.InvokeAsync(() =>
        {
            // this code is executed in main thread
            //playerController.MoveByController(move);
            color = response.PayloadString;

            switch (color){
                case "Red":
                    updateColor = Color.red;
                    break;
                case "Green":
                    updateColor = Color.green;
                    break;
                case "Blue":
                    updateColor = Color.blue;
                    break;
                case "Pink":
                    updateColor = Color.pink;
                    break;
                case "Purple":
                    updateColor = Color.purple;
                    break;
                case "White":
                    updateColor = Color.white;
                    break;
                default:
                    updateColor = Color.white;
                    break;
            }
            playerRenderer.material.SetColor("_Color", updateColor);
        });
    }



    public void CancelObservation(){
           
        // if the only way?
        Request request = new Request(Method.GET, true);
        request.MarkObserveCancel();
        client.Send(request);
    }





}
