using System;
using UnityEngine;
using System.Collections;
using Com.AugustCellars.CoAP;

public class ViewClient: MonoBehaviour
{
    private CoapClient client;

	// Use this for initialization
	void Start()
	{
        client = new CoapClient();
	}

    public void StartClient(){
        
        client.Uri = new Uri("coap://californium.eclipse.org:5683/large-update");
        var response = client.Put("haalo", MediaType.ApplicationJson);
        Debug.Log(response);
    }



}
