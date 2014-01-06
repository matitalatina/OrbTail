using UnityEngine;
using System.Collections;

public class GUIChooseArena : MonoBehaviour {
	private GameBuilder builder;

	// Use this for initialization
	void Start () {
		builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonUp(0) ||
		    Input.touchCount > 0)
		{
			
			Ray mouse_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit raycast_hit;
			
			if (Physics.Raycast(mouse_ray, out raycast_hit) ||
			    Input.touchCount > 0 &&
			    Physics.Raycast(Camera.main.ScreenPointToRay(Input.touches[0].position), out raycast_hit))
			{
				
				//The touch or the mouse collided with something
				if (raycast_hit.collider.tag.Equals(Tags.ArenaSelector))
				{

					builder.ArenaName = raycast_hit.collider.name;
					Debug.Log(raycast_hit.collider.name);

					Application.LoadLevel("MenuChooseShip");
					
				}
				
			}
			
		}
	}
}
