using UnityEngine;
using UnityEngine.UI;

enum UIType
{
    Difficulty, PlayerCount
}

public enum DiffSet
{
    Easy = 0, Normal = 2, Hard = 4, Hell = 6
}
public enum UserCount
{
    Small = 0, Medium = 2, Large = 4
}

public class _UIChooseBtn : MonoBehaviour
{
    #region 타입 참조
    [SerializeField]
    /// <summary> 기능 분류 Enum </summary>
    private UIType _uiType = UIType.Difficulty;

    /// <summary> 셋팅 속성 Enum </summary>
    private DiffSet _difficultSetting = DiffSet.Easy;
    private UserCount _userCount = UserCount.Small;
    #endregion

    #region 컴포넌트 참조 영역
    /// <summary> 셋팅 텍스트 Enum </summary>
    TMPro.TMP_Text _settingText;
    [SerializeField]
    /// <summary> 0 - 왼쪽, 1 - 오른쪽 Enum </summary> 
    Button[] _arrowBtn;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트
    /// <summary> 디버깅용 에디터 이벤트 메서드 (Awake나 Start에 동일한 코드를 작성 할 것) </summary>
    private void OnValidate()
    {
        GetComponents();
        AddListener();
    }

    void Start()
    {
        GetComponents();
        AddListener();
    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region Private 함수
    private void GetComponents()
    {
        _settingText = transform.Find("Setting").GetComponent<TMPro.TMP_Text>();
        _arrowBtn = GetComponentsInChildren<Button>();
    }
    private void AddListener()
    {
        _arrowBtn[0].onClick.AddListener(LeftBtn);
        _arrowBtn[1].onClick.AddListener(RightBtn);
    }

    private void UpdateText()
    {
        if (_uiType == UIType.Difficulty)
        {
            _settingText.text = _difficultSetting.ToString();
        }
        else if (_uiType == UIType.PlayerCount)
        {
            if (_userCount == UserCount.Small)
            {
                _settingText.text = "1~2";
            }
            else if (_userCount == UserCount.Medium)
            {
                _settingText.text = "2~4";
            }
            else if (_userCount == UserCount.Large)
            {
                _settingText.text = "4~6";
            }
        }
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public 함수
    public DiffSet GetDiffSet => _difficultSetting;
    public UserCount GetUserCount => _userCount;
    #endregion

    /***********************************************************************
    *                               Button Listener
    ***********************************************************************/
    #region 버튼 리스너
    private void LeftBtn()
    {
        if (_uiType == UIType.Difficulty)
        {
            if (_difficultSetting > 0)
            {
                _difficultSetting -= 1;
                UpdateText();
            }
        }
        else if (_uiType == UIType.PlayerCount)
        {
            if (_userCount > 0)
            {
                _userCount -= 1;
                UpdateText();
            }
        }
    }

    private void RightBtn()
    {
        Debug.Log("RightBtn Call");
        if (_uiType == UIType.Difficulty)
        {
            int MaxEnum = System.Enum.GetValues(typeof(DiffSet)).Length;
            if ((int)_difficultSetting < MaxEnum)
            {
                _difficultSetting += 1;
                UpdateText();
            }
        }
        else if (_uiType == UIType.PlayerCount)
        {
            int MaxEnum = System.Enum.GetValues(typeof(UserCount)).Length;
            if ((int)_userCount < MaxEnum)
            {
                _userCount += 1;
                UpdateText();
            }
        }
    }
    #endregion

}
