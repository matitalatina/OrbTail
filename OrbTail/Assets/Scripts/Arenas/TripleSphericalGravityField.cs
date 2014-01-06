using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// The gravity is custom, see TripleSphereArena for an idea
/// </summary>
public class TripleSphericalGravityField : IGravityField
{

    public const float kHoverForce = 9.8f;
    public const float kHoverDampen = 5f;

    /// <summary>
    /// The radius of the sphere
    /// </summary>
    public const float kSphereRadius = 130.0f;

    /// <summary>
    /// The elbow of the sphere
    /// </summary>
    public const float kElbowRadius = 60.0f;

    /// <summary>
    /// The center of gravity
    /// </summary>
    public Vector3 Center { get; private set; }

    public TripleSphericalGravityField(Vector3 center)
    {

        Center = center;

        sphere_centers_[0] = new Vector3(147.0f, 0.0f, 0.0f);
        sphere_centers_[1] = new Vector3(-73.0f, -128.0f, 0.0f);
        sphere_centers_[2] = new Vector3(-73.0f, 128.0f, 0.0f);

        elbow_center_[0] = new Vector3(80.0f, 140.0f, 0.0f);
        elbow_center_[1] = new Vector3(80.0f, -140.0f, 0.0f);
        elbow_center_[2] = new Vector3(-163.0f, 0.0f, 0.0f);

    }

    public void SetGravity(FloatingObject floatie)
    {

        floatie.hoverForce = kHoverForce;
        floatie.hoverDampen = kHoverDampen;

        //Find the sphere the floatie is inside of
        Vector3? parent_sphere = null;

        foreach (Vector3 sphere in sphere_centers_)
        {

            if( (floatie.transform.position - sphere).sqrMagnitude <= kSqrSphereRadius ){

                parent_sphere = sphere;
                break;

            }

        }

        if (parent_sphere != null)
        {

            //The gravity pushes away from the center
            floatie.ArenaDown = (floatie.transform.position - parent_sphere.Value).normalized;

            Debug.DrawLine(floatie.transform.position, parent_sphere.Value);


        }
        else
        {

            Debug.DrawLine(floatie.transform.position, Vector3.zero, Color.red);

            /*
            //Find the nearest elbow
            float min_sqr_distance = elbow_center_.Min((Vector3 center)=>{ return (floatie.transform.position - center).sqrMagnitude; });

            var parent_elbow = elbow_center_.FirstOrDefault((Vector3 center) => { return (floatie.transform.position - center).sqrMagnitude <= min_sqr_distance; });

            Debug.Log("Inside the elbow @" + parent_elbow.ToString());

            //Hypothesis: the torus is aligned along XY plane
            Vector3 projected_position = new Vector3( floatie.transform.position.x,
                                                      floatie.transform.position.y,
                                                      0.0f );

            //Determinates the center of the circle
            var circle_center = (projected_position - parent_elbow).normalized * kElbowRadius + parent_elbow;

            floatie.ArenaDown = floatie.transform.position - circle_center;
             
            floatie.ArenaDown.Normalize();
            */
        }

        
    }
    
    //The ship could be inside a sphere or an elbow

    private const float kSqrSphereRadius = kSphereRadius * kSphereRadius;

    private const float kSqrElbowRadius = kElbowRadius * kElbowRadius;
    
    private Vector3[] sphere_centers_ = new Vector3[3];
    
    private Vector3[] elbow_center_ = new Vector3[3];

}
