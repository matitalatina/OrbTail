using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OwnershipMgr : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    /// <summary>
    /// Return a free view ID owned by the given player
    /// </summary>
    public NetworkViewID FetchViewID(NetworkPlayer player)
    {

        return view_id_table[player].Pop();

    }

    [RPC]
    private void RPCSendViewID()
    {

        Debug.Log("Sending crap");

        if (Network.isServer)
        {

            RPCReceiveViewID(Network.player, Network.AllocateViewID());

        }
        else
        {

            networkView.RPC("RPCReceiveViewID", RPCMode.Server,Network.player, Network.AllocateViewID());

        }

    }

    [RPC]
    private void RPCReceiveViewID(NetworkPlayer player, NetworkViewID view_id)
    {

        Debug.Log("Receiving crap");

        if (!view_id_table.ContainsKey(player))
        {

            view_id_table.Add(player, new Stack<NetworkViewID>());

        }

        view_id_table[player].Push(view_id);

    }

    IDictionary<NetworkPlayer, Stack<NetworkViewID>> view_id_table = new Dictionary<NetworkPlayer, Stack<NetworkViewID>>();

}
