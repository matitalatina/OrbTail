using UnityEngine;
using System.Collections;

public class ClientBuilder_d : PlayerBuilder {

    void Awake()
    {

        MasterServer.RequestHostList("OrbTail");

    }

	// Use this for initialization
    public override void Start()
    {

        base.Start();

	}
	
	// Update is called once per frame
	public override void Update () {

        base.Update();

	}

    // A new level has been loaded
    void OnLevelWasLoaded(int level)
    {

        //Informs the server that this client is ready!
        networkView.RPC("ClientReady", RPCMode.Server, Network.player);

    }

    void OnMasterServerEvent(MasterServerEvent server_event)
    {

        //TODO: fix this crap, seriously

        if (server_event == MasterServerEvent.HostListReceived)
        {

            HostData[] data = MasterServer.PollHostList();

            if (data.Length > 0)
            {

                Network.Connect(data[0]);

                Debug.Log("Connecting to the first server found");

            }

        }
        else
        {

            Debug.Log("Fatal error occured while connecting to the server");

        }       

    }

    void OnConnectedToServer()
    {

        Debug.Log("Connected to the server");

    }
    
}
