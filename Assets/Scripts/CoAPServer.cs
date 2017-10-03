using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EXILANT.Labs.CoAP.Message;
using EXILANT.Labs.CoAP.Channels;
using System.Net;
using System;
using EXILANT.Labs.CoAP.Helpers;
using UnityEngine.UI;

public class CoAPServer : MonoBehaviour
{
    public Vector2 scrollPosition = Vector2.zero;

    private string message = "";

    private CoAPServerChannel _coapServer = null;

	void OnGUI()
	{
        
		message = GUI.TextArea(new Rect(300, 50, 400, 400), message, 500);
	}


    public void StartCoAPServer()
    {
        _coapServer = new CoAPServerChannel();

        _coapServer.Initialize(Dns.GetHostName(), 5683);

        _coapServer.CoAPRequestReceived += CoAPRequestReceivedHandler;

        //_coapServer.CoAPResponseReceived += CoAPResponseReceivedHandler;

        //_coapServer.CoAPError += CoAPErrorHandler;

        Debug.Log($"the hostname is{Dns.GetHostName()}");

        //txtView.Text = "CoAP Server Started.";
    }

    private int GetRoomTemperature()
    {
        int temp = DateTime.Now.Second;
        if (temp < 15) temp = 25;// just do not want to show that it's too cold!
        return temp;
    }

    void CoAPRequestReceivedHandler(CoAPRequest coapReq)
    {

        //InvokeOnMainThread(() =>
        //{
        //	txtView.Text = coapReq.ToString();
        //});
         //For UIKit to update the screen, your code needs to run the main loop. 

        Debug.Log("Received Request::" + coapReq);

        message = coapReq.ToString();

        Debug.Log($"uri path: {coapReq.GetPath()}");

        string reqURIPath = (coapReq.GetPath() != null) ? coapReq.GetPath().ToLower() : "";

		// the NON, GET temp example




		  //This sample only works on NON requests of type GET
		  //This sample simualtes a temperature sensor at the path "sensors/temp"
		  if (coapReq.MessageType.Value != CoAPMessageType.NON)
		  {
		      //only NON  combination supported..we do not understand this send a RST back
		      CoAPResponse msgTypeNotSupported = new CoAPResponse(CoAPMessageType.RST, /*Message type*/
		                                                          CoAPMessageCode.NOT_IMPLEMENTED, /*Not implemented*/
		                                                          coapReq.ID.Value /*copy message Id*/);
		      msgTypeNotSupported.Token = coapReq.Token; //Always match the request/response token

		      //send response to client
		      _coapServer.Send(msgTypeNotSupported);
		  }
		  else if (coapReq.Code.Value != CoAPMessageCode.GET)
		  {
		      //only GET method supported..we do not understand this send a RST back
		      CoAPResponse unsupportedCType = new CoAPResponse(CoAPMessageType.RST, /*Message type*/
		                                          CoAPMessageCode.METHOD_NOT_ALLOWED, /*Method not allowed*/
		                                          coapReq /*copy message Id*/);
		      unsupportedCType.Token = coapReq.Token; //Always match the request/response token
		      //send response to client
		      _coapServer.Send(unsupportedCType);
		  }
		  else if (reqURIPath != "sensors/temp")
		  {
		      //classic 404 not found..we do not understand this send a RST back 
		      CoAPResponse unsupportedPath = new CoAPResponse(CoAPMessageType.RST, /*Message type*/
		                                          CoAPMessageCode.NOT_FOUND, /*Not found*/
		                                          coapReq /*copy message Id*/);
		      unsupportedPath.Token = coapReq.Token; //Always match the request/response token
		      //send response to client
		      _coapServer.Send(unsupportedPath);
		  }
		  else
		  {

		      //All is well...send the measured temperature back
		      //Again, this is a NON message...we will send this message as a JSON
		      //string
		      Hashtable valuesForJSON = new Hashtable();
		      valuesForJSON.Add("temp", this.GetRoomTemperature());
		      string tempAsJSON = JSONResult.ToJSON(valuesForJSON);
		              //Now prepare the object

		              CoAPResponse measuredTemp = new CoAPResponse(CoAPMessageType.NON, CoAPMessageCode.CONTENT, coapReq)
		              {
		                  Token = coapReq.Token, //Always match the request/response token
		                                         //Add the payload
		                  Payload = new CoAPPayload(tempAsJSON)
		              };
		              //Indicate the content-type of the payload
		              measuredTemp.AddOption(CoAPHeaderOption.CONTENT_FORMAT,
		                          AbstractByteUtils.GetBytes(CoAPContentFormatOption.APPLICATION_JSON));

		              // can pass whole request in when construct a new response, that way server know where to send 
		              // the response instead of having to set remote sender (does not work) separately

		      //send response to client
		              Console.WriteLine($" temp returned : {measuredTemp}");

		       _coapServer.Send(measuredTemp);

		  }
		}

	}
