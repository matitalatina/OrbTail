using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Power : PowerView
{

    public const string power_prefab_path = "Prefabs/Power/";

    protected int group { get; private set; }
    protected float? duration { get; set; }
    protected float activatedTime { get; private set; }

    protected float time_accumulator = 0.0f;

    private GameObject fx;

    protected Power(int group, float? duration, string name)
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
    public override int Group
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

        ActivateShared();

        if(NetworkHelper.IsServerSide())
        {
            
            ActivateServer();

        }

        if (NetworkHelper.IsOwnerSide(Owner.networkView))
        {
            
            ActivateClient();

        }

    }

    protected virtual void ActivateShared()
    {

        AddFX();

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

        UpdateTimeToLive();
                
    }

    /// <summary>
    /// Updates the time to live of the power and eventually destroy it
    /// </summary>
    private void UpdateTimeToLive()
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
    public virtual bool Fire() { return false; }

    private void AddFX()
    {

        fx = GameObjectFactory.Instance.Instantiate(power_prefab_path + Name, Owner.transform.position, Quaternion.identity);

        fx.transform.parent = Owner.transform;

    }

    private void RemoveFX()
    {

        GameObjectFactory.Instance.Destroy(power_prefab_path + Name, fx);

    }

    public override float IsReady
    {

        get
        {

            return 0.0f;

        }

    }

}