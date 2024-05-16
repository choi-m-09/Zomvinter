using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZMonster_Range : ZMoveController, BattleSystem
{

    /// <summary> 적 레이어 마스크 </summary>
    LayerMask EnemyMask;
    /// <summary> 추격할 상대 </summary>
    public Transform myTarget = null;

    /// <summary> 공격 관련 필드 </summary>
    public Transform myWeapon;
    public Transform BTarget;
    public GameObject Bullet;
    public float BMoveSpeed;

    /// <summary> 좀비 정보 및 스텟 </summary>
    MonsterData myData;
    [SerializeField]
    CharacterStat myStat;

    public int rnd;
    //bool AttackTerm = false;

    /// <summary> 상태 기계 </summary>
    [SerializeField]
    STATE myState;

    /// <summary> 상태 변화 시 호출 </summary>
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

    /// <summary> 상태 확인 후 프레임 마다 호출 </summary>
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
                ChaseTarget();
                break;
            case STATE.DEAD:
                break;
        }
    }

    void Init()
    {
        myStat.HP = 1000.0f;
        BMoveSpeed = 15.0f;
        myStat.MoveSpeed = 2.5f;
        myStat.TurnSpeed = 180.0f;
        myData.AttRange = 6.0f;
        myData.AttDelay = 2.5f;
        myData.AttSpeed = 1.0f;
        myStat.DP = 8.0f;
        EnemyMask = LayerMask.GetMask("Player");
        BTarget = GameObject.Find("Player").transform;
        GetComponentInChildren<AnimEvent>().AttackStart += OnAttackStart;
        GetComponentInChildren<AnimEvent>().AttackEnd += OnAttackEnd;
        GetComponentInChildren<AnimEvent>().RangeAttack += OnRangeAttack;
    }


    void Start()
    {
        ChangeState(STATE.CREATE); 
    }

    void Update()
    {
        StateProcess();
    }

    protected override void OnAttack()
    {
        // 근접 공격    
    }
    /// <summary> 공격 애니메이션 시작 </summary>
    void OnAttackStart()
    {
        myAnim.SetBool("AttackTerm", true);
        OnAttack();
    }
    /// <summary> 공격 애니메이션 끝 </summary>
    void OnAttackEnd()
    {
        myAnim.SetBool("AttackTerm", false);
    }

    void OnRangeAttack()
    {
        Transform Target = BTarget;
        StartCoroutine(BulletMove(Target.position));
    }
    
    public void OnDamage(float Damage)
    {
        if (myState == STATE.DEAD) return;
        myStat.HP -= Damage;
        if (myStat.HP <= 0) ChangeState(STATE.DEAD);
    }

    public void OnCritDamage(float CritDamage)
    {

    }

    public bool IsLive()
    {
        return true;
    }
   

    protected void FindTarget()
    {
        if (mySensor.myEnemy != null)
        {
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
    /// <summary> 플레이어 추적 </summary>
    private void ChaseTarget()
    {
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

    /// <summary> 원거리 공격 시 생성되는 Bullet 무브먼트 코루틴 </summary>
    IEnumerator BulletMove(Vector3 pos)
    {
        GameObject obj = Instantiate(Bullet, myWeapon.position, myWeapon.rotation);
        Vector3 Dir = obj.transform.position - pos;
        float Dist = Dir.magnitude * 2;
        Dir.Normalize();
        
        while (Dist > Mathf.Epsilon)
        {
            if (obj == null) yield break;
            float delta = BMoveSpeed * Time.deltaTime;
            if(Dist < delta)
            {
                Dist = delta;
            }
            obj.transform.Translate(-Dir * delta, Space.World);
            Dist -= delta;
            yield return null;
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
