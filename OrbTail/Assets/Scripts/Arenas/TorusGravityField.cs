using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ZXTorusGravityField : IGravityField
{

    public const float hoverForce = 100.0f;
    public const float hoverDampen = 5.0f;

    /// <summary>
    /// The center of gravity
    /// </summary>
    public Vector3 Center { get; private set; }

    public float Radius { get; private set; }

    public ZXTorusGravityField(Vector3 center, float radius)
    {

        Center = center;
        Radius = radius;

    }

    public void SetGravity(FloatingObject floatie)
    {

        floatie.hoverForce = hoverForce;
        floatie.hoverDampen = hoverDampen;

        //Hypothesis: the torus is aligned along ZX plane
        Vector3 projected_position = new Vector3(floatie.transform.position.x,
                                                 0.0f,
                                                 floatie.transform.position.z);

        //Determinates the center of the circle
        var circle_center = (projected_position - Center).normalized * Radius + Center;

        floatie.ArenaDown = (floatie.transform.position - circle_center).normalized;

    }

}

