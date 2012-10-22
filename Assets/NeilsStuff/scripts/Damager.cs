using UnityEngine;
using System.Collections;

public class Damager : MonoBehaviour 
{
	public float damageAmount = 5.0f;
	public bool damagePlayer = false;
	public bool damageMiner = false;
	public bool damageTurret = true;
		
	public float GetDamageToPlayer()
	{
		return damagePlayer?damageAmount:0.0f;
	}
	
	public float GetDamageToMiner()
	{
		return damageMiner?damageAmount:0.0f;
	}

	public float GetDamageToTurret()
	{
		return damageTurret?damageAmount:0.0f;
	}
}
