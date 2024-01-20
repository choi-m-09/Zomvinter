using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//LJM
public class ZMonster_Normal : ZMoveController, BattleSystem
{
    /* ��ȯ ���� -----------------------------------------------------------------------------------------------*/

    /// <summary> ���� �ν� ���� ������Ʈ ��ȯ </summary>
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



    /* ���� ���� -----------------------------------------------------------------------------------------------*/

    /// <summary> �� Ÿ�� ������Ʈ ���̾� </summary>
    LayerMask EnemyMask;
    /// <summary> �� Ÿ�� ������Ʈ ��ġ �� </summary>
    public Transform myTarget = null;

    /// <summary> �� ���� ���� ������Ʈ ��ġ�� </summary>
    public Transform myWeapon;

    /// <summary> ĳ���� ���� ����ü ���� </summary>
    MonsterData myData;
    [SerializeField]
    CharacterStat myStat;

    public int rnd;
    //bool AttackTerm = false;

    /* ���� ���� ��� -----------------------------------------------------------------------------------------------*/

    /// <summary> ���� ���� ��� ���� </summary>
    enum STATE
    {
        CREATE, IDLE, ROAM, BATTLE, DEAD
    }
    [SerializeField]
    STATE myState;

    /// <summary> ���� ���� ��� Start </summary>
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

    /// <summary> ���� ���� ��� Update </summary>
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
    /* ���� �Լ� -----------------------------------------------------------------------------------------------*/
    
    void Start()
    {
        ChangeState(STATE.IDLE); // ���� ���� ��� �ʱ�ȭ

        /// ��������Ʈ �߰� ///
        GetComponentInChildren<AnimEvent>().AttackStart += OnAttackStart;
        GetComponentInChildren<AnimEvent>().AttackEnd += OnAttackEnd;
    }

    void Update()
    {
        StateProcess();
    }

    /* ��Ʋ �ý��� - ���� -----------------------------------------------------------------------------------------------*/
    /// <summary> ���� ���� �Լ� </summary>
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
    /// <summary> ���� �ִϸ��̼� ���� ���� üũ �Լ� </summary>
    void OnAttackStart() 
    {
        myAnim.SetBool("AttackTerm", true); 
        OnAttack();
    }
    /// <summary> ���� �ִϸ��̼� �� ���� üũ �Լ� </summary>
    void OnAttackEnd()
    {
        myAnim.SetBool("AttackTerm", false);
    }
    /* ��Ʋ �ý��� - �ǰ� -----------------------------------------------------------------------------------------------*/
    /// <summary> �ǰ� �Լ� </summary>
    public void OnDamage(float Damage)
    {
        if (myState == STATE.DEAD) return;
        myStat.HP -= Damage;
        if (myStat.HP <= 0) ChangeState(STATE.DEAD);
    }
    /// <summary> ũ��Ƽ�� �ǰ� �Լ� </summary>
    public void OnCritDamage(float CritDamage)
    {

    }

    public bool IsLive()
    {
        return true;
    }
    /* ���� �Լ� -----------------------------------------------------------------------------------------------*/

    /// <summary> Ÿ�� �˻� �Լ� </summary>
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

    /// <summary> Ÿ�� �߰� �Լ� </summary>
    private void ChaseTarget()
    {
        //Ÿ��Pos, �̵� �ӵ�, ���� �Ÿ�, ���� ������, ���� �ӵ�, �� �ӵ�
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
