using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OwnershipManager : MonoBehaviour {

    /// <summary>
    /// Acquire the ownership of an object given its networkview
    /// </summary>
    public void RequestOwnership(GameObject game_object)
    {
        
        NetworkPlayer owner = ComponentHelper.GetNetworkView<Rigidbody>(game_object).viewID.owner;
        
        if (Network.player != owner)
        {

            NetworkViewID target_invariant = ComponentHelper.GetRPCNetworkView(game_object).viewID;

            var view_id = AllocateNetworkView();

            //Ask the owner to yield object's ownership
            networkView.RPC("RPCRequestOwnership", owner, target_invariant, view_id);
       
        }
        
    }

    [RPC]
    private void RPCRequestOwnership(NetworkViewID target_invariant, NetworkViewID view_id)
    {

        NetworkView target_rpc = NetworkView.Find(target_invariant);

        GameObject game_object = target_rpc.gameObject;

        var rigid_body_view = ComponentHelper.GetNetworkView<Rigidbody>(gameObject);

        DeallocateNetworkView(rigid_body_view.viewID);

        rigid_body_view.viewID = view_id;

        target_rpc.RPC("RPCChangeOwnership", RPCMode.OthersBuffered, target_invariant, view_id);

    }

    [RPC]
    private void RPCChangeOwnership(NetworkViewID target_invariant, NetworkViewID view_id)
    {

        GameObject game_object = NetworkView.Find(target_invariant).gameObject;

        var rigid_body_view = ComponentHelper.GetNetworkView<Rigidbody>(gameObject);

        rigid_body_view.viewID = view_id;

    }

    /// <summary>
    /// Allocate a new networkview (or recycle an existing one)
    /// </summary>
    private NetworkViewID AllocateNetworkView()
    {

        if (view_stack_.Count > 0)
        {

            //Recycle an unused viewID
            return view_stack_.Pop();

        }
        else
        {

            //Allocates a new viewID
            return Network.AllocateViewID();

        }

    }

    /// <summary>
    /// Deallocates a network view id
    /// </summary>
    private void DeallocateNetworkView(NetworkViewID view_id)
    {

        //Saves the old view ID to recycle it
        view_stack_.Push(view_id);

    }

    /// <summary>
    /// Used to recycle unused network view ID
    /// </summary>
    private Stack<NetworkViewID> view_stack_ = new Stack<NetworkViewID>();
    
}
