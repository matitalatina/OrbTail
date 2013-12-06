using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OwnershipManager : MonoBehaviour {

    /// <summary>
    /// Acquire the ownership of an object given its networkview
    /// </summary>
    public void AcquireOwnership(GameObject game_object)
    {

        if (!game_object.networkView.isMine)
        {

            NetworkViewID view_id;
            
            if( view_stack_.Count > 0 ){

                //Recycle an unused viewID
                view_id = view_stack_.Pop();

            }else{

                //Allocates a new viewID
                view_id = Network.AllocateViewID();

            }

            //Allocates a new viewID and change the ownership via RPC call
            networkView.RPC("ChangeOwnership", RPCMode.AllBuffered, game_object.networkView.viewID, view_id);

        }

    }

    [RPC]
    private void ChangeOwnership(NetworkViewID target_object, NetworkViewID view_id)
    {

        if (target_object.isMine)
        {

            //Saves the old view ID to recycle it
            view_stack_.Push(target_object);

        }

        //Changes the actual owner
        NetworkView.Find(target_object).viewID = view_id;

    }

    /// <summary>
    /// Used to recycle unused network view ID
    /// </summary>
    private Stack<NetworkViewID> view_stack_ = new Stack<NetworkViewID>();


}
