using UnityEngine;
using System.Collections;

public class Deactivator {
	private bool toggleSwitch = true;


	/// <summary>
	/// Deactivate the power linked to it.
	/// </summary>
	public void Deactivate() {
		toggleSwitch = false;
	}


	/// <summary>
	/// Determines whether the linked power is active.
	/// </summary>
	/// <returns><c>true</c> if the linked power is active; otherwise, <c>false</c>.</returns>
	public bool IsActive() {
		return toggleSwitch;
	}
}
