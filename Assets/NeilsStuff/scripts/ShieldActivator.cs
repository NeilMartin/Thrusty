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
}
