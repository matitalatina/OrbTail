using System;
using System.Collections.Generic;
   
/// <summary>
/// Interface intherited by all the powers
/// </summary>
public abstract class PowerView
{

    public delegate void DelegateDestroyed(object sender, IGroup group);

    /// <summary>
    /// Fired when the power has been destroyed and cannot be used anymore
    /// </summary>
    public event DelegateDestroyed EventDestroyed;
    
    /// <summary>
    /// Get the group of this power
    /// </summary>
    public abstract IGroup Group { get; }
    
    /// <summary>
    /// Get the readyness of this power. 1 if ready, less than 1 otherwise
    /// </summary>
    protected abstract float IsReady{ get; }

    protected void Destroy(IGroup group)
    {

        if (EventDestroyed != null)
        {
            EventDestroyed(this, group);
        }

    }

}
