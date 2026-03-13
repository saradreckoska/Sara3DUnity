using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour {

	Transform target;		
	NavMeshAgent agent;		
	// Get references
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		if (agent == null) Debug.LogError("PlayerMotor: NavMeshAgent component missing!");
		else {
			Debug.Log("PlayerMotor: NavMeshAgent found, speed: " + agent.speed + ", enabled: " + agent.enabled);
		}
	}

	void Update ()
	{
		
		if (target != null)
		{
			
			agent.SetDestination(target.position);
			FaceTarget();
		}
	}
	
	public void MoveToPoint (Vector3 point)
	{
		Debug.Log("Moving to point: " + point);
		agent.SetDestination(point);
		Debug.Log("Agent has path: " + agent.hasPath + ", path status: " + agent.pathStatus);
	}

	
	public void FollowTarget (Interactable newTarget)
	{
		agent.stoppingDistance = newTarget.radius * .8f;
		agent.updateRotation = false;

		target = newTarget.interactionTransform;
	}

	
	public void StopFollowingTarget ()
	{
		agent.stoppingDistance = 0f;
		agent.updateRotation = true;

		target = null;
	}

	
	void FaceTarget ()
	{
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}

}
