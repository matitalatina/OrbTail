using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Reads the input data from a desktop platform
/// </summary>
class DesktopInputBroker: IInputBroker
{

    #region "Axis names"
    
    public string acceleration_axis_name = "Vertical";

    public string steering_axis_name = "Horizontal";

    public string fire_special_axis_name = "FireSpecial";

    public string fire_main_axis_name = "FireMain";
    
    #endregion

    /// <summary>
    /// Returns the acceleration command's status. 0 no acceleration, 1 maximum acceleration.
    /// </summary>
    public float Acceleration { get; private set; }

    /// <summary>
    /// Returns the steering command's status. -1 steer left, 0 no steering, 1 steer right
    /// </summary>
    public float Steering { get; private set; }

    /// <summary>
    /// Returns a collection which indicates all the power ups the user wants to fire. The elements indicates just the group of the proper power
    /// </summary>
    public ICollection<IGroup> FiredPowerUps
    {
        get { return fired_power_ups_; }
    }

    public void Update()
    {

        if (fired_power_ups_.Count > 0)
        {

            fired_power_ups_.Clear();

        }

        Acceleration = Input.GetAxis(acceleration_axis_name);
        Steering = Input.GetAxis(steering_axis_name);
        
        if (Input.GetAxis(fire_special_axis_name) > 0.0f)
        {

            fired_power_ups_.Add(SpecialPowerGroup.Instance.groupID);

        }

        if (Input.GetAxis(fire_main_axis_name) > 0.0f)
        {

            fired_power_ups_.Add(MainPowerGroup.Instance.groupID);

        }

    }


    private IList<IGroup> fired_power_ups_ = new List<IGroup>();

}

