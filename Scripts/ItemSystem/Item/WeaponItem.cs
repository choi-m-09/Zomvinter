using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipmentItem
{
    /// <summary> ConsumableData로부터 가져온 정보를 Data에 저장 </summary>
    public WeaponItemData WeaponData;

    public WeaponItem(WeaponItemData data) : base(data) 
    {
        WeaponData = data;
    }

    public Transform MuzzlePoint => _muzzlePoint;
    [SerializeField] private Transform _muzzlePoint;
}