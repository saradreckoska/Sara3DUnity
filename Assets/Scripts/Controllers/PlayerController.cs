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
		motor = GetComponent<PlayerMotor>();
	}
	
	void Update () {

		if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
			return;

		
		if (Mouse.current.leftButton.wasPressedThisFrame)
		{
			
			Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
			RaycastHit hit;

			
			if (Physics.Raycast(ray, out hit, 100, movementMask))
			{
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
