using UnityEngine;
using System.Collections;

public class OwnershipManager : MonoBehaviour {

    /// <summary>
    /// Acquire the ownership of an object given its networkview
    /// </summary>
    public void AcquireOwnership(GameObject game_object)
    {

        if (!game_object.networkView.isMine)
        {

            //Allocates a new viewID and change the ownership via RPC call
            networkView.RPC("ChangeOwnership", RPCMode.AllBuffered, game_object.networkView.viewID, Network.AllocateViewID());

        }

    }

    [RPC]
    private void ChangeOwnership(NetworkViewID target_object, NetworkViewID view_id)
    {

        Debug.Log("Changing ownership from " + target_object + " to " + view_id);

        //Changes the actual ownership
        NetworkView.Find(target_object).viewID = view_id;

    }

}
