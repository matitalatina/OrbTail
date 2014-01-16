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

		var stack = view_id_table[player];

		lock( stack ){

			return stack.Pop();

		}

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

		Stack<NetworkViewID> stack;

        if (!view_id_table.ContainsKey(player))
        {

			stack = new Stack<NetworkViewID>();

            view_id_table.Add(player, stack);

        }

		stack = view_id_table[player];

		lock( stack ){

        	stack.Push(view_id);

		}

    }

    IDictionary<NetworkPlayer, Stack<NetworkViewID>> view_id_table = new Dictionary<NetworkPlayer, Stack<NetworkViewID>>();

}
