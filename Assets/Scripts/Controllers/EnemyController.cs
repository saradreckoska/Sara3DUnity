using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyController : MonoBehaviour {

	public float lookRadius = 10f;	

	Transform target;	
	NavMeshAgent agent; 
	CharacterCombat combat;

	
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
		else
			Debug.Log("EnemyController: NavMeshAgent found on " + gameObject.name + ", speed: " + agent.speed + ", enabled: " + agent.enabled);

		combat = GetComponent<CharacterCombat>();
		if (combat == null)
			Debug.LogWarning("EnemyController: CharacterCombat component missing on " + gameObject.name + " (enemy won't be able to attack)");
	}
	
	
	void Update () {
		
		if (target == null)
		{
			if (PlayerManager.instance != null && PlayerManager.instance.player != null)
				target = PlayerManager.instance.player.transform;
			else
			{
				
				return;
			}
		}

		if (agent == null)
		{
			agent = GetComponent<NavMeshAgent>();
			if (agent == null)
				return; 
		}

		
		float distance = Vector3.Distance(target.position, transform.position);

		
		if (distance <= lookRadius)
		{
			Debug.Log("Enemy setting destination to player at " + target.position);
			agent.SetDestination(target.position);
			Debug.Log("Enemy agent has path: " + agent.hasPath + ", path status: " + agent.pathStatus);

			
			if (distance <= agent.stoppingDistance)
			{
				CharacterStats targetStats = target.GetComponent<CharacterStats>();
				if (targetStats != null)
				{
					combat.Attack(targetStats);
				}

				FaceTarget();	
			}
		}
	}

		void FaceTarget ()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}

	
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}
}