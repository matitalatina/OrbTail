using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Reads the data from a mobile platform
/// </summary>
class MobileInputBroker: IInputBroker
{

    /// <summary>
    /// Returns the acceleration command's status. 0 no acceleration, 1 maximum acceleration.
    /// </summary>
    public float Acceleration { get; private set; }

    /// <summary>
    /// Returns the steering command's status. -1 steer left, 0 no steering, 1 steer right
    /// </summary>
    public float Steering { get; private set; }

    public ICollection<IGroup> FiredPowerUps
    {
        get { throw new NotImplementedException(); }
    }

    public void Update()
    {
        ////////////
        // Accelerometer
        //		Vector3 forceVector = Vector3.zero;
        //        int i = 0;
        //        while (i < Input.accelerationEventCount) {
        //            AccelerationEvent accEvent = Input.GetAccelerationEvent(i);
        //            forceVector += accEvent.acceleration * forcePower;
        //            ++i;
        //        }
        ///////////////
        throw new NotImplementedException();
    }

}
