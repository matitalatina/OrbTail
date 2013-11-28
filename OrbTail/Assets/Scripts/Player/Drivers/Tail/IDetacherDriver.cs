using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDetacherDriver {

	/// <summary>
	/// Detachs the orbs.
	/// </summary>
	/// <returns>The orbs detached. The numbers of the orb must be less or equal than
	/// the numbers given in the parameter.</returns>
	/// <param name="nOrbs">Number of the orbs to detach.</param>
	/// <param name="tail">The tail which we detach orbs from.</param>
	List<GameObject> DetachOrbs(int nOrbs, Tail tail);


	void Update();

}
