using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Equipment_", menuName = "Inventory System/Item Data/Equipment/Weapon/Melee", order = 1)]
public class MeleeItemData : EquipmentItemData
{
    public int DP => dp;
    [SerializeField] private int dp;
}
