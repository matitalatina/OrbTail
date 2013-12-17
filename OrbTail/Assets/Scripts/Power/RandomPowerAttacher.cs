using UnityEngine;
using System;
using System.Collections.Generic;

public class RandomPowerAttacher : MonoBehaviour
{

    private GameObject particle_dummy;

    void Start()
    {

        AddFX();
        
    }

    [RPC]
    private void AddFX()
    {

        var particle_dummy_resource = Resources.Load("Prefabs/Power/PowerGlow");

        particle_dummy = GameObject.Instantiate(particle_dummy_resource, gameObject.transform.position, Quaternion.identity) as GameObject;

        particle_dummy.transform.parent = gameObject.transform;

    }

    public void RemoveFX()
    {

        Destroy(particle_dummy);
        Destroy(this);

    }

}
