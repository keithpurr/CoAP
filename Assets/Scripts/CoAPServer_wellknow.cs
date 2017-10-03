using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EXILANT.Labs.CoAP.Message;
using EXILANT.Labs.CoAP.Channels;
using System.Net;
using System;
using EXILANT.Labs.CoAP.Helpers;
using UnityEngine.UI;

public class CoAPServer1 : MonoBehaviour
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
        /**
			* Draft 18 of the specification, section 5.2.3 states, that if against a NON message,
			* a response is required, then it must be sent as a NON message
			*/

        // /.well-know/ example


        /*
   * For well-know path, we should support both CON and NON.
   * For NON request, we send back the details in another NON message
   * For CON request, we send back the details in an ACK
   */
        /*Well known should be a GET*/
        if (coapReq.Code.Value != CoAPMessageCode.GET)
        {
            if (coapReq.MessageType.Value == CoAPMessageType.CON)
            {
                CoAPResponse resp = new CoAPResponse(CoAPMessageType.ACK,
                                                    CoAPMessageCode.METHOD_NOT_ALLOWED,
                                                    coapReq /*Copy all necessary values from request in the response*/);
                //When you use the constructor that accepts a request, then automatically
                //the message id , token and remote sender values are copied over to the response
                _coapServer.Send(resp);
            }
            else
            {
                //For NON, we can only send back a RST
                CoAPResponse resp = new CoAPResponse(CoAPMessageType.RST,
                                                    CoAPMessageCode.METHOD_NOT_ALLOWED,
                                                    coapReq /*Copy all necessary values from request in the response*/);
                //When you use the constructor that accepts a request, then automatically
                //the message id , token and remote sender values are copied over to the response
                _coapServer.Send(resp);
            }
        }
        else
        {
            //Message type is GET...check the path..this server only supports well-known path
            if (reqURIPath != ".well-known/core")
            {
                if (coapReq.MessageType.Value == CoAPMessageType.CON)
                {
                    CoAPResponse resp = new CoAPResponse(CoAPMessageType.ACK,
                                                        CoAPMessageCode.NOT_FOUND,
                                                        coapReq /*Copy all necessary values from request in the response*/);
                    _coapServer.Send(resp);
                }
                else
                {
                    //For NON, we can only send back a RST
                    CoAPResponse resp = new CoAPResponse(CoAPMessageType.RST,
                                                        CoAPMessageCode.NOT_FOUND,
                                                        coapReq /*Copy all necessary values from request in the response*/);
                    _coapServer.Send(resp);
                }
            }
            else
            {
                //Request is GET and path is right
                if (coapReq.MessageType.Value == CoAPMessageType.CON)
                {
                    CoAPResponse resp = new CoAPResponse(CoAPMessageType.ACK,
                                                        CoAPMessageCode.CONTENT,
                                                        coapReq /*Copy all necessary values from request in the response*/);
                    //Add response payload
                    resp.AddPayload(GetSupportedResourceDescriptions());
                    //Tell recipient about the content-type of the response
                    // The content-type contains the list of resources in “CoRE Link Format” is “application/link-format”
                    resp.AddOption(CoAPHeaderOption.CONTENT_FORMAT, AbstractByteUtils.GetBytes(CoAPContentFormatOption.APPLICATION_LINK_FORMAT));
                    _coapServer.Send(resp);
                }
                else
                {
                    //Its a NON, send a NON back...in CoAPSharp, NON is always considered as request
                    CoAPResponse resp = new CoAPResponse(CoAPMessageType.NON,
                                                        CoAPMessageCode.CONTENT,
                                                        coapReq);
                    //Copy over other needed values from the reqeust
                    //resp.Token = coapReq.Token;

                    resp.AddPayload(this.GetSupportedResourceDescriptions());
                    //Tell recipient about the content-type of the response
                    resp.AddOption(CoAPHeaderOption.CONTENT_FORMAT, AbstractByteUtils.GetBytes(CoAPContentFormatOption.APPLICATION_LINK_FORMAT));

                    message += resp.ToString();
                    //send it
                    _coapServer.Send(resp);
                }
            }
        }


    }
	private string GetSupportedResourceDescriptions()
	{
		string resDesc = "<sensors/temp>;ct=" + CoAPContentFormatOption.APPLICATION_JSON +
							";title=Temperature Sensor"; //temperature sensor
		resDesc += ","; //A comma is used to separate each entry
		resDesc += "<sensors/pressure>;ct=" + CoAPContentFormatOption.APPLICATION_JSON +
							";title=Pressure Sensor"; //pressure sensor
		return resDesc;
	}

}
