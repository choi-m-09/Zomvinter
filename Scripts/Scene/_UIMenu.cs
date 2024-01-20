using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _UIMenu : MonoBehaviour
{
    #region ������Ʈ ���� ����
    [SerializeField]
    Button _backButtons;
    Button _startButtn;
    [SerializeField]
    Button[] _modeButtons;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ
    /// <summary> ������ ������ �̺�Ʈ �޼��� (Awake�� Start�� ������ �ڵ带 �ۼ� �� ��) </summary>
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
    #region Priavte �Լ�
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
    #region ��ư ������
    private void Back()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void StartGame()
    {
        SceneLoader.Inst.LoadScene(5); // ���� �� �Ҵ�
    }
    #endregion
}
