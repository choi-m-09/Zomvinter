using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TableSlot : MonoBehaviour, IPointerClickHandler
{ 
    public ItemData Data;

    Inventory _inventory;

    void Start()
    {
        this.GetComponent<Image>().sprite = Data.ItemImage;
        _inventory = Canvas.FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Data != null)
            {
                   int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                   _inventory.Items[Index] = Data;
                   Data = null;
                   Destroy(this.gameObject);
            }
            
        }
    }
}
