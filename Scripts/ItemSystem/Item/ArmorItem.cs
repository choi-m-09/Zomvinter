using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ��� - �� ������ </summary>
public class ArmorItem : EquipmentItem
{
    /// <summary> ConsumableData�κ��� ������ ������ Data�� ���� </summary>
    public ArmorItemData ArmorData { get; set; }

    public ArmorItem(ArmorItemData data) : base(data)
    {
        ArmorData = data;
    }
}
