using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemData : ScriptableObject
{

    public ItemType ItemType => _type;
    public int ID => _id; // 아이템 ID (아이템 수량 검사 또는 확인 시 사용)
    public string ItemName => _itemName; // 아이템 이름
    public Sprite ItemImage => _itemImage; // 인벤토리에 나타나는 아이템 Sprite
    public GameObject ItemPrefab => _itemPrefab; // Drop시 게임 필드에 생성될 아이템 
    public string ItemTooltip => _itemTooltip; // 마우스 Over 시 나타날 Tooltip

    [SerializeField]
    private ItemType _type = ItemType.Any;
    [SerializeField]
    private int _id;
    [SerializeField]
    private string _itemName;
    [SerializeField]
    private Sprite _itemImage;
    [SerializeField]
    private GameObject _itemPrefab;
    [SerializeField]
    private string _itemTooltip;
}
