using UnityEngine;
using System.Collections;

public class GUIMainMenu : MonoBehaviour {
	
	private GameObject multiplayer_button;
	private GameObject single_player_button;
	private GameObject master;
	
	// Use this for initialization
	void Start () {
		master = GameObject.FindGameObjectWithTag(Tags.Master);

		multiplayer_button = GameObject.Find("MultiPlayerButton");
		
		single_player_button = GameObject.Find("SinglePlayerButton");

		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetMouseButtonUp(0) ||
		   Input.touchCount > 0)
		{
			
			Ray mouse_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit raycast_hit;
			
			if (Physics.Raycast(mouse_ray, out raycast_hit) ||
			    Input.touchCount > 0 &&
			    Physics.Raycast(Camera.main.ScreenPointToRay(Input.touches[0].position), out raycast_hit))
			{
				
				//The touch or the mouse collided with something
				
				if (raycast_hit.collider.gameObject == single_player_button)
				{
					StartSinglePlayer();
					
				}
				else if (raycast_hit.collider.gameObject == multiplayer_button)
				{
					StartMultiPlayer();
					
				}
				
			}
			
		}
		
	}
	

	private void StartSinglePlayer() {
		
		var builder = master.GetComponent<GameBuilder>();
		builder.Action = GameBuilder.BuildMode.SinglePlayer;
		
        Application.LoadLevel("MenuChooseShip");
		
	}
	
	private void StartMultiPlayer()
	{
		
		Application.LoadLevel("MenuServerClient");
		
	}


}
