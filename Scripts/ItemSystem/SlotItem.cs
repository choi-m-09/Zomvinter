using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private bool IsOverUI = false;

    /***********************************************************************
    *                               Option Fields
    ***********************************************************************/
    #region
    /// <summary> 아이템 이미지의 부모 슬롯 </summary>
    [SerializeField] private Transform CurParent = null;

    private Inventory _inventory;
    #endregion

    /***********************************************************************
    *                               Properties
    ***********************************************************************/
    #region 프로퍼티
    /// <summary> 아이템 이미지가 가진 인덱스 </summary>
    public int CurIndex = 0;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트
    private void Start()
    {
        SetIndex();
        SetParent();
    }

    private void Awake()
    {
        
    }

    private void OnValidate()
    {
        _inventory = GetComponentInParent<Inventory>();
    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region Private 함수
    private void SetIndex()
    {
        CurIndex = this.GetComponentInParent<Slot>().SlotIndex;
    }

    private void SetParent()
    {
        CurParent = this.transform.parent;
    }
    #endregion


    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region
    /// <summary> 부모 변경 </summary>
    public void ChangeParent(Transform parent)
    {
        SlotItem tempItem = parent.GetComponentInChildren<SlotItem>();

        if (tempItem != null)
        {
            tempItem.ChangeParent(CurParent);
        }

        CurParent = parent;
        this.transform.SetParent(CurParent);
        this.transform.localPosition = Vector2.zero;

        SetIndex();
    }

    /// <summary> 위치가 바뀐 아이템을 Inventory.Items 리스트에서 Swap </summary>
    /// <typeparam name="Item">인벤토리 리스트를 불러올 타입</typeparam>
    /// <param name="items">바뀔 아이템의 정보가 임시 저장 될 아이템 변수</param>
    /// <param name="Swap">바뀜을 당하는 대상의 정보</param>
    public void SwapItem(List<ItemData> items, Transform Other)
    {
        //임시 저장 될 아이템 타입 변수
        ItemData temp;
        int OtherIndex = Other.GetComponentInChildren<Slot>().SlotIndex;
        ItemData OtherItem = Other.GetComponentInParent<Slot>().ItemProperties;

        if (OtherItem != null)
        {
            temp = items[OtherIndex];
            items[OtherIndex] = items[CurIndex];
            items[CurIndex] = temp;
        }
        else
        {
            items[OtherIndex] = items[CurIndex];
            items[CurIndex] = null;
        }
    }
    #endregion

    /***********************************************************************
    *                               Mouse Events
    ***********************************************************************/
    #region 마우스 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        Slot ClickedSlot = eventData.pointerClick.GetComponentInParent<Slot>();
        // 1. 슬롯에 오른쪽 클릭을 한 경우
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            // 1-1. 슬롯이 Backpack 슬롯이고 아이템 정보를 가지고 있는 경우
            if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Any)
            {
                Debug.Log("Right Click");
                ItemData item;
                item = ClickedSlot.ItemProperties;

                //1-1-1. 아이템 정보가 Primary인 경우
                if (item.ItemType == ItemType.Primary)
                {
                    Debug.Log("주무기 선택");
                    // 아이템 리스트가 가득 찼는지 체크 (False인 경우)
                    if (!_inventory.isFull(_inventory.PrimaryItems))
                    {
                        // 아이템 리스트에 빈 인덱스 찾기
                        int Index = _inventory.FindEmptySlotIndex(_inventory.PrimaryItems, _inventory.PrimaryItems.Count);
                        _inventory.PrimaryItems[Index] = item;

                        ClickedSlot.ItemProperties = null;
                        _inventory.Items[ClickedSlot.SlotIndex] = null;


                    }
                    // 가득 찬 경우
                    else
                    {
                        ItemData temp;
                        temp = _inventory.PrimaryItems[0];
                        _inventory.PrimaryItems[0] = item;


                        _inventory.Items[ClickedSlot.SlotIndex] = temp;
                    }

                }
                //1-1-2. 아이템 정보가 Secondary인 경우
                else if (item.ItemType == ItemType.Secondary)
                {
                    Debug.Log("보조무기 선택");
                    // 보조무기 리스트가 가득 찼는지 체크 (False인 경우)
                    if (!_inventory.isFull(_inventory.SecondaryItems))
                    {
                        _inventory.SecondaryItems = item;

                        ClickedSlot.ItemProperties = null;
                        _inventory.Items[ClickedSlot.SlotIndex] = null;
                    }
                    // 가득 찬 경우
                    else
                    {
                        ItemData temp;
                        temp = _inventory.SecondaryItems;
                        _inventory.SecondaryItems = item;

                        _inventory.Items[ClickedSlot.SlotIndex] = temp;
                    }
                }
                //1-1-3. 아이템 정보가 Expand인 경우
                else if (item.ItemType == ItemType.Expand)
                {
                    Debug.Log("소모품 선택");
                    // 소모품 리스트가 가득 찼는지 체크 (False인 경우)
                    if (!_inventory.isFull(_inventory.ConsumableItems))
                    {
                        // 아이템 리스트에 빈 인덱스 찾기
                        int Index = _inventory.FindEmptySlotIndex(_inventory.ConsumableItems, _inventory.ConsumableItems.Count);

                        _inventory.ConsumableItems[Index] = item;

                        ClickedSlot.ItemProperties = null;
                        _inventory.Items[ClickedSlot.SlotIndex] = null;
                    }
                    // 가득 찬 경우
                    else
                    {
                        ItemData temp;
                        temp = _inventory.ConsumableItems[0];
                        _inventory.ConsumableItems[0] = item;


                        _inventory.Items[ClickedSlot.SlotIndex] = temp;
                    }
                }
                //1-1-4. 아이템 정보가 Helmet인 경우
                else if (item.ItemType == ItemType.Helmet)
                {

                }
                //1-1-5. 아이템 정보가 Bodyarmor인 경우
                else if (item.ItemType == ItemType.Bodyarmor)
                {

                }
                //1-1-1. 아이템 정보가 Backpack인 경우
                else if (item.ItemType == ItemType.Backpack)
                {

                }
            }
            // 1-2-1. 슬롯이 주 무기 슬롯이고 아이템 정보를 가지고 있는 경우
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Primary)
            {
                // 아이템 리스트가 가득 찼는지 체크 (False인 경우)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    _inventory.PrimaryItems[ClickedSlot.SlotIndex] = null;
                }
            }
            // 1-3-1. 슬롯이 보조 무기 슬롯이고 아이템 정보를 가지고 있는 경우
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Secondary)
            {
                // 아이템 리스트가 가득 찼는지 체크 (False인 경우)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    Debug.Log("보조무기 제거");
                    _inventory.SecondaryItems = null;
                }
            }
            // 1-4-1. 슬롯이 소모품 슬롯이고 아이템 정보를 가지고 있는 경우
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Expand)
            {
                // 아이템 리스트가 가득 찼는지 체크 (False인 경우)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    _inventory.ConsumableItems[ClickedSlot.SlotIndex] = null;
                }
            }
            // 1-5-1. 슬롯이 헬멧 슬롯이고 아이템 정보를 가지고 있는 경우
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Helmet)
            {
                // 아이템 리스트가 가득 찼는지 체크 (False인 경우)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    _inventory.HelmetItem = null;
                }
            }
            // 1-6-1. 슬롯이 방어구 슬롯이고 아이템 정보를 가지고 있는 경우
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Bodyarmor)
            {
                // 아이템 리스트가 가득 찼는지 체크 (False인 경우)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    _inventory.BodyArmorItem = null;
                }
            }
            // 1-7-1. 슬롯이 가방 슬롯이고 아이템 정보를 가지고 있는 경우
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Backpack)
            {
                // 아이템 리스트가 가득 찼는지 체크 (False인 경우)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    _inventory.BackpackItem = null;
                }
            }
        }
        // 2. 슬롯에 왼쪽 클릭을 한 경우
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 2-1. 슬롯이 소모품 슬롯이고 아이템 정보를 가지고 있는 경우
            if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Expand)
            {
                //아이템 사용
                PotionItemData po = (PotionItemData)ClickedSlot.ItemProperties;
            }
        }
    }
    /// <summary> 드래그 시작 시 </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //CurParent = this.transform.parent;
        //this.transform.SetParent(CurParent.parent);
        Slot MoveSlot = eventData.pointerClick.GetComponentInParent<Slot>();
        if (MoveSlot.SlotState == ItemType.Any)
        {
            this.gameObject.GetComponent<Image>().raycastTarget = false;
        }
    }

    /// <summary> 드래그 중 </summary>
    public void OnDrag(PointerEventData eventData)
    {
        Slot MoveSlot = eventData.pointerClick.GetComponentInParent<Slot>();
        if (MoveSlot.SlotState == ItemType.Any)
        {
            this.transform.position = eventData.position;
            IsOverUI = EventSystem.current.IsPointerOverGameObject();
        }
    }

    /// <summary> 드래그 끝날 시 </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        //Slot MoveSlot = eventData.pointerClick.GetComponentInParent<Slot>();
        //if (MoveSlot.SlotState == ItemType.Any)
        //{
            if (IsOverUI)
            {
                //this.transform.SetParent(CurParent);
                this.transform.localPosition = Vector2.zero;
                this.gameObject.GetComponent<Image>().raycastTarget = true;
            }
            else
            {
                Vector3 DropPos = this.GetComponentInParent<Inventory>().myPlayerPos.position;
                DropPos.y += 2.0f;

                //this.transform.SetParent(CurParent);
                this.transform.localPosition = Vector2.zero;
                this.gameObject.GetComponent<Image>().raycastTarget = true;

                if (this.gameObject.GetComponentInParent<Slot>().ItemProperties.ItemPrefab != null)
                {
                    ItemData item = this.gameObject.GetComponentInParent<Slot>().ItemProperties;
                    GameObject obj = Instantiate(this.gameObject.GetComponentInParent<Slot>().ItemProperties.ItemPrefab,
                        DropPos, Quaternion.identity);
                    //if(item.ItemType == ItemType.Expand)
                    //{
                    //    obj.AddComponent<PotionItem>();
                    //    obj.GetComponent<PotionItem>().PotionData = (PotionItemData)item;
                    //}
                    //else if (item.ItemType == ItemType.Primary || item.ItemType == ItemType.Secondary)
                    //{
                    //    obj.AddComponent<WeaponItem>();
                    //    obj.GetComponent<WeaponItem>().WeaponData = (WeaponItemData)item;
                    //}
                }

                int CurIndex = this.GetComponentInParent<Slot>().SlotIndex;

                this.gameObject.GetComponentInParent<Inventory>().Items.RemoveAt(CurIndex);

                this.GetComponentInParent<Slot>().RemoveItem();

            }
        //}
    }
    #endregion
}