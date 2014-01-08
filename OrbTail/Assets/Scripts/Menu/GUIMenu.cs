using UnityEngine;
using System.Collections;

public abstract class GUIMenu : MonoBehaviour {
	private bool pressed;
	private GameObject target;

	// Use this for initialization
	public virtual void Start () {

	}
	
	// Update is called once per frame
	public virtual void Update () {

		// Someone is touching
		if (Input.GetMouseButton(0) ||
		    Input.touchCount > 0)
		{
			
			Ray mouse_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit raycast_hit;

			bool isPressingOnSomething = Physics.Raycast(mouse_ray, out raycast_hit, Mathf.Infinity, Layers.MenuButton) ||
				Input.touchCount > 0 && Physics.Raycast(Camera.main.ScreenPointToRay(Input.touches[0].position), out raycast_hit, Mathf.Infinity, Layers.MenuButton);
	
			// First touch
			if (!pressed)
			{
				pressed = true;

				if (isPressingOnSomething) {

					GameObject newTarget = raycast_hit.collider.gameObject;
					OnPress(newTarget);
					target = newTarget;
				}
				
			}
			// Hold touch
			else {

				if (isPressingOnSomething) {
					GameObject newTarget = raycast_hit.collider.gameObject;

					// Change target
					if (target != newTarget) {

						if (target != null) {
							OnLeave(target);
						}

						target = newTarget;
						OnPress(newTarget);
					}
				}
				else if (target != null) {
					OnLeave(target);
					target = null;
				}

			}
			
		}
		// Someone stop touching
		else if (Input.GetMouseButtonUp(0) ||
		         (Input.touchCount <= 0 && pressed)) {

			pressed = false;

			if (target != null) {
				OnSelect(target);
			}

		}
	}

	protected abstract void OnPress(GameObject target);

	protected abstract void OnLeave(GameObject target);

	protected abstract void OnSelect(GameObject target);

}
