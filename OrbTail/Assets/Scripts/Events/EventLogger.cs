using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script used to broadcast messages to the network and the game
/// </summary>
public class EventLogger : MonoBehaviour {

    void Awake()
    {

        orbs_accumulator_ = new RPCAccumulator<GameObject>();

        player_identities_accumulator_ = new RPCAccumulator<PlayerIdentity>();

    }

    // Update is called once per frame
    void Update() {

    }

    #region "Notifications"

    /// <summary>
    /// Notifies than an orb has been attached
    /// </summary>
    /// <param name="orb">The orb that has been attached</param>
    /// <param name="ship">The ship that has been attached</param>
    public void NotifyOrbAttached(GameObject orb, GameObject ship)
    {

        if (!Network.isClient)
        {

            if (Network.isServer)
            {
             
                //RPC to other clients if this is the server
                networkView.RPC("ReceiveOrbAttached", RPCMode.Others, orb.networkView.viewID, ship.networkView.viewID);

            }

            if (EventOrbAttached != null)
            {

                EventOrbAttached(this, orb, ship);

            }

        }

    }

    /// <summary>
    /// Notify that two ships have fought
    /// </summary>
    /// <param name="orbs">The list of the orbs lost by the defender</param>
    /// <param name="attacker">The attacker's ship</param>
    /// <param name="defender">The defender's ship</param>
    public void NotifyFight(IList<GameObject> orbs, GameObject attacker, GameObject defender)
    {

        if (!Network.isClient)
        {

            if (Network.isServer)
            {

                //TODO: get a real fight UID
                int fight_uid = UIDGenerator.Instance.GetNewUID();

                //RPC to other clients if this is the server
                foreach (GameObject orb in orbs)
                {

                    networkView.RPC("ReceiveAccumulateOrb", RPCMode.Others, fight_uid, orb.networkView.viewID);

                }

                networkView.RPC("ReceiveFight", RPCMode.Others, fight_uid, attacker.networkView.viewID, defender.networkView.viewID);
                
            }

            if (EventFight != null)
            {

                EventFight(this, orbs, attacker, defender);

            }

        }

    }

    /// <summary>
    /// Notifies that a power has been attached to a player. This event is not transmitted over the network!
    /// </summary>
    /// <param name="power">The power that has been attached</param>
    /// <param name="ship">The ship who gained the power</param>
    public void NotifyPowerAttached(Power power, GameObject ship)
    {

        if (!Network.isClient)
        {

            if (EventPowerAttached != null)
            {

                EventPowerAttached(this, power, ship);

            }

        }

        //Call the typed version
        NotifyPowerTypeAttached(power.GetType(), ship);
        
    }

    /// <summary>
    /// Notifies that a power has been detached from a ship. This event is not transmitted over the network!
    /// </summary>
    /// <param name="power">The power that has been detached</param>
    /// <param name="ship">The ship who lost the poweup</param>
    public void NotifyPowerDetached(Power power, GameObject ship)
    {

        if (!Network.isClient)
        {

            if (EventPowerDetached != null)
            {

                EventPowerDetached(this, power, ship);

            }

        }

        //Call the typed version
        NotifyPowerTypeDetached(power.GetType(), ship);

    }

    /// <summary>
    /// Notifies the match's initialization
    /// </summary>
    /// <param name="identities">The identities of the players</param>
    public void NotifyInitializeMatch(IList<PlayerIdentity> identities)
    {

        if (!Network.isClient)
        {

            if (Network.isServer)
            {

                int initialization_uid = UIDGenerator.Instance.GetNewUID();

                //RPC to other clients if this is the server
                foreach (PlayerIdentity identity in identities)
                {

                    networkView.RPC("ReceiveAccumulateIdentity", RPCMode.Others, initialization_uid, identity.networkView.viewID);

                }

                networkView.RPC("ReceiveInitializeMatch", RPCMode.Others, initialization_uid);

            }

            if (EventInitializeMatch != null)
            {

                EventInitializeMatch(this, identities);

            }

        }

    }

