using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {

    void Awake()
    {

        MasterServer.RequestHostList("OrbTail");

    }

	// Use this for initialization
	void Start () {
	
        

	}
	
	// Update is called once per frame
    void Update()
    {

    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
        {
           
            HostData[] data = MasterServer.PollHostList();

            if (data.Length > 0)
            {

                Network.Connect(data[0]);

                Debug.Log("Connecting to a server...");

            }

        }

    }


    void OnConnectedToServer()
    {

        Debug.Log("Connected to server");

    }

        
}
