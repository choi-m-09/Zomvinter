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



    /* ���� ���� -----------------------------------------------------------------------------------------------*/

    /// <summary> �� Ÿ�� ������Ʈ ���̾� </summary>
    public LayerMask EnemyMask;
    /// <summary> �� Ÿ�� ������Ʈ ��ġ �� </summary>
    public Transform myTarget = null;

    /// <summary> �� ���� ���� ������Ʈ ��ġ�� </summary>
    public Transform myWeapon;
    public Transform BTarget;
    public GameObject Bullet;
    public float BMoveSpeed;

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
        GetComponentInChildren<AnimEvent>().RangeAttack += OnRangeAttack;
    }

    void Update()
    {
        StateProcess();
    }

    /* ��Ʋ �ý��� - ���� -----------------------------------------------------------------------------------------------*/
    /// <summary> ���� ���� �Լ� </summary>
    void OnAttack()
    {
        
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

    void OnRangeAttack()
    {
        Transform Target = BTarget;
        StartCoroutine(BulletMove(Target.position));
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
