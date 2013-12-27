using UnityEngine;
using System.Collections;

public class GUIChooseShip : MonoBehaviour {

    private GameObject master;
    private GameObject[] selectors;

	// Use this for initialization
	void Start () {
	
        master = GameObject.FindGameObjectWithTag(Tags.Master);
        selectors = GameObject.FindGameObjectsWithTag(Tags.ShipSelector);


	}

    private void BuildGame()
    {

        var builder = master.GetComponent<GameBuilder>();

        //Generate 3 more players in single player
        if (builder.Action == GameBuilder.BuildMode.SinglePlayer)
        {

            PlayerIdentity identity;

            System.Random rng = new System.Random();

            var i = GameBuilder.kMaxPlayerCount - 1;

            while (i > 0)
            {

                //Select a random ships and clone the identity
                identity = master.AddComponent<PlayerIdentity>();

                selectors[rng.Next(selectors.Length)].GetComponent<PlayerIdentity>().CopyTo(identity);

                identity.IsHuman = false;

                i--;

            }

        }

        //Build the game
        master.GetComponent<GameBuilder>().BuildGame();

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
                if (raycast_hit.collider.tag.Equals(Tags.ShipSelector))
                {

                    var chosen_identity = raycast_hit.collider.GetComponent<PlayerIdentity>();

                    //A ship has been chosen
                    var identity = master.AddComponent<PlayerIdentity>();

                    chosen_identity.CopyTo(identity);

                    identity.IsHuman = true;

                    BuildGame();

                }

            }

        }
		
	}

}