    /// <summary>
    /// Notifies the match's start
    /// </summary>
    public void NotifyStartMatch()
    {

        if (!Network.isClient)
        {

            if (Network.isServer)
            {

                //RPC to other clients if this is the server
                networkView.RPC("ReceiveStartMatch", RPCMode.Others);

            }

            if (EventStartMatch != null)
            {

                EventStartMatch(this);

            }

        }

    }

    /// <summary>
    /// Notifies the match's end
    /// </summary>
    public void NotifyEndMatch()
    {

        if (!Network.isClient)
        {

            if (Network.isServer)
            {

                //RPC to other clients if this is the server
                networkView.RPC("ReceiveEndMatch", RPCMode.Others);

            }

            if (EventEndMatch != null)
            {

                EventEndMatch(this);

            }

        }
        
    }

    /// <summary>
    /// Notifies that a player has been eliminated from the game
    /// </summary>
    /// <param name="player_identity">The identity of the player that has been eliminated</param>
    public void NotifyPlayerEliminated(PlayerIdentity player_identity)
    {

        if (!Network.isClient)
        {

            if (Network.isServer)
            {

                //RPC to other clients if this is the server
                networkView.RPC("ReceivePlayerEliminated", RPCMode.Others, player_identity.networkView.viewID);

            }

            if (EventPlayerEliminated != null)
            {

                EventPlayerEliminated(this, player_identity);

            }

        }

    }

    #endregion

    #region "Delegates"

    /// <summary>
    /// Delegate used by EventOrbAttached
    /// </summary>
    public delegate void DelegateOrbAttached(object sender, GameObject orb, GameObject ship);

    /// <summary>
    /// Delegate used by EventOrbDetached
    /// </summary>
    public delegate void DelegateOrbDetached(object sender, GameObject orb, GameObject ship);
    
    /// <summary>
    /// Delegate used by EventFight
    /// </summary>
    public delegate void DelegateFight(object sender, IList<GameObject> orbs, GameObject attacker, GameObject defender);

    /// <summary>
    /// Delegate used by EventPowerAttached
    /// </summary>
    public delegate void DelegatePowerAttached(object sender, Power power, GameObject ship);

    /// <summary>
    /// Delegate used by EventPowerDetached
    /// </summary>
    public delegate void DelegatePowerDetached(object sender, Power power, GameObject ship);

    /// <summary>
    /// Delegate used by EventPowerAttached
    /// </summary>
    public delegate void DelegatePowerTypeAttached(object sender, System.Type power_type, GameObject ship);

    /// <summary>
    /// Delegate used by EventPowerDetached
    /// </summary>
    public delegate void DelegatePowerTypeDetached(object sender, System.Type power_type, GameObject ship);
    
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

    #endregion

    #region "Events"

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
    /// Raised when a ship have lost a power
    /// </summary>
    public event DelegatePowerDetached EventPowerDetached;

    /// <summary>
    /// Raised when a ship has gained a power type
    /// </summary>
    public event DelegatePowerTypeAttached EventPowerTypeAttached;

    /// <summary>
    /// Raised when a ship have lost a power type
    /// </summary>
    public event DelegatePowerTypeDetached EventPowerTypeDetached;

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

    #endregion

    /// <summary>
    /// Notifies that a power has been attached to a player
    /// </summary>
    /// <param name="power">The power that has been attached</param>
    /// <param name="ship">The ship who gained the power</param>
    private void NotifyPowerTypeAttached(System.Type power_type, GameObject ship)
    {

        if (!Network.isClient)
        {

            if (Network.isServer)
            {

                //RPC to other clients if this is the server
                networkView.RPC("ReceivePowerTypeAttached", RPCMode.Others, power_type.FullName, ship.networkView.viewID);

            }

            if (EventPowerAttached != null)
            {

                EventPowerTypeAttached(this, power_type, ship);

            }

        }

    }

