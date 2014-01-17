using UnityEngine;
using System.Collections;

public class ScreenDimmerArenaManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (SystemInfo.deviceType == DeviceType.Handheld) {
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}

		enabled = false;
	}

}
