using UnityEngine;
using System.Collections;

public class DestroyWhenShot : MonoBehaviour 
{
	public enum Faction { None, Turret, Player, Miner };
	
	public float health;
	public Faction faction; 
	public bool applyVelocityDamage = false;
	public GameObject damageEffect = null;
	public bool bShieldActive = false;
	
	private float mDamage;
	
	// Use this for initialization
	void Start () 
	{
		mDamage=0.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void EnableShield( bool bEnable )
	{
		bShieldActive = bEnable;
	}
	
	void OnTriggerEnter( Collider coll )
	{
		//HandleCollision( coll );
	}
	
	void OnCollisionEnter( Collision coll )
	{
		HandleCollision( coll );
	}
	
	void HandleCollision( Collision coll )
	{
		if( bShieldActive )
			return;
		
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
				
			case Faction.Turret:
				damage = damager.GetDamageToTurret();
				break;
	
			case Faction.Player:
				damage = damager.GetDamageToPlayer();
				break;
				
			case Faction.Miner:
				damage = damager.GetDamageToMiner();
				break;
				
			default:
				Debug.LogError( "Unhandled case" );
				break;
			}
		}
		else if (applyVelocityDamage) // no damager, so perform velocity damage
		{
			float safeSpeed = 2.0f;
			float speed = rigidbody.velocity.magnitude - safeSpeed;
			if( speed > 0.0f )
			{
				float damageScalar = 5.0f;
				damage = speed * damageScalar;
			}
		}
		ApplyDamage( damage );
	}
	
	void ApplyDamage( float damage )
	{
		if(( damage > 0.0f ) && (null != damageEffect))
		{
			Instantiate( damageEffect, transform.position, transform.rotation );
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
