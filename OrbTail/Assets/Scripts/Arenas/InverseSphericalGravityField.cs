using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Gravity field which pushes from the inside of a sphere to its surface
/// </summary>
public class InverseSphericalGravityField: IGravityField
{
       
    public const float hoverForce = 50f;
    public const float hoverDampen = 5f;

    /// <summary>
    /// The center of gravity
    /// </summary>
    public Vector3 Center { get; private set; }

    public InverseSphericalGravityField(Vector3 center)
    {

        Center = center;

    }

    public void SetGravity(FloatingObject floatie)
    {

        floatie.hoverForce = hoverForce;
        floatie.hoverDampen = hoverDampen;

        floatie.ArenaDown = -Center + floatie.transform.position.normalized;

    }

}
