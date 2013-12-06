using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tail : MonoBehaviour {
	private Stack<GameObject> orbStack = new Stack<GameObject>();
	private GameObject firstOrb;
	private EventLogger eventLogger;
	
	

	// Use this for initialization
	void Start () {
		eventLogger = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<EventLogger>();
	}

	// TODO: to implement
	public void AttachToPlayer(GameObject player) {

	}


	/// <summary>
	/// Attachs the orb to the tail.
	/// </summary>
	/// <param name="orb">The orb to attach</param>
	public void AttachOrb(GameObject orb) {
		GameObject target;

		if (orbStack.Count <= 0) {
			target = gameObject;
			firstOrb = orb;
		} 
		else {
			target = orbStack.Peek();
		}

		orbStack.Push(orb);
		//orb.GetComponent<OrbController>().ApproachTo(target, this);
		orb.GetComponent<OrbController>().LinkTo(target);

		eventLogger.NotifyOrbAttached(orb, gameObject);

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
			detachedOrbs.Add(orbToDetach);
			i++;
		}

		if (orbStack.Count <= 0) {
			firstOrb = null;
		}

		return detachedOrbs;
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
