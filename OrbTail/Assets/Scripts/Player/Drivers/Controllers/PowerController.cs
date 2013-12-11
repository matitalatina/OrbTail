using UnityEngine;
using System;
using System.Collections.Generic;

public class PowerController : MonoBehaviour
{
    private Dictionary<IGroup, Power> powers;
    private InputProxy input;

    public void Start()
    {

        input = GetComponent<InputProxy>();

    }

    public void Awake(){

        powers = new Dictionary<IGroup, Power>();

    }

    public void AddPower(Power power)
    {
        
        power.Activate(gameObject);

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

        //The server is the only one able to add a power to everyone
        if (Network.isServer)
        {

            networkView.RPC("RPCAddPower", RPCMode.Others, power.Name);

        }

    }

    void power_EventDestroyed(object sender, IGroup group)
    {

        if (powers.ContainsKey(group))
        {

            var power = powers[group];

            powers.Remove(power.Group);

            if (networkView.isMine &&
                Network.peerType != NetworkPeerType.Disconnected)
            {

                //This is sent just for when the power is fired and then destroyed
                networkView.RPC("RPCRemovePower", RPCMode.Others, power.Name);

            }

        }

    }

    [RPC]
    private void RPCAddPower(string power_name)
    {

        var power = PowerFactory.Instance.PowerFromName(power_name);

        AddPower(power);

    }

    [RPC]
    private void RPCRemovePower(string power_name)
    {

        power_EventDestroyed(this, PowerFactory.Instance.GroupFromName(power_name));
        
    }

    public void Update()
    {

        //Needed everywhere as orbs could belong to different clients

        foreach (Power power in new List<Power>( powers.Values ))
        {

            power.Update();

        }

        //Needed only owner-side

        if (networkView.isMine ||
            Network.peerType == NetworkPeerType.Disconnected)
        {

            foreach (IGroup group in input.FiredPowerUps)
            {

                if (powers.ContainsKey(group))
                {

                    powers[group].Fire();

                }

            }

        }

    }

    public PowerView GetPowerView(IGroup group)
    {

        return powers[group];

    }
}
