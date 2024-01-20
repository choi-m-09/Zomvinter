using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentItemData : ItemData
{
    public float MaxDurability => _maxDurability; // ³»±¸µµ

    [SerializeField]
    private float _maxDurability;
}
