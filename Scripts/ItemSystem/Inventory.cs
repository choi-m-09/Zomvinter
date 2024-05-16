using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
/*
    [Item의 상속구조]
    - Item
        - Consumable : IUsableItem.Use() -> 사용 및 수량 1 소모

        - EquipmentItem
            - WeaponItem

    [ItemData의 상속구조]
      (ItemData는 해당 아이템이 공통으로 가질 데이터 필드 모음)
      (개체마다 달라져야 하는 현재 내구도, 강화도 등은 Item 클래스에서 관리)

    - ItemData
        - ConsumableData : 효과량(Value : 회복량, 공격력 등에 사용)

        - EquipmentData : 최대 내구도
            - WeaponData : 기본 공격력
            - ArmorData : 기본 방어력
*/

/*
    [API]
    - bool HasItem(int) : 해당 인덱스의 슬롯에 아이템이 존재하는지 여부
    - int GetCurrentAmount(int) : 해당 인덱스의 아이템 수량
        - -1 : 잘못된 인덱스
        -  0 : 빈 슬롯
        -  1 : 셀 수 없는 아이템이거나 수량 1
    - ItemData GetItemData(int) : 해당 인덱스의 아이템 정보
    - string GetItemName(int) : 해당 인덱스의 아이템 이름

    - int Add(Item, int) : 해당 타입의 아이템을 지정한 개수만큼 인벤토리에 추가
    - void Remove(int) : 해당 인덱스의 슬롯에 있는 아이템 제거
    - void Swap(int, int) : 두 인덱스의 아이템 위치 서로 바꾸기
    - void Use(int) : 해당 인덱스의 아이템 사용
*/

public class Inventory : MonoBehaviour
{
    /***********************************************************************
    *                               Public Properties
    ***********************************************************************/
    #region Public 프로퍼티

    public static Inventory _inventory;

    public static bool is_craftWin = false;
    /// <summary> 백팩 수용 한도 </summary>
    [SerializeField]
    public int ItemCapacity { get; set; }

    /// <summary> 아이템 수용 한도 </summary>
    [SerializeField]
    public int PrimaryCapacity { get; set; }

    /// <summary> 보조무기 수용 한도 </summary>
    [SerializeField]
    public int SecondaryCapacity { get; set; }

    /// <summary> 소모품 수용 한도 </summary>
    [SerializeField]
    public int ConsumableCapacity { get; set; }

    public int EquipmentCapacity { get; set; }

    #endregion
    /***********************************************************************
    *                               Private Fields
    ***********************************************************************/
    #region 가방

    /// <summary> 백팩 아이템 목록 리스트 </summary>
    public Item[] Items;
    /// <summary> 백팩 최대 수용 한도 </summary>
    [Range(4, 16)]
    private int _itemsMaxCapacity = 16;
    #endregion -------------------------------------------------------------------

    #region 주무기
    /// <summary> 주 무기 아이템 목록 리스트 </summary>
    public Item[] PrimaryItems;

    /// <summary> 아이템 최대 수용 한도 </summary>
    [SerializeField, Range(2, 2)]
    private int _PrimaryMaxCapacity = 2;
    #endregion -------------------------------------------------------------------

    #region 보조무기
    /// <summary> 보조 무기 아이템 목록 리스트 </summary>
    public Item SecondaryItem;

    /// <summary> 보조무기 최대 수용 한도 </summary>
    [SerializeField, Range(1, 1)]
    private int _SecondaryMaxCapacity = 1;
    #endregion -------------------------------------------------------------------

    #region 소모품
    /// <summary> 소모품 아이템 목록 리스트 </summary>
    public Item[] ConsumableItems;
    public Item[] ArmorItems;

    /// <summary> 소모품 최대 수용 한도 </summary>
    [SerializeField, Range(1, 3)]
    private int _ConsumableMaxCapacity = 3;
    [SerializeField, Range(1, 3)]
    private int _ArmorItemsMaxCapacity = 3;
    #endregion -------------------------------------------------------------------

    #region 장비
    /// <summary> 헬멧에 저장 될 아이템 </summary>
    public Item HelmetItem = null;

    /// <summary> 방어구에 저장 될 아이템 </summary>
    public Item BodyArmorItem = null;

    /// <summary> 가방에 저장 될 아이템 </summary>
    public Item BackpackItem = null;

    #endregion -------------------------------------------------------------------

    #region 슬롯
    /// <summary> Slot을 담을 리스트 </summary>
    public Slot[] ItemSlots;

