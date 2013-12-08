using UnityEngine;
using System.Collections;

public class MissleFollower : MonoBehaviour {

    public GameObject Target { get; set; }
    public GameObject Owner { get; set; }
    private const float maxMissleSteering = 3.0f;
    private const float maxMissleSpeed = 15.0f;

	void Start () {

	}
	
	void Update () {
        if (Target != null)
        {
            Vector3 direction = Target.transform.position - this.transform.position;
            direction.Normalize();

            Vector3 new_forward  = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime * maxMissleSteering, 0);
            new_forward.Normalize();

            FloatingObject floating = GetComponent<FloatingObject>();

            float dot = Mathf.Clamp01(Vector3.Dot( direction, new_forward) );
                        
            Debug.Log(dot);

            this.transform.rotation = Quaternion.LookRotation(new_forward,
                                                              -floating.ArenaDown);

            Vector3 forwardProjected = Vector3.Cross(floating.ArenaDown,
                                                     Vector3.Cross(-floating.ArenaDown, this.transform.forward)
                                                     ).normalized;

            this.GetComponent<Rigidbody>().AddForce(forwardProjected * maxMissleSpeed, ForceMode.VelocityChange);
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == Tags.Ship) && (collision.gameObject != Owner))
        {
            Debug.Log("BUM HEADSHOT!");

            Destroy(this.gameObject);
            collision.gameObject.GetComponent<TailController>().GetDetacherDriverStack().GetHead().DetachOrbs(int.MaxValue, collision.gameObject.GetComponent<Tail>());
        }
    }
}
