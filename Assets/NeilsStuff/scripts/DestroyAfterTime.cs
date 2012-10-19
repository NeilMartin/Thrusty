using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour 
{
	public float secondsOfLife = 3.0f;
	private float mTimeAlive;
	
	void Start()
	{
		mTimeAlive = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		mTimeAlive += Time.deltaTime;
		if( mTimeAlive > secondsOfLife )
		{
			Destroy(gameObject);	
		}
	}
}
