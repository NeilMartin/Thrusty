using UnityEngine;
using System.Collections;

public class EscapeToggle : MonoBehaviour 
{
	public GameObject toggleObject;

	void Start () 
	{
		toggleObject.SetActiveRecursively( false );
	}
	
	void OnGUI()
	{
		Update();	
	}
	
	void Update() 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if( null != toggleObject )
			{
				if( toggleObject.active )
				{
					Time.timeScale = 1.0f;
				}
				else
				{
					Time.timeScale = 0.0f;
				}
				toggleObject.SetActiveRecursively( !toggleObject.active );
			}
		}
	}
}
