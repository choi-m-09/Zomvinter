using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Countable_", menuName = "Inventory System/Item Data/Countable/Ammo", order = 2)]
public class AmmoItemData : CountableItemData
{
    /// <summary> ÇÑ °³´ç Åº¾à °¹¼ö </summary>
    public float NumOfPack => _numOfPack;
    [SerializeField] private float _numOfPack;

    /// <summary> Åº¾à µ¥¹ÌÁö </summary>
    public float AmmoDamage => _ammoDamage;
    [SerializeField] private float _ammoDamage;

    /// <summary> Åº¾à ¼Óµµ </summary>
    public float BulletSpeed => _bulletSpeed;
    [SerializeField] private float _bulletSpeed;
}
