using UnityEngine;

/* Handles combat for characters. */

public class CharacterCombat : MonoBehaviour {

	public float attackCooldown = 1f;	// Time between attacks
	float attackTimer = 0f;

	CharacterStats stats;	// Reference to character stats

	void Start()
	{
		stats = GetComponent<CharacterStats>();
	}

	void Update()
	{
		attackTimer -= Time.deltaTime;
	}

	// Attack a target
	public void Attack(CharacterStats targetStats)
	{
		// Only attack if cooldown is ready
		if (attackTimer <= 0f)
		{
			int damageAmount = stats.damage.GetValue();
			targetStats.TakeDamage(damageAmount);

			attackTimer = attackCooldown;	// Reset cooldown
		}
	}
}
