using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TailController : MonoBehaviour {

	private DriverStack<IAttacherDriver> attacherDriverStack;
	private DriverStack<IDetacherDriver> detacherDriverStack;
	private DriverStack<IOffenceDriver> offenceDriverStack;
	private DriverStack<IDefenceDriver> defenceDriverStack;

	public Tail Tail { get; set;}


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


	void Start () {
		attacherDriverStack = new DriverStack<IAttacherDriver>();
		detacherDriverStack = new DriverStack<IDetacherDriver>();
		offenceDriverStack = new DriverStack<IOffenceDriver>();
		defenceDriverStack = new DriverStack<IDefenceDriver>();

		Tail = new Tail(this.gameObject);

		// TODO: to remove injection of stacks
		attacherDriverStack.Push(new DefaultAttacherDriver());
		detacherDriverStack.Push(new DefaultDetacherDriver());
	}

	void OnCollisionEnter(Collision collision) {
		GameObject collidedObj = collision.gameObject;

		if (collidedObj.tag == Tags.Orb) {
			OrbController orbController = collidedObj.GetComponent<OrbController>();

			if (!orbController.IsAttached()) {
				attacherDriverStack.GetHead().AttachOrbs(collidedObj, Tail);
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
