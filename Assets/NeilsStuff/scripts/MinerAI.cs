using UnityEngine;
using System.Collections;

public class MinerAI : MonoBehaviour 
{
	
	public float goToShipRadius = 5.0f;
	public float idleDriftDistance = 1.0f;
	public float speed = 1.0f;
	
	private GameObject mPlayer;
	private enum MinerState 
	{
		TeleportToCrystal,
		Idle,
		GoToCrystal,
		MineCrystal,
		GoToShip,
		EnterShip,
	}
	private MinerState mState;
	private MinerState mNextState;
	private Vector3 mTargetPos;
	private float mStateTimer;
	private bool mbStateChanged;
	private bool mbCollected;
	
	void Start () 
	{
		mPlayer = null;
		mState = MinerState.TeleportToCrystal;
		mNextState = MinerState.TeleportToCrystal;
		mTargetPos = CalcTargetForState( mState );
		mStateTimer = 0.0f;
		mbStateChanged = false;
	}
	
	public void SetCollected()
	{
		mbCollected = true;
	}
	
	private void SetNextState( MinerState nextState )
	{
		mbStateChanged = true;
		mNextState = nextState;
	}
	
	private MinerState GetNextState()
	{
		return mNextState;
	}
	
	private Vector3 CalcIdleTarget()
	{
		Vector3 targetPos = gameObject.transform.position;
		targetPos.x += Random.Range( -idleDriftDistance, idleDriftDistance );
		targetPos.y += Random.Range( -idleDriftDistance, idleDriftDistance );
		return targetPos;
	}

	private Vector3 CalcCrystalTarget( bool bFindClosest )
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag("MinerPointOfInterest");
		Vector3 targetPos = gameObject.transform.position;
		if( gos.Length > 0 )
		{
			targetPos = gos[0].transform.position;	
			if( gos.Length > 1 )
			{
				if( bFindClosest )
				{
					float fMinDistSqrd = (gameObject.transform.position - targetPos).sqrMagnitude;
					for( int i=1; i<gos.Length; ++i )
					{
						Vector3 testTargetPos = gos[i].transform.position;	
						float fTestDistSqrd = (gameObject.transform.position - testTargetPos).sqrMagnitude;	
						if( fTestDistSqrd < fMinDistSqrd )
						{
							targetPos = testTargetPos;
							fMinDistSqrd = fTestDistSqrd;
						}
					}
				}
				else
				{
					targetPos = gos[Random.Range(0,gos.Length-1)].transform.position;
				}
			}
		}
		return targetPos;
	}
	
	private Vector3 CalcTargetForState( MinerState state )
	{
		// default stay still
		Vector3 targetPos = gameObject.transform.position;
		switch(state)
		{
		case MinerState.Idle:
			targetPos = CalcIdleTarget();
			break;

		case MinerState.TeleportToCrystal:
			targetPos = CalcCrystalTarget(false);
			break;
			
		case MinerState.GoToCrystal:
			targetPos = CalcCrystalTarget(true);
			break;
			
		case MinerState.GoToShip:
			targetPos = mPlayer.transform.position;
			break;

		case MinerState.MineCrystal:
			targetPos = gameObject.transform.position;
			break;

		default:
			Debug.LogError("Unknown state");
			break;
		}
		return targetPos;
	}
	

	void FixedUpdate() 
	{
		if( null == mPlayer )
		{
			mPlayer = GameObject.FindWithTag("Player");
		}
		
		if( mbStateChanged )
		{
			mStateTimer = 0.0f;
			mState = mNextState;
			mbStateChanged = false;
		}
		else
		{
			mStateTimer += Time.fixedDeltaTime;
		}
		
		switch(mState)
		{
		case MinerState.TeleportToCrystal:
			{
				mTargetPos = CalcTargetForState(mState);
				transform.position = mTargetPos;
				SetNextState(MinerState.Idle);
				mTargetPos = CalcTargetForState(GetNextState());
			}
			break;
		case MinerState.Idle:
			{
				MoveTowardsTarget();
				if( (null != mPlayer) && IsInRange( mPlayer.transform.position, goToShipRadius ) )
				{
					SetNextState(MinerState.GoToShip);
					mTargetPos = CalcTargetForState(GetNextState());
				}
				else if( IsInRange(mTargetPos, 0.1f) )
				{
					SetNextState( (Random.Range(0,100) > 50) ? MinerState.Idle : MinerState.GoToCrystal );
					mTargetPos = CalcTargetForState(GetNextState());
				}
			}
			break;
			
		case MinerState.GoToCrystal:
			{
				MoveTowardsTarget();
				if( (null != mPlayer) && IsInRange( mPlayer.transform.position, goToShipRadius ) )
				{
					SetNextState( MinerState.GoToShip );
					mTargetPos = CalcTargetForState(GetNextState());
				}
				else if( IsInRange(mTargetPos, 0.1f) )
				{
					SetNextState( MinerState.MineCrystal );
				}
			}
			break;

		case MinerState.MineCrystal:
			{
				MoveTowardsTarget();
				if( (null != mPlayer) && IsInRange( mPlayer.transform.position, goToShipRadius ) )
				{
					SetNextState( MinerState.GoToShip );
					mTargetPos = CalcTargetForState(GetNextState());
				}
				else if( mStateTimer > 5.0f )
				{
					SetNextState( MinerState.Idle );
					mTargetPos = CalcTargetForState(GetNextState());
				}
			}
			break;

		case MinerState.GoToShip:
			{
				mTargetPos = CalcTargetForState(mState);
				MoveTowardsTarget();
				if( (null == mPlayer) || (false == IsInRange( mPlayer.transform.position, goToShipRadius ) ) )
				{
					SetNextState( MinerState.Idle );
					mTargetPos = CalcTargetForState(GetNextState());
				}
				else if( mbCollected )
				{
					SetNextState( MinerState.EnterShip );
				}
			}
			break;

		case MinerState.EnterShip:
			{
				GlobalCounter.Increment( CountMe.CountType.MinerCollected );
				Destroy(gameObject);
			}
			break;
		}
	}
	
	private void MoveTowardsTarget()
	{
		Vector3 vDesiredVelocity = mTargetPos - gameObject.transform.position;
		vDesiredVelocity.Normalize();
		vDesiredVelocity *= speed;
		
		Vector3 vDesiredUp = new Vector3(0.0f,1.0f,0.0f);
		Vector3 vDesiredFacing = vDesiredVelocity;
		Quaternion desiredRot = new Quaternion();
		desiredRot.SetLookRotation( vDesiredFacing, vDesiredUp );
		transform.rotation = desiredRot;
			
		Vector3 vVelChange = (vDesiredVelocity - rigidbody.velocity);
		rigidbody.AddForce(vVelChange);
	}

	private bool IsInRange( Vector3 vTargetPos, float fTolerance )
	{
		Vector3 vOffset = (vTargetPos - gameObject.transform.position);
		float distSqrd = vOffset.sqrMagnitude;
		bool bTargetReached = ( distSqrd < (fTolerance*fTolerance));
		return bTargetReached;
	}

}
