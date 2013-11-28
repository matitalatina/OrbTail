using UnityEngine;
using System;
using System.Collections.Generic;

public class PowerController : MonoBehaviour
{
    private Power power { get; set; }
    
    public PowerController()
    {

    }

    public void AddPower(Power power)
    {
        this.power = power;
    }

    public PowerView GetPowerView<T>()
    {
        return power;
    }
}
