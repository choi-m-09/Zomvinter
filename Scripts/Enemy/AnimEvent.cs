using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    /// ��������Ʈ ���� ///
    public event UnityAction Attack = null;
    public event UnityAction AttackStart = null;
    public event UnityAction AttackEnd = null;
    public event UnityAction IsRushing = null;
    public event UnityAction Attackclear = null;
    public event UnityAction endRush = null;
    public event UnityAction Camerashake = null;
    public event UnityAction RangeAttack = null;

    /// <summary> ���� ��������Ʈ </summary>
    public void OnAttack()
    {
        Attack?.Invoke();
    }

    /// <summary> ���� ���� ���� ��������Ʈ </summary>
    public void OnAttackStart()
    {
        AttackStart?.Invoke();
    }

    /// <summary> ���� �� ���� ��������Ʈ </summary>
    public void OnAttackEnd()
    {
        AttackEnd?.Invoke();
    }

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

    public void CameraShake()
    {
        Camerashake?.Invoke();
    }

    public void OnRangeAttack()
    {
        RangeAttack?.Invoke();
    }
}
