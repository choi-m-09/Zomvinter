using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Equipment_", menuName = "Inventory System/Item Data/Equipment/Weapon/Gun", order = 2)]
public class GunItemData : EquipmentItemData
{
    public GameObject Bullet => _bullet;
    [SerializeField] private GameObject _bullet;
    
    public GameObject Effect => _effect;
    [SerializeField] private GameObject _effect;

    public int Capacity => _capacity;
    [SerializeField] private int _capacity;

}
