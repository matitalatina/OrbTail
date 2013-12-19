using UnityEngine;
using System;
using System.Collections.Generic;

public class RandomPowerAttacher : MonoBehaviour
{
    public const string glow_prefab_path = "Prefabs/Power/PowerGlow";
    private GameObject particle_dummy;

    void Start()
    {

        AddFX();

    }

    [RPC]
    private void AddFX()
    {

        particle_dummy = GameObjectFactory.Instance.Instantiate(glow_prefab_path, gameObject.transform.position, Quaternion.identity);
        particle_dummy.transform.parent = gameObject.transform;

    }

    public void RemoveFX()
    {

        GameObjectFactory.Instance.Destroy(glow_prefab_path, particle_dummy);

        this.enabled = false;

    }

}
