using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GUIMenuChoose : GUIMenu {
	private const float scaleFactor = 0.85f;
	private const float timeScale = 0.2f;
	private Dictionary<GameObject, Vector3> originalScales;

	public override void Start() {
		base.Start();

		originalScales = new Dictionary<GameObject, Vector3>();
	}
	

	protected override void OnPress(GameObject target) {
		Vector3 originalLocalScale = target.transform.localScale;
		if (!originalScales.ContainsKey(target)) {
			originalScales.Add(target, originalLocalScale);
		}

		iTween.ScaleTo(target, originalScales[target] * scaleFactor, timeScale);
	}
	

	protected override void OnLeave(GameObject target) {
		iTween.ScaleTo(target, originalScales[target], timeScale);
	}

	protected override void OnSelect(GameObject target) {
		iTween.ScaleTo(target, originalScales[target], timeScale);
	}
}
