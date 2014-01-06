using UnityEngine;
using System.Collections;

public class GravityField : MonoBehaviour {

    /// <summary>
    /// Indicates the shape for the gravity field
    /// </summary>
    public enum GravityShape
    {

        Flat,
        Spherical,
        InverseSpherical,
        TripleSpherical

    }

    /// <summary>
    /// The shape of the field
    /// </summary>
    public GravityShape shape = GravityShape.Flat;

    /// <summary>
    /// The field used to impress the gravity
    /// </summary>
    private IGravityField Field { get; set; }

    void Awake()
    {

        switch (shape)
        {

            case GravityShape.Flat:

                Field = new FlatGravityField();
                break;

            case GravityShape.Spherical:

                Field = new SphericalGravityField(Vector3.zero);
                break;

            case GravityShape.InverseSpherical:

                Field = new InverseSphericalGravityField(Vector3.zero);
                break;

            case GravityShape.TripleSpherical:

                Field = new TripleSphericalGravityField(Vector3.zero);
                break;

            default:

                break;
        }

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetGravity(FloatingObject floatie)
    {

        Field.SetGravity(floatie);

    }

}
