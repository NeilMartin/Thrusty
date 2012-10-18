using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour 
{
	public GameObject target;
	public float lookAheadScale = 0.2f;
	public float camLag = 0.5f; // 1.0 = no lag. 0.0 = infinite lag
	
	private Vector3 mTargetOffset;
		
	// Use this for initialization
	void Start () 
	{
		mTargetOffset = transform.position - target.transform.position;
	}
	
	void FixedUpdate () 
	{
		Vector3 desiredPos = target.transform.position + mTargetOffset;
		
		if( lookAheadScale > 0.0f )
		{
			if( null != target.rigidbody )			
			{
				desiredPos += target.rigidbody.velocity * lookAheadScale;
			}
		}
		
		transform.position += (desiredPos - transform.position)*camLag;
	}
}
