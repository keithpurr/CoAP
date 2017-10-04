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


    void CoAPRequestReceivedHandler(CoAPRequest coapReq)
    {


        Debug.Log("Received Request::" + coapReq);

        message = coapReq.ToString();

        Debug.Log($"uri path: {coapReq.GetPath()}");

        string reqURIPath = (coapReq.GetPath() != null) ? coapReq.GetPath().ToLower() : "";

        // the NON, POST 

        //This sample only works on NON requests of type POST
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
        else if (coapReq.Code.Value != CoAPMessageCode.POST)
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

            // Request contains instruction from controller
            // Check if request received contains “CONTENT”
            //if (coapReq.Code.Value == CoAPMessageCode.POST)
            //{
            // Get the instruction (move/color) from the payload byte array
            // By convert that byte array to UTF-8 string
            // tempAsJSON is a JSON string in the format {“instruction” : NN}

            string instAsJSON = AbstractByteUtils.ByteToStringUTF8(coapReq.Payload.Value);

			Debug.Log("received instruction:" + instAsJSON);
            // hashable key/value pair
            Hashtable instructionValues = JSONResult.FromJSON(instAsJSON);


            string instruction = instructionValues["instruction"].ToString();
            //Now do something with this temperature received from the server
    
            Debug.Log("the instruction received is:" + instruction);
  
        }

    }
}
