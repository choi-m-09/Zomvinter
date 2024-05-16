using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    /// �������Ʈ ���� ///
    public event UnityAction Attack = null;
    public event UnityAction AttackStart = null;
    public event UnityAction AttackEnd = null;
    public event UnityAction IsRushing = null;
    public event UnityAction Attackclear = null;
    public event UnityAction endRush = null;
    public event UnityAction Camerashake = null;
    public event UnityAction RangeAttack = null;

    /// <summary> 공격 시작 </summary>
    public void OnAttack()
    {
        Attack?.Invoke();
    }

    /// <summary> 공격 애니메이션 시작 </summary>
    public void OnAttackStart()
    {
        AttackStart?.Invoke();
    }

    /// <summary> 공격 애니메이션 끝 </summary>
    public void OnAttackEnd()
    {
        AttackEnd?.Invoke();
    }

    /// <summary> Tank 좀비 특수공격 준비 모션 </summary>
    public void IsRush()
    {
        IsRushing?.Invoke();
    }

    public void EndRush()
    {
        endRush?.Invoke();
    }

    public void AttackClear()
    {
        Attackclear?.Invoke();
    }

    /// <summary> Tank 좀비 특수공격 시 카메라 Shake 이벤트 </summary>
    public void CameraShake()
    {
        Camerashake?.Invoke();
    }

    public void OnRangeAttack()
    {
        RangeAttack?.Invoke();
    }
}
