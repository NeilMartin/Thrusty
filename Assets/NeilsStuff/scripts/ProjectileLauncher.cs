using UnityEngine;
using System.Collections;

public class ProjectileLauncher : MonoBehaviour 
{
	public Transform  launchPos;
	public GameObject velocityObject = null;
	public float launchSpeed = 10.0f;
	
	public void Shoot( GameObject projectile )
	{
		if( launchPos != null )
		{
			Vector3 vel = new Vector3(0.0f,launchSpeed,0.0f);
			Quaternion rot = new Quaternion();
			if( velocityObject != null )
			{
				rot = velocityObject.transform.rotation;
				vel = rot * vel;
				if( velocityObject.rigidbody != null )
				{
					vel += velocityObject.rigidbody.velocity;
				}
			}
			GameObject go = (GameObject)Instantiate( projectile, launchPos.transform.position, rot );
			if( go.rigidbody != null )
			{
				go.rigidbody.velocity = vel;
			}
		}
		else
		{
			Debug.Log("Try to launch a projectile without a launchPos being set");
		}
	}

	public void Shoot( GameObject projectile, GameObject target )
	{
		if( launchPos != null )
		{
			Vector3 vel = target.transform.position - launchPos.transform.position;
			vel.Normalize();
			vel *= launchSpeed;
			Quaternion rot = new Quaternion();
			rot.SetLookRotation(vel);
			GameObject go = (GameObject)Instantiate( projectile, launchPos.transform.position, rot );
			if( go.rigidbody != null )
			{
				go.rigidbody.velocity = vel;
			}
		}
		else
		{
			Debug.Log("Try to launch a projectile without a launchPos being set");
		}
	}

}
