using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// The gravity is always pointing downwards
/// </summary>
public class FlatGravityField: IGravityField
{

    public void SetGravity(FloatingObject floatie)
    {

        floatie.ArenaDown = Vector3.down;

    }

}
