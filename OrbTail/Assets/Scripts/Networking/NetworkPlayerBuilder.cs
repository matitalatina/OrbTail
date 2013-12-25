using UnityEngine;
using System.Collections;
using System.Linq;

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

    /// <summary>
    /// Request the registration of a player
    /// </summary>
    [RPC]
    protected void RPCRegisterPlayer(NetworkPlayer player, string name)
    {

        RegisterPlayer(player, name);

    }

    /// <summary>
    /// Loads an arena
    /// </summary>
    /// <param name="arena">arena name</param>
    [RPC]
    protected void RPCLoadArena(string arena)
    {

        DontDestroyOnLoad(gameObject);

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

        player.GetComponent<GameIdentity>().Id = Id;

        Destroy(identity);

        //Tells the server that the player is ready to go
        if (!Network.isServer) {
            
            networkView.RPC("RPCPlayerCreated", RPCMode.Server, Id);

        }
        else
        {

            RPCPlayerCreated(Id);

        }
        

        //Find the camera
        GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<CameraMovement>().LookAt(player);

    }

    [RPC]
    protected void RPCPlayerCreated(int id)
    {

        PlayerCreated(id);

    }

    protected virtual void RegisterPlayer(NetworkPlayer player, string name){

        //Let the host builder implement this

    }

    protected virtual void ArenaLoaded(int id)
    {

        //Let the host builder implement this

    }

    protected virtual void PlayerCreated(int id)
    {

        //Let the host builder implement this

    }

}
