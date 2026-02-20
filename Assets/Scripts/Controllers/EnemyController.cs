using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Controls the Enemy AI */

public class EnemyController : MonoBehaviour {

	public float lookRadius = 10f;	// Detection range for player

	Transform target;	// Reference to the player
	NavMeshAgent agent; // Reference to the NavMeshAgent
	CharacterCombat combat;

	// Use this for initialization
	void Start () {
		if (PlayerManager.instance == null || PlayerManager.instance.player == null)
		{
			Debug.LogError("EnemyController: PlayerManager or player not found. Make sure PlayerManager exists and has a player assigned.");
		}
		else
		{
			target = PlayerManager.instance.player.transform;
		}

		agent = GetComponent<NavMeshAgent>();
		if (agent == null)
			Debug.LogError("EnemyController: NavMeshAgent component missing on " + gameObject.name);

		combat = GetComponent<CharacterCombat>();
		if (combat == null)
			Debug.LogWarning("EnemyController: CharacterCombat component missing on " + gameObject.name + " (enemy won't be able to attack)");
	}
	
	// Update is called once per frame
	void Update () {
		// Ensure we have a valid target and agent
		if (target == null)
		{
			if (PlayerManager.instance != null && PlayerManager.instance.player != null)
				target = PlayerManager.instance.player.transform;
			else
			{
				// Player not ready yet; skip this frame
				return;
			}
		}

		if (agent == null)
		{
			agent = GetComponent<NavMeshAgent>();
			if (agent == null)
				return; // can't navigate without agent
		}

		// Distance to the target
		float distance = Vector3.Distance(target.position, transform.position);

		// If inside the lookRadius
		if (distance <= lookRadius)
		{
			// Move towards the target
			agent.SetDestination(target.position);

			// If within attacking distance
			if (distance <= agent.stoppingDistance)
			{
				CharacterStats targetStats = target.GetComponent<CharacterStats>();
				if (targetStats != null)
				{
					combat.Attack(targetStats);
				}

				FaceTarget();	// Make sure to face towards the target
			}
		}
	}

	// Rotate to face the target
	void FaceTarget ()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}

	// Show the lookRadius in editor
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}
}