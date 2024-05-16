using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    /***********************************************************************
*                               Option Fields
***********************************************************************/
    #region .
    [Tooltip("슬롯 패딩값")]
    [SerializeField] private float _padding = 1.0f;

    [Tooltip("아이템 아이콘")]
    [SerializeField] private Image _iconImage;

    [Tooltip("아이템 수량 텍스트")]
    [SerializeField] private TMPro.TMP_Text _amountText;
    #endregion

    /***********************************************************************
    *                               Properties
    ***********************************************************************/
    public int SlotIndex;

    public Item _item;

    private ItemData itemData;

    public ItemData ItemProperties
    {
        get
        {
            return itemData;
        }
        set
        {
            itemData = value;
        }
    }

    /// <summary> 슬롯이 아이템을 가지고 있는지 여부 </summary>
    public bool HasItem => ItemProperties != null;
    /***********************************************************************
    *                               Fields
    ***********************************************************************/
    #region �ʵ�
    public ItemType SlotState;
    /// <summary> ���� </summary>
    private SlotItem _slotItem;

    /// <summary> 슬롯에 나타나는 이미지 </summary>
    private Image _slotImage;

    private RectTransform _slotRect;
    private RectTransform _iconRect;

    [SerializeField]
    private GameObject _iconGo;
    [SerializeField]
    private GameObject _textGo;

    /// <summary>  </summary>
    private bool _isAccessibleSlot;
    /// <summary> ������ ���ٰ��� ���� </summary>
    private bool _isAccessibleItem;

    /// <summary>  </summary>
    private static readonly Color InaccessibleSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    /// <summary> ��Ȱ��ȭ�� ������ ���� </summary>
    private static readonly Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    #endregion

    /***********************************************************************
    *                               Unity Event
    ***********************************************************************/
    #region
    private void OnValidate()
    {
        InitComponent();
        InitValue();
    }

    void Start()
    {
        _iconGo = this.GetComponentInChildren<SlotItem>().gameObject;
        _textGo = this.GetComponentInChildren<TMPro.TMP_Text>().gameObject;
    }

    private void Update()
    {
        if (_item != null) ItemProperties = SetItemData(_item);
        else ItemProperties = null;

        if (ItemProperties != null)
        {
            SetItem(ItemProperties.ItemImage);
            if (_item is CountableItem ci && ci.Amount > 1)
            {
                if (_amountText.gameObject.activeSelf == false) ShowText();
                SetItemAmount(ci.Amount);
            }
            else HideText();
        }
        else
        {
            SetItem(null);
        }

    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    /// <summary> �ʱ� ���� �Լ� </summary>
    private void InitComponent()
    {
        // Scripts
        _slotItem = GetComponentInChildren<SlotItem>();

        // Rect
        _amountText = GetComponentInChildren<TMPro.TMP_Text>();
        _slotRect = GetComponent<RectTransform>();
        _iconRect = _iconImage.rectTransform;

        // Image
        _slotImage = GetComponent<Image>();

    }

    private void InitValue()
    {
        _iconImage.raycastTarget = true;
    }

    private void ShowIcon() => _iconGo.SetActive(true);

    private void ShowText() => _textGo.SetActive(true);
    private void HideText() => _textGo.SetActive(false);


    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    public void SetItem(Sprite sprite)
    {
        if (sprite != null)
        {
            _iconImage.sprite = sprite;
            ShowIcon();
        }
        else
        {
            RemoveItem();
        }
    }

    /// <summary> 아이템 데이터 캐스팅 </summary>
    public ItemData SetItemData(Item item)
    {
        if (item is EquipmentItem ei) return ei.EquipmentData;
        else if (item is CountableItem ci) return ci.CountableData;
        return null;
    }
    /// <summary> 슬롯이 비었을 때 슬롯 배경 이미지 및 텍스트 초기화 </summary>
    public void RemoveItem()
    {
        _iconImage.sprite = null;
        HideText();
    }

    /// <summary> 수량이 1개 이하인 경우 텍스트 미표시 </summary>
    public void SetItemAmount(int amount)
    {
        //if (!this.IsAccessible) return;

        if (HasItem && amount > 1)
        {
            ShowText();
        }
        else
        {
            HideText();
        }

        _amountText.text = amount.ToString();
    }

    /***********************************************************************
    *                               Old Methods
    ***********************************************************************/

    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
        SlotItem item = eventData.pointerDrag.GetComponent<SlotItem>();
        Slot itemSlot = eventData.pointerDrag.GetComponentInParent<Slot>();

        if (item != null)
        {
            item.SwapItem(Inventory._inventory.Items, this.transform);
            //item.ChangeParent(this.transform);
        }
        this.GetComponentInChildren<SlotItem>().CurIndex = SlotIndex;
    }
}