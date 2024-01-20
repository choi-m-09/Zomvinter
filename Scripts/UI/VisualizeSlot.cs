using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizeSlot : MonoBehaviour
{
    [SerializeField]
    ///<summary> ���� Ÿ�� </summary>
    private ItemType SlotType;

    #region ������Ʈ ���� ����
    ///<summary> ������ ������ </summary>
    private ItemData Data = null;

    ///<summary> ������ �̹��� ������Ʈ </summary>
    private GameObject _itemGo;
    ///<summary> ������ �̹��� </summary>
    private Image _itemImage;

    ///<summary> ������ �ؽ�Ʈ ������Ʈ </summary>
    private GameObject _textGo;
    ///<summary> ������ �ؽ�Ʈ </summary>
    private TMPro.TMP_Text _itemText;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ

    /// <summary> ������ ������ �̺�Ʈ �޼��� (Awake�� Start�� ������ �ڵ带 �ۼ� �� ��) </summary>
    private void OnValidate()
    {
        FindItemGo();
        FIndTextGO();
        GetScripts();
    }

    private void Awake()
    {
        FindItemGo();
        FIndTextGO(); 
        GetScripts();
    }

    private void Update()
    {
        UpdateIcon();
    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region Private �Լ�
    private void ShowText() => _textGo.SetActive(true);
    private void HideText() => _textGo.SetActive(false);

    private void GetScripts()
    {
        _itemImage = _itemGo.GetComponent<Image>();
        _itemText = _textGo.GetComponent<TMPro.TMP_Text>();
    }

    /// <summary> ������ �����ͷκ��� �̹����� �ؽ�Ʈ�� ���� </summary>
    private void UpdateIcon()
    {
        // 1. ������ �����Ͱ� �ִ� ���
        if (Data != null)
        {
            // Active�� ���� ����
            ShowText();
            _itemImage.sprite = Data.ItemImage;

            // 1-1. ������ �����Ͱ� CountableData�� ���
            if (Data is CountableItemData)
            {
                //_itemText.text = (CountableItem)((CountableItemData)Data)
            }
            // 1-2 ������ �����Ͱ� CountableData�� �ƴ� ���
            else
            {
                _itemText.text = Data.ItemName;
            }
        }
        // 2. ������ �����Ͱ� ���� ���
        else
        {
            _itemImage.sprite = null;
            _itemText.text = "";
            //Deactive�� ���� ���߿�
            HideText();
        }
    }

    /// <summary> _itemGo ���� ������Ʈ ã�� �޼��� </summary>
    private void FindItemGo()
    {
        if(_itemGo == null)
        {
            _itemGo = this.transform.GetChild(0).gameObject;
        }
    }

    /// <summary> _textGo ���� ������Ʈ ã�� �޼��� </summary>
    private void FIndTextGO()
    {
        // 1. _textGo�� null �� ���
        if(_textGo == null)
        {
            _textGo = this.transform.GetChild(1).gameObject;
            
            // 1-1. SlotType�� Primary �̰ų� Secondary �� ���
            if (this.SlotType == ItemType.Primary || this.SlotType == ItemType.Secondary)
            {
                _textGo = _textGo.transform.GetChild(0).gameObject;
            }
        }
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public �Լ�
    public void SetItemData(ItemData data) { Data = data; }
    #endregion
}
