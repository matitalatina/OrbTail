using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDetacherDriver {

	List<GameObject> DetachOrbs(int nOrbs, Tail tail);
	void Update();

}
