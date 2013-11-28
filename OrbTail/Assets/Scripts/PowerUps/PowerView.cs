using System;
using System.Collections.Generic;
   
/// <summary>
/// Interface intherited by all the powers
/// </summary>
public abstract class PowerView
{

    /// <summary>
    /// Fired when the power has been destroyed and cannot be used anymore
    /// </summary>
    event EventHandler<EventArgs> EventDestroyed;
    
    /// <summary>
    /// Get the group of this power
    /// </summary>
    protected abstract IGroup Group { get; }
    
    /// <summary>
    /// Get the readyness of this power. 1 if ready, less than 1 otherwise
    /// </summary>
    protected abstract float IsReady{ get; }

    /// <summary>
    /// Fire EventDestroyed
    /// </summary>
    protected void Deactivate()
    {
        if (EventDestroyed != null)
        {
            EventDestroyed(this, null);
        }
    }

}
