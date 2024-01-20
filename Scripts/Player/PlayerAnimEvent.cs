using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
    #region 참조 영역
    Player _player;
    #endregion

    #region 딜리게이트 영역
    public event UnityAction OnAttackKnife = null;
    public event UnityAction EndAttackKnife = null;
    public event UnityAction GetRifle = null;
    public event UnityAction GetPistol = null;
    public event UnityAction PutGun = null;
    public event UnityAction AnimStart = null;
    public event UnityAction AnimEnd = null;
    #endregion

    public Transform Knife; // 칼(근접무기)

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region 유니티 이벤트
    private void Awake()
    {
        _player = this.GetComponentInParent<Player>();
    }
    #endregion

    /***********************************************************************
    *                               Anim Events
    ***********************************************************************/
    #region 애니메이션 이벤트
    /// <summary> 근접 공격 애니메이션 시작 </summary>
    public void StartStabbing()
    {
        OnAttackKnife?.Invoke();
    }

    /// <summary> 근접 공격 애니메이션 끝 </summary>
    public void EndStabbing()
    {
        EndAttackKnife?.Invoke();
    }

    /// <summary> 주무기 장착 </summary>
    public void OnGetRifle()//총 꺼내드는 애니메이션에 작동
    {
        GetRifle?.Invoke();
        if (_player.isFirst) _player.GetGun(0); // 주무장 1번째 슬롯의 아이템을 Hand 소켓에 생성
        if (_player.isSecond) _player.GetGun(1); // 주무장 2번째 슬롯의 아이템을 Hand 소켓에 생성
        _player.UpdateBackWeapon(); // 백 소켓 업데이트
    }

    /// <summary> 보조무기 장착 </summary>
    public void OnGetPistol()
    {
        GetPistol?.Invoke();
        _player.GetPistol(); // 부무장 슬롯의 아이템을 Hand 소켓에 생성
        _player.UpdateBackWeapon(); // 백 소켓 업데이트
    }

    /// <summary> 무장 해제 </summary>
    public void OnPutGun()
    {
        PutGun?.Invoke();
        _player.PutGun();
        _player.UpdateBackWeapon(); // 백 소켓 업데이트
    }

    /// <summary> 애니메이션 동작 시작 </summary>
    public void OnAnimStart()
    {
        AnimStart?.Invoke();
        _player.AnimStart();
    }

    /// <summary> 애니메이션 동작 끝 </summary>
    public void OnAnimEnd()
    {
        AnimEnd?.Invoke();
        _player.AnimEnd();
    }
    #endregion
}
