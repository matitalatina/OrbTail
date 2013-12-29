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

    public void NotifyWin()
    {

        if (EventWin != null)
        {

            EventWin(this);

        }

    }

    public BaseGameMode(Game game)
    {

        Game = game;

    }

    /// <summary>
    /// Return the winner
    /// </summary>
    public abstract GameObject Winner { get; }

    /// <summary>
    /// The match duration
    /// </summary>
    public abstract int Duration { get;  }

    /// <summary>
    /// The game mode name
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// The game instance
    /// </summary>
    protected Game Game { get; private set; }

}

