using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HostFetcher : MonoBehaviour {

    public void Fetch()
    {

        MasterServer.RequestHostList(GameBuilder.kGameTypeName);

    }

    public void Fetch(string master_address, int master_port, int nat_port)
    {

        //Setup the ip and the NAT facilitator

        MasterServer.ipAddress = master_address;
        MasterServer.port = master_port;
        Network.natFacilitatorIP = master_address;
        Network.natFacilitatorPort = nat_port;

        MasterServer.RequestHostList(GameBuilder.kGameTypeName);

    }

    void OnMasterServerEvent(MasterServerEvent server_event)
    {

        if (server_event == MasterServerEvent.HostListReceived)
        {

            var builder = GetComponent<GameBuilder>();

            var all_hosts = MasterServer.PollHostList();

            //Only non-full servers
            hosts_found_ = new Stack<HostData>(all_hosts.Where((HostData h) =>
            {

                return h.connectedPlayers < h.playerLimit;

            }));

        }

    }

    /// <summary>
    /// List of all hosts found so far
    /// </summary>
    private Stack<HostData> hosts_found_;
    

}
