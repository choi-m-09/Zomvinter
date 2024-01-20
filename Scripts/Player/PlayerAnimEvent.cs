using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
    #region ���� ����
    Player _player;
    #endregion

    #region ��������Ʈ ����
    public event UnityAction OnAttackKnife = null;
    public event UnityAction EndAttackKnife = null;
    public event UnityAction GetRifle = null;
    public event UnityAction GetPistol = null;
    public event UnityAction PutGun = null;
    public event UnityAction AnimStart = null;
    public event UnityAction AnimEnd = null;
    #endregion

    public Transform Knife; // Į(��������)

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ
    private void Awake()
    {
        _player = this.GetComponentInParent<Player>();
    }
    #endregion

    /***********************************************************************
    *                               Anim Events
    ***********************************************************************/
    #region �ִϸ��̼� �̺�Ʈ
    /// <summary> ���� ���� �ִϸ��̼� ���� </summary>
    public void StartStabbing()
    {
        OnAttackKnife?.Invoke();
    }

    /// <summary> ���� ���� �ִϸ��̼� �� </summary>
    public void EndStabbing()
    {
        EndAttackKnife?.Invoke();
    }

    /// <summary> �ֹ��� ���� </summary>
    public void OnGetRifle()//�� ������� �ִϸ��̼ǿ� �۵�
    {
        GetRifle?.Invoke();
        if (_player.isFirst) _player.GetGun(0); // �ֹ��� 1��° ������ �������� Hand ���Ͽ� ����
        if (_player.isSecond) _player.GetGun(1); // �ֹ��� 2��° ������ �������� Hand ���Ͽ� ����
        _player.UpdateBackWeapon(); // �� ���� ������Ʈ
    }

    /// <summary> �������� ���� </summary>
    public void OnGetPistol()
    {
        GetPistol?.Invoke();
        _player.GetPistol(); // �ι��� ������ �������� Hand ���Ͽ� ����
        _player.UpdateBackWeapon(); // �� ���� ������Ʈ
    }

    /// <summary> ���� ���� </summary>
    public void OnPutGun()
    {
        PutGun?.Invoke();
        _player.PutGun();
        _player.UpdateBackWeapon(); // �� ���� ������Ʈ
    }

    /// <summary> �ִϸ��̼� ���� ���� </summary>
    public void OnAnimStart()
    {
        AnimStart?.Invoke();
        _player.AnimStart();
    }

    /// <summary> �ִϸ��̼� ���� �� </summary>
    public void OnAnimEnd()
    {
        AnimEnd?.Invoke();
        _player.AnimEnd();
    }
    #endregion
}
