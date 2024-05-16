using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItem : Item
{
    /// <summary> ConsumableData�κ��� ������ ������ Data�� ���� </summary>
    public EquipmentItemData EquipmentData { get; set; }

    /// <summary> ���� ������ </summary>
    private float _durability;

    public float Durability
    {
        get => _durability;
        set
        {
            if (value < 0) value = 0;
            if (value > EquipmentData.MaxDurability)
                value = EquipmentData.MaxDurability;

            _durability = value;
        }
    }

    public EquipmentItem(EquipmentItemData data) : base(data)
    {
        EquipmentData = data;
        Durability = data.MaxDurability;
    }

    private void Start()
    {
        _durability = EquipmentData.MaxDurability;
    }
}
