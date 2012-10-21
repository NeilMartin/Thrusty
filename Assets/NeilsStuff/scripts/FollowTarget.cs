using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour 
{
	public GameObject target;
	public float lookAheadScale = 0.2f;
	public float camLag = 0.5f; // 1.0 = no lag. 0.0 = infinite lag
	
	private Vector3 mTargetOffset;
	private float mSpeed;
	private Vector3 mCamOffset;
	
		
	// Use this for initialization
	void Start () 
	{
		mTargetOffset = transform.position - target.transform.position;
	}
	
	void FixedUpdate()
	{
		if( target == null )
		{
			target = GameObject.FindWithTag("Player");
			if( target != null )
			{
				mTargetOffset = transform.position - target.transform.position;
			}
		}
		if( target != null )
		{
			Vector3 desiredPos = target.transform.position + mTargetOffset;
			if( lookAheadScale > 0.0f )
			{
				if( null != target.rigidbody )			
				{
					//mSpeed = target.rigidbody.velocity.magnitude;
					//Vector3 vOffset = target.rigidbody.velocity;
					//vOffset.Normalize();
					desiredPos +=  target.rigidbody.velocity * lookAheadScale;
				}
			}
			desiredPos = transform.position + ((desiredPos - transform.position)*camLag);
			mCamOffset = desiredPos - target.transform.position;
		}
	}
	
	void LateUpdate () 
	{
		if( target != null )
		{
			transform.position = target.transform.position + mCamOffset;
		}
	}
}
