using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class EquipmentManager : MonoBehaviour {

	#region Singleton

    public enum MeshBlendShape {Torso, Arms, Legs };
    public Equipment[] defaultEquipment;

	public static EquipmentManager instance;
	public SkinnedMeshRenderer targetMesh;
	public Transform fallbackAttachRoot; // used when player has no SkinnedMeshRenderer

	GameObject[] currentMeshes;

	void Awake ()
	{
		instance = this;
	}

	void StartDiagnostics()
	{
		if (defaultEquipment != null && defaultEquipment.Length > 0)
		{
			foreach (Equipment e in defaultEquipment)
			{
				if (e == null) continue;
				Debug.Log("Default equipment slot: " + e.name + " slot=" + e.equipSlot + " hasMesh=" + (e.mesh!=null) + " hasModel=" + (e.model!=null));
			}
		}

		Debug.Log("EquipmentManager targetMesh assigned=" + (targetMesh != null));
	}

	#endregion

	Equipment[] currentEquipment;  

	public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
	public OnEquipmentChanged onEquipmentChanged;
   

	Inventory inventory;	
	void Start ()
	{
		inventory = Inventory.instance;		

		int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
		currentEquipment = new Equipment[numSlots];
		currentMeshes = new GameObject[numSlots];


		
		if (targetMesh == null)
		{
			if (PlayerManager.instance != null && PlayerManager.instance.player != null)
			{
				targetMesh = PlayerManager.instance.player.GetComponentInChildren<SkinnedMeshRenderer>();
				if (targetMesh != null)
				{
					Debug.Log("EquipmentManager: auto-found targetMesh (SkinnedMeshRenderer) = true");
				}
				else
				{
					
					fallbackAttachRoot = PlayerManager.instance.player.transform;
					Debug.Log("EquipmentManager: no SkinnedMeshRenderer found on player; using fallback attach root " + fallbackAttachRoot.name);
				}
			}
			else
			{
				Debug.LogWarning("EquipmentManager: targetMesh not assigned and PlayerManager/player not available to auto-find.");
			}
		}

		StartDiagnostics();

		if (targetMesh != null)
			EquipDefaults();
		else
			Debug.LogError("EquipmentManager: targetMesh still null - skipping EquipDefaults to avoid errors. Assign Target Mesh in the Inspector.");
	}

	void AwakeChecks()
	{
		if (targetMesh == null)
			Debug.LogError("EquipmentManager: targetMesh is not assigned!");
	}

	
	public void Equip (Equipment newItem)
	{
		
		int slotIndex = (int)newItem.equipSlot;

        Equipment oldItem = Unequip(slotIndex);

		
		if (onEquipmentChanged != null)
		{
			onEquipmentChanged.Invoke(newItem, oldItem);
		}

		
		currentEquipment[slotIndex] = newItem;
        AttachToMesh(newItem, slotIndex);
	}

	
	public Equipment Unequip (int slotIndex)
	{
        Equipment oldItem = null;
		
		if (currentEquipment[slotIndex] != null)
		{
			
			oldItem = currentEquipment[slotIndex];
			inventory.Add(oldItem);

            SetBlendShapeWeight(oldItem, 0);
			
			if (currentMeshes[slotIndex] != null)
			{
				Destroy(currentMeshes[slotIndex]);
			}

			
			currentEquipment[slotIndex] = null;

			
			if (onEquipmentChanged != null)
			{
				onEquipmentChanged.Invoke(null, oldItem);
			}
		}
        return oldItem;
	}

		public void UnequipAll ()
	{
		for (int i = 0; i < currentEquipment.Length; i++)
		{
			Unequip(i);
		}

        EquipDefaults();
	}

	void AttachToMesh(Equipment item, int slotIndex)
	{
		if (item == null)
		{
			Debug.LogWarning("AttachToMesh called with null item for slot " + slotIndex);
			return;
		}

		
		if (targetMesh == null && fallbackAttachRoot == null)
		{
			Debug.LogError("Cannot attach item " + item.name + " because no targetMesh or fallback attach root is available");
			return;
		}

		Debug.Log("Attaching item '" + item.name + "' to slot " + item.equipSlot);
		
		if (item.mesh != null && targetMesh != null)
		{
			
			SkinnedMeshRenderer newMesh = Instantiate(item.mesh) as SkinnedMeshRenderer;
			newMesh.transform.parent = targetMesh.transform.parent;

			newMesh.rootBone = targetMesh.rootBone;
			newMesh.bones = targetMesh.bones;

			currentMeshes[slotIndex] = newMesh.gameObject;

			SetBlendShapeWeight(item, 100);
		}
		else if (item.mesh != null && fallbackAttachRoot != null)
		{
			
			GameObject newObj = Instantiate(item.mesh.gameObject) as GameObject;
			newObj.transform.parent = fallbackAttachRoot;
			newObj.transform.localPosition = Vector3.zero;
			newObj.transform.localRotation = Quaternion.identity;
			newObj.transform.localScale = Vector3.one;
			currentMeshes[slotIndex] = newObj;
		}
		
		else if (item.model != null)
		{
			GameObject newObj = Instantiate(item.model) as GameObject;

			
			Transform attachPoint = targetMesh.transform.Find(item.equipSlot.ToString());
			if (attachPoint == null)
				attachPoint = targetMesh.transform.Find("Shield");
			if (attachPoint == null)
				attachPoint = targetMesh.transform.Find("Shield_R");
			if (attachPoint == null)
				attachPoint = targetMesh.transform; // fallback

			newObj.transform.parent = attachPoint;
			newObj.transform.localPosition = Vector3.zero;
			newObj.transform.localRotation = Quaternion.identity;
			newObj.transform.localScale = Vector3.one;

			
			newObj.SetActive(true);
			newObj.layer = targetMesh.gameObject.layer;

			currentMeshes[slotIndex] = newObj;

			Debug.Log("Instantiated static model for " + item.name + " under " + attachPoint.name);
		}
		else
		{
			Debug.LogWarning("Item '" + item.name + "' has no mesh or model assigned.");
		}
	}

	void SetBlendShapeWeight(Equipment item, int weight)
	{
		if (item == null || item.coveredMeshRegions == null)
			return;

		foreach (MeshBlendShape blendshape in item.coveredMeshRegions)
		{
			int shapeIndex = (int)blendshape;
			targetMesh.SetBlendShapeWeight(shapeIndex, weight);
		}
	}

    void EquipDefaults()
    {
		foreach (Equipment e in defaultEquipment)
		{
			Equip(e);
		}
    }

	void Update ()
	{
		
		if (Keyboard.current.uKey.wasPressedThisFrame)
			UnequipAll();
	}

}