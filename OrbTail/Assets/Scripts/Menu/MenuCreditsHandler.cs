using UnityEngine;
using System.Collections;

public class MenuCreditsHandler : GUIMenuChoose {

	protected override void OnSelect (GameObject target)
	{
		base.OnSelect (target);
		if (target.tag == Tags.BackButton) {
			Application.LoadLevel("MenuMain");
		}
	}
}
