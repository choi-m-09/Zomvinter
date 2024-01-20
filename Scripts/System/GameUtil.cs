using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType // ������ Ÿ��
{
    Primary, Secondary, Expand, Helmet, Bodyarmor, Backpack, Any
}

/// <summary> ���� Ÿ�� ���� ����ü </summary>
public struct time
{
    public static int day;
    public static float Gametime;
}
public class GameUtil : MonoBehaviour
{

}

/// <summary> ���� �ý��� �������̽� </summary>
public interface BattleSystem
{
    void OnDamage(float Damage);
    void OnCritDamage(float CritDamage);
    bool IsLive();
    Transform transform { get; }

}