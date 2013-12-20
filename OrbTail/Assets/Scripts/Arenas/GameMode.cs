using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMode : MonoBehaviour {

	// Use this for initialization
	public virtual void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}

    /// <summary>
    /// Returns the identity of each player
    /// </summary>
    public IEnumerable<PlayerIdentity> Players { get; private set; }

}
