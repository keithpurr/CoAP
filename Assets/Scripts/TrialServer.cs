using UnityEngine;
using System.Collections;
using System;
using Com.AugustCellars.CoAP.Server;

public class TrialServer : MonoBehaviour
{
    private CoapServer server;

    // Use this for initialization
    void Start()
    {
        server = new CoapServer();
    }

    public void StartServer(){

        server.Add(new HelloWorldResource("hello"));
        //server.Add(new FibonacciResource("fibonacci"));
        //server.Add(new StorageResource("storage"));
        //server.Add(new ImageResource("image"));
        //server.Add(new MirrorResource("mirror"));
        //server.Add(new LargeResource("large"));
        //server.Add(new CarelessResource("careless"));
        //server.Add(new SeparateResource("separate"));
        //server.Add(new TimeResource("time"));
        //server.Add(new PlayerColorResource("player_color"));
        //server.Add(new CameraViewResource("camera_view"));
        //server.Add(new PlayerMoveResource("player_move"));
        try
        {
            server.Start();

            Debug.Log("CoAP server [{0}] is listening on" + server.Config.Version);

            foreach (var item in server.EndPoints)
            {
                Debug.Log(" ");
                Debug.Log(item.LocalEndPoint);
            }
            Debug.Log("\n");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void StopServer()
    {

        server.Stop();

    }

}
