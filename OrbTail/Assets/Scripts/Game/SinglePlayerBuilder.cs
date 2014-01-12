using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SinglePlayerBuilder : MonoBehaviour {

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(gameObject);

        //Loads the proper arena
        Application.LoadLevel(GetComponent<GameBuilder>().ArenaName);

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

            //TODO: find a better way
            if (!identity.IsHuman)
            {

                player.AddComponent<PlayerAI>();

            }
            else
            {

                GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<CameraMovement>().LookAt(player);

            }

            ++player_id;

            //Destroy(identity);

        }

        //Loads the Game prefab and enables the Game component
        var game_obj = GameObjectFactory.Instance.Instantiate("Prefabs/Game", Vector3.zero, Quaternion.identity);

        var game = game_obj.GetComponent<Game>();

        game.GameMode = GameModes.Resolve(GetComponent<GameBuilder>().GameMode);
        game.enabled = true;

    }

}
