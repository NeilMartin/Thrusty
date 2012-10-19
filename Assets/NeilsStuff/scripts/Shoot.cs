using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour 
{
	public GameObject projectile;
	
	private ProjectileLauncher mLauncher;
		
	void Start () 
	{
		mLauncher = gameObject.GetComponent<ProjectileLauncher>();
	}
	
	void FixedUpdate () 
	{
		if (Input.GetKeyDown (KeyCode.Space))
		{
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
