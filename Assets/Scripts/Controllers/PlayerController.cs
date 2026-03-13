using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	public Interactable focus;	
	public LayerMask movementMask;	

	Camera cam;			
	PlayerMotor motor;	

	
	void Start () {
		cam = Camera.main;
		if (cam == null) Debug.LogError("PlayerController: No main camera found!");
		motor = GetComponent<PlayerMotor>();
		if (motor == null) Debug.LogError("PlayerController: PlayerMotor component missing!");
		Debug.Log("PlayerController: movementMask = " + movementMask.value + ", LayerMask = " + LayerMask.LayerToName(movementMask.value));
	}
	
	void Update () {

		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
		{
			Debug.Log("PlayerController: pointer over UI, skipping movement");
			return;
		}

		if (Mouse.current.leftButton.wasPressedThisFrame)
		{
			Debug.Log("Left mouse button pressed");
			
			Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100, movementMask))
			{
				Debug.Log("Raycast hit at " + hit.point + " (" + hit.collider.name + ")");
				motor.MoveToPoint(hit.point);   
				RemoveFocus();
			}
			else
			{
				Debug.Log("PlayerController: raycast did not hit movementMask (" + movementMask.value + ")");
			}
		}


		
		if (Mouse.current.leftButton.wasPressedThisFrame)
		{
			Debug.Log("Left mouse button pressed");
			
			Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
			RaycastHit hit;

			
			if (Physics.Raycast(ray, out hit, 100, movementMask))
			{
				Debug.Log("Raycast hit at " + hit.point);
				motor.MoveToPoint(hit.point);   
				RemoveFocus();
			}
		}

		
		if (Mouse.current.rightButton.wasPressedThisFrame)
		{
			
			Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
			RaycastHit hit;

			
			if (Physics.Raycast(ray, out hit, 100))
			{
				Interactable interactable = hit.collider.GetComponent<Interactable>();
				if (interactable != null)
				{
					SetFocus(interactable);
				}
			}
		}
	}

		void SetFocus (Interactable newFocus)
	{
		
		if (newFocus != focus)
		{
			
			if (focus != null)
				focus.OnDefocused();

			focus = newFocus;	
			motor.FollowTarget(newFocus);	
		}
		
		newFocus.OnFocused(transform);
	}

	
	void RemoveFocus ()
	{
		if (focus != null)
			focus.OnDefocused();

		focus = null;
		motor.StopFollowingTarget();
	}
}
