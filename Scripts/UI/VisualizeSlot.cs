using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizeSlot : MonoBehaviour
{
    [SerializeField]
    private ItemType SlotType;

    private GunItem gunItem = null;

    private ItemData _itemData = null;

    private GameObject _itemGo;

    private Image _itemImage;

    private GameObject _textGo;

    private TMPro.TMP_Text _itemText;


    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region Unity Events

    /// <summary> ������ ������ �̺�Ʈ �޼��� (Awake�� Start�� ������ �ڵ带 �ۼ� �� ��) </summary>
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
    #region Private Method
    private void ShowText() => _textGo.SetActive(true);
    private void HideText() => _textGo.SetActive(false);

    private void GetScripts()
    {
        _itemImage = _itemGo.GetComponent<Image>();
        _itemText = _textGo.GetComponentInChildren<TMPro.TMP_Text>();
    }

    /// <summary> 아이템 이미지 업데이트 </summary>
    private void UpdateIcon()
    {

        if (_itemData != null)
        {

            ShowText();
            _itemImage.sprite = _itemData.ItemImage;

            // 1-1. 수량이 있는 아이템인 경우
            if (_itemData is CountableItemData)
            {
                //_itemText.text = (CountableItem)((CountableItemitem.Data)item.Data)
            }
            
            else
            {
                if(gunItem == null)_itemText.text = _itemData.ItemName;
                else _itemText.text = _itemData.ItemName + " : " + gunItem.c_bullet.ToString() + "/ " + gunItem.GunData.Capacity.ToString();
            }
        }
        // 2. itemData가 없을 때 텍스트 및 이미지 비활성화
        else
        {
            _itemImage.sprite = null;
            _itemText.text = "";
            HideText();
        }
    }

    /// <summary> 자식에 있는 이미지 불러오기 </summary>
    private void FindItemGo()
    {
        if(_itemGo == null)
        {
            _itemGo = this.transform.GetChild(0).gameObject;
        }
    }

    /// <summary> 자식에 있는 텍스트 불러오기 </summary>
    private void FIndTextGO()
    {
        if(_textGo == null)
        {
            _textGo = this.transform.GetChild(1).gameObject;
            
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
    public void SetItemData(Slot data) 
    {
        if (data._item is GunItem gi) gunItem = gi; 
        _itemData = data.ItemProperties;
    }
    #endregion
}
