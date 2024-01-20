using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    [Item의 상속구조]
    - Item
        - Consumable : IUsableItem.Use() -> 사용 및 수량 1 소모

        - EquipmentItem
            - WeaponItem
            - ArmorItem

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
    - bool IsCountableItem(int) : 해당 인덱스의 아이템이 셀 수 있는 아이템인지 여부
    - int GetCurrentAmount(int) : 해당 인덱스의 아이템 수량
        - -1 : 잘못된 인덱스
        -  0 : 빈 슬롯
        -  1 : 셀 수 없는 아이템이거나 수량 1
    - ItemData GetItemData(int) : 해당 인덱스의 아이템 정보
    - string GetItemName(int) : 해당 인덱스의 아이템 이름

    - int Add(ItemData, int) : 해당 타입의 아이템을 지정한 개수만큼 인벤토리에 추가
        - 자리 부족으로 못넣은 개수만큼 리턴(0이면 모두 추가 성공했다는 의미)
    - void Remove(int) : 해당 인덱스의 슬롯에 있는 아이템 제거
    - void Swap(int, int) : 두 인덱스의 아이템 위치 서로 바꾸기
    - void SeparateAmount(int a, int b, int amount)
        - a 인덱스의 아이템이 셀 수 있는 아이템일 경우, amount만큼 분리하여 b 인덱스로 복제
    - void Use(int) : 해당 인덱스의 아이템 사용
    - void UpdateSlot(int) : 해당 인덱스의 슬롯 상태 및 UI 갱신
    - void UpdateAllSlot() : 모든 슬롯 상태 및 UI 갱신
    - void UpdateAccessibleStatesAll() : 모든 슬롯 UI에 접근 가능 여부 갱신
    - void TrimAll() : 앞에서부터 아이템 슬롯 채우기
    - void SortAll() : 앞에서부터 아이템 슬롯 채우면서 정렬

// 날짜 : 2021-03-07 PM 7:33:52
*/

public class Inventory : MonoBehaviour
{
    /***********************************************************************
    *                               Public Properties
    ***********************************************************************/
    #region Public 프로퍼티
    /// <summary> 내 플레이어의 위치 </summary>
    public Transform myPlayerPos;

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
    #region 
    /// <summary> 연결된 InventoryUI 스크립트 </summary>
    [SerializeField]
    private InventoryUI _inventoryUI;

    #region 가방

    /// <summary> 백팩 아이템 목록 리스트 </summary>
    public List<ItemData> Items;
    /// <summary> 백팩 최대 수용 한도 </summary>
    [Range(4, 16)]
    private int _itemsMaxCapacity = 16;
    #endregion -------------------------------------------------------------------

    #region 주무기
    /// <summary> 주 무기 아이템 목록 리스트 </summary>
    public List<ItemData> PrimaryItems;

    /// <summary> 아이템 최대 수용 한도 </summary>
    [SerializeField,Range(2, 2)]
    private int _PrimaryMaxCapacity = 2;
    #endregion -------------------------------------------------------------------

    #region 보조무기
    /// <summary> 보조 무기 아이템 목록 리스트 </summary>
    public ItemData SecondaryItems;

    /// <summary> 보조무기 최대 수용 한도 </summary>
    [SerializeField, Range(1, 1)]
    private int _SecondaryMaxCapacity = 1;
    #endregion -------------------------------------------------------------------

    #region 소모품
    /// <summary> 소모품 아이템 목록 리스트 </summary>
    public List<ItemData> ConsumableItems;
    public List<ItemData> ArmorItems;

    /// <summary> 소모품 최대 수용 한도 </summary>
    [SerializeField, Range(1, 3)]
    private int _ConsumableMaxCapacity = 3;
    [SerializeField, Range(1, 3)]
    private int _ArmorItemsMaxCapacity = 3;
    #endregion -------------------------------------------------------------------

    #region 장비
    /// <summary> 헬멧에 저장 될 아이템 </summary>
    public ItemData HelmetItem = null;

    /// <summary> 방어구에 저장 될 아이템 </summary>
    public ItemData BodyArmorItem = null;

    /// <summary> 가방에 저장 될 아이템 </summary>
    public ItemData BackpackItem = null;

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

    #region
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

#endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region



    /// <summary> 에디터 상에서 실행 되는 함수 </summary>
    private void OnValidate()
    {
        ConnectUI(GetComponent<InventoryUI>());

        ItemCapacity = SetInitalCapacity(_itemsMaxCapacity);
        PrimaryCapacity = SetInitalCapacity(_PrimaryMaxCapacity);
        SecondaryCapacity = SetInitalCapacity(_SecondaryMaxCapacity);
        ConsumableCapacity = SetInitalCapacity(_ConsumableMaxCapacity);

        InitSlot(out ItemSlots, ItemBag);
        InitSlot(out PrimarySlots, PrimaryBag);
        InitSlot(out SecondarySlots, SecondaryBag);
        InitSlot(out ConsumableSlots, ConsumableBag);
    }
    #endregion

    private void Start()
    {
        UpdateAccessibleStatesAll();
    }

