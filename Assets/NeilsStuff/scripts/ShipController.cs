using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour 
{
	public Rigidbody spinnerRigidbody;
	public GameObject leftSupport;
	public GameObject rightSupport;
    
    public float forwardAcc = 10f;
	public float turnPower = 100f;
	public float turnsPerSecond = 0.5f;
 
	private bool mbIsGrounded = true;
	private float mSpinPower = 0.0f;
	
	public bool IsGrounded()
	{
		return mbIsGrounded;
	}
	
	public float GetSpinPower()
	{
		return mSpinPower;
	}
	
	// Use this for initialization
	void Start () 
	{
		rigidbody.centerOfMass.Set( 0.0f, 0.0f, 0.0f );
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	float CalcHeightAboveGround( GameObject obj, float length, float radius )
	{
		Vector3 pos = obj.transform.position;
		Vector3 downDir = new Vector3(0.0f,-1.0f,0.0f);
		RaycastHit hit = new RaycastHit();
		if( Physics.Raycast(pos, downDir, out hit, length + radius) )
		{
			length = hit.distance - radius;
		}
		return length;
	}
	
	bool CalcIfGrounded()
	{
		bool bIsGrounded = false;
		if((null != leftSupport) && (null != rightSupport))
		{
			float supportRadius = 0.0f;
			float checkLength = 10.0f;
			float leftDist = CalcHeightAboveGround( leftSupport, checkLength, supportRadius );
			float rightDist = CalcHeightAboveGround( rightSupport, checkLength, supportRadius );
			if( leftDist + rightDist < 1.0f )
			{
				bIsGrounded = true;
			}
		}
		return bIsGrounded;
	}
	
	void FixedUpdate()
	{
		mbIsGrounded = CalcIfGrounded();
		if( mbIsGrounded )
		{
			// ground controls
		}
		else
		{
			// air controls
			float spinRange = -Mathf.Clamp(Input.GetAxis("Horizontal"), -1.0f, 1.0f);
			float desiredSpin = spinRange * (2.0f * Mathf.PI * turnsPerSecond);
			float currentSpin = spinnerRigidbody.angularVelocity.z;
			float deltaSpin = desiredSpin - currentSpin;
		
			mSpinPower = deltaSpin * turnPower;
			spinnerRigidbody.AddTorque( new Vector3( 0.0f, 0.0f, mSpinPower ) );
		}
    }
}

