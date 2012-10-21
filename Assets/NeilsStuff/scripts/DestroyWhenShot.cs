using UnityEngine;
using System.Collections;

public class DestroyWhenShot : MonoBehaviour 
{
	public enum Faction { None, Enemy, Player };
	
	public float health;
	public Faction faction; 
	
	private float mDamage;
	
	// Use this for initialization
	void Start () 
	{
		mDamage=0.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter( Collision coll )
	{
		GameObject other = coll.gameObject;
		Damager damager = other.GetComponent<Damager>();
		float damage = 0.0f;
		if( null != damager )
		{
			switch( faction )
			{
			case Faction.None:
				damage = damager.damageAmount;
				break;
				
			case Faction.Enemy:
				{
					if(    ( damager.GetDamageType() == Damager.DamageType.DamageEnemyOnly )
				   		|| ( damager.GetDamageType() == Damager.DamageType.DamageAll ))
					{
						damage = damager.damageAmount;	
					}
				}
				break;
	
			case Faction.Player:
				{
					if(    ( damager.GetDamageType() == Damager.DamageType.DamagePlayerOnly )
				   		|| ( damager.GetDamageType() == Damager.DamageType.DamageAll ))
					{
						damage = damager.damageAmount;	
					}
				}
				break;
				
			default:
				Debug.LogError( "Unhandled case" );
				break;
			}
		}
		else // no damager, so perform velocity damage
		{
			float safeSpeed = 2.0f;
			float speed = rigidbody.velocity.magnitude - safeSpeed;
			if( speed > 0.0f )
			{
				float damageScalar = 5.0f;
				damage = speed * damageScalar;
			}
		}
		mDamage += damage;
		if( mDamage >= health )
		{
			Destroy(gameObject);	
		}
	}
	
	public int GetHealthAsInt()
	{
		return (int)(health-mDamage);
	}
}
