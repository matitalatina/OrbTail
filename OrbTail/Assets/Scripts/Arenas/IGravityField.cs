using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Interface inherited by the gravity field
/// </summary>
public interface IGravityField
{

    /// <summary>
    /// Sets the gravity for the floatie
    /// </summary>
    void SetGravity(FloatingObject floatie);

}

