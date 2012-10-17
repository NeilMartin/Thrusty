using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour 
{
	public GameObject target;
	
	private Vector3 mTargetOffset;
		
	// Use this for initialization
	void Start () 
	{
		mTargetOffset = transform.position - target.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = target.transform.position + mTargetOffset;
	}
}
