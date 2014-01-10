using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tail : MonoBehaviour {

    private Stack<GameObject> orbStack = new Stack<GameObject>();
    private OwnershipManager ownershipManager;

    private GameIdentity game_identity = null;
    private float kOrbColorThreshold = 10;  //Number of orbs to have in order to have a full intensity tail
    private Color kOrbDeactiveColor;
	private Material defaultOrbMaterial;
	private Material myOrbMaterial;

    private float detachForce = 0.06f;
    private float attachForce = 0.03f;

	public delegate void DelegateOnOrbAttached(object sender, GameObject orb, GameObject ship);

    public delegate void DelegateOnOrbDetached(object sender, GameObject ship, int count);

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

    void Awake()
    {

        defaultOrbMaterial = Resources.Load<Material>("Materials/OrbMat");
        kOrbDeactiveColor = defaultOrbMaterial.color;
        myOrbMaterial = new Material(defaultOrbMaterial);
        
    }

	// Use this for initialization
	void Start () {

        var game = GameObject.FindGameObjectWithTag(Tags.Game);
		
        ownershipManager = game.GetComponent<OwnershipManager>();
       
        game_identity = GetComponent<GameIdentity>();

        UpdateTailColor();

        game_identity.EventIdSet += game_identity_EventIdSet;

	}

    void game_identity_EventIdSet(object sender, int id)
    {

        UpdateTailColor();

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


        if (OnEventOrbAttached != null)
        {

            OnEventOrbAttached(this, orb, gameObject);

        }

        if (networkView.isMine ||
            Network.peerType == NetworkPeerType.Disconnected)
        {
         
            orb.rigidbody.AddForce(-orb.GetComponent<FloatingObject>().ArenaDown * attachForce, ForceMode.Impulse);
        
        }

        //Attach the orb to this player
        GameObject target;

        if (orbStack.Count <= 0)
        {
            target = gameObject;
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

        ColorizeOrb(orb);

        //Acquire the ownership
        if (networkView.isMine)
        {

            ownershipManager.AcquireOwnership(orb);

        }

               
	}

    private void ColorizeOrb(GameObject orb)
    {
       
		UpdateTailColor();
		orb.renderer.material = myOrbMaterial;
        
    }

	private void UpdateTailColor() {

        var color = Color.Lerp(kOrbDeactiveColor, game_identity.Color, orbStack.Count / kOrbColorThreshold);
        myOrbMaterial.color = color;

		
	}

    private void DecolorizeOrbs(List<GameObject> orbs)
    {
        
        foreach (GameObject orb in orbs)
        {

			orb.renderer.material = defaultOrbMaterial;

        }

		UpdateTailColor();
        
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

		/*if (orbStack.Count <= 0) {
			firstOrb = null;
		}*/


        if (OnEventOrbDetached != null)
        {
            OnEventOrbDetached(this, gameObject, nOrbs);
        }

        //Warns other players
        if (Network.isServer)
        {

            //eventLogger.NotifyOrbAttached(orb, gameObject);
            networkView.RPC("RPCDetachOrbs", RPCMode.Others, nOrbs);

        }

        DecolorizeOrbs(detachedOrbs);

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
