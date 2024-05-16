using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeItem : EquipmentItem
{
    [SerializeField]
    private MeleeItemData meleeData;
    public MeleeItemData MeleeData => meleeData;
    
    public Transform hitPoint;

    public MeleeItem(MeleeItemData data) : base(data) { }

    private void Awake()
    {
        base.EquipmentData = meleeData;
    }

}
