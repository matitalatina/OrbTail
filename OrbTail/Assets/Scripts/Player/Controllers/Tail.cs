using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tail : MonoBehaviour {
	
    private Stack<GameObject> orbStack = new Stack<GameObject>();
	private GameObject firstOrb;
	private EventLogger eventLogger;
    private OwnershipManager ownershipManager;

    private float detachForce = 0.06f;
    private float attachForce = 0.03f;

	// Use this for initialization
	void Start () {
        
        var game = GameObject.FindGameObjectWithTag(Tags.Game);
		
        eventLogger = game.GetComponent<EventLogger>();
        ownershipManager = game.GetComponent<OwnershipManager>();

	}

	// TODO: to implement
	public void AttachToPlayer(GameObject player) {



	}


	/// <summary>
	/// Attachs the orb to the tail.
	/// </summary>
	/// <param name="orb">The orb to attach</param>
	public void AttachOrb(GameObject orb) {

        if (networkView.isMine ||
            Network.peerType == NetworkPeerType.Disconnected)
        {
            
            eventLogger.NotifyOrbAttached(orb, gameObject);

            orb.rigidbody.AddForce(-orb.GetComponent<FloatingObject>().ArenaDown * attachForce, ForceMode.Impulse);
        
        }

        //Acquire the ownership
        if (networkView.isMine)
        {
            
            ownershipManager.AcquireOwnership(orb);

        }

        //Attach the orb to this player
        GameObject target;

        if (orbStack.Count <= 0)
        {
            target = gameObject;
            firstOrb = orb;
        }
        else
        {
            target = orbStack.Peek();
        }

        orbStack.Push(orb);

        orb.GetComponent<OrbController>().LinkTo(target);

        //Warns other players if this is the server
        if (Network.isServer)
        {

            networkView.RPC("RPCAttachOrb", RPCMode.OthersBuffered, orb.networkView.viewID);

        }
        
	}

    [RPC]
    private void RPCAttachOrb(NetworkViewID orb_view_id)
    {

        AttachOrb(NetworkView.Find(orb_view_id).gameObject);

    }


	/// <summary>
	/// Detachs the orbs.
	/// </summary>
	/// <returns>The list of the orbs detached. It can be less than the number of the passed parameter.</returns>
	/// <param name="nOrbs">Number of orbs to deatch.</param>
	public List<GameObject> DetachOrbs(int nOrbs) {

		List<GameObject> detachedOrbs = new List<GameObject>();

		int i = 0;

		while (i < nOrbs && orbStack.Count > 0) {
			GameObject orbToDetach = orbStack.Pop();
			orbToDetach.GetComponent<OrbController>().Unlink();

            orbToDetach.rigidbody.AddForce(Random.onUnitSphere * detachForce, ForceMode.Impulse);


			detachedOrbs.Add(orbToDetach);
			i++;
		}

		if (orbStack.Count <= 0) {
			firstOrb = null;
		}

        //Warns other players
        if (Network.isServer)
        {

            //eventLogger.NotifyOrbAttached(orb, gameObject);
            networkView.RPC("RPCDetachOrbs", RPCMode.OthersBuffered, nOrbs);

        }

		return detachedOrbs;

	}

    [RPC]
    private void RPCDetachOrbs(int nOrbs)
    {

        DetachOrbs(nOrbs);

    }


	/// <summary>
	/// Gets the number of the orbs in the tail.
	/// </summary>
	/// <returns>The number of the orbs in the tail.</returns>
	public int GetOrbCount() {
		return orbStack.Count;
	}


	public void Update() {}




}
