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

    #endregion

    /***********************************************************************
    *                               Properties
    ***********************************************************************/
    #region 
    /// <summary> </summary>
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
        Debug.Log("Swap Item");
        SetIndex();
    }

    /// <summary> ��ġ�� �ٲ� �������� Inventory.Items ����Ʈ���� Swap </summary>
    /// <typeparam name="Item">�κ��丮 ����Ʈ�� �ҷ��� Ÿ��</typeparam>
    /// <param name="items">�ٲ� �������� ������ �ӽ� ���� �� ������ ����</param>
    /// <param name="Swap">�ٲ��� ���ϴ� ����� ����</param>
    public void SwapItem(Item[] items, Transform Other)
    {
        //�ӽ� ���� �� ������ Ÿ�� ����
        Item temp;
        int OtherIndex = Other.GetComponentInChildren<Slot>().SlotIndex;
        Item OtherItem = Other.GetComponentInParent<Slot>()._item;

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
    #region Mouse Event
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

                //1-1-1. 주무기 장착
                if (item.ItemType == ItemType.Primary)
                {
                    // 비어있는 주무기 칸 찾은 후 할당
                    if (!Inventory._inventory.isFull(Inventory._inventory.PrimaryItems))
                    {
                        int Index = Inventory._inventory.FindEmptySlotIndex(Inventory._inventory.PrimaryItems);
                        Inventory._inventory.PrimaryItems[Index] = ClickedSlot._item;

                        ClickedSlot._item = null;
                        Inventory._inventory.Items[ClickedSlot.SlotIndex] = null;


                    }
                    // 주무기 칸이 이미 할당 되었을 때 스왑
                    else
                    {
                        Item temp;
                        temp = Inventory._inventory.PrimaryItems[0];
                        Inventory._inventory.PrimaryItems[0] = ClickedSlot._item;


                        Inventory._inventory.Items[ClickedSlot.SlotIndex] = temp;
                    }

                }
                //1-1-2. 보조무기 장착
                else if (item.ItemType == ItemType.Secondary)
                {
                    // 보조무기 미착용일 경우 할당
                    if (Inventory._inventory.SecondaryItem == null)
                    {
                        Inventory._inventory.SecondaryItem = ClickedSlot._item;

                        ClickedSlot._item = null;
                        Inventory._inventory.Items[ClickedSlot.SlotIndex] = null;
                    }
                    // 이미 착용 되어있을 경우 스왑
                    else
                    {
                        Item temp;
                        temp = Inventory._inventory.SecondaryItem;
                        Inventory._inventory.SecondaryItem = ClickedSlot._item;

                        Inventory._inventory.Items[ClickedSlot.SlotIndex] = temp;
                    }
                }
                //1-1-3. 소비 아이템인 경우 사용
                else if (item.ItemType == ItemType.Consumable)
                {
                    Inventory._inventory.Use(GameObject.Find("Player").GetComponent<Player>(),ClickedSlot.SlotIndex);
                }
                //1-1-4. Helmet 착용(구현 예정)
                else if (item.ItemType == ItemType.Helmet)
                {

                }
                //1-1-5. Bodyarmor 착용(구현 예정)
                else if (item.ItemType == ItemType.Bodyarmor)
                {

                }
                //1-1-1. Backpack 착용(구현 예정)
                else if (item.ItemType == ItemType.Backpack)
                {

                }
            }
            // 1-2-1. 주무기 탈착
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Primary)
            {   
                // 백팩 공간 확인
                if (!Inventory._inventory.isFull(Inventory._inventory.Items))
                {
                    int Index = Inventory._inventory.FindEmptySlotIndex(Inventory._inventory.Items);
                    Inventory._inventory.Items[Index] = ClickedSlot._item;
                    Inventory._inventory.PrimaryItems[ClickedSlot.SlotIndex] = null;
                }
            }
            // 1-3-1. 권총 탈착
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Secondary)
            {   
                if (!Inventory._inventory.isFull(Inventory._inventory.Items))
                {
                    int Index = Inventory._inventory.FindEmptySlotIndex(Inventory._inventory.Items);
                    Inventory._inventory.Items[Index] = ClickedSlot._item;
                    Inventory._inventory.SecondaryItem = null;
                }
            }
            // 1-4-1. 퀵슬롯 탈착
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Consumable)
            {   
                if (!Inventory._inventory.isFull(Inventory._inventory.Items))
                {
                    int Index = Inventory._inventory.FindEmptySlotIndex(Inventory._inventory.Items);
                    Inventory._inventory.Items[Index] = ClickedSlot._item;
                    Inventory._inventory.ConsumableItems[ClickedSlot.SlotIndex] = null;
                }
            }
            // 1-5-1. 핼멧 탈착
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Helmet)
            {
                if (!Inventory._inventory.isFull(Inventory._inventory.Items))
                {
                    int Index = Inventory._inventory.FindEmptySlotIndex(Inventory._inventory.Items);
                    Inventory._inventory.Items[Index] = ClickedSlot._item;
                    Inventory._inventory.HelmetItem = null;
                }
            }
            // 1-6-1. 방어구 탈착
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Bodyarmor)
            {
                
                if (!Inventory._inventory.isFull(Inventory._inventory.Items))
                {
                    int Index = Inventory._inventory.FindEmptySlotIndex(Inventory._inventory.Items);
                    Inventory._inventory.Items[Index] = ClickedSlot._item;
                    Inventory._inventory.BodyArmorItem = null;
                }
            }
            
            else if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Backpack)
            {
                
                if (!Inventory._inventory.isFull(Inventory._inventory.Items))
                {
                    int Index = Inventory._inventory.FindEmptySlotIndex(Inventory._inventory.Items);
                    Inventory._inventory.Items[Index] = ClickedSlot._item;
                    Inventory._inventory.BackpackItem = null;
                }
            }
        }
        // 2. ���Կ� ���� Ŭ���� �� ���
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 2-1. 확장창으로 이동할 수 있는 버튼 UI 띄우기(개발 예정)
            if (ClickedSlot.ItemProperties != null && ClickedSlot.SlotState == ItemType.Consumable)
            {
                
                
            }
        }
    }
    /// <summary> �巡�� ���� �� </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
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
            if (IsOverUI)
            {
                this.transform.localPosition = Vector2.zero;
                this.gameObject.GetComponent<Image>().raycastTarget = true;
            }
            else
            {
            Vector3 DropPos = GameObject.Find("Player").transform.position;
                DropPos.y += 2.0f;
                DropPos.x += 1.0f;

                this.transform.localPosition = Vector2.zero;
                this.gameObject.GetComponent<Image>().raycastTarget = true;
    
                int CurIndex = this.GetComponentInParent<Slot>().SlotIndex;

                Inventory._inventory.Items[CurIndex].gameObject.transform.position = DropPos;

                Inventory._inventory.Items[CurIndex].gameObject.SetActive(true);

                Inventory._inventory.Items[CurIndex] = null;

                this.GetComponentInParent<Slot>().RemoveItem();

            }
        //}
    }
    #endregion
}