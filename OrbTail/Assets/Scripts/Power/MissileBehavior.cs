using UnityEngine;
using System.Collections;

public class MissileBehavior : MonoBehaviour {


    public GameObject Target { get; set; }
    public GameObject Owner { get; set; }
    private const float maxMissileSteering = 3.0f;
    private const float maxMissileSpeed = 8.0f;
    private const float explosionForce = 100.0f;
    private const float timeToLive = 2.5f;


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
        
        forwardProjected = Vector3.Lerp(forwardProjected, this.transform.forward, Time.deltaTime * 5.0f);

        this.GetComponent<Rigidbody>().AddForce(forwardProjected * maxMissileSpeed, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision collision)
    {

		if(NetworkHelper.IsServerSide())
        {

            if ((collision.gameObject.tag == Tags.Ship))
            {

                OnImpact(collision.gameObject);

                collision.gameObject.GetComponent<TailController>().GetDetacherDriverStack().GetHead().DetachOrbs(int.MaxValue, collision.gameObject.GetComponent<Tail>());

            }

        }

    }

    private IEnumerator DestroyMissileTTL()
    {
        yield return new WaitForSeconds(timeToLive);
        RPCDestroyMissile();

        if (Network.isServer)
        {
            networkView.RPC("RPCDestroyMissile", RPCMode.Others);
        }

    }

    private void OnImpact(GameObject target)
    {

        
		StartCoroutine("DestroyMissile");

        target.rigidbody.AddForce(transform.forward * explosionForce, ForceMode.Impulse);

        if (Network.isServer)
        {

            networkView.RPC("RPCOnImpact", RPCMode.Others, target.networkView.viewID);

        }

    }


    [RPC]
    private void RPCOnImpact(NetworkViewID target_id)
    {

        OnImpact(NetworkView.Find(target_id).gameObject);

    }

    [RPC]
    private void RPCDestroyMissile()
    {
        StartCoroutine("DestroyMissile");
    }

    private IEnumerator DestroyMissile()
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
