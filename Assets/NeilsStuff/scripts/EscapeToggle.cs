using UnityEngine;
using System.Collections;

public class EscapeToggle : MonoBehaviour 
{
	public GameObject toggleObject;
	public bool startActive = false;
	public bool pauseGame = false;
	
	void Start () 
	{
		toggleObject.SetActiveRecursively( startActive );
		if( pauseGame )
		{
			if( toggleObject.active )
			{
				Time.timeScale = 1.0f;
			}
			else
			{
				Time.timeScale = 0.0f;
			}
		}
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
				toggleObject.SetActiveRecursively( !toggleObject.active );
				if( pauseGame )
				{
					if( toggleObject.active )
					{
						Time.timeScale = 1.0f;
					}
					else
					{
						Time.timeScale = 0.0f;
					}
				}
			}
		}
	}
	
	void ResumeGame()
	{
		toggleObject.SetActiveRecursively( startActive );
		if( pauseGame )
		{
			Time.timeScale = 1.0f;
		}
	}
}
