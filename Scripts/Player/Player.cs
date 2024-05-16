using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary> 플레이어 상태 열거 </summary>
public enum STATE
{
    NONE, CREATE, ALIVE, BATTLE, DEAD
}

public class Player : PlayerController, BattleSystem
{
    #region 전역 필드
    
    public CharacterStat Stat;

    public static float AttackSpeed = 1;
    #endregion

    #region 인벤토리 및 스탯 UI 관련 필드
    private StatUI _statUI;

    private bool ActiveInv = false;
    private bool ActiveStat = false;
    #endregion

    #region 

    Vector3 pos = Vector3.zero;



    #endregion

    #region UI 및 카메라 필드

    [Header("캔버스")]
    [SerializeField]
    private Transform _canvas;
    [SerializeField]
    /// <summary> ī�޶� ���� �� </summary>
    private Transform _cameraArm;
    #endregion

    #region 
    [Header("현재 상태")]
    public STATE myState = STATE.NONE;
    #endregion

    #region 코루틴 필드
    // ���� ��� �ڷ�ƾ
    private Coroutine aliveCycle = null;

    // ���׹̳� ��� �ڷ�ƾ
    private Coroutine Use = null;
    // ���׹̳� ȸ�� �ڷ�ƾ
    private Coroutine Recovery = null;

    #endregion

    #region 플레이어 무기 관련 필드
    [Header("무기 소켓")]
    [SerializeField]
    public GameObject myWeapon = null;
    [SerializeField]
    private LayerMask EnemyMask;
    [SerializeField]
    public Transform HandSorket;
    [SerializeField]
    public Transform BackLeftSorket;
    [SerializeField]
    public Transform BackRightSorket;
    [SerializeField]
    public Transform PistolGrip;


    public GameObject Text_GameOver;


    [Header("상태 확인")]
    /// <summary> 애니메이션 끝났는지 체크 </summary>
    public bool MotionEnd = true;

    [SerializeField]
    /// <summary> 첫번째 주무기 장착 중 여부 </summary>
    public bool isFirst = false;
    [SerializeField]
    /// <summary> 두번째 주무기 장착 중 여부 </summary>
    public bool isSecond = false;
    [SerializeField]
    /// <summary> 보조 무기 장착 여부 </summary>
    public bool isPistol = false;

    public bool Reloading = false;

    [SerializeField]
    /// <summary> 공격 준비 또는 조준 상태 확인 </summary>
    private bool Aimed = false;

    public bool AimCheck
    {
        get { return Aimed; }
    }
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region Unity Events
    void Start()
    {
        Stat = new CharacterStat();
        Init();
        ChangeState(STATE.CREATE);
    }

    void Update()
    {
        StateProcess();
    }

    private void FixedUpdate()
    {
        if (!myAnim.GetBool("isDead"))
        {
            Move(Stat.MoveSpeed);
        }
    }
    #endregion

