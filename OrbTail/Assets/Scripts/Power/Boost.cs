using System;
using System.Collections.Generic;
using UnityEngine;

public class Boost : Power
{
    private const float power_time = 5.0f;
    private const float reload_power_time = power_time * 2.0f;
    private float time_accumulator_to_reload = 0.0f;

    private Deactivator deactivator;

    public Boost() : base(SpecialPowerGroup.Instance.groupID, float.MaxValue) { }
    
    protected override float IsReady
    {
        get
        {
            return (time_accumulator_to_reload / (reload_power_time / 100)) / 100;
        }
    }
    
    public override void Update()
    {
        time_accumulator += Time.deltaTime;
        
        Debug.Log(time_accumulator);

        if (time_accumulator > (duration ?? float.MaxValue))
        {

            time_accumulator = 0.0f;
            duration = null;
        }

        if (time_accumulator_to_reload < reload_power_time)
        {
            time_accumulator_to_reload += Time.deltaTime;
        }
    }

    /// <summary>
    /// Activate boost on ship if it's avaiable
    /// </summary>
    public override void Fire()
    {
        if (IsReady >= 1.0f)
        {
            time_accumulator_to_reload = 0.0f;
            Activate(shipOwner);
        }
    }


    public override void Activate(UnityEngine.GameObject gameObj)
    {

        Debug.Log("Ship Boosted!");

        base.Activate(gameObj);

        var engine_stack = gameObj.GetComponent<MovementController>().GetEngineDriverStack();

        deactivator = engine_stack.Push(new BoostEngineDriver(engine_stack.GetPrototype().GetPower()));

    }

    public override void Deactivate()
    {

        Debug.Log("Ship no more Boosted!");

        base.Deactivate();

        deactivator.Deactivate();

    }

}