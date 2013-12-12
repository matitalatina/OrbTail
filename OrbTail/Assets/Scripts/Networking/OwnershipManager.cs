using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OwnershipManager : MonoBehaviour {

    /// <summary>
    /// Acquire the ownership of an object given its networkview
    /// </summary>
    public void AcquireOwnership(GameObject game_object)
    {

        NetworkPlayer owner = game_object.networkView.viewID.owner;

        if (Network.player != owner)
        {

            Debug.Log("Acquiring ownership");

            var view_id = AllocateNetworkViewID();
            var target_view_id = game_object.networkView.viewID;

            //Tells the previous owner that the ownership has been changed
            networkView.RPC("RPCChangeOwnership", owner, target_view_id, view_id);

        }

    }
    
    [RPC]
    private void RPCChangeOwnership(NetworkViewID target_view_id, NetworkViewID view_id)
    {

        //Changes the actual owner
        NetworkView.Find(target_view_id).viewID = view_id;

        if (DisposeNetworkViewID(target_view_id))
        {

            //The object was mine...
            Debug.Log("Yielding my ownership");
            
            networkView.RPC("RPCChangeOwnership", RPCMode.Others, target_view_id, view_id);

        }
        else
        {

            Debug.Log("Changing the ownership");

        }

    }

    /// <summary>
    /// Allocates a new networkview id or recycles a new one
    /// </summary>
    private NetworkViewID AllocateNetworkViewID()
    {

        NetworkViewID view_id;

        if (view_stack_.Count > 0)
        {

            //Recycle an unused viewID
            view_id = view_stack_.Pop();

        }
        else
        {

            //Allocates a new viewID
            view_id = Network.AllocateViewID();

        }

        return view_id;

    }

    /// <summary>
    /// Dispose an unused networkview id
    /// </summary>
    private bool DisposeNetworkViewID(NetworkViewID view_id)
    {

        if (view_id.isMine)
        {

            //Saves the old view ID to recycle it
            view_stack_.Push(view_id);
            return true;

        }
        else
        {

            return false;

        }

    }

    /// <summary>
    /// Used to recycle unused network view ID
    /// </summary>
    private Stack<NetworkViewID> view_stack_ = new Stack<NetworkViewID>();
    
}
