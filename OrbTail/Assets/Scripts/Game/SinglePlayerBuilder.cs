using UnityEngine;
using System.Collections;
using System.Linq;

public class SinglePlayerBuilder : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // A new level has been loaded
    void OnLevelWasLoaded(int level)
    {

        //Gathers the players' identities
        var identities = GetComponents<PlayerIdentity>().Take(GameBuilder.kMaxPlayerCount);

        //Create the ships at the spawn points
        int player_id = 0;

        GameObject[] spawn_points = GameObject.FindGameObjectsWithTag(Tags.SpawnPoint);

        var factory = GameObjectFactory.Instance;
        GameObject player;

        foreach (PlayerIdentity identity in identities)
        {

            player = factory.Instantiate("Prefabs/Ships/" + identity.ShipName, 
                                         spawn_points[player_id].transform.position, 
                                         Quaternion.identity) as GameObject;

            identity.CopyTo(player.GetComponent<PlayerIdentity>());

            player.GetComponent<GameIdentity>().Id = player_id;

            if (!identity.IsHuman)
            {

                //That's an AI!
                player.AddComponent<PlayerAI>();

            }
            else
            {
				player.AddComponent<AudioListener>();
                GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<CameraMovement>().LookAt(player);
                GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>().ActivePlayer = player;
            }

            ++player_id;

            Destroy(identity);

        }

    }

}
