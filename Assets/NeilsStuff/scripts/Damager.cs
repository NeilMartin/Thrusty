using UnityEngine;
using System.Collections;

public class Damager : MonoBehaviour 
{
	public enum DamageType { None, DamageEnemyOnly, DamagePlayerOnly, DamageAll }
	
	public float damageAmount;
	public DamageType type;
		
	public DamageType GetDamageType()
	{
		return type;
	}
}
