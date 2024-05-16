using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ���� ������ ����ü </summary>
[Serializable]
struct MonsterData
{
    public float AttRange; // ���� ����
    public float AttDelay; // ���� �����
    public float AttSpeed; // ���� �ִϸ��̼� �ӵ�
}

/// <summary> ĳ���� ������ ����ü</summary>
[Serializable]
public class CharacterStat
{
    public float HP; //Health Point
    public float MaxHP;

    public float Stamina;
    public float MaxStamina;

    public float AP; //Armor Point

    public float DP; //Damage Point

    public float SP; // Skill Point

    public float Hunger;
    public float MaxHunger; 

    public float Thirsty; 
    public float MaxThirsty;

    public float Minus_Hunger; // 배고픔 수치 줄어드는 속도
    public float Minus_Thirsty; // 목마름 수치 줄어드는 속도
    public float RecoverStamina; // 스테미너 자연 회복 속도

    //----------- SP ���� �����ϴ� ���� ---------------
    public int Strength; // 힘
    public int Constitution; // 최대 체력
    public int Dexterity; // 허기
    public int Endurance; // 지구력
    public int Intelligence; // 지능

    public float MoveSpeed;
    public float TurnSpeed;
}

/// <summary> </summary>
public struct ROTATEDATA
{
    public float Angle;
    public float Dir;
}

public class Character : MonoBehaviour
{
    /// <summary> Animator ������Ʈ ��ȯ </summary>
    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
                if(_anim == null)
                {
                    _anim = GetComponentInChildren<Animator>();
                }
            }
            return _anim;
        }
    }

    /// <param name="src">시작 지점 벡터</param>
    /// <param name="des">목표 지점 벡터</param>
    /// <param name="right"> 오른쪽 벡터 </param>
    /// <param name="data"> 회전 정보 데이터 </param>
    public static void CalcAngle(Vector3 src, Vector3 des, Vector3 right, out ROTATEDATA data)
    {
        data = new ROTATEDATA();
        float Radian = Mathf.Acos(Vector3.Dot(src, des));
        // Radian 값을 Degree로 변환
        data.Angle = 180.0f * (Radian / Mathf.PI);
        // 오른쪽 벡터와 목표 지점 내적값에 따라 시계 방향 혹은 시계 반대 방향으로 회전
        data.Dir = 1.0f;
        if (Vector3.Dot(right, des) < 0.0f)
        {
            data.Dir = -1.0f;
        }
    }

}
