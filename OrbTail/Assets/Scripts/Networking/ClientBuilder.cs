using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ClientBuilder : NetworkPlayerBuilder {

    #region Events

    public delegate void DelegateDisconnected(object sender, string message);

    public event DelegateDisconnected EventDisconnected;

    private void NotifyDisconnected(string message)
    {

        if (EventDisconnected != null)
        {

            EventDisconnected(this, message);

        }

    }

    public delegate void DelegateNoGame(object sender);

    public event DelegateNoGame EventNoGame;

    private void NotifyNoGame()
    {

        if (EventNoGame != null)
        {

            EventNoGame(this);

        }

    }

    #endregion

	// Use this for initialization
	void Start () {

        if (!LocalMasterServer)
        {
            
            MasterServer.RequestHostList(GameBuilder.kGameTypeName);
            
        }
        else
        {

            //Ask the local master server

        }

	}

	// Update is called once per frame
	void Update () {
	
	}

    void OnConnectedToServer()
    {

        PlayerIdentity identity = GetComponents<PlayerIdentity>().SkipWhile((PlayerIdentity p) => { return !p.IsHuman; }).First();
       
        EventIdAcquired += ClientBuilder_EventIdAcquired;

        //Register the server identity
        Debug.Log("Registering to server...");

        networkView.RPC("RPCRegisterPlayer", RPCMode.Server, Network.player, identity.Name);
        
    }

    void OnFailedToConnect(NetworkConnectionError error)
    {

        Debug.Log("Failed to connect");
        
        //Try with the another host
        Connect();

    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {

        switch (info)
        {
            case NetworkDisconnection.Disconnected:

                NotifyDisconnected("The server shutdown the connection");
                break;

            case NetworkDisconnection.LostConnection:
                
                NotifyDisconnected("Connection lost");
                break;
        
        }

    }

    void OnMasterServerEvent(MasterServerEvent server_event)
    {

        if (server_event == MasterServerEvent.HostListReceived)
        {

            Debug.Log("Fetching hosts...");

            string arena_name = GetComponent<GameBuilder>().ArenaName;

            var all_hosts = MasterServer.PollHostList();

            //Filters out the servers found
            hosts_found_ = new Stack<HostData>(all_hosts.Where((HostData h) =>
            {

                return h.connectedPlayers < h.playerLimit &&
                       (h.gameName == arena_name ||
                       arena_name == ""); 
            } ));

            Debug.Log("Found " + hosts_found_.Count() + " games out of " + all_hosts.Count());

            Connect();

        }

    }
    
    // A new level has been loaded
    void OnLevelWasLoaded(int level)
    {

        //Tells the server that the arena was loaded successfully
        networkView.RPC("RPCArenaLoaded", RPCMode.Server, Id);

    }


    private void ClientBuilder_EventIdAcquired(object sender, int id)
    {

        Debug.Log("Acquiring id " + id);

        //TODO: remove this
        SetReady(true);

    }

    /// <summary>
    /// Attempts to connect to the next server found
    /// </summary>
    private void Connect()
    {

        if (hosts_found_.Count > 0)
        {

            var host = hosts_found_.Pop();

            Debug.Log("Connecting...");

            Network.Connect(host);

        }
        else
        {

            Debug.Log("No game found...");

            NotifyNoGame();

        }

    }

    /// <summary>
    /// List of all hosts found so far
    /// </summary>
    private Stack<HostData> hosts_found_;
	
}