    /// <summary> Slot을 담을 리스트 </summary>
    public Slot[] PrimarySlots;

    /// <summary> Slot을 담을 공간 </summary>
    public Slot[] SecondarySlots;

    /// <summary> Slot을 담을 리스트 </summary>
    public Slot[] ConsumableSlots;

    /// <summary> Slot을 담을 공간 </summary>
    public Slot[] HelmetSlot;
    /// <summary> Slot을 담을 공간 </summary>
    public Slot[] BodyArmorSlot;
    /// <summary> Slot을 담을 공간 </summary>
    public Slot[] BackpackSlot;
    #endregion

    #region 슬롯 위치
    [Header("Connected Objects")]
    /// <summary> 슬롯들이 위치할 영역 </summary>
    [SerializeField] private Transform ItemBag;
    /// <summary> 슬롯들이 위치할 영역 </summary>
    [SerializeField] private Transform PrimaryBag;
    /// <summary> 슬롯들이 위치할 영역 </summary>
    [SerializeField] private Transform SecondaryBag;
    /// <summary> 슬롯들이 위치할 영역 </summary>
    [SerializeField] private Transform ConsumableBag;
    /// <summary> 슬롯들이 위치할 영역 </summary>
    [SerializeField] private Transform EquipmentBag;
    #endregion

    [SerializeField] GameObject CraftUI;
    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region



    /// <summary> 에디터 상에서 실행 되는 함수 </summary>
    private void OnValidate()
    {
        ItemCapacity = SetInitalCapacity(_itemsMaxCapacity);
        PrimaryCapacity = SetInitalCapacity(_PrimaryMaxCapacity);
        SecondaryCapacity = SetInitalCapacity(_SecondaryMaxCapacity);
        ConsumableCapacity = SetInitalCapacity(_ConsumableMaxCapacity);

        InitSlot(out ItemSlots, ItemBag);
        InitSlot(out PrimarySlots, PrimaryBag);
        InitSlot(out SecondarySlots, SecondaryBag);
        InitSlot(out ConsumableSlots, ConsumableBag);

        Items = new Item[ItemCapacity];
    }
    #endregion

    private void Awake()
    {
        if (_inventory == null) _inventory = this;
        else Destroy(this.gameObject);
    }

    private void Update()
    {
        UpdateAllSlotData(Items, ItemSlots);
        UpdateAllSlotData(PrimaryItems, PrimarySlots);
        UpdateSlotData(SecondaryItem, SecondarySlots);
        UpdateAllSlotData(ConsumableItems, ConsumableSlots);
        UpdateSlotData(HelmetItem, HelmetSlot);
        UpdateSlotData(BodyArmorItem, BodyArmorSlot);
        UpdateSlotData(BackpackItem, BackpackSlot);
    }
    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region Private 메소드
    /// <summary> Bag에 있는 슬롯을 찾아서 Slot배열에 할당 </summary>
    private void InitSlot(out Slot[] _slotList, Transform Bag)
    {
        _slotList = Bag.GetComponentsInChildren<Slot>();
        InitSlotIndex(ref _slotList);
    }

    /// <summary> Slot에 인덱스 할당 </summary>
    private void InitSlotIndex(ref Slot[] _slotList)
    {
        for (int i = 0; i < _slotList.Length; i++)
        {
            _slotList[i].SlotIndex = i;
        }
    }

    /// <summary> 인덱스가 수용 범위 내에 있는지 검사 </summary>
    private bool IsValidIndex(int Index, int Capacity)
    {
        return Index >= 0 && Index < Capacity;
    }

    /// <summary> 앞에서부터 개수 여유가 있는 Countable 아이템의 슬롯 인덱스 탐색 </summary>
    private int FindCountableItemSlotIndex(Item[] list,CountableItemData target, int startIndex = 0)
    {
        for (int i = startIndex; i < list.Length; i++)
        {
            if (list[i] != null)
            {
                CountableItem current = list[i] as CountableItem;
                if (current == null) continue;

                // 아이템 종류 일치, 개수 여유 확인
                if (current.CountableData == target)
                {
                    if (!current.IsMax) return i;
                }
            }
        }

        return -1;
    }

