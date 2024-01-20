using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{
    #region ��ũ��Ʈ ���� ����
    ///<summary> Canvas </summary>
    private Canvas _canvas;
    ///<summary> Canvas �ȿ� �ִ� Inventory.cs </summary>
    private Inventory _inventory;

    ///<summary> �� ���� ���� ��ũ��Ʈ </summary>
    private VisualizeSlot Primary;
    ///<summary> ���� ���� ���� ��ũ��Ʈ </summary>
    private VisualizeSlot Seconcdary;
    ///<summary> �Ҹ�ǰ ���� ��ũ��Ʈ </summary>
    private VisualizeSlot[] Expands;
    #endregion

    #region ������Ʈ ���� ����
    ///<summary> �� ���� ���� ���� </summary>
    private GameObject _primaryGo;
    ///<summary> ���� ���� ���� ���� </summary>
    private GameObject _secondaryGo;
    ///<summary> �Ҹ�ǰ ���� ���� </summary>
    private GameObject _expandGo;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ

    /// <summary> ������ ������ �̺�Ʈ �޼��� (Awake�� ������ �ڵ带 �ۼ� �� ��) </summary>
    private void OnValidate()
    {
        GetScripts();
    }

    private void Awake()
    {
        GetScripts();
    }

    void Update()
    {
        UpdateSlot();
    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region Private �Լ�
    /// <summary> �ʿ� ��ũ��Ʈ ���� �޼��� </summary>
    private void GetScripts()
    {
        // Inventory.cs ����
        _canvas = GetComponentInParent<Canvas>();
        _inventory = _canvas.GetComponentInChildren<Inventory>();

        // GameObject ���� ����
        _primaryGo = transform.GetChild(0).Find("CurPrimary").gameObject;
        _secondaryGo = transform.GetChild(0).Find("CurSecondary").gameObject;
        _expandGo = transform.Find("Item").gameObject;

        // VisualizeSlot ����
        Primary = _primaryGo.GetComponentInChildren<VisualizeSlot>();
        Seconcdary = _secondaryGo.GetComponentInChildren<VisualizeSlot>();
        Expands = _expandGo.GetComponentsInChildren<VisualizeSlot>();
    }

    /// <summary> HUD�� ǥ�� �� ���Ե��� ������ �� ���� �޼��� </summary>
    private void UpdateSlot()
    {
        // 1. ���� �������� �� ���Ⱑ null �� �ƴ� ���
        if (_inventory.PrimaryItems[0] != null) // ���� ���� �ʿ�
        {
            Primary.SetItemData(_inventory.PrimaryItems[0]);
        }
        // 1-1. null �� ���
        else
        {
            Primary.SetItemData(null);
        }

        // 2. ���� �������� �������Ⱑ null �� �ƴ� ���
        if (_inventory.SecondaryItems != null)
        {
            Seconcdary.SetItemData(_inventory.SecondaryItems);
        }
        // 2-1. null �� ���
        else
        {
            Seconcdary.SetItemData(null);
        }

        for (int i = 0; i < _inventory.ConsumableItems.Count; i++)
        {
            // 3. ���� �������� �Ҹ�ǰ�� null�� �ƴ� ���
            if (_inventory.ConsumableItems[i] != null)
            {
                Expands[i].SetItemData(_inventory.ConsumableItems[i]);
            }
            // 3-1. null �� ���
            else
            {
                Expands[i].SetItemData(null);
            }
        }
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public �Լ�
    /// <summary> HUD ���̱� </summary>
    public void ShowHUD() => gameObject.SetActive(true);
    /// <summary> HUD ����� </summary>
    public void HideHUD() => gameObject.SetActive(false);
    #endregion

}
