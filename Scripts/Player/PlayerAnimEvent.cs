using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{

    #region 델리게이트 필드
    public event UnityAction GetWeapon = null;
    public event UnityAction GetPistol = null;
    public event UnityAction PutGun = null;
    public event UnityAction AnimStart = null;
    public event UnityAction AnimEnd = null;
    public event UnityAction AttackStart = null;
    #endregion

    /***********************************************************************
    *                               Anim Events
    ***********************************************************************/

    /// <summary> 주무기 장착 시 델리게이트 호출 </summary>
    public void OnGetWeapon()
    {
        GetWeapon?.Invoke();
    }

    /// <summary> 보조무기 장착 시 델리게이트 호출 </summary>
    public void OnGetPistol()
    {
        GetPistol?.Invoke();
    }

    /// <summary> 주무기 탈착 시 델리게이트 호출</summary>
    public void OnPutGun()
    {
        PutGun?.Invoke();
    }
    
    /// <summary> 애니메이션 시작 시 델리게이트 호출 </summary>
    public void OnAnimStart()
    {
        AnimStart?.Invoke();
    }

    /// <summary> 애니메이션 완료 시 델리게이트 호출  </summary>
    public void OnAnimEnd()
    {
        AnimEnd?.Invoke();
    }

    /// <summary> 근접 무기 공격 시 델리게이트 호출 </summary>
    public void OnAttackStart()
    {
        AttackStart?.Invoke();
    }
}
