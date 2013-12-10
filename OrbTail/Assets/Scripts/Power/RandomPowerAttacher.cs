using UnityEngine;
using System;
using System.Collections.Generic;

static class PowerFactory
{
    public static Power GetPower()
    {
        System.Random rng = new System.Random();

        switch (rng.Next(0, 5))
        {
            case 1:
            case 3:
            case 0:{ 
                return new Missle();
            }
            /*case 1: { 
                return new Magnet();
            }*/
            case 2: { 
                return new Invincibility();
            }
            /*case 3: { 
                return new TailSwap();
            }*/
            case 4: { 
                return new OrbSteal();
            }
            case 5: { 
                return new Jam();
            }
            default: {
                System.Diagnostics.Debug.Assert(false);
                return null;
            }
        }
    }
}

public class RandomPowerAttacher : MonoBehaviour
{

    private GameObject particle_dummy;

    void OnCollisionEnter(Collision collision)
    {
        var collidedObj = collision.gameObject;

        if (collidedObj.tag == Tags.Ship)
        {

			RemoveFX();

            if (!Network.isClient)
            {

                Power randomPower = PowerFactory.GetPower();

				randomPower.Activate(collidedObj);

                collidedObj.GetComponent<PowerController>().AddPower(randomPower);


            }

			if (Network.peerType != NetworkPeerType.Disconnected)
			{

            	networkView.RPC("RemoveFX", RPCMode.Others);

			}


        }  

    }

    void Start()
    {

        AddFX();
        
    }

    void Update()
    {
    }

    [RPC]
    private void AddFX()
    {

        var particle_dummy_resource = Resources.Load("Prefabs/PowerGlow");

        particle_dummy = GameObject.Instantiate(particle_dummy_resource, gameObject.transform.position, Quaternion.identity) as GameObject;

        particle_dummy.transform.parent = gameObject.transform;

    }

    [RPC]
    private void RemoveFX()
    {

        Destroy(particle_dummy);
        Destroy(this);

    }

}
