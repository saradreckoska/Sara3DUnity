using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAnimator : CharacterAnimator {

	public WeaponAnimation[] weaponAnimations;
	WeaponAnimation currentWeaponAnimation;

	void Awake() {
		if (weaponAnimations != null && weaponAnimations.Length > 0) {
			currentWeaponAnimation = weaponAnimations[0];
		} else {
			Debug.LogWarning("PlayerAnimator: No weapon animations assigned. Please assign weapon animations in the Inspector.");
		}
	}

	protected override void Start() {
		base.Start ();
		if (EquipmentManager.instance != null) {
			EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
		}
	}
		

	protected override void OnAttack() {
		if (currentWeaponAnimation != null) {
			int attackIndex = Random.Range (0, currentWeaponAnimation.numAnimations);
			animator.SetFloat ("Attack Index", attackIndex);
			animator.SetFloat ("Weapon Index", currentWeaponAnimation.weaponIndex);
		}

		base.OnAttack ();
	}

	void OnEquipmentChanged(Equipment newItem, Equipment oldItem) {
		
		if (oldItem != null) {
			if (oldItem.equipSlot == EquipmentSlot.Weapon) {
				animator.SetLayerWeight (1, 0); 
			}
			if (oldItem.equipSlot == EquipmentSlot.Shield) {
				animator.SetLayerWeight (2, 0); 
			}
		}

		if (newItem != null) {
			
			if (newItem.equipSlot == EquipmentSlot.Weapon) {
				WeaponAnimation newC = weaponAnimations.First (x => x.weapons.Contains (newItem));
				if (newC != null) {
					currentWeaponAnimation = newC;

				}
				animator.SetLayerWeight (1, 1); 
			}
			if (newItem.equipSlot == EquipmentSlot.Shield) {
				animator.SetLayerWeight (2, 1); 
			}
		}

	

	}

	[System.Serializable]
	public class WeaponAnimation {
		public Equipment[] weapons;
		public int weaponIndex;
		public int numAnimations;
	}
}