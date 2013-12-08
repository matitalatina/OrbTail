using System;
using System.Collections.Generic;
using UnityEngine;

public class Boost : Power
{
    private const float reload_power_time = 5.0f;
    private float time_accumulator_to_reload = reload_power_time;

    private Deactivator deactivator;

    public Boost() : base(SpecialPowerGroup.Instance.groupID, float.MaxValue) {}
    
    protected override float IsReady
    {
        get
        {
            return time_accumulator_to_reload / reload_power_time;
        }
    }
    
    public override void Update()
    {
        base.Update();

        time_accumulator_to_reload = Mathf.Clamp(time_accumulator_to_reload + Time.deltaTime, 0.0f, reload_power_time);

    }

    /// <summary>
    /// Activate boost on ship if it's avaiable
    /// </summary>
    public override void Fire()
    {
        if (IsReady >= 1.0f)
        {
            time_accumulator_to_reload = 0.0f;

            shipOwner.GetComponent<Rigidbody>().AddForce(shipOwner.transform.forward * 100, ForceMode.Impulse);
        }
    }

}