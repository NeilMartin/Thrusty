using UnityEngine;
using System.Collections;

public class TurretAI : MonoBehaviour 
{
	public GameObject target;
	public GameObject projectile;
	public float range;
	public float reloadTime;
	
	private float mTimeUntilReady;
	private ProjectileLauncher mLauncher;

	void Start () 
	{
		mLauncher = gameObject.GetComponent<ProjectileLauncher>();
	}
	
	void Update () 
	{
		if( null == target )
		{
			target =  GameObject.FindWithTag("Player");
		}
		if( mTimeUntilReady > 0.0f )
		{
			mTimeUntilReady -= Time.deltaTime;
		}
		else
		{
			Vector3 targetOffset = target.transform.position - transform.position;
			if(targetOffset.sqrMagnitude < range*range)
			{
				if( mLauncher != null )
				{
					mTimeUntilReady = reloadTime;
					mLauncher.Shoot(projectile, target);
				}
				else
				{
					Debug.Log("TurretAI trying to shoot on an object with no Projectile launcher");
				}
			}
		}
	}
}
