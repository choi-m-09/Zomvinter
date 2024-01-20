using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//LJM
public class ZMonster_Normal : ZMoveController, BattleSystem
{
    /* 반환 변수 -----------------------------------------------------------------------------------------------*/

    /// <summary> 좀비 인식 범위 오브젝트 반환 </summary>
    ZSensor _sensor = null;
    ZSensor mySensor
    {
        get
        {
            if (_sensor == null)
            {
                _sensor = this.GetComponentInChildren<ZSensor>();
            }
            return _sensor;
        }
    }



    /* 전역 변수 -----------------------------------------------------------------------------------------------*/

    /// <summary> 내 타겟 오브젝트 레이어 </summary>
    LayerMask EnemyMask;
    /// <summary> 내 타겟 오브젝트 위치 값 </summary>
    public Transform myTarget = null;

    /// <summary> 내 공격 판정 오브젝트 위치값 </summary>
    public Transform myWeapon;

    /// <summary> 캐릭터 정보 구조체 선언 </summary>
    MonsterData myData;
    [SerializeField]
    CharacterStat myStat;

    public int rnd;
    //bool AttackTerm = false;

    /* 유한 상태 기계 -----------------------------------------------------------------------------------------------*/

    /// <summary> 유한 상태 기계 선언 </summary>
    enum STATE
    {
        CREATE, IDLE, ROAM, BATTLE, DEAD
    }
    [SerializeField]
    STATE myState;

    /// <summary> 유한 상태 기계 Start </summary>
    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.CREATE:
                break;
            case STATE.IDLE:
                if (rnd == 0)
                {
                    myAnim.SetBool("IsRun", true);
                    myStat.MoveSpeed = 1.5f;
                }
                else if (rnd == 1)
                {
                    myAnim.SetBool("IsRun", false);
                    myStat.MoveSpeed = 0.7f;
                }
                myStat.HP = 200;
                myStat.TurnSpeed = 180.0f;
                myStat.HP = 100.0f;
                myData.AttRange = 1.5f;
                myData.AttDelay = 1.5f;
                myData.AttSpeed = 1.0f;
                myData.UnChaseTime = 3.0f;
                myStat.DP = 5.0f;
                EnemyMask = LayerMask.GetMask("Player");
                ChangeState(STATE.ROAM);
                break;
            case STATE.ROAM:
                myAnim.SetBool("isMoving", false);
                StopAllCoroutines();
                StartCoroutine(Waitting(Random.Range(1.0f, 3.0f), Roaming));
                break;
            case STATE.BATTLE:
                myAnim.SetBool("isMoving", false);
                StopAllCoroutines();
                break;
            case STATE.DEAD:
                StopAllCoroutines();
                myAnim.SetTrigger("Dead");
                StartCoroutine(Death());
                break;
        }
    }

    /// <summary> 유한 상태 기계 Update </summary>
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.CREATE:
                break;
            case STATE.IDLE:
                break;
            case STATE.ROAM:
                FindTarget();
                break;
            case STATE.BATTLE:
                ChaseTarget();
                //OnAttack();
                break;
            case STATE.DEAD:
                break;
        }
    }
    /* 실행 함수 -----------------------------------------------------------------------------------------------*/
    
    void Start()
    {
        ChangeState(STATE.IDLE); // 유한 상태 기계 초기화

        /// 딜리게이트 추가 ///
        GetComponentInChildren<AnimEvent>().AttackStart += OnAttackStart;
        GetComponentInChildren<AnimEvent>().AttackEnd += OnAttackEnd;
    }

    void Update()
    {
        StateProcess();
    }

    /* 배틀 시스템 - 공격 -----------------------------------------------------------------------------------------------*/
    /// <summary> 공격 판정 함수 </summary>
    void OnAttack()
    {
        if (myAnim.GetBool("AttackTerm"))
        {
            Collider[] list = Physics.OverlapSphere(myWeapon.position, 1.0f, EnemyMask);
            foreach (Collider col in list)
            {
                BattleSystem bs = col.gameObject.GetComponent<BattleSystem>();
                if (bs != null)
                {
                    bs.OnDamage(myStat.DP);
                }
            }
        }
        else
        {

        }
    }
    /// <summary> 공격 애니메이션 시작 지점 체크 함수 </summary>
    void OnAttackStart() 
    {
        myAnim.SetBool("AttackTerm", true); 
        OnAttack();
    }
    /// <summary> 공격 애니메이션 끝 지점 체크 함수 </summary>
    void OnAttackEnd()
    {
        myAnim.SetBool("AttackTerm", false);
    }
    /* 배틀 시스템 - 피격 -----------------------------------------------------------------------------------------------*/
    /// <summary> 피격 함수 </summary>
    public void OnDamage(float Damage)
    {
        if (myState == STATE.DEAD) return;
        myStat.HP -= Damage;
        if (myStat.HP <= 0) ChangeState(STATE.DEAD);
    }
    /// <summary> 크리티컬 피격 함수 </summary>
    public void OnCritDamage(float CritDamage)
    {

    }

    public bool IsLive()
    {
        return true;
    }
    /* 지역 함수 -----------------------------------------------------------------------------------------------*/

    /// <summary> 타겟 검색 함수 </summary>
    protected void FindTarget()
    {
        if (mySensor.myEnemy != null)
        {
            //if (UnChaseCor != null) StopAllCoroutines();
            myTarget = mySensor.myEnemy.transform;
            ChangeState(STATE.BATTLE);
        }
        else
        {
            ChangeState(STATE.ROAM);
            return;
        }
    }

    void Roaming()
    {
        Vector3 pos = this.transform.position;
        pos.x = transform.position.x + Random.Range(-5.0f, 5.0f);
        pos.z = transform.position.z + Random.Range(-5.0f, 5.0f);
        base.RoamToPosition(pos, myStat.MoveSpeed, myStat.TurnSpeed, () => StartCoroutine(Waitting(Random.Range(1.0f, 3.0f), Roaming)));
    }

    IEnumerator Waitting(float t, UnityAction done)
    {
        yield return new WaitForSeconds(t);
        done?.Invoke();
    }

    /// <summary> 타겟 추격 함수 </summary>
    private void ChaseTarget()
    {
        //타겟Pos, 이동 속도, 공격 거리, 공격 딜레이, 공격 속도, 턴 속도
        if (mySensor.myEnemy != null)
        {
            MoveToPosition(myTarget.transform, myStat.MoveSpeed,
                myData.AttRange, myData.AttDelay, myData.AttSpeed, myStat.TurnSpeed);
        }
        else
        {
            myTarget = null;
            ChangeState(STATE.ROAM);
        }
    }

    IEnumerator Death()
    {
        Destroy(this.GetComponent<Rigidbody>());
        Destroy(this.GetComponent<CapsuleCollider>());
        yield return new WaitForSeconds(3.0f);
        float dist = 1.0f;
        while (dist > 0.0f)
        {
            float delta = Time.deltaTime * 0.5f;
            this.transform.Translate(-Vector3.up * Time.deltaTime, Space.World);
            dist -= delta;
            yield return null;
        }
        Destroy(this.gameObject);
        
    }

    /*
    Coroutine UnChaseCor = null;
    IEnumerator UnChaseTimer (float T)
    {
        if(mySensor.myEnemy == null)
        {
            yield return new WaitForSeconds(T);
        }

        myTarget = null;
        UnChaseCor = null;
    }
    */
}
