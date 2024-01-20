using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    /***********************************************************************
*                               Option Fields
***********************************************************************/
    #region .
    [Tooltip("슬롯 내에서 아이콘과 슬롯 사이의 여백")]
    [SerializeField] private float _padding = 1.0f;

    [Tooltip("아이템 아이콘 이미지")]
    [SerializeField] private Image _iconImage;

    [Tooltip("아이템 개수 텍스트")]
    [SerializeField] private TMPro.TMP_Text _amountText;
    #endregion

    /***********************************************************************
    *                               Properties
    ***********************************************************************/
    #region 프로퍼티
    /// <summary> 슬롯의 인덱스 </summary>
    public int SlotIndex;

    [SerializeField]
    public ItemData _item;

    public ItemData ItemProperties
    {
        get { return _item; }
        set { _item = value; }
    }

    /// <summary> 슬롯의 타입 </summary>
    public ItemType SlotState;

    /// <summary> 슬롯이 아이템을 보유하고 있는지 여부 </summary>
    public bool HasItem => ItemProperties != null;

    /// <summary> 접근 가능한 슬롯인지 여부 </summary>
    public bool IsAccessible => _isAccessibleSlot && _isAccessibleItem;
    #endregion
    /***********************************************************************
    *                               Fields
    ***********************************************************************/
    #region 필드
    /// <summary> 인벤토리 </summary>
    private Inventory _Inventory;
    /// <summary> 인벤토리UI </summary>
    private InventoryUI _InventoryUI;
    /// <summary> 슬롯 </summary>
    private SlotItem _slotItem;

    /// <summary> 슬롯 이미지 </summary>
    private Image _slotImage;

    private RectTransform _slotRect;
    private RectTransform _iconRect;

    [SerializeField]
    private GameObject _iconGo;
    [SerializeField]
    private GameObject _textGo;

    /// <summary> 슬롯 접근가능 여부 </summary>
    private bool _isAccessibleSlot;
    /// <summary> 아이템 접근가능 여부 </summary>
    private bool _isAccessibleItem;

    /// <summary> 비활성화된 슬롯의 색상 </summary>
    private static readonly Color InaccessibleSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    /// <summary> 비활성화된 아이콘 색상 </summary>
    private static readonly Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    #endregion

    /***********************************************************************
    *                               Unity Event
    ***********************************************************************/
    #region
    private void OnValidate()
    {
        InitComponenet();
        InitValue();
    }

    private void Awake()
    {

    }

    private void Start()
    {
    }
    private void Update()
    {
        // GameObject
        _iconGo = this.GetComponentInChildren<SlotItem>().gameObject;
        _textGo = this.GetComponentInChildren<TMPro.TMP_Text>().gameObject;

        if (ItemProperties != null)
        {
            SetItem(ItemProperties.ItemImage);
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
    #region
    /// <summary> 초기 설정 함수 </summary>
    private void InitComponenet ()
    {
        // Scripts
        _Inventory = GetComponentInParent<Inventory>();
        _InventoryUI = GetComponentInParent<InventoryUI>();
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
        // 1. Item Icon, Highlight Rect

        //_iconRect.pivot = new Vector2(0.5f, 0.5f); // 피벗은 중앙
        //_iconRect.anchoredPosition = new Vector2(0.0f, 15.0f); // 위치
        //_iconRect.sizeDelta = new Vector2(80.0f, 32.5f); // 사이즈

        //_iconRect.pivot = new Vector2(0.5f, 0.5f); // 피벗은 중앙
        //_iconRect.anchoredPosition = new Vector2(0.5f, 0.5f); // 위치
        //_iconRect.sizeDelta = new Vector2(25.0f, 25.0f); // 사이즈


        // 2. Image
        _iconImage.raycastTarget = true;

        // 3. Deactivate Icon
        //HideIcon();
    }

    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);

    private void ShowText() => _textGo.SetActive(true);
    private void HideText() => _textGo.SetActive(false);

    private void GetItemInfo(List<Item> list)
    {
        if (list[SlotIndex].Data != null)
        {
            ItemProperties = list[SlotIndex].Data;
        }
    }
    private void GetItemInfo(Item item)
    {
        //if (item != null)
        //{
        //    ItemProperties = item.Data;
        //}
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public 함수

    /// <summary> 슬롯 자체의 활성화/비활성화 여부 제어 </summary>
    public void SetSlotAccessState(bool value)
    {
        // 중복 처리는 지양
        if (_isAccessibleSlot == value) return;

        if (value)
        {
            // 슬롯 이미지 색 활성화
            _slotImage.color = Color.white;
        }
        else
        {
            // 아이템 이미지 색 활성화
            _slotImage.color = InaccessibleSlotColor;
            //HideIcon();
            //HideText();
        }

        _isAccessibleSlot = value;
    }
    
    /// <summary> 아이템 활성화/비활성화 여부 제어 </summary>
    public void SetItemAccessState(bool value)
    {
        // 중복 처리는 지양
        if (_isAccessibleItem == value) return;

        if (value)
        {
            _iconImage.raycastTarget = true;
            _iconImage.color = Color.white;
            _amountText.color = Color.white;

        }
        else
        {
            _iconImage.raycastTarget = false;
            _iconImage.color = InaccessibleIconColor;
            _amountText.color = InaccessibleIconColor;
        }

        _isAccessibleItem = value;
    }
    #endregion

    /// <summary> 슬롯에 아이템 등록 </summary>
    public void SetItem(Sprite sprite)
    {
        //if (!this.IsAccessible) return;

        if (sprite != null)
        {
            _iconImage.sprite = sprite;
            ShowIcon();
            //SetItemAccessState(true);
            //SetSlotAccessState(true);
        }
        else
        {
            RemoveItem();
            //SetItemAccessState(false);
        }
    }
    
    /// <summary> 슬롯에서 아이템 제거 </summary>
    public void RemoveItem()
    {
        _iconImage.sprite = null;
        //HideIcon();
        //HideText();
    }

    /// <summary> 아이템 개수 텍스트 설정(amount가 1 이하일 경우 텍스트 미표시) </summary>
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

    /// <summary> OnDrop 이벤트가 발생했을 때 실행 될 이벤트 </summary>
    /// <param name="eventData">OnDrop이 발생 되는 지점의 오브젝트</param>
    public void OnDrop(PointerEventData eventData)
    {
        SlotItem item = eventData.pointerDrag.GetComponent<SlotItem>();
        Slot itemSlot = eventData.pointerDrag.GetComponentInParent<Slot>();

        if (item != null)
        {
            item.SwapItem(_Inventory.Items, this.transform);
            //item.ChangeParent(this.transform);
        }
        this.GetComponentInChildren<SlotItem>().CurIndex = SlotIndex;
    }
}
