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
    /// <summary> ������ �̹����� �θ� ���� </summary>
    [SerializeField] private Transform CurParent = null;

    private Inventory _inventory;
    #endregion

    /***********************************************************************
    *                               Properties
    ***********************************************************************/
    #region ������Ƽ
    /// <summary> ������ �̹����� ���� �ε��� </summary>
    public int CurIndex = 0;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ
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
    #region Private �Լ�
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
    /// <summary> �θ� ���� </summary>
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

    /// <summary> ��ġ�� �ٲ� �������� Inventory.Items ����Ʈ���� Swap </summary>
    /// <typeparam name="Item">�κ��丮 ����Ʈ�� �ҷ��� Ÿ��</typeparam>
    /// <param name="items">�ٲ� �������� ������ �ӽ� ���� �� ������ ����</param>
    /// <param name="Swap">�ٲ��� ���ϴ� ����� ����</param>
    public void SwapItem(List<ItemData> items, Transform Other)
    {
        //�ӽ� ���� �� ������ Ÿ�� ����
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
    #region ���콺 �̺�Ʈ
    public void OnPointerClick(PointerEventData eventData)
    {
        Slot ClickedSlot = eventData.pointerClick.GetComponentInParent<Slot>();
        // 1. ���Կ� ������ Ŭ���� �� ���
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            // 1-1. ������ Backpack �����̰� ������ ������ ������ �ִ� ���
            if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Any)
            {
                Debug.Log("Right Click");
                ItemData item;
                item = ClickedSlot.ItemProperties;

                //1-1-1. ������ ������ Primary�� ���
                if (item.ItemType == ItemType.Primary)
                {
                    Debug.Log("�ֹ��� ����");
                    // ������ ����Ʈ�� ���� á���� üũ (False�� ���)
                    if (!_inventory.isFull(_inventory.PrimaryItems))
                    {
                        // ������ ����Ʈ�� �� �ε��� ã��
                        int Index = _inventory.FindEmptySlotIndex(_inventory.PrimaryItems, _inventory.PrimaryItems.Count);
                        _inventory.PrimaryItems[Index] = item;

                        ClickedSlot.ItemProperties = null;
                        _inventory.Items[ClickedSlot.SlotIndex] = null;


                    }
                    // ���� �� ���
                    else
                    {
                        ItemData temp;
                        temp = _inventory.PrimaryItems[0];
                        _inventory.PrimaryItems[0] = item;


                        _inventory.Items[ClickedSlot.SlotIndex] = temp;
                    }

                }
                //1-1-2. ������ ������ Secondary�� ���
                else if (item.ItemType == ItemType.Secondary)
                {
                    Debug.Log("�������� ����");
                    // �������� ����Ʈ�� ���� á���� üũ (False�� ���)
                    if (!_inventory.isFull(_inventory.SecondaryItems))
                    {
                        _inventory.SecondaryItems = item;

                        ClickedSlot.ItemProperties = null;
                        _inventory.Items[ClickedSlot.SlotIndex] = null;
                    }
                    // ���� �� ���
                    else
                    {
                        ItemData temp;
                        temp = _inventory.SecondaryItems;
                        _inventory.SecondaryItems = item;

                        _inventory.Items[ClickedSlot.SlotIndex] = temp;
                    }
                }
                //1-1-3. ������ ������ Expand�� ���
                else if (item.ItemType == ItemType.Expand)
                {
                    Debug.Log("�Ҹ�ǰ ����");
                    // �Ҹ�ǰ ����Ʈ�� ���� á���� üũ (False�� ���)
                    if (!_inventory.isFull(_inventory.ConsumableItems))
                    {
                        // ������ ����Ʈ�� �� �ε��� ã��
                        int Index = _inventory.FindEmptySlotIndex(_inventory.ConsumableItems, _inventory.ConsumableItems.Count);

                        _inventory.ConsumableItems[Index] = item;

                        ClickedSlot.ItemProperties = null;
                        _inventory.Items[ClickedSlot.SlotIndex] = null;
                    }
                    // ���� �� ���
                    else
                    {
                        ItemData temp;
                        temp = _inventory.ConsumableItems[0];
                        _inventory.ConsumableItems[0] = item;


                        _inventory.Items[ClickedSlot.SlotIndex] = temp;
                    }
                }
                //1-1-4. ������ ������ Helmet�� ���
                else if (item.ItemType == ItemType.Helmet)
                {

                }
                //1-1-5. ������ ������ Bodyarmor�� ���
                else if (item.ItemType == ItemType.Bodyarmor)
                {

                }
                //1-1-1. ������ ������ Backpack�� ���
                else if (item.ItemType == ItemType.Backpack)
                {

                }
            }
            // 1-2-1. ������ �� ���� �����̰� ������ ������ ������ �ִ� ���
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Primary)
            {
                // ������ ����Ʈ�� ���� á���� üũ (False�� ���)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    _inventory.PrimaryItems[ClickedSlot.SlotIndex] = null;
                }
            }
            // 1-3-1. ������ ���� ���� �����̰� ������ ������ ������ �ִ� ���
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Secondary)
            {
                // ������ ����Ʈ�� ���� á���� üũ (False�� ���)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    Debug.Log("�������� ����");
                    _inventory.SecondaryItems = null;
                }
            }
            // 1-4-1. ������ �Ҹ�ǰ �����̰� ������ ������ ������ �ִ� ���
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Expand)
            {
                // ������ ����Ʈ�� ���� á���� üũ (False�� ���)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    _inventory.ConsumableItems[ClickedSlot.SlotIndex] = null;
                }
            }
            // 1-5-1. ������ ��� �����̰� ������ ������ ������ �ִ� ���
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Helmet)
            {
                // ������ ����Ʈ�� ���� á���� üũ (False�� ���)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    _inventory.HelmetItem = null;
                }
            }
            // 1-6-1. ������ �� �����̰� ������ ������ ������ �ִ� ���
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Bodyarmor)
            {
                // ������ ����Ʈ�� ���� á���� üũ (False�� ���)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    _inventory.BodyArmorItem = null;
                }
            }
            // 1-7-1. ������ ���� �����̰� ������ ������ ������ �ִ� ���
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Backpack)
            {
                // ������ ����Ʈ�� ���� á���� üũ (False�� ���)
                if (!_inventory.isFull(_inventory.Items))
                {
                    int Index = _inventory.FindEmptySlotIndex(_inventory.Items, _inventory.Items.Count);
                    _inventory.Items[Index] = ClickedSlot.ItemProperties;
                    _inventory.BackpackItem = null;
                }
            }
        }
        // 2. ���Կ� ���� Ŭ���� �� ���
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 2-1. ������ �Ҹ�ǰ �����̰� ������ ������ ������ �ִ� ���
            if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Expand)
            {
                //������ ���
                PotionItemData po = (PotionItemData)ClickedSlot.ItemProperties;
            }
        }
    }
    /// <summary> �巡�� ���� �� </summary>
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

    /// <summary> �巡�� �� </summary>
    public void OnDrag(PointerEventData eventData)
    {
        Slot MoveSlot = eventData.pointerClick.GetComponentInParent<Slot>();
        if (MoveSlot.SlotState == ItemType.Any)
        {
            this.transform.position = eventData.position;
            IsOverUI = EventSystem.current.IsPointerOverGameObject();
        }
    }

    /// <summary> �巡�� ���� �� </summary>
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