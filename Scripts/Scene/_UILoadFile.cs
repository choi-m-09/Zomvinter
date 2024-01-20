using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _UILoadFile : MonoBehaviour
{
    #region ������Ʈ ���� ����
    [SerializeField]
    Button _backButton;
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
        _backButton = transform.Find("Back").GetComponent<Button>();
    }

    private void AddListener()
    {
        _backButton.onClick.AddListener(Back);
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
    #endregion
}
