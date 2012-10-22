using UnityEngine;
using System.Collections;

public class ShieldActivator : MonoBehaviour 
{
	public GameObject shieldObject;
	
	void Start () 
	{
		if( null != shieldObject )
		{
			shieldObject.active = false;
		}
	}
	
	void EnableShield( bool bActive )
	{
		if( null != shieldObject )
		{
			shieldObject.active = bActive;
		}
	}
	
	void Update()
	{
		if( ( null != shieldObject ) && ( shieldObject.active ))
		{
			shieldObject.transform.rotation.SetLookRotation( new Vector3( 	Random.Range( -1.0f, 1.0f ),
																			Random.Range( -1.0f, 1.0f ),
																			Random.Range( -1.0f, 1.0f ) ) );
		}
	}
}
