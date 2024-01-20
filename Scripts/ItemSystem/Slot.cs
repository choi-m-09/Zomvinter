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
    [Tooltip("���� ������ �����ܰ� ���� ������ ����")]
    [SerializeField] private float _padding = 1.0f;

    [Tooltip("������ ������ �̹���")]
    [SerializeField] private Image _iconImage;

    [Tooltip("������ ���� �ؽ�Ʈ")]
    [SerializeField] private TMPro.TMP_Text _amountText;
    #endregion

    /***********************************************************************
    *                               Properties
    ***********************************************************************/
    #region ������Ƽ
    /// <summary> ������ �ε��� </summary>
    public int SlotIndex;

    [SerializeField]
    public ItemData _item;

    public ItemData ItemProperties
    {
        get { return _item; }
        set { _item = value; }
    }

    /// <summary> ������ Ÿ�� </summary>
    public ItemType SlotState;

    /// <summary> ������ �������� �����ϰ� �ִ��� ���� </summary>
    public bool HasItem => ItemProperties != null;

    /// <summary> ���� ������ �������� ���� </summary>
    public bool IsAccessible => _isAccessibleSlot && _isAccessibleItem;
    #endregion
    /***********************************************************************
    *                               Fields
    ***********************************************************************/
    #region �ʵ�
    /// <summary> �κ��丮 </summary>
    private Inventory _Inventory;
    /// <summary> �κ��丮UI </summary>
    private InventoryUI _InventoryUI;
    /// <summary> ���� </summary>
    private SlotItem _slotItem;

    /// <summary> ���� �̹��� </summary>
    private Image _slotImage;

    private RectTransform _slotRect;
    private RectTransform _iconRect;

    [SerializeField]
    private GameObject _iconGo;
    [SerializeField]
    private GameObject _textGo;

    /// <summary> ���� ���ٰ��� ���� </summary>
    private bool _isAccessibleSlot;
    /// <summary> ������ ���ٰ��� ���� </summary>
    private bool _isAccessibleItem;

    /// <summary> ��Ȱ��ȭ�� ������ ���� </summary>
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
    /// <summary> �ʱ� ���� �Լ� </summary>
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

        //_iconRect.pivot = new Vector2(0.5f, 0.5f); // �ǹ��� �߾�
        //_iconRect.anchoredPosition = new Vector2(0.0f, 15.0f); // ��ġ
        //_iconRect.sizeDelta = new Vector2(80.0f, 32.5f); // ������

        //_iconRect.pivot = new Vector2(0.5f, 0.5f); // �ǹ��� �߾�
        //_iconRect.anchoredPosition = new Vector2(0.5f, 0.5f); // ��ġ
        //_iconRect.sizeDelta = new Vector2(25.0f, 25.0f); // ������


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
    #region Public �Լ�

    /// <summary> ���� ��ü�� Ȱ��ȭ/��Ȱ��ȭ ���� ���� </summary>
    public void SetSlotAccessState(bool value)
    {
        // �ߺ� ó���� ����
        if (_isAccessibleSlot == value) return;

        if (value)
        {
            // ���� �̹��� �� Ȱ��ȭ
            _slotImage.color = Color.white;
        }
        else
        {
            // ������ �̹��� �� Ȱ��ȭ
            _slotImage.color = InaccessibleSlotColor;
            //HideIcon();
            //HideText();
        }

        _isAccessibleSlot = value;
    }
    
    /// <summary> ������ Ȱ��ȭ/��Ȱ��ȭ ���� ���� </summary>
    public void SetItemAccessState(bool value)
    {
        // �ߺ� ó���� ����
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

    /// <summary> ���Կ� ������ ��� </summary>
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
    
    /// <summary> ���Կ��� ������ ���� </summary>
    public void RemoveItem()
    {
        _iconImage.sprite = null;
        //HideIcon();
        //HideText();
    }

    /// <summary> ������ ���� �ؽ�Ʈ ����(amount�� 1 ������ ��� �ؽ�Ʈ ��ǥ��) </summary>
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

    /// <summary> OnDrop �̺�Ʈ�� �߻����� �� ���� �� �̺�Ʈ </summary>
    /// <param name="eventData">OnDrop�� �߻� �Ǵ� ������ ������Ʈ</param>
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
