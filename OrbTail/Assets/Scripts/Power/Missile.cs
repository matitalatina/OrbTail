using System;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Power
{

    public const string missile_prefab_path = "Prefabs/Power/MissileRocket";
    private const float power_time = 7.0f;
	private const float missileforwardOffset = 3f;

    public Missile() : base(PowerGroups.Main, float.MaxValue, "Missile") { }
    
    public override bool Fire()
    {

        if (NetworkHelper.IsOwnerSide(Owner.networkView))
        {

            //Create a new rocket
            GameObject missile;

            if (Network.peerType == NetworkPeerType.Disconnected)
            {

                missile = GameObjectFactory.Instance.Instantiate(missile_prefab_path, Owner.transform.position + Owner.transform.forward * missileforwardOffset, Owner.transform.rotation);

            }
            else
            {

                //TODO: fix this SH!T
                var missileRes = Resources.Load(missile_prefab_path);
                missile = Network.Instantiate(missileRes, Owner.transform.position + Owner.transform.forward * missileforwardOffset, Owner.transform.rotation, 0) as GameObject;

            }

            //The missile should have at least the speed of its owner...
            missile.rigidbody.AddForce(Owner.rigidbody.velocity, ForceMode.VelocityChange);

            //Set its target
            missile.GetComponent<MissileBehavior>().SetTarget(FindTarget(Owner),
                                                              Owner);

        }

        //Once fire it is destroyed
        Deactivate();

        return true;

    }

    private GameObject FindTarget(GameObject owner)
    {

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

        return nearestEnemyShip;

    }

    protected override float IsReady { get { return 1.0f; } }

    public override Power Generate()
    {

        return new Missile();

    }

}
