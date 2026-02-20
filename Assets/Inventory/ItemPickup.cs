using UnityEngine;

public class ItemPickup : Interactable {

	public Item item;	// Item to put in the inventory on pickup

	// When the player interacts with the item
	public override void Interact()
	{
		base.Interact();

		PickUp();	// Pick it up!
	}

	// Pick up the item
	void PickUp ()
	{
		Debug.Log("Picking up " + item.name);
		bool wasPickedUp = Inventory.instance.Add(item); 	// Add to inventory

		// If successfully picked up
		if (wasPickedUp)
		{
			// If the item is equipment, auto-equip it and remove from inventory
			Equipment equip = item as Equipment;
			if (equip != null && EquipmentManager.instance != null)
			{
				Debug.Log("Auto-equipping " + equip.name + " on pickup");
				EquipmentManager.instance.Equip(equip);
				Inventory.instance.Remove(equip);
			}

			Destroy(gameObject); 	// Destroy item from scene
		}
	}

}