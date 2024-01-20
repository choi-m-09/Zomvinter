using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemData : ScriptableObject
{

    public ItemType ItemType => _type;
    public int ID => _id; // �ε���
    public string ItemName => _itemName; // ������ �̸�
    public Sprite ItemImage => _itemImage; // ������ ��ǥ �̹���
    public GameObject ItemPrefab => _itemPrefab; // �ٴڿ� ������ �� ������ ������
    public string ItemTooltip => _itemTooltip; // ������ ����

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
