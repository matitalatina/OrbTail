using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script used to broadcast messages to the network and the game
/// </summary>
public class EventLogger : MonoBehaviour {
    
    /// <summary>
    /// Notifies than an orb has been attached
    /// </summary>
    /// <param name="orb">The orb that has been attached</param>
    /// <param name="ship">The ship that has been attached</param>
    public void NotifyOrbAttached(GameObject orb, GameObject ship)
    {

        if (EventOrbAttached != null)
        {

            EventOrbAttached(this, orb, ship);

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

        if (EventFight != null)
        {

            EventFight(this, orbs, attacker, defender);

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
    /// Raised when an orb has been attached to a player
    /// </summary>
    public event DelegateOrbAttached EventOrbAttached;

    /// <summary>
    /// Raised when two ships have fought
    /// </summary>
    public event DelegateFight EventFight;
  
}
