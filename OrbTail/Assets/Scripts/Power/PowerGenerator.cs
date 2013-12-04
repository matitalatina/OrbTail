using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PowerGenerator : MonoBehaviour {

    public float delta_generation = 1.0f;

    private IList<GameObject> orbs;

    private IEnumerable<GameObject> DetachedOrbs
    {
        get
        {
            return orbs.Where((GameObject orb) =>
            {
                return !orb.GetComponent<OrbController>().IsAttached() &&
                        orb.GetComponent<RandomPowerAttacher>() == null;
            });
        }
    }

    private System.Random rng = new System.Random();

    private float time_accumulator = 0.0f;

	// Use this for initialization
	void Start () {

        orbs = new List<GameObject>(GameObject.FindGameObjectsWithTag(Tags.Orb));

	}
	
	// Update is called once per frame
	void Update () {

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

                    Debug.Log("Spawning some powerup");
                    orb.AddComponent<RandomPowerAttacher>();
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
