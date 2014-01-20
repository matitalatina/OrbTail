using UnityEngine;
using System.Collections;

public class HUDMenuButtonHandler : GUIMenuChoose {
	private TextMesh exitMessage;
	private SpriteRenderer menuSprite;
	private float shortFadeTime = 0.1f;
	private float longFadeTime = 2f;
	private bool alreadyPressed = false;
	private Color originalButtonColor;

	// Use this for initialization
	public override void Start () {
		base.Start();
		exitMessage = transform.Find("ExitMessage").GetComponent<TextMesh>();
		menuSprite = transform.Find("MenuButtonSprite").GetComponent<SpriteRenderer>();
		originalButtonColor = menuSprite.color;
	}
	
	protected override void OnSelect (GameObject target)
	{
		base.OnSelect (target);
		if (target.name == "MenuButtonSprite") {
			if (!alreadyPressed) {
				alreadyPressed = true;
				menuSprite.color = Color.white;

				iTween.ValueTo(this.gameObject, iTween.Hash(
					"from", 0f,
					"to", 1f,
					"time", shortFadeTime,
					"onUpdate","ChangeAlphaColor",
					"onComplete", "WaitSecondTap"));
			}
			else {
				Destroy(GameObject.FindGameObjectWithTag(Tags.Master));
				
				GameObjectFactory.Instance.Purge();
				
				//Okay, good game, let's go home...
				Network.Disconnect();
				
				Application.LoadLevel("MenuMain");
			}
		}
	}

	private void WaitSecondTap() {
		iTween.ValueTo(this.gameObject, iTween.Hash(
			"from", 1f,
			"to", 0f,
			"time", longFadeTime,
			"easetype", iTween.EaseType.easeInCubic,
			"onUpdate","ChangeAlphaColor",
			"onComplete", "ResetButton"));
	}

	private void ResetButton() {
		alreadyPressed = false;
		menuSprite.color = originalButtonColor;
	}

	private void ChangeAlphaColor(float alpha) {
		Color color = exitMessage.color;
		color.a = alpha;
		exitMessage.color = color;
	}
}
