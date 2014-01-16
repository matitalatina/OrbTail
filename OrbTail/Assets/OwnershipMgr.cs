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
    
	public void StoreViewID(NetworkViewID view_id){

		Stack<NetworkViewID> stack;

		var player = view_id.owner;

		if (!view_id_table.ContainsKey(player))
		{
			
			stack = new Stack<NetworkViewID>();
			
			view_id_table.Add(player, stack);
			
		}
		
		stack = view_id_table[player];
		
		stack.Push(view_id);

	}

    /// <summary>
    /// Return a free view ID owned by the given player
    /// </summary>
    public NetworkViewID FetchViewID(NetworkPlayer player)
    {

		var stack = view_id_table[player];

		Debug.Log (player);

		if( !view_id_table.ContainsKey(player)){

			Debug.LogError("OWNED! No player");

		}

		
		if( view_id_table[player].Count == 0){
			
			Debug.LogError("OWNED! No views");
			
		}

		return stack.Pop();

    }

    [RPC]
    private void RPCSendViewID()
    {

        Debug.Log("Sending crap");

        if (Network.isServer)
        {

            RPCReceiveViewID(Network.AllocateViewID());

        }
        else
        {

            networkView.RPC("RPCReceiveViewID", RPCMode.Server, Network.AllocateViewID());

        }

    }

    [RPC]
    private void RPCReceiveViewID(NetworkViewID view_id)
    {

		StoreViewID(view_id);

    }

    IDictionary<NetworkPlayer, Stack<NetworkViewID>> view_id_table = new Dictionary<NetworkPlayer, Stack<NetworkViewID>>();

}
