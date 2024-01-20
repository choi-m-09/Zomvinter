using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType // 아이템 타입
{
    Primary, Secondary, Expand, Helmet, Bodyarmor, Backpack, Any
}

/// <summary> 월드 타임 변수 구조체 </summary>
public struct time
{
    public static int day;
    public static float Gametime;
}
public class GameUtil : MonoBehaviour
{

}

/// <summary> 전투 시스템 인터페이스 </summary>
public interface BattleSystem
{
    void OnDamage(float Damage);
    void OnCritDamage(float CritDamage);
    bool IsLive();
    Transform transform { get; }

}