using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Power
{
    protected Power(PowerUpGroup powerUpGroup, int duration) 
    {
        // TODO: magic stuff
    } 
    public abstract void Activate(GameObject gameObj);
    public abstract void Deactivate();
    public abstract void Update();
    public abstract void Fire();
    public abstract GameObject GetShip();
}