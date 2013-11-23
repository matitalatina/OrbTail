using UnityEngine;
using System.Collections;

public interface IApproachListener {

	void ApproachedTo(GameObject destination, GameObject caller);
}
