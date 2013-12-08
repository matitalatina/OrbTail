using System;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : Power
{
    private const float power_time = 10.0f;

    static IGroup invincibility_group = new GroupID();

    public Invincibility() : base(invincibility_group, power_time) { }

    private Deactivator deactivator;

    public override void Activate(UnityEngine.GameObject gameObj)
    {

        Debug.Log("Ship Jammed!");

        base.Activate(gameObj);

        var wheel_stack = gameObj.GetComponent<MovementController>().GetWheelDriverStack();

        deactivator = wheel_stack.Push( new JammedWheelDriver( wheel_stack.GetPrototype().GetSteering() ));
        
    }

    public override void Deactivate()
    {

        Debug.Log("Ship no more Jammed!");

        base.Deactivate();

        deactivator.Deactivate();

    }
    
    protected override float IsReady { get { return 0.0f; } }
}
