using UnityEngine;
using UnityEngine.InputSystem;


public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;

    Inventory inventory;

    InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        if (inventory != null)
            inventory.onItemChangedCallback += UpdateUI;

       
        if (inventoryUI == null)
        {
            inventoryUI = GameObject.Find("Inventory") ?? GameObject.Find("InventoryUI");
            if (inventoryUI != null)
                Debug.Log("InventoryUI: auto-found inventory UI GameObject: " + inventoryUI.name);
        }

        
        if (inventoryUI != null)
        {
            RectTransform rt = inventoryUI.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.localScale = Vector3.one;
                
                rt.anchorMin = new Vector2(1, 1);
                rt.anchorMax = new Vector2(1, 1);
                rt.anchoredPosition = new Vector2(-10, -10);
                rt.pivot = new Vector2(1, 1); t
            }
        }

        if (itemsParent == null && inventoryUI != null)
        {
            
            Transform found = inventoryUI.transform.Find("ItemsParent");
            if (found != null)
                itemsParent = found;
        }

        if (itemsParent != null)
        {
            itemsParent.localScale = Vector3.one;
            slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        }
        else
        {
            Debug.LogWarning("InventoryUI: itemsParent is not assigned and could not be auto-found.");
        }
    }

    void Update()
    {
        if (inventoryUI == null)
            return;

        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            Debug.Log("Inventory UI toggled to: " + inventoryUI.activeSelf);
        }
    }

    void UpdateUI()
    {
       
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                
                slots[i].ClearSlot();
            }
        }
    }
}
                
