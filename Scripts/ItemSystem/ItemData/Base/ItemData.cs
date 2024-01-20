using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemData : ScriptableObject
{

    public ItemType ItemType => _type;
    public int ID => _id; // 인덱스
    public string ItemName => _itemName; // 아이템 이름
    public Sprite ItemImage => _itemImage; // 아이템 대표 이미지
    public GameObject ItemPrefab => _itemPrefab; // 바닥에 떨어질 때 생성할 프리팹
    public string ItemTooltip => _itemTooltip; // 아이템 설명

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
