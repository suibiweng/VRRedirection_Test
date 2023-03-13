using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * A simple implementation of animated power lines in Unity.
 * This script generates a certain number of segments between two empty GameObjects (start, end)
 * and animates the segments if desired.
 * See this for details: https://nerdhut.de/?p=11336
 * You are free to use this code in your personal and educational projects. Please do not use
 * it commercially. Do not redistribute the code.
 * If you want to use it commercially, please contact: https://nerdhut.de/contact/
 * 
 **/

public class DynamicWire : MonoBehaviour
{
	[Header("Functional Settings")]
	// The start and the end point between which the segments will be generated
	// The segments are generated from the start along the GameObject's forward axis
	public GameObject start;
	public GameObject end;
	public Material segmentMaterial;
	[Space(10)]

	// When simplified is true, cubes will be used for the segments
	// Otherwise, the script generates cylinders
	public bool simplified = false;

	// Desired number of segments between start and end
	// More segments = More detail and nicer curve
	// However, this also increases the overall weight of the wire and it will
	// be more saggy. You can counter this effect by increasing the restingforce.
	public int segments = 16;

	// Diameter of the wire segments
	public float diameter = 0.1f;

	// The mass of each wire segment
	public float segmentMass = 0.1f;

	[Space(10)]
	[Header("Visual Settings")]

	public float damper = 1.0f;
	public float drag = 10f;
	public float spring = 0.25f;
	public float restingForce = 1.45f;

	[Space(10)]
	[Header("Animation settings")]
	// Turn the animation on and off
	public bool animate = false;

	// Number of segments that will be animated actively. See video for details.
	public int animatedSegments = 4;

	// Delay between the generation of the segments and the start of the animation
	// See the video for details
	public float animationStartDelay = 0.55f;

	// The wire swings between minforce and maxforce and the current force is increased
	// by the value stored in increase every time the co-routine is called
	public float minForce = 1.2f;
	public float maxForce = 1.75f;
	public float increase = 0.1f;

	// The time between the co-routine calls
	public float increaseWaitTime = 0.1f;

	// The wire will stay in its final position for a certain time that is between minHoldTime and maxHoldTime
	public float minHoldTime = 0.25f;
	public float maxHoldTime = 0.5f;

	// Private fields
	private bool forward = true;
	private bool reverseDirectionRequested = false;
	private float distance;
	private float segmentLength;
	private float currentForce = 0f;

	private List<DynamicWireSegment> wireSegments = new List<DynamicWireSegment>();

	void Start ()
	{
		// Note the video for implementation details

		GameObject previous = null;

		distance = Vector3.Distance(start.transform.position, end.transform.position);
		segmentLength = (distance / segments) / (simplified ? 1.0f : 2.0f);

		for (int i = 0; i < segments; i++)
		{
			GameObject n = GameObject.CreatePrimitive(simplified ? PrimitiveType.Cube : PrimitiveType.Cylinder);
			Renderer r = n.GetComponent<Renderer>();

			n.transform.localScale = new Vector3(diameter, segmentLength, diameter);
			n.transform.position = start.transform.position + (start.transform.forward * (segmentLength)) / (simplified ? 2.0f : 1.0f) + (start.transform.forward * (segmentLength * i * (simplified ? 1.0f : 2.0f)));
			n.transform.Rotate(new Vector3(270f, 0f, 0f));

			r.material = segmentMaterial;

			n.transform.parent = this.transform;
			n.name = "WireSegment_" + (i + 1);

			Rigidbody rb = n.AddComponent(typeof(Rigidbody)) as Rigidbody;
			HingeJoint conn = n.AddComponent(typeof(HingeJoint)) as HingeJoint;
			DynamicWireSegment dws = n.AddComponent(typeof(DynamicWireSegment)) as DynamicWireSegment;
			ConstantForce cf = n.AddComponent(typeof(ConstantForce)) as ConstantForce;
			JointSpring s = new JointSpring();

			s.damper = damper;
			s.spring = spring;
			rb.mass = (i % 2 != 0) ? segmentMass : segmentMass * 2f;
			rb.drag = rb.angularDrag = drag;
			conn.useSpring = true;
			conn.spring = s;

			dws.restingForce = restingForce;
			dws.rb = rb;
			dws.cf = cf;

			if (i == 0)
				conn.connectedBody = start.GetComponent<Rigidbody>();
			else
				conn.connectedBody = previous.GetComponent<Rigidbody>();

			previous = n;
			wireSegments.Add(dws);
		}

		end.GetComponent<HingeJoint>().connectedBody = previous.GetComponent<Rigidbody>();

		Invoke("startAnimation", animationStartDelay);
	}

	private void startAnimation()
	{
		StartCoroutine(doAnimation());
	}

	private void reverseDirection()
	{
		forward = !forward;
		reverseDirectionRequested = false;
	}

	private IEnumerator doAnimation()
	{
		while(true)
		{
			// Warning: The Code omits all error handling

			int centerSegmentIndex = segments / 2;

			if (forward)
			{
				if (currentForce < maxForce)
				{
					currentForce += increase;
				}
				else if (!reverseDirectionRequested)
				{
					Invoke("reverseDirection", Random.Range(minHoldTime, maxHoldTime));
					reverseDirectionRequested = true;
				}
			}
			else
			{
				if (currentForce > minForce)
				{
					currentForce -= increase;
				}
				else if (!reverseDirectionRequested)
				{
					Invoke("reverseDirection", Random.Range(minHoldTime, maxHoldTime));
					reverseDirectionRequested = true;
				}
			}

			for (int i = 0; i < animatedSegments; i++)
			{
				int selected = (centerSegmentIndex - animatedSegments / 2) + i;

				wireSegments[selected].rb.AddForce(new Vector3(currentForce, 0f, 0f));
			}

			yield return new WaitForSeconds(animate ? increaseWaitTime : 2.0f);
		}
	}
}