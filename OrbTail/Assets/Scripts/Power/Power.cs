using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Power : PowerView
{
    protected IGroup group { get; private set; }
    protected float? duration { get; set; }
    protected float activatedTime { get; private set; }

    protected float time_accumulator = 0.0f;

    private GameObject fx;

    protected Power(IGroup group, float? duration, string name)
    {

        this.group = group;
        this.duration = duration;
        this.Name = name;

    }

    /// <summary>
    /// The owner of the power
    /// </summary>
    public GameObject Owner { get; private set; }

    /// <summary>
    /// Get the power's group
    /// </summary>
    public override IGroup Group
    {
        get
        {
            return group;
        }
    }

    /// <summary>
    /// The power name
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
    /// Clone this power
    /// </summary>
    public abstract Power Generate();

    /// <summary>
    /// Activate the power up
    /// </summary>
    /// <param name="gameObj">Ship with activated power up</param>
    public void Activate(GameObject owner)
    {
        this.Owner = owner;
        this.activatedTime = Time.time;
        this.time_accumulator = 0.0f;

        AddFX();

        ActivateShared();
        if(Network.isServer || Network.peerType == NetworkPeerType.Disconnected)
        {
            ActivateServer();
        }
        if ((Network.isClient && Network.player == Owner.networkView.viewID.owner) || Network.peerType == NetworkPeerType.Disconnected)
        {
            ActivateClient();
        }
    }

    protected virtual void ActivateShared()
    {
    
    }
    /// <summary>
    /// Modify TailController and Tail
    /// </summary>
    protected virtual void ActivateServer()
    {
    
    }
    /// <summary>
    /// Modify movement controller
    /// </summary>
    protected virtual void ActivateClient()
    {

    }

    /// <summary>
    /// Deactivate power up
    /// </summary>
    public virtual void Deactivate()
    {
        RemoveFX();

        Destroy(group);
    }

    /// <summary>
    /// Counter to deactivate the active power up
    /// </summary>
    public virtual void Update()
    {

        time_accumulator += Time.deltaTime;

        // If power up time is expired, deactivate power up
        if (time_accumulator > (duration ?? float.MaxValue))
        {

            time_accumulator = 0.0f;
            duration = null;

            Deactivate();

        }
        
    }

    /// <summary>
    /// Fire avaiable power up
    /// </summary>
    public virtual void Fire() { }

    private void AddFX()
    {
        Debug.Log("AddFX Name: " + Name);
        var fx_resource = Resources.Load("Prefabs/Power/" + Name);
        fx = GameObject.Instantiate(fx_resource, Owner.transform.position, Quaternion.identity) as GameObject;
        fx.transform.parent = Owner.transform;
    }

    private void RemoveFX()
    {

        GameObject.Destroy(fx);

    }

}