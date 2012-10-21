using UnityEngine;
using System.Collections;

public class MinerCollector : MonoBehaviour 
{
	void OnCollisionEnter( Collision coll )
	{
		GameObject other = coll.gameObject;
		MinerAI miner = other.GetComponent<MinerAI>();
		if( null != miner )
		{
			miner.SetCollected();
		}
	}
}
