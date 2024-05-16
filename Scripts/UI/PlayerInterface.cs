using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterface : MonoBehaviour
{
    #region 캔버스 오브젝트 및 PlayetInterface 슬롯
    ///<summary> Canvas </summary>
    [SerializeField]
    private GameObject _canvas;
    ///<summary> Canvas �ȿ� �ִ� Inventory.cs </summary>

    ///<summary> �� ���� ���� ��ũ��Ʈ </summary>
    private VisualizeSlot Primary;
    ///<summary> ���� ���� ���� ��ũ��Ʈ </summary>
    private VisualizeSlot Seconcdary;
    ///<summary> �Ҹ�ǰ ���� ��ũ��Ʈ </summary>
    private VisualizeSlot[] Expands;
    #endregion

    #region 
    ///<summary>  </summary>
    private GameObject _primaryGo;
    ///<summary>  </summary>
    private GameObject _secondaryGo;
    ///<summary>  </summary>
    private GameObject _expandGo;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region Unity Events

    /// <summary> ������ ������ �̺�Ʈ �޼��� (Awake�� ������ �ڵ带 �ۼ� �� ��) </summary>

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
    #region Private Methods
    /// <summary>  </summary>
    private void GetScripts()
    {
        _primaryGo = transform.GetChild(0).Find("CurPrimary").gameObject;
        _secondaryGo = transform.GetChild(0).Find("CurSecondary").gameObject;
        _expandGo = transform.Find("Item").gameObject;

        Primary = _primaryGo.GetComponentInChildren<VisualizeSlot>();
        Seconcdary = _secondaryGo.GetComponentInChildren<VisualizeSlot>();
        Expands = _expandGo.GetComponentsInChildren<VisualizeSlot>();
    }

    /// <summary>  </summary>
    private void UpdateSlot()
    {
        // 1. 주무기 슬롯 인터페이스
        if (Inventory._inventory.PrimarySlots[0] != null) 
        {
            Primary.SetItemData(Inventory._inventory.PrimarySlots[0]);
        }

        else
        {
            Primary.SetItemData(null);
        }

        // 2. 보조무기 슬롯 인터페이스
        if (Inventory._inventory.SecondarySlots[0] != null)
        {
            Seconcdary.SetItemData(Inventory._inventory.SecondarySlots[0]);
        }
        
        else
        {
            Seconcdary.SetItemData(null);
        }

        for (int i = 0; i < Inventory._inventory.ConsumableSlots.Length; i++)
        {
            // 3. 확장 슬롯 인터페이스
            if (Inventory._inventory.ConsumableSlots[i] != null)
            {
                Expands[i].SetItemData(Inventory._inventory.ConsumableSlots[i]);
            }
            
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
    #region public Methods
    /// <summary> HUD ���̱� </summary>
    public void ShowHUD() => gameObject.SetActive(true);
    /// <summary> HUD ����� </summary>
    public void HideHUD() => gameObject.SetActive(false);
    #endregion

}
