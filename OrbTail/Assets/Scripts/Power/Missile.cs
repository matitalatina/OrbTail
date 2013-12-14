using System;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Power
{
    private const float power_time = 7.0f;
	private const float missileforwardOffset = 0.3f;

    public Missile() : base(MainPowerGroup.Instance.groupID, float.MaxValue, "Missile") { }
    
    public override void Fire()
    {
        base.Fire();

        Debug.Log("Ship "+ Owner +" shooted Missile!");

        var missileRes = Resources.Load("Prefabs/Power/MissileRocket");
        GameObject missile;
        
        if(Network.peerType == NetworkPeerType.Disconnected)
        {
            missile = GameObject.Instantiate(missileRes, Owner.transform.position + Owner.transform.forward + Owner.rigidbody.velocity * missileforwardOffset, Owner.transform.rotation) as GameObject;
        }
        else
        {
            missile = Network.Instantiate(missileRes, Owner.transform.position + Owner.transform.forward + Owner.rigidbody.velocity * missileforwardOffset, Owner.transform.rotation, 0) as GameObject;

            Debug.Log(Owner);


        }        

        var ships = GameObject.FindGameObjectsWithTag(Tags.Ship);
        float nearestEnemyDistance = float.MaxValue;
        GameObject nearestEnemyShip = null;
        foreach (GameObject ship in ships)
        {
            if (ship == Owner)
            {
                continue;
            }

            Vector3 distanceVector = (ship.transform.position - Owner.transform.position);
            var distance = distanceVector.sqrMagnitude;

            if (distance < nearestEnemyDistance)
            {
                nearestEnemyShip = ship;
                nearestEnemyDistance = distance;
            }
        }

        
        // TODO: IMPOSTARE IN REMOTO SIA IL TARGET CHE L'OWNER
        var follower = missile.GetComponent<MissileBehavior>();

        follower.SetTarget(nearestEnemyShip, Owner);

        //Once fire it is destroyed
        Deactivate();

    }

    protected override float IsReady { get { return 1.0f; } }

    public override Power Generate()
    {

        return new Missile();

    }

}
