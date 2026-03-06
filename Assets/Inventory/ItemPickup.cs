using UnityEngine;

public class ItemPickup : Interactable {

	public Item item;	

	
	public override void Interact()
	{
		base.Interact();

		PickUp();	
	}

	
	void PickUp ()
	{
		Debug.Log("Picking up " + item.name);
		bool wasPickedUp = Inventory.instance.Add(item); 	
		
		if (wasPickedUp)
		{
			
			Equipment equip = item as Equipment;
			if (equip != null && EquipmentManager.instance != null)
			{
				Debug.Log("Auto-equipping " + equip.name + " on pickup");
				EquipmentManager.instance.Equip(equip);
				Inventory.instance.Remove(equip);
			}

			Destroy(gameObject); 	
		}
	}

}