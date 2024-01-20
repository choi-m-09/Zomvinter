using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class _UITitle : MonoBehaviour
{
    #region 컴포넌트 참조 영역
    [SerializeField]
    Button[] _titleButtons;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트
    /// <summary> 디버깅용 에디터 이벤트 메서드 (Awake나 Start에 동일한 코드를 작성 할 것) </summary>
    private void OnValidate()
    {
        InitButtonSetup();
        AddListener();
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
            _titleButtons = GetComponentsInChildren<Button>();
        
    }

    private void AddListener()
    {
        _titleButtons[0].onClick.AddListener(New);
        _titleButtons[1].onClick.AddListener(Continue);
        _titleButtons[2].onClick.AddListener(Setting);
        _titleButtons[3].onClick.AddListener(Quit);
    }

    #endregion

    /***********************************************************************
    *                               Button Listener
    ***********************************************************************/

    #region 버튼 리스너
    private void New()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void Continue()
    {
        SceneManager.LoadSceneAsync(2);
    }

    private void Setting()
    {
        SceneManager.LoadSceneAsync(3);
    }

    private void Quit()
    {
        Application.Quit();
    }
    #endregion

}