    /// <summary>
    /// Notifies that a power has been detached from a ship
    /// </summary>
    /// <param name="power">The power that has been detached</param>
    /// <param name="ship">The ship who lost the poweup</param>
    private void NotifyPowerTypeDetached(System.Type power_type, GameObject ship)
    {

        if (!Network.isClient)
        {

            if (Network.isServer)
            {

                //RPC to other clients if this is the server
                networkView.RPC("ReceivePowerTypeDetached", RPCMode.Others, power_type.FullName, ship.networkView.viewID);

            }

            if (EventPowerTypeDetached != null)
            {

                EventPowerTypeDetached(this, power_type, ship);

            }

        }
    }

    #region "RPC Stuffs"

    private RPCAccumulator<GameObject> orbs_accumulator_;

    private RPCAccumulator<PlayerIdentity> player_identities_accumulator_;

    [RPC]
    private void ReceiveOrbAttached(NetworkViewID orb_id, NetworkViewID ship_id)
    {

        GameObject orb = NetworkView.Find(orb_id).gameObject;
        GameObject ship = NetworkView.Find(ship_id).gameObject;

        //Call the actual event
        if (EventOrbAttached != null)
        {

            EventOrbAttached(this, orb, ship);

        }

    }

    [RPC]
    private void ReceiveAccumulateOrb(int unique_id, NetworkViewID orb_id)
    {

        //Accumulates the orb
        orbs_accumulator_.Accumulate(unique_id, NetworkView.Find(orb_id).gameObject);

    }

    [RPC]
    private void ReceiveFight(int unique_id, NetworkViewID attacker_id, NetworkViewID defender_id)
    {

        GameObject attacker = NetworkView.Find(attacker_id).gameObject;
        GameObject defender = NetworkView.Find(defender_id).gameObject;
        IList<GameObject> orbs = orbs_accumulator_.Fetch(unique_id);

        if (EventFight != null)
        {

            EventFight(this, orbs, attacker, defender);

        }

    }

    [RPC]
    private void ReceivePowerTypeAttached(string type_string, NetworkViewID ship_id)
    {

        System.Type power_type = System.Type.GetType(type_string);
        GameObject ship = NetworkView.Find(ship_id).gameObject;

        if (EventPowerAttached != null)
        {

            EventPowerTypeAttached(this, power_type, ship);

        }

    }

    [RPC]
    private void ReceivePowerTypeDetached(string type_string, NetworkViewID ship_id)
    {

        System.Type power_type = System.Type.GetType(type_string);
        GameObject ship = NetworkView.Find(ship_id).gameObject;

        if (EventPowerAttached != null)
        {

            EventPowerTypeDetached(this, power_type, ship);

        }

    }

    [RPC]
    private void ReceiveAccumulateIdentity(int unique_id, NetworkViewID identity_id)
    {

        //Accumulates the identity
        player_identities_accumulator_.Accumulate(unique_id, NetworkView.Find(identity_id).gameObject.GetComponent<PlayerIdentity>());

    }

    [RPC]
    private void ReceiveInitializeMatch(int unique_id)
    {

        IList<PlayerIdentity> identities = player_identities_accumulator_.Fetch(unique_id);

        if (EventInitializeMatch != null)
        {

            EventInitializeMatch(this, identities);

        }

    }

    [RPC]
    private void ReceiveStartMatch()
    {

        if (EventStartMatch != null)
        {

            EventStartMatch(this);

        }

    }

    [RPC]
    private void ReceiveEndMatch()
    {

        if (EventEndMatch != null)
        {

            EventEndMatch(this);

        }

    }

    [RPC]
    private void ReceivePlayerEliminated(NetworkViewID player_identity_id)
    {

        PlayerIdentity player_identity = NetworkView.Find(player_identity_id).gameObject.GetComponent<PlayerIdentity>();

        if (EventPlayerEliminated != null)
        {

            EventPlayerEliminated(this, player_identity);

        }

    }

    #endregion

}
