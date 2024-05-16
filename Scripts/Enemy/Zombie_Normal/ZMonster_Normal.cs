using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//LJM
public class ZMonster_Normal : ZMoveController, BattleSystem
{

    bool stun = false;
    /// <summary> 적 레이어 </summary>
    LayerMask EnemyMask;
    /// <summary> 타겟 </summary>
    public Transform myTarget = null;

    /// <summary> 공격 지점 </summary>
    public Transform myWeapon;

    /// <summary> 좀비 스탯 </summary>
    MonsterData myData;
    [SerializeField]
    CharacterStat myStat;

    public int rnd;

    /// <summary> 몬스터 상태 열거 </summary>s
    [SerializeField]
    STATE myState;

    /// <summary> 상태 변경 </summary>
    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.CREATE:
                Init();
                ChangeState(STATE.ROAM);
                break;
            case STATE.ROAM:
                myAnim.SetBool("isMoving", false);
                StopAllCoroutines();
                StartCoroutine(base.Waitting(Random.Range(1.0f, 3.0f), Roaming));
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

    /// <summary> 상태 진행 </summary>
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.CREATE:
                break;
            case STATE.ROAM:
                FindTarget();
                break;
            case STATE.BATTLE:
                if (!stun) ChaseTarget();
                //OnAttack();
                break;
            case STATE.DEAD:
                break;
        }
    }

    void Start()
    {
        ChangeState(STATE.CREATE);
        GetComponentInChildren<AnimEvent>().AttackStart += OnAttackStart;
    }

    void Update()
    {
        StateProcess();
    }


    /// <summary> 공격 시 충돌체 생성 및 체크  </summary>
    protected override void OnAttack()
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

    void Init()
    {
        rnd = Random.Range(0, 1);
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
        myStat.TurnSpeed = 180.0f;
        myStat.HP = 100.0f;
        myData.AttRange = 0.8f;
        myData.AttDelay = 1.5f;
        myData.AttSpeed = 1.0f;
        myStat.DP = 5.0f;
        EnemyMask = LayerMask.GetMask("Player");
        ChangeState(STATE.ROAM);
    }
    /// <summary> 공격 시작 시 델리게이트로 전달해줄 함수 </summary>
    void OnAttackStart()
    {
        OnAttack();
    }

    /// <summary> 데미지 인터페이스 </summary>
    public void OnDamage(float Damage)
    {
        if (myState == STATE.DEAD) return;
        myStat.HP -= Damage;
        if (myStat.HP > 0)
        {
            if (!stun)
            {
                myAnim.SetTrigger("Hit");
                stun = true;
                Invoke("Stun", 0.6f);
            }
            StopAllCoroutines();
        }
        else ChangeState(STATE.DEAD);

    }
    /// <summary> 크리티컬 데미지 인터페이스 </summary>
    public void OnCritDamage(float CritDamage)
    {

    }

    public bool IsLive()
    {
        return true;
    }

    /// <summary> 넉백 해제 </summary>
    public void Stun() => stun = false;

    /* ���� �Լ� -----------------------------------------------------------------------------------------------*/

    /// <summary> 로밍 상태일때 플레이어 검색 </summary>
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
        base.RoamToPosition(pos, myStat.MoveSpeed, myStat.TurnSpeed, () => StartCoroutine(base.Waitting(Random.Range(1.0f, 3.0f), Roaming)));
    }
    /// <summary> 플레이어 추격 </summary>
    private void ChaseTarget()
    {
        // 범위 내에 있는지 확인
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
}
