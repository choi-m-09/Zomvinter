using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnEvent : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    TableSlot[] Tableslot;
    [SerializeField]
    Inventory _inventory;

    // Start is called before the first frame update
    void Start()
    {
        _inventory = Canvas.FindObjectOfType<Inventory>();
    }

    void Update()
    {
        Tableslot = parent.GetComponentsInChildren<TableSlot>();
    }

    public void BtnEvents()
    {
        for(int i = 0; i < Tableslot.Length; i++)
        {
            int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
            _inventory.Items[Index] = Tableslot[i].Data;
            Destroy(Tableslot[i].gameObject);
        }
    }
}
