using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GunItem : EquipmentItem
{
    /// <summary> Data </summary>
    [SerializeField] private GunItemData gunData;
    public GunItemData GunData => gunData;

    public int c_bullet;

    public float duration;
    public GunItem(GunItemData data) : base(data) { }

    public Transform MuzzlePoint => _muzzlePoint;
    [SerializeField]
    private Transform _muzzlePoint;

    private void Awake()
    {
        base.EquipmentData = gunData;
        duration = Durability;
    }

    private void Update()
    {
        Mathf.Clamp(c_bullet, 0, GunData.Capacity);
        Mathf.Clamp(duration, 0, GunData.MaxDurability);
    }
}