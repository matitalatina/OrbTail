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

    public const int kNoGroup = 0;
    public const int kSphereGroup = 1;
    public const int kElbowGroup = 2;

    public const int kRedSphere = 0;
    public const int kGreenSphere = 1;
    public const int kBlueSphere = 2;

    public const int kYellowElbow = 0;
    public const int kCyanElbow = 1;
    public const int kMagentaElbow = 2;

    public const int kSourceGroupOffset = 0x4f;
    public const int kSourceGroupMask = 0xF << kSourceGroupOffset; 

    public const int kSourceIndexOffset = 0x0f;
    public const int kSourceIndexMask = 0xF << kSourceIndexOffset;

    /// <summary>
    /// The radius of the sphere
    /// </summary>
    public const float kSphereRadius = 260.0f;

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

        //Spheres
        sphere_centers_[kRedSphere] = new Vector3(147.0f, 0.0f, 0.0f);
        sphere_centers_[kGreenSphere] = new Vector3(-73.0f, 128.0f, 0.0f);
        sphere_centers_[kBlueSphere] = new Vector3(-73.0f, -128.0f, 0.0f);

        //Elbows
        elbow_center_[kYellowElbow] = new Vector3(80.0f, 140.0f, 0.0f);
        elbow_center_[kCyanElbow] = new Vector3(-163.0f, 0.0f, 0.0f);
        elbow_center_[kMagentaElbow] = new Vector3(80.0f, -140.0f, 0.0f);

        //Elbow planes
        elbow_planes[kYellowElbow, 0] = new Plane(new Vector3(40.0f, 190.0f, 0.0f),
                                                  new Vector3(0.86f, 0.5f, 0.0f));
        elbow_planes[kYellowElbow, 1] = new Plane(new Vector3(145.0f, 125.0f, 0.0f),
                                                  new Vector3(0.0f, 1.0f, 0.0f));

        elbow_planes[kCyanElbow, 0] = new Plane(new Vector3(-185.0f, -60.0f, 0.0f),
                                                   new Vector3(-0.86f, -0.5f, 0.0f));
        elbow_planes[kCyanElbow, 1] = new Plane(new Vector3(-185.0f, 60.0f, 0.0f),
                                                   new Vector3(0.86f, -0.5f, 0.0f));
        
        elbow_planes[kMagentaElbow, 0] = new Plane(new Vector3(145.0f, -125.0f, 0.0f),
                                                new Vector3(0.0f, -1.0f, 0.0f));
        elbow_planes[kMagentaElbow, 1] = new Plane(new Vector3(40.0f, -190.0f, 0.0f),
                                                new Vector3(0.86f, -0.5f, 0.0f));

    }

    public void SetGravity(FloatingObject floatie)
    {

        floatie.hoverForce = kHoverForce;
        floatie.hoverDampen = kHoverDampen;

        int index = 0;
        int group = 0;

        //Set the proper gravity source
        SetGravitySource(floatie, out group, out index);

        //Depending on the source applies the proper force
        if (group == kSphereGroup)
        {

            floatie.ArenaDown = (floatie.transform.position - sphere_centers_[index]).normalized;

            Debug.DrawLine(floatie.transform.position, sphere_centers_[index]);

        }
        else if (group == kElbowGroup)
        {

            //Hypothesis: the torus is aligned along XY plane
            Vector3 projected_position = new Vector3(floatie.transform.position.x,
                                                     floatie.transform.position.y,
                                                     0.0f);

            //Determinates the center of the circle
            var circle_center = (projected_position - elbow_center_[index]).normalized * kElbowRadius + elbow_center_[index];

            floatie.ArenaDown = (floatie.transform.position - circle_center).normalized;

            Debug.DrawLine(floatie.transform.position, sphere_centers_[index]);

        }

    }

    /// <summary>
    /// Is the object still inside its previous group?
    /// </summary>
    private bool StillInside(Vector3 position, int group, int index)
    {

        if (group == kSphereGroup)
        {

            return InsideSphere(position, index);

        }
        else if (group == kElbowGroup)
        {

            return InsideElbow(position, index);

        }
        else
        {

            return false;

        }

    }

    /// <summary>
    /// Is the given point inside the sphere?
    /// </summary>
    private bool InsideSphere(Vector3 position, int sphere_index){

        return (position - sphere_centers_[sphere_index]).sqrMagnitude <= kSqrSphereRadius;

    }

    /// <summary>
    /// Returns true if the floatie is inside the elbow
    /// </summary>
    private bool InsideElbow(Vector3 position, int elbow_index)
    {

        //The position must be in front of each elbow plane
        for(int p = 0; p < kElbowPlanes; p++){

            if (elbow_planes[elbow_index, p].GetDistanceToPoint(position) < 0)
            {

                //If one plane fails, the check fails
                return false;

            }

        }

        return true;
        
    }

    /// <summary>
    /// Set the proper gravity source for the floatie
    /// </summary>
    private void SetGravitySource(FloatingObject floatie, out int group, out int index)
    {

        index = (floatie.GravitySourceIndex & kSourceIndexMask) >> kSourceIndexOffset;
        group = (floatie.GravitySourceIndex & kSourceGroupMask) >> kSourceGroupOffset;

        var position = floatie.transform.position;

        if (!StillInside(position, group, index))
        {

            //If the ship was in a sphere, now it will be in an elbow
            if( group == kSphereGroup ||
                group == kNoGroup)
            {

                group = kElbowGroup;

                if (InsideElbow(position, kYellowElbow))
                {

                    index = kYellowElbow;
                    return;

                }
                else if (InsideElbow(position, kCyanElbow))
                {

                    index = kCyanElbow;
                    return;

                }
                else if (InsideElbow(position, kMagentaElbow))
                {

                    index = kMagentaElbow;
                    return;

                }

            }

            //If the ship was in an elbow, now it will be in a sphere
            if( group == kElbowGroup ||
                group == kNoGroup)
            {

                group = kSphereGroup;

                if (InsideSphere(position, kRedSphere))
                {

                    index = kRedSphere;
                    return;

                }
                else if (InsideSphere(position, kGreenSphere))
                {

                    index = kGreenSphere;
                    return;

                }
                else if (InsideSphere(position, kBlueSphere))
                {

                    index = kBlueSphere;
                    return;

                }

            }

            //If no group was selected, then we have a problem
            System.Diagnostics.Debug.Assert(false, "Houston we have a problem. Check the 'InsideWhatever' part!");

        }
        else
        {

            //No need to change the source, yay!

        }

    }
    
    //The ship could be inside a sphere or an elbow

    private const float kSqrSphereRadius = kSphereRadius * kSphereRadius;

    private const float kSqrElbowRadius = kElbowRadius * kElbowRadius;

    private const int kSphereCount = 3;

    private const int kElbowCount = 3;

    private const int kElbowPlanes = 2;
    
    private Vector3[] sphere_centers_ = new Vector3[kSphereCount];
    
    private Vector3[] elbow_center_ = new Vector3[kElbowCount];

    private Plane[,] elbow_planes = new Plane[kElbowCount,kElbowPlanes];

}
