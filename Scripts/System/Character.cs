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
    public float UnChaseTime; // ��׷� ���� ����
}

/// <summary> ĳ���� ������ ����ü</summary>
[Serializable]
public struct CharacterStat
{
    public float HP; //ü�� Health Point
    public float MaxHP;

    public float Stamina;   // ���׹̳�
    public float MaxStamina;

    public float AP; //���� Armor Point

    public float DP; //���ݷ� Damage Point

    public float Hunger;    // ���
    public float MaxHunger; 

    public float Thirsty;   // ����
    public float MaxThirsty;

    public float CycleSpeed; // ���, ���� ���� �ӵ�
    public float StaminaCycle; // ���׹̳� ���� �ӵ�

    //----------- SP ���� �����ϴ� ���� ---------------
    public int Strength; // �� 
    public int Constitution; // �ִ�ü�� ���� 
    public int Dexterity; // ������
    public int Endurance; // ���¹̳� ���� 
    public int Intelligence; // ����

    public float MoveSpeed;
    public float TurnSpeed;
}

/// <summary> ȸ�� ���� ���� ����ü </summary>
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

    /// <summary> ȸ�� ���� �Լ� </summary>
    /// <param name="src">ȸ�� ���� ����</param>
    /// <param name="des">ȸ�� �� ����</param>
    /// <param name="right">�ش� ������Ʈ�� Vector.Right��</param>
    /// <param name="data">���� �� ���� ��</param>
    public static void CalcAngle(Vector3 src, Vector3 des, Vector3 right, out ROTATEDATA data)
    {
        data = new ROTATEDATA();
        float Radian = Mathf.Acos(Vector3.Dot(src, des));
        //�����̼� ��
        data.Angle = 180.0f * (Radian / Mathf.PI);
        //ȸ���� ��, ����� ��
        data.Dir = 1.0f;
        if (Vector3.Dot(right, des) < 0.0f)
        {
            data.Dir = -1.0f;
        }
    }

}
