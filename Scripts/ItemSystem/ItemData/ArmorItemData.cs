using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Equipment_", menuName = "Inventory System/Item Data/Equipment/Armor", order = 3)]
public class ArmorItemData : EquipmentItemData
{
    /// <summary> ¹æ¾î·Â </summary>
    public int Defence => _defence;

    [SerializeField] private int _defence = 1;
}