    /***********************************************************************
    *                               Finite-state machine
    ***********************************************************************/
    #region State Machine
    /// <summary> 상태 전환 시 </summary>
    /// <param name="s"> 플레이어 상태 </param>
    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.NONE:
                break;
            case STATE.CREATE:
                InitStat();
                UpdateBackWeapon();
                ChangeState(STATE.ALIVE);
                break;
            case STATE.ALIVE:
                break;
            case STATE.BATTLE:
                break;
            case STATE.DEAD:
                myAnim.SetTrigger("Dead");
                myAnim.SetBool("isDead", true);
                StopAllCoroutines();
                StartCoroutine(Dead());
                break;
        }
    }
    /// <summary> 상태에 따라 프로세스 진행 </summary>
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.NONE:
                break;
            case STATE.CREATE:
                break;
            case STATE.ALIVE:
                myAnim.SetFloat("AttackSpeed", AttackSpeed);
                StatClamp();
                AliveCoroutine();
                UpdateBackWeapon();
                InputMethods();
                break;
            case STATE.BATTLE:
                break;
            case STATE.DEAD:
                break;
        }
    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region Private 

    #region Init Methods
    private void InitStat()
    {
        // 초기 최대 수치량
        Stat.MaxHP = 100.0f;
        Stat.MaxHunger = 100.0f;
        Stat.MaxStamina = 100.0f;
        Stat.MaxThirsty = 100.0f;

        // 플레이어 실제 수치량
        Stat.HP = Stat.MaxHP;
        Stat.AP = 10.0f;
        Stat.DP = 5.0f;
        Stat.MoveSpeed = 2.0f;
        Stat.TurnSpeed = 180.0f;
        Stat.Hunger = Stat.MaxHunger;
        Stat.Thirsty = Stat.MaxThirsty;
        Stat.Stamina = Stat.MaxStamina;

        // 플레이어 능력치 레벨
        Stat.Strength = 0;
        Stat.Constitution = 0;
        Stat.Dexterity = 0;
        Stat.Endurance = 0;
        Stat.Intelligence = 0;

        // 허기, 갈증 감소 수치 및 스테미너 자연 회복 수치
        Stat.RecoverStamina = 1.0f;
        Stat.Minus_Hunger = 0.8f;
        Stat.Minus_Thirsty = 1.2f;
    }

    private void OnGetWeapon()
    {
        if (isFirst) GetWeapon(0);
        if (isSecond) GetWeapon(1);
        myAnim.SetBool("isArmed", true);
    }

    private void OnAttackStart()
    {
        MeleeItem mi = myWeapon.GetComponentInChildren<MeleeItem>();
        Debug.Log(Stat.DP);
        MeleeAttack(mi.hitPoint, mi.MeleeData.DP + Stat.DP);
    }
    /// <summary> 초기 설정 </summary>
    private void Init()
    {
        _statUI = _canvas.GetComponentInChildren<StatUI>();
        PlayerAnimEvent AnimEvent = GetComponentInChildren<PlayerAnimEvent>();
        AnimEvent.GetWeapon += OnGetWeapon;
        AnimEvent.GetPistol += GetPistol;
        AnimEvent.PutGun += PutGun;
        AnimEvent.AnimStart += AnimStart;
        AnimEvent.AnimEnd += AnimEnd;
        AnimEvent.AttackStart += OnAttackStart;
        ChangeState(STATE.CREATE);
        Inventory._inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(1100.0f, 110.0f);
        _statUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1100.0f, 110.0f);
    }
    #endregion

    /// <summary> 스탯량이 0 이하, 최대 수치 이상으로 올라가지 않도록 설정 </summary>
    void StatClamp()
    {
        Stat.HP = Mathf.Clamp(Stat.HP, 0, Stat.MaxHP);
        Stat.Hunger = Mathf.Clamp(Stat.Hunger, 0, Stat.MaxHunger);
        Stat.Thirsty = Mathf.Clamp(Stat.Thirsty, 0, Stat.MaxThirsty);
        Stat.Stamina = Mathf.Clamp(Stat.Stamina, 0, Stat.MaxStamina);
    }

    /// <summary> 키보드 Axis값에 기반하여 이동 </summary>
    void Move(float MoveSpeed)
    {
        pos.x = Input.GetAxis("Horizontal");
        pos.z = Input.GetAxis("Vertical");

        base.Moving(this.transform, ref pos, MoveSpeed, _cameraArm);

        base.Rotate(this.transform, Aimed);
    }

    /// <summary> 플레이어 상태가 Alive일때 실행되는 코루틴 </summary>
    void AliveCoroutine()
    {
        if (aliveCycle != null) return;
        aliveCycle = StartCoroutine(AliveCycle());
    }

    void GetPrimary(int index, ref bool check)
    {
        if (Inventory._inventory.PrimaryItems[index] != null && !check && MotionEnd)
        {
            check = true;

            if (Inventory._inventory.PrimaryItems[index] is GunItem) myAnim.SetTrigger("GetGun");
            else if (Inventory._inventory.PrimaryItems[index] is MeleeItem) myAnim.SetTrigger("GetAxe");

            if (!myAnim.GetBool("isArmed")) myAnim.SetBool("isArmed", true);
        }
    }
    void MeleeAttack(Transform hitPoint, float DP)
    {
        Collider[] list = Physics.OverlapSphere(hitPoint.position, 1.0f, EnemyMask);
        foreach (Collider col in list)
        {
            BattleSystem bs = col.gameObject.GetComponent<BattleSystem>();
            if (bs != null)
            {
                bs.OnDamage(DP);
            }
        }


    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public
    #region 무기 관련 메소드
    public void GetWeapon(int index)
    {
        GameObject go = Inventory._inventory.PrimarySlots[index]._item.gameObject;

        if (myWeapon != null) WeaponSwap(myWeapon.gameObject.transform);

        if (isFirst || isSecond)
        {
            go.transform.parent = null;
        }
        //if(isSecond) // ���� � �ִ� ������ ����

        myWeapon = go;
        // Gun ������Ʈ ���Ͽ� �ڽ�ȭ
        myWeapon.layer = 0;
        myWeapon.transform.parent = HandSorket.transform;
        if (go.GetComponent<EquipmentItem>() is GunItem)
        {
            go.gameObject.transform.localPosition = new Vector3(-0.0006f, 0.2683f, 0.0333f);
            go.gameObject.transform.localEulerAngles = new Vector3(84.598f, 175.911f, -90.631f);
        }
        if (go.GetComponent<EquipmentItem>() is MeleeItem)
        {
            go.gameObject.transform.localPosition = new Vector3(0.003f, 0.04f, -0.003f);
            go.gameObject.transform.localEulerAngles = new Vector3(5.42f, 0.0f, 4.6f);
        }

    }


    /// <summary> �������� ���� </summary>
    public void GetPistol()
    {
        if (myWeapon != null) Destroy(HandSorket.GetComponentInChildren<GunItem>().gameObject); // 이미 권총 장착 상태일때


        myWeapon.transform.parent = HandSorket.transform;
        myWeapon.layer = 0;
        myWeapon.GetComponent<Rigidbody>();
        myWeapon.GetComponent<BoxCollider>();
    }

    public void WeaponSwap(Transform Target)
    {
        if (isFirst)
        {
            Target.transform.parent = null;
            Target.transform.position = BackRightSorket.position;
            Target.transform.rotation = BackRightSorket.rotation;
        }
        else if (isSecond)
        {
            Target.transform.parent = null;
            Target.transform.position = BackLeftSorket.position;
            Target.transform.rotation = BackLeftSorket.rotation;
        }
    }
    /// <summary> 총 소켓으로 넣기 </summary>
    public void PutGun()
    {
        myWeapon.gameObject.transform.parent = null;
        myWeapon = null;
        isFirst = false;
        isSecond = false;
        isPistol = false;
        // Armed = false;
    }



    /// <summary> 플레이어 무기 소켓 업데이트 </summary>
    public void UpdateBackWeapon()
    {
        //  
        if (Inventory._inventory.PrimarySlots[0]._item != null && BackLeftSorket.GetComponentInChildren<EquipmentItem>() == null && !isFirst)
        {
            GameObject gun1 = Inventory._inventory.PrimarySlots[0]._item.gameObject; // 인벤토리 슬롯 인덱스의 게임오브젝트 가져오기
            gun1.transform.position = new Vector3(BackLeftSorket.transform.position.x, BackLeftSorket.transform.position.y, BackLeftSorket.transform.position.z);
            gun1.transform.rotation = BackLeftSorket.rotation;
            gun1.GetComponent<Rigidbody>().isKinematic = true; // 등 뒤 소켓으로 이동 시 충돌 방지를 위함
            gun1.GetComponent<BoxCollider>().enabled = false; // 위와 동일
            gun1.layer = 0;
            gun1.transform.parent = BackLeftSorket.transform;
            gun1.SetActive(true);
        }
        if (Inventory._inventory.PrimarySlots[1]._item != null && BackRightSorket.GetComponentInChildren<EquipmentItem>() == null && !isSecond)
        {
            GameObject gun2 = Inventory._inventory.PrimarySlots[1]._item.gameObject;
            gun2.transform.position = new Vector3(BackRightSorket.transform.position.x, BackRightSorket.transform.position.y, BackRightSorket.transform.position.z);
            gun2.transform.rotation = BackRightSorket.rotation;
            gun2.GetComponent<Rigidbody>().isKinematic = true;
            gun2.GetComponent<BoxCollider>().enabled = false;
            gun2.layer = 0;
            gun2.transform.parent = BackRightSorket.transform;
            gun2.SetActive(true);
        }
        if (Inventory._inventory.SecondaryItem != null && PistolGrip.GetComponentInChildren<EquipmentItem>() == null)
        {
            GameObject pistol = Instantiate(Inventory._inventory.SecondarySlots[0].ItemProperties.ItemPrefab, PistolGrip);
            Destroy(pistol.GetComponent<Rigidbody>());
            Destroy(pistol.GetComponent<BoxCollider>());
            pistol.layer = 0;
            pistol.transform.parent = PistolGrip.transform;
        }
    }

    // 총기 장전 메소드
    public void Reload(GunItem gi, int index)
    {
        AmmoItem am = Inventory._inventory.Items[index] as AmmoItem;
        int require = gi.GunData.Capacity - gi.c_bullet;
        int take = 0;
        while (take < require)
        {
            if (am.Amount <= 0)
            {
                Inventory._inventory.Remove(index);
                int next_index = Inventory._inventory.FindAmmo(am.AmmoData, index);
                if (next_index != -1)
                {
                    index = next_index;
                    am = Inventory._inventory.Items[index] as AmmoItem;
                    continue;
                }
                else break;
            }
            take++;
            am.Amount--;
        }
        StartCoroutine(TakeBullet(gi, take));

    }
    #endregion

    /***********************************************************************
    *                               Anim Methods
    ***********************************************************************/
    #region Animation Methods
    /// <summary> 특정 애니메이션 시작 시 하체 애니메이션 블랜딩 및 모션 값 True </summary>
    /// 모션 값은 애니메이션 실행 중 다른 애니메이션이 실행되지 않도록 제어하기 위해 선언
    public void AnimStart()
    {
        MotionEnd = false;
        myAnim.SetLayerWeight(1, 0.5f);
    }

    /// <summary> 특정 애니메이션 종료 시 하체 애니메이션 블랜딩 및 모션 값 false </summary>
    public void AnimEnd()
    {
        MotionEnd = true;
        myAnim.SetLayerWeight(1, 0.0f);
    }
    #endregion

    /***********************************************************************
    *                               Input Methods
    ***********************************************************************/
    #region 키보드 Input 메소드
    private void InputMethods()
    {
        #region 달리기

        ///<summary> 달리기 키 Down 시 달리기 조건에 맞는지 검사 후 스테미너 차감 </summary>
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Stat.Stamina > 5.0f && !Aimed && MotionEnd)
            {
                myAnim.SetBool("isRun", true);
                // 1. 무기 장착 중
                if (myAnim.GetBool("isArmed"))
                {
                    Stat.MoveSpeed = 3.5f;
                }
                // 2. 무기 미착용
                else
                {
                    Stat.MoveSpeed = 4.0f;
                }

                StopCoroutine("RecoveryStamina");
                Recovery = null;
                Use = StartCoroutine("UseStamina");
            }
            else return;
        }


        ///<summary> 달리기 종료 </summary>
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            myAnim.SetBool("isRun", false);

            // 1. 무기 장착 중
            if (myAnim.GetBool("isArmed"))
            {
                Stat.MoveSpeed = 1.5f;
            }
            // 2. 무기 미착용
            else
            {
                Stat.MoveSpeed = 2.0f;
            }

            StopCoroutine("UseStamina");
            Use = null;
            Recovery = StartCoroutine("RecoveryStamina");
        }
        #endregion

        #region 공격
        ///<summary> 공격 버튼 입력 </summary>
        if (Input.GetMouseButtonDown(0))
        {
            // 공격 준비상태 혹은 조준 상태인지 확인
            if (myAnim.GetBool("isAiming"))
            {
                if (MotionEnd)
                {
                    if (myWeapon.GetComponentInChildren<EquipmentItem>() is GunItem) Fire(myWeapon.GetComponentInChildren<GunItem>());
                    else if (myWeapon.GetComponentInChildren<EquipmentItem>() is MeleeItem) myAnim.SetTrigger("MeleeAttack");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && (ArmCheck() && myWeapon.gameObject.GetComponentInChildren<EquipmentItem>() is GunItem gi))
        {
            if (MotionEnd && gi.c_bullet < gi.GunData.Capacity)
            {
                int index = Inventory._inventory.FindAmmo(gi.GunData.Bullet.GetComponent<BulletMovement>().AmmoData);
                if (index != -1)
                {
                    myAnim.SetTrigger("Reload");
                    Reload(gi, index);
                }
            }
        }

        ///<summary> 주무기 1 버튼 </summary>
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 1. 인벤토리 슬롯 1 검사
            // 그리고 다른 애니메이션 실행 중 여부와 이미 주무기 1을 장착중인지 검사 해당 줄 프로세스는 넘버만 다르고 로직 동일

            isSecond = false;
            isPistol = false;

            GetPrimary(0, ref isFirst);
        }
        ///<summary> 주무기 2 버튼 </summary>
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isFirst = false;
            isPistol = false;

            GetPrimary(1, ref isSecond);
        }
        ///<summary> 보조 무기 버튼 </summary>
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isFirst = false;
            isSecond = false;
            // 1. 보조 무기 슬롯 검사
            if (Inventory._inventory.SecondaryItem != null && !isPistol && MotionEnd)
            {
                // if (!Armed) Armed = true;
                if (isFirst) isFirst = false;
                if (isSecond) isSecond = false;
                if (!isPistol) isPistol = true;

                if (isPistol) myAnim.SetTrigger("GetPistol");

                if (!myAnim.GetBool("isPistol")) myAnim.SetBool("isPistol", true);

                if (!myAnim.GetBool("isArmed")) myAnim.SetBool("isArmed", true);
            }
        }

        ///<summary> 소지 중인 무기 집어넣기 </summary>
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (ArmCheck())
            {
                if (HandSorket.GetComponentInChildren<GunItem>() != null && MotionEnd)
                {
                    if (isPistol) myAnim.SetTrigger("PutPistol");
                    else myAnim.SetTrigger("PutGun");

                    // if (Armed) Armed = false;
                    myAnim.SetBool("isAiming", false);
                    myAnim.SetBool("isArmed", false);
                }
            }
        }
        #endregion

        #region Aim Input Methods

        ///<summary> 현재 무기 장착중인지 확인 후 공격 준비 상태 전환 및 조준 </summary>
        if (Input.GetMouseButton(1) && ArmCheck())
        {
            Aimed = true;
            if (myAnim.GetBool("isArmed")) myAnim.SetBool("isAiming", true);
        }


        if (Input.GetMouseButtonUp(1))
        {
            Aimed = false;
            if (myAnim.GetBool("isArmed"))
            {
                myAnim.SetBool("isAiming", false);
            }
        }


        #endregion

        #region UI Input Methods
        ///<summary> 인벤토리 오브젝트 창 버튼 </summary>
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (ActiveInv)
            {
                Inventory._inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(1100.0f, 110.0f);
            }
            else
            {
                Inventory._inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 110.0f);
            }
            ActiveInv = !ActiveInv;
        }

        ///<summary> 스탯 오브젝트 창 버튼  </summary>
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (ActiveStat)
            {
                _statUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1100.0f, 110.0f);
            }
            else
            {
                _statUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 110.0f);
            }
            ActiveStat = !ActiveStat;
        }
        #endregion
    }
    #endregion

    /***********************************************************************
    *                       Private BattleSystem Methods
    ***********************************************************************/
    #region Private BattleSystem
    private void Fire(GunItem gi)
    {
        if (gi.c_bullet > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 9999.0f))
            {
                // 총알 방향 벡터 계산
                Vector3 dir = hit.point - myWeapon.GetComponent<GunItem>().MuzzlePoint.position;
                dir.y = 0;
                // 위에서 계산된 벡터의 방향으로 바라보도록 Rotation 값 할당
                Quaternion rot = Quaternion.LookRotation(dir.normalized);

                // 총 데이터에 저장되어 있는 Bullet 오브젝트 및 Effect 생성
                Instantiate(myWeapon.GetComponent<GunItem>().GunData.Bullet, myWeapon.GetComponent<GunItem>().MuzzlePoint.position, rot);
                Instantiate(myWeapon.GetComponent<GunItem>().GunData.Effect, myWeapon.GetComponent<GunItem>().MuzzlePoint.position, rot);

                //bullet.transform.parent = null;

            }
            gi.c_bullet--;
            gi.Durability--;
        }
        else
        {
            // 빈 탄창 사운드
        }
    }
    /// <summary> 손에 무기를 들고 있는지 체크 </summary>
    private bool ArmCheck() { return isFirst || isSecond || isPistol; }
    #endregion

    /***********************************************************************
    *                       Public BattleSystem Interface
    ***********************************************************************/
    #region 전투시스템 인터페이스

    public void OnDamage(float Damage)
    {
        if (myState == STATE.DEAD) return;
        myAnim.SetTrigger("Hit");
        MotionEnd = true;
        Stat.HP -= Damage;
        if (Stat.HP <= 0) ChangeState(STATE.DEAD);
        else
        {

        }
    }
    public void OnCritDamage(float CritDamage)
    {
        if (myState == STATE.DEAD) return;
        Stat.HP -= CritDamage;
        myAnim.SetTrigger("Hit");
        if (Stat.HP <= 0) ChangeState(STATE.DEAD);
        else
        {

        }
    }
    public bool IsLive()
    {
        return true;
    }
    #endregion

    /***********************************************************************
    *                               Corutine
    ***********************************************************************/
    #region 코루틴
    IEnumerator AliveCycle()
    {
        while (Stat.HP > Mathf.Epsilon)
        {
            // 허기가 0일때
            if (Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty > Mathf.Epsilon)
            {
                Stat.HP -= Stat.Minus_Hunger * 2f; // HP 감소
                Stat.Thirsty -= Stat.Minus_Thirsty + Stat.Minus_Hunger; // 갈증 수치가 더 빠르게 줄어듦
                Stat.Stamina += Stat.RecoverStamina;
                yield return new WaitForSeconds(1.0f);
            }
            // 갈증이 0일때
            else if (Stat.Thirsty <= Mathf.Epsilon && Stat.Hunger > Mathf.Epsilon)
            {
                Stat.HP -= Stat.Minus_Thirsty;
                Stat.Hunger -= Stat.Minus_Thirsty; // 허기가 더 빠르게 줄어듦
                Stat.Stamina += Stat.RecoverStamina / 2; // 스테미너가 더 늦게 참 
                yield return new WaitForSeconds(1.0f);
            }
            // 허기, 갈증 모두 0일때
            else if (Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty <= Mathf.Epsilon)
            {
                Stat.HP -= 5.0f; // 큰 폭으로 HP 감소
                yield return new WaitForSeconds(1.0f);
            }
            Stat.Hunger -= Stat.Minus_Hunger;
            Stat.Thirsty -= Stat.Minus_Thirsty;
            Stat.Stamina += Stat.RecoverStamina;
            yield return new WaitForSecondsRealtime(1.0f);
        }
        ChangeState(STATE.DEAD);
    }

    /// <summary> 달리기 시 스테미너 차감 코루틴 </summary>
    IEnumerator UseStamina()
    {
        while (Stat.Stamina > 0.0f)
        {
            if (Stat.Stamina <= 0.0f)
            {
                Stat.Stamina = 0.0f;
            }
            Stat.Stamina -= 5.0f;
            yield return new WaitForSeconds(1.0f);
        }
        Use = null;
    }

    /// <summary> 스테미너 자연 회복 코루틴 </summary>
    IEnumerator RecoveryStamina()
    {
        yield return new WaitForSeconds(1.5f);
        while (Stat.Stamina < Stat.MaxStamina)
        {
            if (Stat.Stamina >= Stat.MaxStamina)
            {
                Stat.Stamina = 100.0f;
            }
            Stat.Stamina += 1.0f;
            yield return new WaitForSeconds(1.0f);
        }
        Recovery = null;
    }

    IEnumerator TakeBullet(GunItem gi, int take)
    {
        Reloading = true;
        yield return new WaitForSeconds(2.5f);
        gi.c_bullet += take;
        Reloading = false;
    }

    IEnumerator Dead()
    {
        Text_GameOver.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }
    #endregion
}
#endregion