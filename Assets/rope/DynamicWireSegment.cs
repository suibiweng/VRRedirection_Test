using UnityEngine;

public class DynamicWireSegment : MonoBehaviour
{
	public float restingForce;

	public Rigidbody rb;
	public ConstantForce cf;

	void Start ()
	{
		Invoke("applyRestingForce", 0.5f);
	}
	
	private void applyRestingForce()
	{
		cf.force = new Vector3(0f, restingForce, 0f);
	}
}