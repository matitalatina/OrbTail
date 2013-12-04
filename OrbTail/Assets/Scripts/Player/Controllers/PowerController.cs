using UnityEngine;
using System;
using System.Collections.Generic;

public class PowerController : MonoBehaviour
{
    private Dictionary<IGroup, Power> powers;

    public void Awake(){

        powers = new Dictionary<IGroup, Power>();

    }

    public void AddPower(Power power)
    {
        // If exist another power with the same family
        if(powers.ContainsKey(power.Group))
        {
            powers[power.Group].Deactivate();
            powers[power.Group] = power;
        }
        else
        {
            powers.Add(power.Group, power);
        }

    }

    public void Update()
    {

        foreach (Power power in powers.Values)
        {
            power.Update();
        }

    }

    public PowerView GetPowerView(IGroup group)
    {
        return powers[group];
    }
}
