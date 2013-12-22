using UnityEngine;
using System.Collections;

public class NetworkPlayerBuilder : MonoBehaviour
{

    /// <summary>
    /// Is the player using a local master server?
    /// </summary>
    public bool LocalMasterServer = false;

    /// <summary>
    /// The id of this device
    /// </summary>
    public int Id { get; private set; }

    #region Event

    public delegate void DelegatePlayerRegistered(object sender, int id, string name);

    public event DelegatePlayerRegistered EventPlayerRegistered;

    private void NotifyPlayerRegistered(int id, string name)
    {

        if (EventPlayerRegistered != null)
        {

            EventPlayerRegistered(this, id, name);

        }

    }

    public delegate void DelegatePlayerUnregistered(object sender, int id);

    public event DelegatePlayerUnregistered EventPlayerUnregistered;

    private void NotifyPlayerUnregistered(int id)
    {

        if (EventPlayerUnregistered != null)
        {

            EventPlayerUnregistered(this, id);

        }

    }

    public delegate void DelegatePlayerReady(object sender, int id, bool value);

    public event DelegatePlayerReady EventPlayerReady;

    private void NotifyPlayerReady(int id, bool value)
    {

        if (EventPlayerReady != null)
        {

            EventPlayerReady(this, id, value);

        }

    }

    public delegate void DelegateIdAcquired(object sender, int id);

    public event DelegateIdAcquired EventIdAcquired;

    private void NotifyIdAcquired(int id)
    {

        if (EventIdAcquired != null)
        {

            EventIdAcquired(this, id);

        }

    }

    #endregion

    /// <summary>
    /// Set the ready value for this client
    /// </summary>
    public void SetReady(bool value)
    {

        networkView.RPC("RPCPlayerReady", RPCMode.All, Id, value);

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    /// <summary>
    /// Called when this device has acquired an id
    /// </summary>
    [RPC]
    protected void RPCIdAcquired(int id)
    {

        Id = id;

        NotifyIdAcquired(id);

    }

    /// <summary>
    /// Called when a new player has been registered
    /// </summary>
    [RPC]
    protected void RPCPlayerRegistered(int id, string name)
    {

        NotifyPlayerRegistered(id, name);

    }

    /// <summary>
    /// Called when a new player has been registered
    /// </summary>
    [RPC]
    protected void RPCPlayerUnregistered(int id)
    {

        NotifyPlayerUnregistered(id);

    }
    
    /// <summary>
    /// Called when a new player declared himself *ready*
    /// </summary>
    [RPC]
    protected void RPCPlayerReady(int id, bool value)
    {

        NotifyPlayerReady(id, value);

    }


}
