using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZSensor : MonoBehaviour
{
    /// <summary> �̻�� �������Ʈ </summary>
    public UnityAction FindTarget = null;
    /// <summary> �� Ÿ�� ���̾��ũ </summary>
    public LayerMask myEnemyMask;
    /// <summary> �� Ÿ�� ������Ʈ </summary>
    //public BattleSystem myTarget = null;
    /// <summary> �� Ÿ�� ������Ʈ </summary>
    public GameObject myEnemy = null;

    /* Ʈ���� ���� �Լ� -----------------------------------------------------------------------------------------------*/

    private void OnTriggerEnter(Collider other)
    {
        //EnemyMask & (1 << other.gameObject.layer)) != 0
        if ((myEnemyMask & (1 << other.gameObject.layer)) != 0)
        {
            myEnemy = other.gameObject;
            //FindTarget?.Invoke();
            /*
            if (myTarget == null)
            {
                myTarget = other.gameObject.GetComponent<BattleSystem>();
                FindTarget?.Invoke();
            }
            */
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((myEnemyMask & (1 << other.gameObject.layer)) != 0)
        {
            myEnemy = other.gameObject;
            //FindTarget?.Invoke();
            /*
            if (myTarget == null)
            {
                myTarget = other.gameObject.GetComponent<BattleSystem>();
                FindTarget?.Invoke();
            }
            */
        }
    }

    private void OnTriggerExit(Collider other)
    {
        myEnemy = null;
    }

    /*-----------------------------------------------------------------------------------------------*/
}
