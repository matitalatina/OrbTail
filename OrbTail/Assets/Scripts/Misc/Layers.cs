using UnityEngine;
using System.Collections;

public class Layers {

	public const int Field = 1 << 8;
	public const int Obstacles = 1 << 9;

	public const int FieldAndObstacles = Field | Obstacles;

	public const int MenuButton = 1 << 10;

}
