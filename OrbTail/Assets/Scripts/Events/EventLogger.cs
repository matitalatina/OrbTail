using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventLogger : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

        NetworkInterface = GetComponent<NetworkView>();

    }

    // Update is called once per frame
    void Update() { }

    /// <summary>
    /// Notifies than an orb has been attached
    /// </summary>
    /// <param name="orb">The orb that has been attached</param>
    /// <param name="ship">The ship that has been attached</param>
    [RPC]
    public void NotifyOrbAttached(GameObject orb, GameObject ship)
    {

        if (EventOrbAttached != null)
        {

            EventOrbAttached(this, orb, ship);

        }

        if (Network.isServer)
        {

            //Sends the routine to the client
            NetworkInterface.RPC("NotifyOrbAttached", RPCMode.Others, orb, ship);

        }

    }

    /// <summary>
    /// Notify that two ships have fought
    /// </summary>
    /// <param name="orbs">The list of the orbs lost by the defender</param>
    /// <param name="attacker">The attacker's ship</param>
    /// <param name="defender">The defender's ship</param>
    [RPC]
    public void NotifyFight(IList<GameObject> orbs, GameObject attacker, GameObject defender)
    {

        if (EventFight != null)
        {

            EventFight(this, orbs, attacker, defender);

        }

        if (Network.isServer)
        {

            //Sends the routine to the client
            NetworkInterface.RPC("NotifyFight", RPCMode.Others, orbs, attacker, defender);

        }

    }

    /// <summary>
    /// Notifies that a power has been attached to a player
    /// </summary>
    /// <param name="power">The power that has been attached</param>
    /// <param name="ship">The ship who gained the power</param>
    [RPC]
    public void NotifyPowerAttached(Power power, GameObject ship)
    {

        if (EventPowerAttached != null)
        {

            EventPowerAttached(this, power, ship);

        }

        if (Network.isServer)
        {

            //Sends the routine to the client
            NetworkInterface.RPC("NotifyPowerAttached", RPCMode.Others, power, ship);

        }

    }

    /// <summary>
    /// Notifies the match's initialization
    /// </summary>
    /// <param name="identities">The identities of the players</param>
    [RPC]
    public void NotifyInitializeMatch(IList<PlayerIdentity> identities)
    {

        if (EventInitializeMatch != null)
        {

            EventInitializeMatch(this, identities);

        }

        if (Network.isServer)
        {

            //Sends the routine to the client
            NetworkInterface.RPC("NotifyInitializeMatch", RPCMode.Others, identities);

        }

    }

    /// <summary>
    /// Notifies the match's start
    /// </summary>
    [RPC]
    public void NotifyStartMatch()
    {

        if (EventStartMatch != null)
        {

            EventStartMatch(this);

        }

        if (Network.isServer)
        {

            //Sends the routine to the client
            NetworkInterface.RPC("NotifyStartMatch", RPCMode.Others);

        }

    }

    /// <summary>
    /// Notifies the match's end
    /// </summary>
    [RPC]
    public void NotifyEndMatch()
    {

        if (EventEndMatch != null)
        {

            EventEndMatch(this);

        }

        if (Network.isServer)
        {

            //Sends the routine to the client
            NetworkInterface.RPC("NotifyEndMatch", RPCMode.Others);

        }

    }

    /// <summary>
    /// Notifies that a player has been eliminated from the game
    /// </summary>
    /// <param name="player_identity">The identity of the player that has been eliminated</param>
    [RPC]
    public void NotifyPlayerEliminated(PlayerIdentity player_identity)
    {

        if (EventPlayerEliminated != null)
        {

            EventPlayerEliminated(this, player_identity);

        }

        if (Network.isServer)
        {

            //Sends the routine to the client
            NetworkInterface.RPC("NotifyInitializeMatch", RPCMode.Others, player_identity);

        }

    }
    
    /// <summary>
    /// Delegate used by EventOrbAttached
    /// </summary>
    public delegate void DelegateOrbAttached(object sender, GameObject orb, GameObject ship);

    /// <summary>
    /// Delegate used by EventFight
    /// </summary>
    public delegate void DelegateFight(object sender, IList<GameObject> orbs, GameObject attacker, GameObject defender);

    /// <summary>
    /// Delegate used by EventPowerAttached
    /// </summary>
    public delegate void DelegatePowerAttached(object sender, Power power, GameObject ship);

    /// <summary>
    /// Delegate used by EventInitializeMatch
    /// </summary>
    public delegate void DelegateInitializeMatch(object sender, IList<PlayerIdentity> identities);

    /// <summary>
    /// Delegate used by EventStartMatch
    /// </summary>
    public delegate void DelegateStartMatch(object sender);

    /// <summary>
    /// Delegate used by EventEndMatch
    /// </summary>
    public delegate void DelegateEndMatch(object sender);

    /// <summary>
    /// Delegate used by EventPlayerEliminated
    /// </summary>
    public delegate void DelegatePlayerEliminated(object sender, PlayerIdentity player_identity);

    /// <summary>
    /// Raised when an orb has been attached to a player
    /// </summary>
    public event DelegateOrbAttached EventOrbAttached;

    /// <summary>
    /// Raised when two ships have fought
    /// </summary>
    public event DelegateFight EventFight;

    /// <summary>
    /// Raised when a ship has gained a power
    /// </summary>
    public event DelegatePowerAttached EventPowerAttached;

    /// <summary>
    /// Raised at match's start
    /// </summary>
    public event DelegateInitializeMatch EventInitializeMatch;

    /// <summary>
    /// Raised just after the initialization
    /// </summary>
    public event DelegateStartMatch EventStartMatch;

    /// <summary>
    /// Raised to declare the match's termination
    /// </summary>
    public event DelegateEndMatch EventEndMatch;

    /// <summary>
    /// Raised when a player has been eliminated
    /// </summary>
    public event DelegatePlayerEliminated EventPlayerEliminated;

    /// <summary>
    /// The network interface used to broadcast the RPC or used to receive them
    /// </summary>
    private NetworkView NetworkInterface { get; set; }

}
