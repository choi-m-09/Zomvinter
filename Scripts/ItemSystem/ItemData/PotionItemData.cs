using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �Һ� ������ ���� </summary>
[CreateAssetMenu(fileName = "Item_Countable_", menuName = "Inventory System/Item Data/Countable/Potion", order = 1)]
public class PotionItemData : CountableItemData
{
    /// <summary> ȿ����(ȸ���� ��) </summary>
    public float Value => _value;
    [SerializeField] private float _value;
}