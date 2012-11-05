using UnityEngine;
using System.Collections;

public class Transition : StateThing
{
	public GameObject nextState;
	public bool       not;
	
	private Condition[]  conditions;	
	private Action     action1;
	private Action     action2;
	
	private const int MAX_CONDITIONS = 4;
	
	void Start()
	{
		Condition[] tempConditions = new Condition[MAX_CONDITIONS+1];
		Transform[] trans = GetComponentsInChildren<Transform>();
		int numConditions = 0;
		foreach( Transform tran in trans )
		{
			GameObject go = tran.gameObject;
			tempConditions[numConditions++] = go.GetComponent<Condition>();
			if( numConditions > MAX_CONDITIONS )
			{
				Debug.LogError("Too many conditions");	
			}
		}
		conditions = new Condition[numConditions];
		for(int i=0;i<numConditions;++i)
		{
			conditions[i] = tempConditions[i];
		}
		Debug.Log("Num conditions=" + conditions.Length);
	}
	
}
