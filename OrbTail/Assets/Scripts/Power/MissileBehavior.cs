using UnityEngine;
using System.Collections;

public class MissileBehavior : MonoBehaviour {


    public GameObject Target { get; set; }
    public GameObject Owner { get; set; }
    private const float maxMissileSteering = 3.0f;
    private const float maxMissileSpeed = 15.0f;
    private const float explosionForce = 100.0f;
    private const float timeToLive = 10f;


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
        StartCoroutine("DestroyMissileTTL");
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
        Vector3 forwardProjected /*= Vector3.Cross(floating.ArenaDown,
                                                    Vector3.Cross(-floating.ArenaDown, this.transform.forward)
                                                    ).normalized*/;

        forwardProjected = this.transform.forward;

        this.GetComponent<Rigidbody>().AddForce(forwardProjected * maxMissileSpeed, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision collision)
    {

        if (Network.isServer ||
            Network.peerType == NetworkPeerType.Disconnected)
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
        StartCoroutine("DestroyMissile");
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
