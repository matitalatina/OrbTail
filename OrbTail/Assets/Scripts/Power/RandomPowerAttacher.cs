using UnityEngine;
using System;
using System.Collections.Generic;

public class RandomPowerAttacher : MonoBehaviour
{

    private GameObject particle_dummy;

    private EventLogger event_logger_;

    void OnCollisionEnter(Collision collision)
    {

        if (!Network.isClient)
        {

            var collidedObj = collision.gameObject;

            if (collidedObj.tag == Tags.Ship)
            {

                RemoveFX();

                Power randomPower = PowerFactory.Instance.RandomPower;

                collidedObj.GetComponent<PowerController>().AddPower(randomPower);              

                if (Network.peerType != NetworkPeerType.Disconnected)
                {

                    networkView.RPC("RemoveFX", RPCMode.Others);

                }

            }

        }  

    }

    void Start()
    {

        event_logger_ = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<EventLogger>();

        AddFX();
        
    }

    [RPC]
    private void AddFX()
    {

        var particle_dummy_resource = Resources.Load("Prefabs/Power/PowerGlow");

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
