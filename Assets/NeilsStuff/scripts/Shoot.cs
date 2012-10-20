using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour 
{
	public GameObject projectile;
	public float reloadTime = 0.1f;
	
	private ProjectileLauncher mLauncher;
	private float mSecondsSinceLaunch;
		
	void Start () 
	{
		mLauncher = gameObject.GetComponent<ProjectileLauncher>();
	}
	
	void FixedUpdate () 
	{
		mSecondsSinceLaunch += Time.fixedDeltaTime;
		if((Input.GetKey(KeyCode.Space)) && (mSecondsSinceLaunch>reloadTime))
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
