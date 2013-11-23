using UnityEngine;
using System.Collections;

public class Deactivator {
	private bool toggleSwitch = true;

	public void Deactivate() {
		toggleSwitch = false;
	}

	public bool IsActive() {
		return toggleSwitch;
	}
}
