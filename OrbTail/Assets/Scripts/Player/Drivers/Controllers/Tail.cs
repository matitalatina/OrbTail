using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tail : MonoBehaviour {

    private Stack<GameObject> orbStack = new Stack<GameObject>();
	private GameObject firstOrb;
    private OwnershipManager ownershipManager;
	private AudioClip gatherOrbSound;

    private float detachForce = 0.06f;
    private float attachForce = 0.03f;

	public delegate void DelegateOnOrbAttached(object sender, GameObject orb, GameObject ship);

    public delegate void DelegateOnOrbDetached(object sender, GameObject ship);

	/// <summary>
	/// Notifies than an orb has been attached
	/// </summary>
	/// <param name="orb">The orb that has been attached</param>
	/// <param name="ship">The ship that has been attached</param>
	public event DelegateOnOrbAttached OnEventOrbAttached;

    /// <summary>
    /// Notifies that an orb has been detached
    /// </summary>
    public event DelegateOnOrbDetached OnEventOrbDetached;

	// Use this for initialization
	void Start () {
        
        var game = GameObject.FindGameObjectWithTag(Tags.Game);
		
        ownershipManager = game.GetComponent<OwnershipManager>();

		gatherOrbSound = Resources.Load<AudioClip>("Sounds/Ship/AttachOrb");

	}

	// TODO: to implement
	public void AttachToPlayer(GameObject player) {



	}


	/// <summary>
	/// Attachs the orb to the tail.
	/// </summary>
	/// <param name="orb">The orb to attach</param>
	public void AttachOrb(GameObject orb) {

        //First removes the randompowerattacher
        var power_attacher = orb.GetComponent<RandomPowerAttacher>();

        if (power_attacher.enabled)
        {

            power_attacher.RemoveFX();

        }

        if (networkView.isMine ||
            Network.peerType == NetworkPeerType.Disconnected)
        {
            
			if (OnEventOrbAttached != null) {
				OnEventOrbAttached(this, orb, gameObject);
			}

            orb.rigidbody.AddForce(-orb.GetComponent<FloatingObject>().ArenaDown * attachForce, ForceMode.Impulse);
			audio.PlayOneShot(gatherOrbSound, 1f);
			//AudioSource.PlayClipAtPoint(gatherOrbSound, orb.transform.position);
        
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

            networkView.RPC("RPCAttachOrb", RPCMode.Others, orb.networkView.viewID);

        }

        //Acquire the ownership
        if (networkView.isMine)
        {

            ownershipManager.AcquireOwnership(orb);

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


        if (OnEventOrbDetached != null)
        {
            OnEventOrbDetached(this, gameObject);
        }

        //Warns other players
        if (Network.isServer)
        {

            //eventLogger.NotifyOrbAttached(orb, gameObject);
            networkView.RPC("RPCDetachOrbs", RPCMode.Others, nOrbs);

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
