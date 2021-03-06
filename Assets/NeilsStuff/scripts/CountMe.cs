using UnityEngine;
using System.Collections;

public class CountMe : MonoBehaviour 
{
	public enum CountType 
	{
		None,
		Turret,
		MinerMining,
		MinerCollected,
		NumTypes,
	};
	
	public CountType type = CountType.None;
	
	void Start () 
	{
		GlobalCounter.Increment( type );
	}

	void OnDestroy()
	{
		GlobalCounter.Decrement( type );
	}
}
