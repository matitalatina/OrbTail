using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HostFetcher : MonoBehaviour
{

    #region Events

    public delegate void DelegateGameFound(object sender, IEnumerable<int> game_modes);

    public event DelegateGameFound EventGameFound;

    #endregion 

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

    public bool HasArena(string arena_name)
    {

        int game_mode = GetComponent<GameBuilder>().GameMode;

        return hosts_found_.Any((string[] h) =>
        {
            
            return h[0] == arena_name &&
                   (h[1] == game_mode.ToString() ||
                    game_mode == -1);

        });

    }

    void OnMasterServerEvent(MasterServerEvent server_event)
    {

        if (server_event == MasterServerEvent.HostListReceived)
        {

            var builder = GetComponent<GameBuilder>();

            var all_hosts = MasterServer.PollHostList();

            hosts_found_ = from host in all_hosts
                           where host.connectedPlayers < host.playerLimit
                           select host.gameName.Split(';');

            if (EventGameFound != null)
            {
                
                HashSet<int> modes = new HashSet<int>();

                foreach (var host in hosts_found_)
                {

                    modes.Add(int.Parse(host[1]));

                }

                EventGameFound(this, modes);

            }

        }
        else {

            Debug.LogError("No server!");

        }

    }

    /// <summary>
    /// List of all hosts found so far arena-game mode
    /// </summary>
    private IEnumerable<string[]> hosts_found_;
    

}
