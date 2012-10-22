using UnityEngine;
using System.Collections;

public class FuelGUI : MonoBehaviour 
{
	private ShipController mPlayerShip;
	
	// Use this for initialization
	void Start () 
	{
		GameObject playerGO = GameObject.FindWithTag("Player");
		if(null != playerGO)
		{
			mPlayerShip = playerGO.GetComponent<ShipController>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( mPlayerShip == null )
		{
			GameObject playerGO = GameObject.FindWithTag("Player");
			if(null != playerGO)
			{
				mPlayerShip = playerGO.GetComponent<ShipController>();
			}
		}
		if( mPlayerShip != null )
		{
			guiText.text = "Fuel " + (int)mPlayerShip.GetFuelRemaining();	
		}
		else
		{
			guiText.text = "Fuel ?";	
		}
	}
}
