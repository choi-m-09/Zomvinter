using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZSensor : MonoBehaviour
{
    /// <summary> 미사용 딜리게이트 </summary>
    public UnityAction FindTarget = null;
    /// <summary> 적 타겟 레이어마스크 </summary>
    public LayerMask myEnemyMask;
    /// <summary> 내 타겟 오브젝트 </summary>
    //public BattleSystem myTarget = null;
    /// <summary> 내 타겟 오브젝트 </summary>
    public GameObject myEnemy = null;

    /* 트리거 제어 함수 -----------------------------------------------------------------------------------------------*/

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
