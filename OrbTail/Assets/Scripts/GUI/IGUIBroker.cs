using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// The broker used to add or remove powers from the GUI
/// </summary>
interface IGUIBroker
{

    /// <summary>
    /// Add a new power to the GUI
    /// </summary>
    /// <param name="power_view">The view of the power to add</param>
    void AddPower(PowerView power_view);

    /// <summary>
    /// Remove a power from the GUI
    /// </summary>
    /// <param name="power_view">The view of the power to remove</param>
    void RemovePower(PowerView power_view);

}

