using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ClientBuilder : NetworkPlayerBuilder {


    public override void Setup()
    {

        if (LocalMasterServer)
        {

            //Setup the ip and the NAT facilitator

            MasterServer.ipAddress = LocalMasterServerAddress;
            MasterServer.port = LocalMasterServerPort;
            Network.natFacilitatorIP = LocalMasterServerAddress;
            Network.natFacilitatorPort = NATFacilitatorPort;

        }

        MasterServer.RequestHostList(GameBuilder.kGameTypeName);   

    }

    void OnConnectedToServer()
    {

        PlayerIdentity identity = GetComponents<PlayerIdentity>().SkipWhile((PlayerIdentity p) => { return !p.IsHuman; }).First();
       
        EventIdAcquired += ClientBuilder_EventIdAcquired;

        //Register the server identity
        Debug.Log("Registering to server...");

        networkView.RPC("RPCRegisterPlayer", RPCMode.Server, Network.player, identity.ShipName);
        
    }

    void OnFailedToConnect(NetworkConnectionError error)
    {

        Debug.Log("Failed to connect");
        
        //Try with the another host
        Connect();

    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {

        NotifyDisconnected("Disconnected from server");

    }

    void OnMasterServerEvent(MasterServerEvent server_event)
    {

        if (server_event == MasterServerEvent.HostListReceived)
        {

            Debug.Log("Fetching hosts...");

            var builder = GetComponent<GameBuilder>();

            var all_hosts = MasterServer.PollHostList();

            //Filters out the servers found
            hosts_found_ = new Stack<HostData>(all_hosts.Where((HostData h) =>
            {

                var bits = h.gameName.Split(';');

                string host_arena = bits[0];
                int host_game = int.Parse(bits[1]);

                return h.connectedPlayers < h.playerLimit &&
                       (builder.ArenaName == "" || builder.ArenaName == host_arena) &&
                       (builder.GameMode == -1 || builder.GameMode == host_game);

            } ));

            Debug.Log("Found " + hosts_found_.Count() + " games out of " + all_hosts.Count());

            Connect();

        }

    }
    
    // A new level has been loaded
    void OnLevelWasLoaded(int level)
    {

        if (IsArenaLoading)
        {

            //Tells the server that the arena was loaded successfully
            networkView.RPC("RPCArenaLoaded", RPCMode.Server, Id);

        }
        
    }


    private void ClientBuilder_EventIdAcquired(object sender, int id)
    {

        Debug.Log("Acquiring id " + id);
        
    }

    /// <summary>
    /// Attempts to connect to the next server found
    /// </summary>
    private void Connect()
    {

        if (hosts_found_.Count > 0)
        {

            var builder = GetComponent<GameBuilder>();

            var host = hosts_found_.Pop();

            var bits = host.gameName.Split(';');

            builder.ArenaName = bits[0];
            builder.GameMode = int.Parse(bits[1]);
            
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
