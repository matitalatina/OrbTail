using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TailController : MonoBehaviour {

	private DriverStack<IAttacherDriver> attacherDriverStack;
	private DriverStack<IDetacherDriver> detacherDriverStack;
	private DriverStack<IOffenceDriver> offenceDriverStack;
	private DriverStack<IDefenceDriver> defenceDriverStack;

	public Tail Tail { get; set;}

	private float dotProductAttackThreshold = 0.2f;

	public DriverStack<IAttacherDriver> GetAttacherDriverStack() {
		return attacherDriverStack;
	}

	public DriverStack<IDetacherDriver> GetDetacherDriverStack() {
		return detacherDriverStack;
	}

	public DriverStack<IOffenceDriver> GetOffenceDriverStack() {
		return offenceDriverStack;
	}

	public DriverStack<IDefenceDriver> GetDefenceDriverStack() {
		return defenceDriverStack;
	}

	void Awake() {
		attacherDriverStack = new DriverStack<IAttacherDriver>();
		detacherDriverStack = new DriverStack<IDetacherDriver>();
		offenceDriverStack = new DriverStack<IOffenceDriver>();
		defenceDriverStack = new DriverStack<IDefenceDriver>();
		
		Tail = new Tail(this.gameObject);
	}

	void Start () {

	}

	void OnCollisionEnter(Collision collision) {
		GameObject collidedObj = collision.gameObject;

		if (collidedObj.tag == Tags.Orb) {

			OrbController orbController = collidedObj.GetComponent<OrbController>();

			if (!orbController.IsAttached()) {
				attacherDriverStack.GetHead().AttachOrbs(collidedObj, Tail);
			}

		}
		else if (collidedObj.tag == Tags.Ship) {

			if (IsAttack(collidedObj)) {
				float damage = collidedObj.GetComponent<TailController>().GetOffenceDriverStack().GetHead().GetDamage(this.gameObject, collision);
				int nOrbsToDetach = defenceDriverStack.GetHead().DamageToOrbs(damage);
				detacherDriverStack.GetHead().DetachOrbs(nOrbsToDetach, this.Tail);
			}

		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	private bool IsAttack(GameObject attacker) {
		Vector3 relVector = this.transform.position - attacker.transform.position;
		float dotProduct = Vector3.Dot(attacker.transform.forward, relVector.normalized);
		return dotProduct >= dotProductAttackThreshold;
	}
}
