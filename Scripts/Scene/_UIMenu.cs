using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _UIMenu : MonoBehaviour
{
    #region 컴포넌트 참조 영역
    [SerializeField]
    Button _backButtons;
    Button _startButtn;
    [SerializeField]
    Button[] _modeButtons;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트
    /// <summary> 디버깅용 에디터 이벤트 메서드 (Awake나 Start에 동일한 코드를 작성 할 것) </summary>
    private void OnValidate()
    {
        //InitButtonSetup();
        //AddListener();
    }

    private void Awake()
    {
        InitButtonSetup();
        AddListener();
    }
    #endregion

    /***********************************************************************
    *                               Private Method
    ***********************************************************************/
    #region Priavte 함수
    private void InitButtonSetup()
    {
        _backButtons = transform.Find("Back").GetComponent<Button>();
        _startButtn = transform.Find("StartGame").GetComponent<Button>();
        _modeButtons = transform.Find("ModSetting").GetComponentsInChildren<Button>();
    }

    private void AddListener()
    {
        _backButtons.onClick.AddListener(Back);
        _startButtn.onClick.AddListener(StartGame);

    }

    #endregion

    /***********************************************************************
    *                               Button Listener
    ***********************************************************************/
    #region 버튼 리스너
    private void Back()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void StartGame()
    {
        SceneLoader.Inst.LoadScene(5); // 시작 씬 할당
    }
    #endregion
}
