using UnityEngine;
using System.Collections;

public class MissleFollower : MonoBehaviour {

    public GameObject Target { get; set; }
    public GameObject Owner { get; set; }
    private const float maxMissleSteering = 3.0f;
    private const float maxMissleSpeed = 15.0f;
	private const float explosionForce = 100.0f;
	private const float timeToLive = 10f;

	void Start () {
        StartCoroutine("DestroyMissleTTL");
	}
	
	void Update () {

        // Movements
        FloatingObject floating = GetComponent<FloatingObject>();
        if (Target != null)
        {
            Vector3 direction = Target.transform.position - this.transform.position;
            direction.Normalize();
            Vector3 new_forward  = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime * maxMissleSteering, 0);
            new_forward.Normalize();
            float dot = Mathf.Clamp01(Vector3.Dot( direction, new_forward) );
            this.transform.rotation = Quaternion.LookRotation(new_forward, -floating.ArenaDown);
        }
        Vector3 forwardProjected = Vector3.Cross(floating.ArenaDown,
                                                    Vector3.Cross(-floating.ArenaDown, this.transform.forward)
                                                    ).normalized;
        this.GetComponent<Rigidbody>().AddForce(forwardProjected * maxMissleSpeed, ForceMode.VelocityChange);
	}

    void OnCollisionEnter(Collision collision)
    {
        if ( (collision.gameObject.tag == Tags.Ship) || (collision.gameObject.tag == Tags.Field) )
        {
            Debug.Log("BUM HEADSHOT! Hit: " + collision.gameObject);
            StartCoroutine("DestroyMissle");

            collision.gameObject.GetComponent<TailController>().GetDetacherDriverStack().GetHead().DetachOrbs(int.MaxValue, collision.gameObject.GetComponent<Tail>());
			collision.gameObject.rigidbody.AddForce(transform.forward * explosionForce, ForceMode.Impulse);

            
		}
    }

    private IEnumerator DestroyMissleTTL()
    {
        yield return new WaitForSeconds(timeToLive);
        Debug.Log("Missle destroyed without hitting nothing.");
        StartCoroutine("DestroyMissle");
    }

    private IEnumerator DestroyMissle()
    {
        var explosionRes = Resources.Load("Prefabs/Power/Explosion");
        GameObject explosion = GameObject.Instantiate(explosionRes, this.gameObject.transform.position, Quaternion.identity) as GameObject;

        Target = null;
        collider.enabled = false;
        particleSystem.enableEmission = false;
        GetComponent<MeshFilter>().mesh = null;

        // Delayed for GFX
        yield return new WaitForSeconds(1.0f);

        Destroy(explosion);
        Destroy(this.gameObject);
    }
}
