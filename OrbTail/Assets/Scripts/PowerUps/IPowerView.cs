using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
   
/// <summary>
/// Interface intherited by all the powers
/// </summary>
public interface IPowerView
{

    /// <summary>
    /// Get the group of this power
    /// </summary>
    IGroup Group { get; }
    
    /// <summary>
    /// Get the readyness of this power. 1 if ready, less than 1 otherwise
    /// </summary>
    float Readyness{ get; }

    /// <summary>
    /// Fired when the power has been destroyed and cannot be used anymore
    /// </summary>
    event EventHandler<EventArgs> EventDestroyed;

}
