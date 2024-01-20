using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
    [기능 - 에디터 전용]
    - 게임 시작 시 동적으로 생성될 슬롯 미리보기(개수, 크기 미리보기 가능)

    [기능 - 유저 인터페이스]
    - 슬롯에 마우스 올리기
      - 사용 가능 슬롯 : 하이라이트 이미지 표시
      - 아이템 존재 슬롯 : 아이템 정보 툴팁 표시

    - 드래그 앤 드롭
      - 아이템 존재 슬롯 -> 아이템 존재 슬롯 : 두 아이템 위치 교환
      - 아이템 존재 슬롯 -> 아이템 미존재 슬롯 : 아이템 위치 변경
        - Shift 또는 Ctrl 누른 상태일 경우 : 셀 수 있는 아이템 수량 나누기
      - 아이템 존재 슬롯 -> UI 바깥 : 아이템 버리기

    - 슬롯 우클릭
      - 사용 가능한 아이템일 경우 : 아이템 사용

    - 기능 버튼(좌측 상단)
      - Trim : 앞에서부터 빈 칸 없이 아이템 채우기
      - Sort : 정해진 가중치대로 아이템 정렬

    - 필터 버튼(우측 상단)
      - [A] : 모든 아이템 필터링
      - [E] : 장비 아이템 필터링
      - [P] : 소비 아이템 필터링
      * 필터링에서 제외된 아이템 슬롯들은 조작 불가
      * 
    [기능 - 기타]
    - InvertMouse(bool) : 마우스 좌클릭/우클릭 반전 여부 설정

    // 날짜 : 2020-05-03

*/

public class InventoryUI : MonoBehaviour
{
    /***********************************************************************
    *                               Option Field
    ***********************************************************************/
    #region
    [Header("Options")]
    [SerializeField, Range(0, 10)]
    private int _horizontalSlotCount = 4; // 슬롯 가로 개수
    [SerializeField, Range(0, 10)]
    private int _verticalSlotCount = 12; // 슬롯 세로 개수
    [SerializeField] private float _slotMargin = 0.0f; // 한 슬롯의 상하좌우 여백
    [SerializeField] private float _contentAreaPadding = 20.0f; // 인벤토리 영역의 내부 여백
    [SerializeField, Range(25, 80)] private float _slotSize = 25.0f; // 각 슬롯의 크기

    [Space]
    //[SerializeField] private bool _showTolltip = true;
    //[SerializeField] private bool _showHighlist = true;
    //[SerializeField] private bool _showRemovingPopup = true;


    [SerializeField] private GameObject _slotUiPrefab;     // 슬롯의 원본 프리팹
    //[SerializeField] private ItemTooltipUI _itemTooltip;   // 아이템 정보를 보여줄 툴팁 UI
    //[SerializeField] private InventoryPopupUI _popup;      // 팝업 UI 관리 객체

    [Header("Buttons")]
    //[SerializeField] private Button _trimButton;
    //[SerializeField] private Button _sortButton;

    #endregion

    /***********************************************************************
    *                               Private Fields
    ***********************************************************************/
    #region
    /// <summary> 연결된 인벤토리 </summary>
    private Inventory _inventory;

    

    private GraphicRaycaster _gr; // 캔버스 내의 오브젝트
    private PointerEventData _ped;
    private List<RaycastResult> _rrList;


    private enum FilterOption
    {
        Primary, Secondary, Expand, Helmet, Bodyarmor, Backpack, Any
    }
    private FilterOption _currentFilterOption = FilterOption.Any;

    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region

    private void OnValidate()
    {
        Init();
    }
    #endregion

    /***********************************************************************
    *                               Init Methods
    ***********************************************************************/
    #region
    private void Init()
    {
        TryGetComponent(out _gr);
        if (_gr == null) _gr = gameObject.AddComponent<GraphicRaycaster>();

        // Graphic Raycaster
        _ped = new PointerEventData(EventSystem.current);
        _rrList = new List<RaycastResult>(10);

        // Item Tooltip UI
        //if(_itemTooltip == null)
        //{
        //    // 인스펙터에서 아이템 툴팁 UI를 직접 지정하지 않아 자식에서 발견하여 초기화
        //    _itemTooltip = GetComponentInChildren<ItemTooltipUI>();
        //}
    }
    #endregion

    /***********************************************************************
    *                               Public Fields
    ***********************************************************************/
    #region
    
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region

    #endregion


    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region
    /// <summary> 인벤토리 참조 등록 (인벤토리에서 직접 호출) </summary>
    public void SetInventoryReference(Inventory inventory)
    {
        _inventory = inventory;
    }

    /// <summary> 슬롯에 아이템 아이콘 등록 </summary>
    public void SetItemIcon(int index, Slot[] _slotUIList, Sprite icon)
    {
        //EditorLog($"Set Item Icon : Slot [{index}]");

        _slotUIList[index].SetItem(icon);
    }

    /// <summary> 해당 슬롯의 아이템 개수 텍스트 지정 </summary>
    public void SetItemAmountText(int index, Slot[] _slotUIList, int amount)
    {
        //EditorLog($"Set Item Amount Text : Slot [{index}], Amount [{amount}]");

        // NOTE : amount가 1 이하일 경우 텍스트 미표시
        _slotUIList[index].SetItemAmount(amount);
    }

    /// <summary> 해당 슬롯의 아이템 개수 텍스트 지정 </summary>
    public void HideItemAmountText(int index, List<Slot> _slotUIList)
    {
        //EditorLog($"Hide Item Amount Text : Slot [{index}]");

        _slotUIList[index].SetItemAmount(1);
    }

    /// <summary> 슬롯에서 아이템 아이콘 제거, 개수 텍스트 숨기기 </summary>
    public void RemoveItem(int index, List<Slot> _slotUIList)
    {
        //EditorLog($"Remove Item : Slot [{index}]");

        _slotUIList[index].RemoveItem();
    }

    /// <summary> 접근 가능한 슬롯 범위 설정 </summary>
    public void SetAccessibleSlotRange(int accessibleSlotCount, Slot[]_slotUIList)
    {
        for (int i = 0; i < _slotUIList.Length; i++)
        {
            _slotUIList[i].SetSlotAccessState(i < accessibleSlotCount);
        }
    }

    /// <summary> 특정 슬롯의 필터 상태 업데이트 </summary>
    public void UpdateSlotFilterState(List<Slot> _slotUIList, int index, ItemData itemData)
    {
        bool isFiltered = true;

        // null인 슬롯은 타입 검사 없이 필터 활성화
        if (itemData != null)
            switch (_currentFilterOption)
            {
                case FilterOption.Primary:
                    isFiltered = (itemData is EquipmentItemData);
                    break;
                case FilterOption.Secondary:
                    isFiltered = (itemData is EquipmentItemData);
                    break;
                case FilterOption.Helmet:
                    isFiltered = (itemData is EquipmentItemData);
                    break;
                case FilterOption.Bodyarmor:
                    isFiltered = (itemData is EquipmentItemData);
                    break;
                case FilterOption.Backpack:
                    isFiltered = (itemData is CountableItemData);
                    break;

                case FilterOption.Expand:
                    isFiltered = (itemData is CountableItemData);
                    break;
            }

        _slotUIList[index].SetItemAccessState(isFiltered);
    }

    /// <summary> 모든 슬롯 필터 상태 업데이트 </summary>
    public void UpdateAllSlotFilters(int Capacity)
    {

    }
    #endregion
}
