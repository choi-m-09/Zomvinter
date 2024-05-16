using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Item_Countable_", menuName = "Inventory System/Item Data/Countable/Ammo", order = 2)]
public class AmmoItemData : CountableItemData
{
    /// <summary> ź�� ������ </summary>
    public float AmmoDamage => _ammoDamage;
    [SerializeField] private float _ammoDamage;

    /// <summary> ź�� �ӵ� </summary>
    public float BulletSpeed => _bulletSpeed;
    [SerializeField] private float _bulletSpeed;
}
