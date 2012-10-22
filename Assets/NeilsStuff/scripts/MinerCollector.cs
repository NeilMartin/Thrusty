using UnityEngine;
using System.Collections;

public class MinerCollector : MonoBehaviour 
{
	private bool mShieldActive = false;
	
	void EnableShield( bool bEnable )
	{
		mShieldActive = bEnable;
	}
	
	void OnCollisionEnter( Collision coll )
	{
		GameObject other = coll.gameObject;
		MinerAI miner = other.GetComponent<MinerAI>();
		if( null != miner )
		{
			if( mShieldActive )
			{
				miner.SendMessage( "ApplyDamage", 5.0f );
			}
			else
			{
				miner.SetCollected();
			}
		}
	}
}
