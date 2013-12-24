using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Interface inherited by all game modes
/// </summary>
public abstract class BaseGameMode
{

    public delegate void DelegateEnd(BaseGameMode sender);

    /// <summary>
    /// Fired when the winning or the end conditions are met
    /// </summary>
    public DelegateEnd EventWin;

    protected void NotifyWin()
    {

        if (EventWin != null)
        {

            EventWin(this);

        }

    }

    /// <summary>
    /// Return the rank for the current game mode
    /// </summary>
    public abstract IList<GameObject> Rank { get; }

}

