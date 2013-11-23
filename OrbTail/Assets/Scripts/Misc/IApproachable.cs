using UnityEngine;
using System.Collections;

public interface IApproachable {

	void ApproachTo(GameObject destination, IApproachListener listener);
	bool IsApproaching();
	void InterruptApproaching();
}
