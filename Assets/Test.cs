using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EXILANT.Labs.CoAP.Channels;
using EXILANT.Labs.CoAP.Message;
using System;

public class Test : MonoBehaviour {

	private static CoAPClientChannel _coapClient = null;

	private static string _mToken = "";

	// Use this for initialization
	void Start () {
		
		StartCoAPClient();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void StartCoAPClient()
	{
		Debug.Log ("about to start server");

		//string host = "Qide-iPhone";
        string host = "Qis-iPhone";

		int serverPort = 5683;

		_coapClient = new CoAPClientChannel();

		try {_coapClient.Initialize(host, serverPort);}
        catch(Exception e){
			
			Debug.Log (e.Message);
		}

		Debug.Log ("server started");

		_coapClient.CoAPResponseReceived += CoAPResponseReceivedHandler;

		//_coapClient.CoAPRequestReceived += OnCoAPRequestReceived;
		//_coapClient.CoAPError += CoAPErrorHandler;


		// Send NON request and expecting NON response
		CoAPRequest coapReq = new CoAPRequest(CoAPMessageType.NON,
			CoAPMessageCode.GET,
			100);//hardcoded message ID as is used only once

		// example 1: temp
		//coapReq.SetURL("coap://localhost:5683/sensors/temp");

		// example: /.well-know/core
		coapReq.SetURL("coap://localhost:5683/.well-known/core");

		_mToken = DateTime.Now.ToString("HHmmss");//Token value must be less than 8 bytes

		coapReq.Token = new CoAPToken(_mToken);//A random token

		Debug.Log(coapReq);

		_coapClient.Send(coapReq);



		//Thread.Sleep(Timeout.Infinite);//blocks
	}

	private void CoAPResponseReceivedHandler(CoAPResponse coapResp){

		Debug.Log ("got it");
		Debug.Log (coapResp);
	}
}
