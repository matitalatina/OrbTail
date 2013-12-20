using UnityEngine;
using System.Collections;

public class PlayerBuilder : MonoBehaviour {

    /// <summary>
    /// The player identity bound to this script
    /// </summary>
    public PlayerIdentity PlayerIdentity { get; protected set; }

    /// <summary>
    /// Delegate used by EventServerReady
    /// </summary>
    /// <param name="sender"></param>
    public delegate void DelegateServerReady(object sender);

    /// <summary>
    /// Delegate used by EventClientReady
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="client_identity"></param>
    public delegate void DelegateClientReady(object sender, NetworkPlayer client_identity);

    /// <summary>
    /// Fired when the server is ready
    /// </summary>
    public event DelegateServerReady EventServerReady;

    /// <summary>
    /// Fired when a client is ready
    /// </summary>
    public event DelegateClientReady EventClientReady;

	// Use this for initialization
    public virtual void Start()
    {

        PlayerIdentity = GetComponent<PlayerIdentity>();

	}
	
	// Update is called once per frame
    public virtual void Update()
    {


	}

    /// <summary>
    /// Informs that the server is ready
    /// </summary>
    [RPC]
    public void ServerReady()
    {

        if (EventServerReady != null)
        {

            EventServerReady(this);

        }

    }

    /// <summary>
    /// Informs that the client is ready
    /// </summary>
    /// <param name="player"></param>
    [RPC]
    public void ClientReady(NetworkPlayer player){

        if (EventClientReady != null)
        {

            EventClientReady(this, player);

        }

    }

    /// <summary>
    /// Initializes the player's ship
    /// </summary>
    /// <param name="id">The unique id of the player</param>
    /// <param name="position">The ship's position</param>
    [RPC]
    public void InitializePlayer(int id)
    {
        //TODO: fixme!
        /**
        PlayerIdentity.id = id;

        //Network-instantiate the ship of this player
        var ship_object = Resources.Load("Prefabs/Ships/" + PlayerIdentity.ship_name);

        Network.Instantiate(ship_object, 
                            GetSpawnPosition(), 
                            Quaternion.identity, 0);
        */
    }

    private Vector3 GetSpawnPosition()
    {
        //TODO: fixme!
        GameObject[] spawn_point = GameObject.FindGameObjectsWithTag(Tags.SpawnPoint);
        /*
        return spawn_point[PlayerIdentity.id].transform.position;
        */
        return Vector3.zero;

    }

}
