using UnityEngine;
using UnityEngine.InputSystem;

/* This object updates the inventory UI. */

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;

    Inventory inventory;

    InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        // Populate our slots array
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
        // Check to see if we should open/close the inventory
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void UpdateUI()
    {
        // Loop through all the slots
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                // Otherwise clear the slot
                slots[i].ClearSlot();
            }
        }
    }
}
