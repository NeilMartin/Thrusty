using UnityEngine;
using System.Collections;

public class HealthGUI : MonoBehaviour 
{
	private DestroyWhenShot mPlayerHealth;
	
	// Use this for initialization
	void Start () 
	{
		GameObject playerGO = GameObject.FindWithTag("Player");
		if(null != playerGO)
		{
			mPlayerHealth = playerGO.GetComponent<DestroyWhenShot>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( mPlayerHealth == null )
		{
			GameObject playerGO = GameObject.FindWithTag("Player");
			if(null != playerGO)
			{
				mPlayerHealth = playerGO.GetComponent<DestroyWhenShot>();
			}
		}
		if( mPlayerHealth != null )
		{
			guiText.text = "Health " + mPlayerHealth.GetHealthAsInt();	
		}
		else
		{
			guiText.text = "Health 0";	
		}
	}
}
