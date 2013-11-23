using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IAttachDriver {
	void AttachOrbs(GameObject orb, Tail tail);
	void AttachOrbs(List<GameObject> orbs, Tail tail);
	void Update();
}
