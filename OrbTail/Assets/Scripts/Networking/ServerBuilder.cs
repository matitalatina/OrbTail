using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerBuilder : PlayerBuilder {

    /// <summary>
    /// Maximum players per match
    /// </summary>
    public int max_players = 4;

    /// <summary>
    /// The port used by the server
    /// </summary>
    public int server_port = 60590;

    /// <summary>
    /// Used by the event EventMatchCreated
    /// </summary>
    public delegate void DelegateMatchCreated(object sender);

    /// <summary>
    /// Fired when the server has been registered
    /// </summary>
    public event DelegateMatchCreated EventMatchCreated;

    /// <summary>
    /// Awakes the server
    /// </summary>
    void Awake()
    {

        System.Random random = new System.Random();

        Debug.Log("Creating a server...");

        //Initializes the server and registers the match
        
        Network.InitializeServer(max_players - 1, server_port, !Network.HavePublicAddress());

        MasterServer.RegisterHost("OrbTail",
                                  "Game #" + random.Next().ToString());

    }

	// Use this for initialization
	public override void Start () {

        base.Start();

        EventClientReady += ServerBuilder_EventClientReady;

	}
	
	// Update is called once per frame
    public override void Update()
    {

        base.Update();
        /*
        foreach (NetworkPlayer player in Network.connections)
        {

            Debug.Log(player.ipAddress + ": " + Network.GetLastPing(player));

        }
        */

    }

    // A new level has been loaded
    void OnLevelWasLoaded(int level)
    {

        //Tells the client that the match's started
        networkView.RPC("ServerReady", RPCMode.OthersBuffered);     

    }
    
    void OnMasterServerEvent(MasterServerEvent server_event)
    {

        if (server_event == MasterServerEvent.RegistrationSucceeded)
        {

            Debug.Log("Registration successful");

            if (EventMatchCreated != null)
            {

                EventMatchCreated(this);

            }

        }
        else
        {

            Debug.Log("Fatal error occurred while creating the server");

        }

    }

    void OnPlayerConnected(NetworkPlayer player)
    {

        Debug.Log("A player has been connected!");

        pending_players.Add(player);
        
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {

        Debug.Log("A player has been disconnected!");

        Network.RemoveRPCs(player);

        //TODO: returns the orbs to the server!!!
        Network.DestroyPlayerObjects(player);

        pending_players.Remove(player);

    }

    void ServerBuilder_EventClientReady(object sender, NetworkPlayer client_identity)
    {

        pending_players.Remove(client_identity);

        if (pending_players.Count == 0)
        {

            InitializeAll();

        }

    }

    private void InitializeAll()
    {
        
        //Creates the game, used to deliver informations and orders to the clients
        var game = CreateGame();
       
        //The server has always the ship's ID equals to 0
        int unique_id = 0;

        //Initializes the server's ship
        InitializePlayer(unique_id);

        //Initializes the clients' ship
        foreach (NetworkPlayer connection in Network.connections)
        {

            networkView.RPC("InitializePlayer", connection, ++unique_id);
            
        }

        game.GetComponent<EventLogger>().NotifyStartMatch();
              
    }

    /// <summary>
    /// The master sends command to other clients
    /// </summary>
    private GameObject CreateGame()
    {
        var game = Resources.Load("Prefabs/Game");

        return Network.Instantiate(game, Vector3.zero, Quaternion.identity, 0) as GameObject;

    }

    private HashSet<NetworkPlayer> pending_players = new HashSet<NetworkPlayer>();
    

}
