using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZMonster_Tank : ZMoveController, BattleSystem
{

    /// <summary> �� Ÿ�� ������Ʈ ���̾� </summary>
    LayerMask EnemyMask;
    /// <summary> �� Ÿ�� ������Ʈ ��ġ �� </summary>
    public Transform myTarget = null;

    /// <summary> �� ���� ���� ������Ʈ ��ġ�� </summary>
    public Transform myWeapon;

    /// <summary> ĳ���� ���� ����ü ���� </summary>
    MonsterData myData;

    [SerializeField] CharacterStat myStat;

    float SkillTime = 0.0f;

    [SerializeField] float SkillDelay;

    [SerializeField] float ShakeForce = 0f;

    Vector3 RushTarget;

    [SerializeField]
    Camera MainCamera;

    Vector3 CameraPos;

    [SerializeField][Range(0.01f, 0.1f)] float shakeRange = 0.05f;

    [SerializeField][Range(0.1f, 1f)] float duration = 0.5f;

    [SerializeField] STATE myState;

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
                myTarget = null;
                StopAllCoroutines();
                StartCoroutine(base.Waitting(Random.Range(1.0f, 3.0f), Roaming));
                RushTarget = Vector3.zero;
                break;
            case STATE.BATTLE:
                myAnim.SetBool("isMoving", false);
                StopAllCoroutines();
                break;
            case STATE.RUSH:
                StopAllCoroutines();
                myAnim.SetTrigger("Rush");
                if (myAnim.GetBool("IsReady") == false && myAnim.GetBool("IsRush") == false)
                {
                    RushTarget = myTarget.gameObject.transform.position;
                }
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
                ChaseTarget();
                break;
            case STATE.RUSH:
                RushCoroutine();
                break;
            case STATE.DEAD:
                break;
        }
    }

    void Start()
    {
        ChangeState(STATE.CREATE);
    }


    void Update()
    {
        StateProcess();
    }

    void Init()
    {
        myStat = new CharacterStat();
        GetComponentInChildren<AnimEvent>().AttackStart += OnAttackStart;
        GetComponentInChildren<AnimEvent>().IsRushing += IsRushing;
        GetComponentInChildren<AnimEvent>().endRush += EndRush;
        GetComponentInChildren<AnimEvent>().Camerashake += CameraShake;
        MainCamera = Camera.main;

        myStat.HP = 800.0f;
        myStat.MoveSpeed = 3.0f;
        myStat.TurnSpeed = 180.0f;
        myData.AttRange = 1.8f;
        myData.AttDelay = 1.5f;
        myData.AttSpeed = 1.0f;
        myStat.DP = 5.0f;
        SkillDelay = 5.0f;
        EnemyMask = LayerMask.GetMask("Player");
    }

    protected override void OnAttack()
    {
        Collider[] list = Physics.OverlapSphere(myWeapon.position, 4.0f, EnemyMask);
        foreach (Collider col in list)
        {
            BattleSystem bs = col.gameObject.GetComponent<BattleSystem>();
            if (bs != null)
            {
                bs.OnDamage(myStat.DP);
            }
        }
    }

    void OnAttackStart()
    {
        OnAttack();
    }

    /// <summary> Rush 이벤트 시작 </summary>
    void IsRushing()
    {
        myAnim.SetBool("IsRush", true);
        myAnim.SetBool("IsReady", false);
    }
    /// <summary> Rush 이벤트 끝 </summary>
    void EndRush()
    {
        myAnim.SetBool("IsRush", false);
    }
    /// <summary> 특수 공격 시 카메라 흔들림 이벤트 </summary>
    void CameraShake()
    {
        Shake();
    }
    /// <summary> 데미지 인터페이스 </summary>
    public void OnDamage(float Damage)
    {
        if (myState == STATE.DEAD) return;
        myStat.HP -= Damage;
        if (myStat.HP <= 0) ChangeState(STATE.DEAD);
        else
        {
            
        }
    }
    /// <summary> 크리티컬 데미지 인터페이스 </summary>
    public void OnCritDamage(float CritDamage)
    {

    }

    public bool IsLive()
    {
        return true;
    }

    /// <summary> 로밍 상태일때 타겟 검색 </summary>
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
        }
    }

    void Roaming()
    {
        Vector3 pos = this.transform.position;
        pos.x = transform.position.x + Random.Range(-5.0f, 5.0f);
        pos.z = transform.position.z + Random.Range(-5.0f, 5.0f);
        base.RoamToPosition(pos, myStat.MoveSpeed, myStat.TurnSpeed, () => StartCoroutine(base.Waitting(Random.Range(1.0f, 3.0f), Roaming)));
    }

    /// <summary> 타겟 추적 </summary>
    private void ChaseTarget()
    {
        if (mySensor.myEnemy != null)
        {
            SkillTime += Time.deltaTime;
            if (SkillTime < SkillDelay)
            {
                MoveToPosition(myTarget.transform, myStat.MoveSpeed,
                    myData.AttRange, myData.AttDelay, myData.AttSpeed, myStat.TurnSpeed);
            }
            else
            {
                ChangeState(STATE.RUSH);
            }
        }
        else
        {
            ChangeState(STATE.ROAM);
        }
    }


    /// <summary> Rush 코루틴 제어 함수 </summary>
    Coroutine rush;
    void RushCoroutine()
    {
        float MoveSpeed = myStat.MoveSpeed * 4;
        if (rush != null) StopCoroutine(rush);
        rush = StartCoroutine(IsRush(RushTarget, MoveSpeed));
    }

    /// <summary> 준비 완료 애니메이션 끝난 후 해당 지점으로 Rush </summary>
    IEnumerator IsRush(Vector3 pos, float MoveSpeed)
    {
        if (myAnim.GetBool("IsAttacking") == false && myAnim.GetBool("IsReady") == false)
        {
            Vector3 Dir = pos - this.transform.position;
            float Dist = Dir.magnitude;
            Dir.Normalize();
            while (Dist > 0.01f)
            {
                float delta = MoveSpeed * Time.deltaTime;
                if (Dist < delta)
                {
                    delta = Dist;
                }
                Dist -= delta;
                this.transform.Translate(Dir * delta, Space.World);
                yield return null;
            }
            myAnim.SetBool("IsRush", false);
            myAnim.SetBool("IsReady", false);
            myAnim.SetTrigger("Skill");
            StartCoroutine(Ready());
        }
    }

    /// <summary> Rush 이벤트 시작 전 준비 이벤트</summary>
    IEnumerator Ready()
    {
        yield return new WaitForSeconds(2.5f);
        SkillTime = 0.0f;
        ChangeState(STATE.BATTLE);
        rush = null;
    }

    /// <summary> 카메라 Shake 이벤트 지연 실행 </summary>
    void Shake()
    {
        CameraPos = MainCamera.transform.position;
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }

    void StartShake()
    {
        float cameraPosX = Random.value * shakeRange * 2 - shakeRange;
        float cameraPosY = Random.value * shakeRange * 2 - shakeRange;
        Vector3 cameraPos = MainCamera.transform.position;
        cameraPos.x += cameraPosX;
        cameraPos.y += cameraPosY;
        MainCamera.transform.position = cameraPos;
    }

    void StopShake()
    {
        CancelInvoke("StartShake");
        MainCamera.transform.position = CameraPos;
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
