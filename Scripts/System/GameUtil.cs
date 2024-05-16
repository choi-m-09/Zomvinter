using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType
{
    Primary, Secondary, Consumable, Helmet, Bodyarmor, Backpack, Any
}

/// <summary> 게임 내 시간 관련 데이터(개발 중) </summary>
public struct time
{
    public static int day;
    public static float Gametime;
}
public class GameUtil
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

public interface IUsableItem
{
    bool Use(Player _player);
}