    /// <summary> 해당하는 인덱스의 슬롯 상태 및 UI 갱신 </summary>
    private void UpdateAllSlotData(Item[] list, Slot[] slotList)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] != null)
            {
                Item item = list[i];

                slotList[i]._item = item;
            }
            else
            {
                slotList[i]._item = null;
            }
        }
    }

    private void UpdateSlotData(Item item, Slot[] slotList)
    {
        for (int i = 0; i < slotList.Length; i++)
        {
            if (item != null)
            {
                slotList[i]._item = item;
            }
            else
            {
                slotList[i]._item = null;
            }
        }
    }

    private void Set_TotalIngredient(IngredientItem item)
    {
        switch (item.IngredientData.ID)
        {
            case 12043:
                CraftManual.Total_CopperAmount += item.Amount;
                break;
            case 12044:
                CraftManual.Total_IronAmount += item.Amount;
                break;
        }
    }

    private void Mius_TotalAmount(int id, int amount)
    {
        switch (id)
        {
            case 12044:
                CraftManual.Total_IronAmount -= amount;
                break;
            case 12043:
                CraftManual.Total_CopperAmount -= amount;
                break;
        }
    }
    #endregion
    /***********************************************************************
    *                               Check & Getter Methods
    ***********************************************************************/
    #region 
    /// <summary> 가방 초기용량 설정 함수 </summary>
    int SetInitalCapacity(int inital)
    {
        return inital;
    }

    /// <summary> 앞에서부터 비어있는 슬롯 인덱스 탐색 </summary>
    public int FindEmptySlotIndex(Item[] list,int StartIndex = 0)
    {
        for (int i = StartIndex; i < list.Length; i++)
        {
            if (list[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary> 인벤토리가 꽉 찼는지 탐색 </summary>
    public bool isFull(Item[] list)
    {
        bool Full = false;
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i] == null)
            {
                Full = false;
                break;
            }
            else
            {
                Full = true;
            }
        }
        return Full;
    }

    public void Remove(int index)
    {
        GameObject go = Items[index].gameObject;
        Items[index] = null;
        Destroy(go);
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public 메소드
    public void ShowCraftUI()
    {
        is_craftWin = !is_craftWin;
        if (is_craftWin) CraftUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 0);
        else CraftUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(700, 2000);
    }
    public void Add(Item item, int amount = 1)
    {
        int index;
        if (item is CountableItem ci)
        {
            if (ci is IngredientItem ing_i) Set_TotalIngredient(ing_i); 

            bool findnextSlot = true;
            index = -1;
            while (ci.Amount > 0)
            {
                if (findnextSlot)
                {
                    index = FindCountableItemSlotIndex(Items,ci.CountableData, index + 1);
                    if (index != -1)
                    {
                        CountableItem root = Items[index] as CountableItem;
                        while (!root.IsMax && ci.Amount > 0)
                        {
                            root.Amount++;
                            ci.Amount--;
                        }
                    }
                    else findnextSlot = false;
                }
                else
                {
                    index = FindEmptySlotIndex(Items ,index + 1);
                    if (index != -1)
                    {
                        Items[index] = ci;
                        break;
                    }
                    else break;
                    
                }
            }
            if (ci.Amount <= 0) Destroy(ci.gameObject);
        }
        else
        {
            index = FindEmptySlotIndex(Items);
            if (index != -1 && item as EquipmentItem) Items[index] = item;
        }
    }


    public int FindAmmo(AmmoItemData data, int startIndex = 0)
    {
        for (int i = startIndex; i < Items.Length; i++)
        {
            if (Items[i] != null)
                if (Items[i] is AmmoItem ai && ai.AmmoData == data ) return i;
        }
        return -1;
    }

    public int Count_Ingredient(IngredientItemData data, int needAmount)
    {
        for(int i = 0; i < Items.Length; i++)
        {
            if (Items[i] is IngredientItem item && item.IngredientData == data)
            {
                bool flag = false;
                while(needAmount > 0 && !flag)
                {
                    if(needAmount < item.Amount)
                    {
                        item.Amount -= needAmount;
                        Mius_TotalAmount(item.IngredientData.ID, needAmount);
                        return 1;
                    }
                    else
                    {
                        needAmount -= item.Amount;
                        Mius_TotalAmount(item.IngredientData.ID, item.Amount);
                        Remove(i);
                        flag = true;
                    }
                }
            }
        }

        return 0;
    }

    public void Use(Player _player,int index)
    {
        if (Items[index] == null) return;

        if (Items[index] is IUsableItem UseI)
        {
            UseI.Use(_player);
            CountableItem ci = UseI as CountableItem;
            if (ci.Amount <= 0) Remove(index);
        }
    }
    #endregion
}