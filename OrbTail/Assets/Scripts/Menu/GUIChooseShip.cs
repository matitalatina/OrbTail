using UnityEngine;
using System.Collections;

public class GUIChooseShip : GUIMenuChoose {

    private GameObject master;
    private GameObject[] selectors;

	// Use this for initialization
	public override void Start () {
	
		base.Start();
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
	

	protected override void OnSelect (GameObject target)
	{

		if (target.tag == Tags.ShipSelector)
		{
			
			var chosen_identity = target.GetComponent<PlayerIdentity>();
			
			//A ship has been chosen
			var identity = master.AddComponent<PlayerIdentity>();
			
			chosen_identity.CopyTo(identity);
			
			identity.IsHuman = true;
			
			BuildGame();
			
		}
		else if (target.tag == Tags.BackButton) {
			Application.LoadLevel("MenuChooseArena");
		}
	}
	
}
