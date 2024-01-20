using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class _UITitle : MonoBehaviour
{
    #region ������Ʈ ���� ����
    [SerializeField]
    Button[] _titleButtons;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ
    /// <summary> ������ ������ �̺�Ʈ �޼��� (Awake�� Start�� ������ �ڵ带 �ۼ� �� ��) </summary>
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
    #region Priavte �Լ�
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

    #region ��ư ������
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