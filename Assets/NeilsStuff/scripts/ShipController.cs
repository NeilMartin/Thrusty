using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour 
{
	public Rigidbody spinnerRigidbody;
	public GameObject leftSupport;
	public GameObject rightSupport;
    
    public float forwardAcc = 10f;
	public float topSpeed = 10f;
	public float turnPower = 100f;
	public float turnsPerSecond = 0.5f;
	public float thrustFuelPerSecond = 2.0f;
	public float shieldFuelPerSecond = 5.0f;
 	public GameObject projectile;
	public float reloadTime = 0.1f;
	
	private ProjectileLauncher mLauncher;
	private float mSecondsSinceLaunch;
	private bool mbIsGrounded = true;
	private float mSpinPower = 0.0f;
	private float mFuel = 1000.0f;
	
	public bool IsGrounded()
	{
		return mbIsGrounded;
	}
	
	public float GetSpinPower()
	{
		return mSpinPower;
	}

	public float GetFuelRemaining()
	{
		return mFuel;
	}
	
	// Use this for initialization
	void Start () 
	{
		rigidbody.centerOfMass.Set( 0.0f, 0.0f, 0.0f );
		mLauncher = gameObject.GetComponent<ProjectileLauncher>();
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
		//float scalerFix = 60.0f * Time.deltaTime;
		float scalerFix = 1.0f;
		mbIsGrounded = CalcIfGrounded();
		if( mbIsGrounded )
		{
			// ground controls
		}
		else
		{
			// air turn controls
			float spinRange = -Mathf.Clamp(Input.GetAxis("Horizontal"), -1.0f, 1.0f);
			float desiredSpin = spinRange * (2.0f * Mathf.PI * turnsPerSecond);
			float currentSpin = spinnerRigidbody.angularVelocity.z;
			float deltaSpin = desiredSpin - currentSpin;
		
			mSpinPower = deltaSpin * turnPower;
			spinnerRigidbody.AddTorque( new Vector3( 0.0f, 0.0f, mSpinPower * scalerFix ) );
		}
		
		// thrust controls
		float thrust = Mathf.Clamp(Input.GetAxis("Vertical"), 0.0f, 1.0f);
		mFuel -= thrust * thrustFuelPerSecond * Time.fixedDeltaTime;		
		Vector3 dir = transform.up;
		float fCurrSpeed = Vector3.Dot(rigidbody.velocity, dir);
		float powerScale = (topSpeed-fCurrSpeed)/topSpeed;
		spinnerRigidbody.AddForce( dir * thrust * forwardAcc * scalerFix * powerScale );
		
		// shield controls
		bool bShield = Input.GetAxis("Vertical") < -0.1f;
		if( bShield )
		{
			mFuel -= shieldFuelPerSecond * Time.fixedDeltaTime;		
		}

		// shoot controls
		mSecondsSinceLaunch += Time.fixedDeltaTime;
		if((Input.GetKey(KeyCode.Space)) && (mSecondsSinceLaunch>reloadTime) && (false==bShield))
		{
			mSecondsSinceLaunch = 0.0f;
			if( null != mLauncher )
			{
				mLauncher.Shoot(projectile);
			}
			else
			{
				Debug.Log("trying to shoot on an object that doesn't have a launcher");
			}
		}
    }
}

