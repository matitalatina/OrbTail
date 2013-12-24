using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Interface inherited by all game modes
/// </summary>
public interface IGameMode
{

    /// <summary>
    /// Return the rank for the current game mode
    /// </summary>
    IList<GameObject> Rank { get; }

}

