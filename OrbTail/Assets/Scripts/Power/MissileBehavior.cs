using UnityEngine;
using System.Collections;

public class MissileBehavior : MonoBehaviour {


    public GameObject Target { get; set; }
    public GameObject Owner { get; set; }
    public const string explosion_prefab_path = "Prefabs/Power/Explosion";
    private const float maxMissileSteering = 6.0f;
    private const float maxMissileSpeed = 8.0f;
    private const float explosionForce = 100.0f;
    private const float timeToLive = 2.5f;
	private const float smoothCurve = 10f;
	private AudioClip explosionSound;
    private bool destroying = false;

    public void SetTarget(GameObject target, GameObject owner)
    {

        Target = target;
        Owner = owner;

        if (Network.peerType != NetworkPeerType.Disconnected &&
            networkView.isMine)
        {

            networkView.RPC("RPCSetTarget", RPCMode.Others, target.networkView.viewID, owner.networkView.viewID);

        }

    }

    [RPC]
    private void RPCSetTarget(NetworkViewID target_id, NetworkViewID owner_id)
    {

        SetTarget(NetworkView.Find(target_id).gameObject,
                  NetworkView.Find(owner_id).gameObject);

    }

    void Start()
    {

		if(NetworkHelper.IsOwnerSide(networkView))
		{
			StartCoroutine("DestroyMissileTTL");
		}

		explosionSound = Resources.Load<AudioClip>("Sounds/Powers/Explosion");

    }

    void Update()
    {

        // Movements
        FloatingObject floating = GetComponent<FloatingObject>();
        if (Target != null)
        {
            Vector3 direction = Target.transform.position - this.transform.position;
            direction.Normalize();
            Vector3 new_forward = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime * maxMissileSteering, 0);
            new_forward.Normalize();
            
            this.transform.rotation = Quaternion.LookRotation(new_forward, -floating.ArenaDown);
        }
        Vector3 forwardProjected = Vector3.Cross(floating.ArenaDown,
                                                    Vector3.Cross(-floating.ArenaDown, this.transform.forward)
                                                    ).normalized;
        
		forwardProjected = Vector3.Lerp(this.transform.forward, forwardProjected, Time.deltaTime * smoothCurve);

        this.GetComponent<Rigidbody>().AddForce(forwardProjected * maxMissileSpeed, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision collision)
    {

		if(NetworkHelper.IsServerSide())
        {

            if (collision.gameObject.tag == Tags.Ship)
            {

                if (NetworkHelper.IsOwnerSide(networkView))
                {

                    OnImpact(collision.gameObject);

                }
                else
                {

                    networkView.RPC("RPCOnImpact", networkView.owner, collision.gameObject.networkView.viewID);

                }

                Debug.Log("Removing all orbs");
                collision.gameObject.GetComponent<TailController>().GetDetacherDriverStack().GetHead().DetachOrbs(int.MaxValue, collision.gameObject.GetComponent<Tail>());

            }

        }

    }

    private IEnumerator DestroyMissileTTL()
    {
        
        //Delayed destrution
        yield return new WaitForSeconds(timeToLive);

        RPCDestroyMissile();

    }

    private void OnImpact(GameObject target)
    {

        target.rigidbody.AddForce(transform.forward * explosionForce, ForceMode.Impulse);

        if (NetworkHelper.IsOwnerSide(networkView))
        {

            RPCDestroyMissile();

        }

    }

    [RPC]
    private void RPCOnImpact(NetworkViewID target_id)
    {

        //Apply the force
        OnImpact(NetworkView.Find(target_id).gameObject);

        //The owner will propagate the impact and request the missile destruction

        if (networkView.isMine &&
            NetworkHelper.IsConnected())
        {

            networkView.RPC("RPCOnImpact", RPCMode.Others, target_id);

        }

    }

    [RPC]
    private void RPCDestroyMissile()
    {

        if (!destroying)
        {

            destroying = true;

            StartCoroutine("DestroyMissile");

            //The owner will destroy all others copies
            if (networkView.isMine &&
                NetworkHelper.IsConnected())
            {

                networkView.RPC("RPCDestroyMissile", RPCMode.Others);

            }

        }
       
    }

    private IEnumerator DestroyMissile()
    {
        var explosion_resource = Resources.Load(explosion_prefab_path);
        GameObject explosion = GameObject.Instantiate(explosion_resource, this.gameObject.transform.position, Quaternion.identity) as GameObject;
        
		AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        Target = null;
        collider.enabled = false;
        particleSystem.enableEmission = false;
        GetComponent<MeshFilter>().mesh = null;

        // Delayed for GFX
        yield return new WaitForSeconds(1.0f);

        //TODO: Fix this SH!T
        Destroy(explosion);
        Destroy(this.gameObject);
        
    }

}
