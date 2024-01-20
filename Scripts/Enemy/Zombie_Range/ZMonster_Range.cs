using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZMonster_Range : ZMoveController, BattleSystem
{
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
    public LayerMask EnemyMask;
    /// <summary> 내 타겟 오브젝트 위치 값 </summary>
    public Transform myTarget = null;

    /// <summary> 내 공격 판정 오브젝트 위치값 </summary>
    public Transform myWeapon;
    public Transform BTarget;
    public GameObject Bullet;
    public float BMoveSpeed;

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
                myStat.HP = 1000.0f;
                BMoveSpeed = 15.0f;
                myStat.MoveSpeed = 2.5f;
                myStat.TurnSpeed = 180.0f;
                myData.AttRange = 6.0f;
                myData.AttDelay = 2.5f;
                myData.AttSpeed = 1.0f;
                myData.UnChaseTime = 3.0f;
                myStat.DP = 8.0f;
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
        GetComponentInChildren<AnimEvent>().RangeAttack += OnRangeAttack;
    }

    void Update()
    {
        StateProcess();
    }

    /* 배틀 시스템 - 공격 -----------------------------------------------------------------------------------------------*/
    /// <summary> 공격 판정 함수 </summary>
    void OnAttack()
    {
        
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

    void OnRangeAttack()
    {
        Transform Target = BTarget;
        StartCoroutine(BulletMove(Target.position));
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

    IEnumerator BulletMove(Vector3 pos)
    {
        GameObject obj = Instantiate(Bullet, myWeapon.position, myWeapon.rotation);
        Vector3 Dir = obj.transform.position - pos;
        float Dist = Dir.magnitude * 2;
        Dir.Normalize();
        
        while (Dist > Mathf.Epsilon)
        {
            float delta = BMoveSpeed * Time.deltaTime;
            if(Dist < delta)
            {
                Dist = delta;
            }
            obj.transform.Translate(-Dir * delta, Space.World);
            Dist -= delta;
            yield return null;
        }
        Destroy(obj);
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
}
