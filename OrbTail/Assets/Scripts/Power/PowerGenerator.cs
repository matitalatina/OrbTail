using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PowerGenerator : MonoBehaviour {

    public float delta_generation = 1.0f;

    private IList<GameObject> orbs = null;

    private IEnumerable<GameObject> DetachedOrbs
    {
        get
        {

            if (orbs == null)
            {

                orbs = new List<GameObject>(GameObject.FindGameObjectsWithTag(Tags.Orb));

            }

            return orbs.Where((GameObject orb) =>
            {
                return !orb.GetComponent<OrbController>().IsAttached() &&
                        orb.GetComponent<RandomPowerAttacher>() == null;
            });
        }
    }

    private System.Random rng = new System.Random();

    private float time_accumulator = 0.0f;

	// Update is called once per frame
	void Update () {

        if (!Network.isClient)
        {

            time_accumulator += Time.deltaTime;

            if (time_accumulator >= delta_generation)
            {

                time_accumulator = 0.0f;

                var detached_orbs = DetachedOrbs;
                
                var index = rng.Next(0, detached_orbs.Count());

                foreach (GameObject orb in detached_orbs)
                {

                    if (index == 0)
                    {

                        SpawnPower(orb);
                        break;

                    }
                    else
                    {

                        --index;

                    }

                }

            }


        }
        
	}

    private void SpawnPower(GameObject orb)
    {

        orb.GetComponent<RandomPowerAttacher>().enabled = true;

        if (Network.isServer)
        {

            networkView.RPC("RPCSpawnPower", RPCMode.OthersBuffered, orb.networkView.viewID);

        }

    }

    [RPC]
    private void RPCSpawnPower(NetworkViewID orb_view_id)
    {

        SpawnPower(NetworkView.Find(orb_view_id).gameObject);

    }

}
