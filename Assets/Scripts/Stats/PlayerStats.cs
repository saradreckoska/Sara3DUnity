using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerStats : CharacterStats {

	
	void Start () {
		if (EquipmentManager.instance != null) {
			EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
		}
	}
	
		void OnEquipmentChanged (Equipment newItem, Equipment oldItem)
	{
		
		if (newItem != null)
		{
			armor.AddModifier(newItem.armorModifier);
			damage.AddModifier(newItem.damageModifier);
		}

		
		if (oldItem != null)
		{
			armor.RemoveModifier(oldItem.armorModifier);
			damage.RemoveModifier(oldItem.damageModifier);
		}
		
	}

	public override void Die()
	{
		base.Die();
		if (PlayerManager.instance != null) {
			PlayerManager.instance.KillPlayer();
		}
	}
}