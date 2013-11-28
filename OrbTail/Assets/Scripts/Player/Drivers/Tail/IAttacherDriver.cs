using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IAttacherDriver {

	/// <summary>
	/// Attachs the orbs to the given tail
	/// </summary>
	/// <param name="orb">The orb to attach.</param>
	/// <param name="tail">The tail which receives the orb to attach.</param>
	void AttachOrbs(GameObject orb, Tail tail);


	/// <summary>
	/// Attachs the orbs to the given tail.
	/// </summary>
	/// <param name="orbs">The list of orbs to attach.</param>
	/// <param name="tail">The tail which receives the orbs to attach.</param>
	void AttachOrbs(List<GameObject> orbs, Tail tail);


	void Update();
}
