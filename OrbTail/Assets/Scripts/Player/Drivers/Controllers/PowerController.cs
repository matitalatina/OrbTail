using UnityEngine;
using System;
using System.Collections.Generic;

public class PowerController : MonoBehaviour
{
    private Dictionary<IGroup, Power> powers;
    private InputProxy input;
	private EventLogger eventLogger;

    public void Start()
    {
        input = GetComponent<InputProxy>();
		eventLogger = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<EventLogger>();
    }

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

        power.EventDestroyed += power_EventDestroyed;
		eventLogger.NotifyPowerAttached(power, gameObject);

    }

    void power_EventDestroyed(object sender, IGroup group)
    {

        powers.Remove(group);

    }


    public void Update()
    {

        foreach (Power power in new List<Power>( powers.Values ))
        {
            power.Update();
        }

        foreach (IGroup group in input.FiredPowerUps)
        {

            if (powers.ContainsKey(group)) {

                powers[group].Fire();

            }
            
        }

    }

    public PowerView GetPowerView(IGroup group)
    {
        return powers[group];
    }
}
