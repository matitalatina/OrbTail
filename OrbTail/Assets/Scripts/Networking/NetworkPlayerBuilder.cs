using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public abstract class NetworkPlayerBuilder : MonoBehaviour
{

    /// <summary>
    /// Is the player using a local master server?
    /// </summary>
    public bool LocalMasterServer = false;

    /// <summary>
    /// The local master server address
    /// </summary>
    public string LocalMasterServerAddress;

    /// <summary>
    /// The NAT facilitator port
    /// </summary>
    public int NATFacilitatorPort;

    /// <summary>
    /// The local master server port
    /// </summary>
    public int LocalMasterServerPort;

    /// <summary>
    /// Is the game loading the proper arena yet?
    /// </summary>
    public bool IsArenaLoading = false;
        
    /// <summary>
    /// The id of this device
    /// </summary>
    public int? Id { get; private set; }

    #region Event

    public delegate void DelegatePlayerRegistered(object sender, int id, string name);

    public event DelegatePlayerRegistered EventPlayerRegistered;

    protected void NotifyPlayerRegistered(int id, string name)
    {

        if (EventPlayerRegistered != null)
        {

            EventPlayerRegistered(this, id, name);

        }

    }

    public delegate void DelegatePlayerUnregistered(object sender, int id);

    public event DelegatePlayerUnregistered EventPlayerUnregistered;

    protected void NotifyPlayerUnregistered(int id)
    {

        if (EventPlayerUnregistered != null)
        {

            EventPlayerUnregistered(this, id);

        }

    }

    public delegate void DelegatePlayerReady(object sender, int id, bool value);

    public event DelegatePlayerReady EventPlayerReady;

    protected void NotifyPlayerReady(int id, bool value)
    {

        if (EventPlayerReady != null)
        {

            EventPlayerReady(this, id, value);

        }

    }

    public delegate void DelegateIdAcquired(object sender, int id);

    public event DelegateIdAcquired EventIdAcquired;

    protected void NotifyIdAcquired(int id)
    {

        if (EventIdAcquired != null)
        {
        
            EventIdAcquired(this, id);

        }

    }

    public delegate void DelegateRegistrationSucceeded(object sender);

    public event DelegateRegistrationSucceeded EventRegistrationSucceeded;

    protected void NotifyRegistrationSucceeded()
    {

        if (EventRegistrationSucceeded != null)
        {

            EventRegistrationSucceeded(this);

        }

    }

    public delegate void DelegateErrorOccurred(object sender, string message);

    public event DelegateErrorOccurred EventErrorOccurred;

    protected void NotifyErrorOccurred(string message)
    {

        Debug.Log(message);

        if (EventErrorOccurred != null)
        {

            EventErrorOccurred(this, message);

        }

    }

    public delegate void DelegateDisconnected(object sender, string message);

    public event DelegateDisconnected EventDisconnected;

    public void NotifyDisconnected(string message)
    {

        if (EventDisconnected != null)
        {

            EventDisconnected(this, message);

        }

    }

    public delegate void DelegateNoGame(object sender);

    public event DelegateNoGame EventNoGame;

    protected void NotifyNoGame()
    {

        Debug.Log("No game!");

        if (EventNoGame != null)
        {

            EventNoGame(this);

        }

    }
    
    #endregion


    public abstract void Setup();

    void Start()
    {

        Id = null;

    }

    /// <summary>
    /// Set the ready value for this client
    /// </summary>
    public void SetReady(bool value)
    {

        networkView.RPC("RPCPlayerReady", RPCMode.AllBuffered, Id, value);

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
    /// Called when a player has been unregistered
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

    /// <summary>
    /// Request the registration of a player
    /// </summary>
    [RPC]
    protected void RPCRegisterPlayer(NetworkPlayer player, string name)
    {

        RegisterPlayer(player, name);

    }

    [RPC]
    protected void RPCTutorialDismissed(NetworkPlayer player)
    {

        TutorialDismissed(player);

    }

    /// <summary>
    /// Loads an arena
    /// </summary>
    /// <param name="arena">arena name</param>
    [RPC]
    protected void RPCLoadArena(string arena)
    {

        DontDestroyOnLoad(gameObject);

        IsArenaLoading = true;

        Application.LoadLevel(arena);

    }

    /// <summary>
    /// Called when the arena has been loaded
    /// </summary>
    [RPC]
    protected void RPCArenaLoaded(int id)
    {

        ArenaLoaded(id);

    }

    [RPC]
    protected void RPCCreatePlayer(Vector3 position)
    {

        //Player identity. Assuming that only one player identity exists
        var identity = GetComponent<PlayerIdentity>();

        var player_ship = Resources.Load("Prefabs/Ships/" + identity.ShipName);

        GameObject player = Network.Instantiate( player_ship, position, Quaternion.identity, 0) as GameObject;

        identity.CopyTo(player.GetComponent<PlayerIdentity>());

        //TODO: fix this
        player.networkView.RPC("RPCSetGameId", RPCMode.All, Id.Value);
        player.GetComponent<GameIdentity>().Id = Id.Value;

        Destroy(identity);

        //Tells the server that the player is ready to go
        if (!Network.isServer) {

            networkView.RPC("RPCPlayerCreated", RPCMode.Server, Id, player.networkView.viewID);

        }
        else
        {

            RPCPlayerCreated(Id.Value, player.networkView.viewID);

        }
        

        //Find the camera
        GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<CameraMovement>().LookAt(player);

    }

    [RPC]
    protected void RPCPlayerCreated(int id, NetworkViewID view_id)
    {

        PlayerCreated(id, view_id);

    }

    protected virtual void RegisterPlayer(NetworkPlayer player, string name){

        //Let the host builder implement this

    }

    protected virtual void ArenaLoaded(int id)
    {

        //Let the host builder implement this

    }

    protected virtual void PlayerCreated(int id, NetworkViewID view_id)
    {

        //Let the host builder implement this

    }

    protected virtual void TutorialDismissed(NetworkPlayer player)
    {

        //Let the host builder implement this

    }

    

}