    private void Update()
    {
        UpdateAllSlotData(Items, ItemSlots);
        UpdateAllSlotIcon(ItemSlots);
        UpdateAllSlotData(PrimaryItems, PrimarySlots);
        UpdateAllSlotIcon(PrimarySlots);
        UpdateSlotData(SecondaryItems, SecondarySlots);
        UpdateAllSlotIcon(SecondarySlots);
        UpdateAllSlotData(ConsumableItems, ConsumableSlots);
        UpdateAllSlotIcon(ConsumableSlots);
        UpdateSlotData(HelmetItem, HelmetSlot);
        UpdateAllSlotIcon(HelmetSlot);
        UpdateSlotData(BodyArmorItem, BodyArmorSlot);
        UpdateAllSlotIcon(BodyArmorSlot);
        UpdateSlotData(BackpackItem, BackpackSlot);
        UpdateAllSlotIcon(BackpackSlot);
    }
    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region
    /// <summary> Bag에 있는 슬롯을 찾아서 Slot배열에 할당 </summary>
    private void InitSlot(out Slot[] _slotList, Transform Bag)
    {
        _slotList = Bag.GetComponentsInChildren<Slot>();
        InitSlotIndex(_slotList);
    }

    /// <summary> Slot에 인덱스 할당 </summary>
    private void InitSlotIndex(Slot[] _slotList)
    {
        for(int i = 0; i < _slotList.Length; i++)
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
    private int FindCountableItemSlotIndex(CountableItemData target, int Capacity, List<Item>list, int startIndex = 0)
    {
        for(int i = startIndex; i< Capacity; i++)
        {
            var current = list[i];
            if (current == null) continue;

            // 아이템 종류 일치, 개수 여유 확인
            if(current.Data == target && current is CountableItem ci)
            {
                if (!ci.IsMax) return i;
            }
        }

        return -1;
    }

    /// <summary> 해당하는 인덱스의 슬롯 상태 및 UI 갱신 </summary>
    private void UpdateAllSlotData(List<ItemData> list, Slot[] slotList)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                ItemData item = list[i];

                slotList[i].ItemProperties = item;
            }
            else
            {
                slotList[i].ItemProperties = null;
            }
        }
    }

    private void UpdateSlotData(ItemData item, Slot[] slotList)
    {
        for (int i = 0; i < slotList.Length; i++)
        {
            if (item != null)
            {
                slotList[i].ItemProperties = item;
            }
            else
            {
                slotList[i].ItemProperties = null;
            }
        }
    }
    /// <summary> 해당하는 인덱스의 슬롯 상태 및 UI 갱신 </summary>
    private void UpdateAllSlotIcon(Slot[] _slotList)
    {
        for(int i = 0; i < _slotList.Length; i++)
        {
            if (_slotList[i].ItemProperties != null)
            {
                _slotList[i].SetItem(_slotList[i].ItemProperties.ItemImage);
            }
            else
            {
                _slotList[i].SetItem(null);
            }
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
    public int FindEmptySlotIndex(List<ItemData> list, int Capacity, int StartIndex = 0)
    {
        for (int i = StartIndex; i < Capacity; i++)
        {
            if (list[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary> 인벤토리가 꽉 찼는지 탐색 </summary>
    public bool isFull(List<ItemData> list)
    {
        bool Full = false;
        for (int i = 0; i < list.Capacity; i++)
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

    public bool isFull(ItemData item)
    {
        if (item == null) return false;
        else return true;
    }

    /// <summary> 해당 슬롯이 아이템을 갖고 있는지 여부 </summary>
    public bool HasItem(int Index, int Capacity, List<Item> list)
    {
        return IsValidIndex(Index, Capacity) && list[Index] != null;
    }


    /// <summary> 해당 슬롯이 셀 수 있는 아이템인지 여부 </summary>
    public bool IsConsumableItem(int Index, int Capacity, List<Item> list)
    {
        return HasItem(Index, Capacity, list) && list[Index] is PotionItem;
    }

    /// <summary> 
    /// 해당 슬롯의 현재 아이템 개수 리턴
    /// <para/> - 잘못된 인덱스 : -1 리턴
    /// <para/> - 빈 슬롯 : 0 리턴
    /// <para/> - 셀 수 없는 아이템 : 1 리턴
    /// </summary>
    public int GetCurrentAmount(int Index, int Capacity, List<ItemData> list)
    {
        if (!IsValidIndex(Index, Capacity)) return -1;
        if (list[Index] == null) return 0;

        PotionItemData con = list[Index] as PotionItemData;
        if (con == null) return 1;

        return con.MaxAmount;
    }

    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public 함수

    public void ConnectUI(InventoryUI inventoryUI)
    {
        _inventoryUI = inventoryUI;
        _inventoryUI.SetInventoryReference(this);
    }

    /// <summary> 모든 슬롯 UI에 접근 가능 여부 업데이트 </summary>
    public void UpdateAccessibleStatesAll()
    {
        _inventoryUI.SetAccessibleSlotRange(ItemCapacity, ItemSlots);
        _inventoryUI.SetAccessibleSlotRange(PrimaryCapacity, PrimarySlots);
        _inventoryUI.SetAccessibleSlotRange(SecondaryCapacity, SecondarySlots);
        _inventoryUI.SetAccessibleSlotRange(ConsumableCapacity, ConsumableSlots);
        // 헬멧
        // 방어구
        // 가방
    }

    public void FindItem()
    {

    }
    #endregion
}