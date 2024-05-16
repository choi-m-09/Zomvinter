using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItemData : ItemData
{
    public float MaxDurability => _maxDurability; // ������

    [SerializeField]
    private float _maxDurability;
}
