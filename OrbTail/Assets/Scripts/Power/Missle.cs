using System;
using System.Collections.Generic;
using UnityEngine;

public class Missle : Power
{
    private const float power_time = 7.0f;
	private const float missileforwardOffset = 1.5f;

    public Missle() : base(MainPowerGroup.Instance.groupID, float.MaxValue) { }
    
    public override void Fire()
    {
        base.Fire();

        Debug.Log("Ship shooted Missile!");

        var missileRes = Resources.Load("Prefabs/Missle");
		GameObject missile = GameObject.Instantiate(missileRes, shipOwner.transform.position + shipOwner.transform.forward * missileforwardOffset, shipOwner.transform.rotation) as GameObject;

        var ships = GameObject.FindGameObjectsWithTag(Tags.Ship);
        float nearestEnemyDistance = float.MaxValue;
        GameObject nearestEnemyShip = null;
        foreach (GameObject ship in ships)
        {
            if (ship == shipOwner)
            {
                continue;
            }

            Vector3 distanceVector = (ship.transform.position - shipOwner.transform.position);
            var distance = distanceVector.sqrMagnitude;

            if (distance < nearestEnemyDistance)
            {
                nearestEnemyShip = ship;
                nearestEnemyDistance = distance;
            }
        }

        var missileFollower = missile.AddComponent<MissleFollower>();
        // Shoot to nearest enemy ship
        missileFollower.Target = nearestEnemyShip;
        missileFollower.Owner = shipOwner;

        //Once fire it is destroyed
        Deactivate();

    }

    public override void Deactivate()
    {

        Debug.Log("Missile hit an object or it's never been shooted!");

        base.Deactivate();

    }

    protected override float IsReady { get { return 1.0f; } }
}
