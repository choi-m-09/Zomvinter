using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary> ���� ���� ��� - ���� ��� </summary>
public enum STATE
{
    NONE, CREATE, ALIVE, BATTLE, DEAD
}



public class Player : PlayerController, BattleSystem
{
    #region �÷��̾� ������ ����
    //���� ����ü
    [Header("�÷��̾� ����")]
    [SerializeField]
    public static CharacterStat Stat;
    #endregion

    #region �κ��丮 UI ����
    private Inventory _Inventory;
    private StatUI _statUI;

    private bool ActiveInv = false;
    private bool ActiveStat = false;
    #endregion

    #region �̵� ���� ����
    /// <summary> �÷��̾� �̵� ���� </summary>
    Vector3 pos = Vector3.zero;

    /// <summary> ũ�ν���� </summary>
    
    #endregion

    #region ���ε� ����

    [Header("���ε� �ʿ�")]
    [SerializeField]
    private Transform _canvas;
    [SerializeField]
    /// <summary> ī�޶� ���� �� </summary>
    private Transform _cameraArm;
    #endregion

    #region ���� ���� ��� - ����
    [Header("���� ���")]
    public STATE myState = STATE.NONE;
    #endregion

    #region �ڷ�ƾ ����
    // ���� ��� �ڷ�ƾ
    private Coroutine aliveCycle = null;

    // ���׹̳� ��� �ڷ�ƾ
    private Coroutine Use = null;
    // ���׹̳� ȸ�� �ڷ�ƾ
    private Coroutine Recovery = null;

    #endregion

    #region ���� ����
    [Header("���� ��ġ ��")]
    [SerializeField]
    public GameObject myWeapon = null;
    [SerializeField]
    public GameObject myKnife = null;
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
    [SerializeField]
    public Transform KnifeGrip;

    public GameObject Text_GameOver;
    

    [Header("�÷��̾� ���� Bool")]
    /// <summary> ���� ���� üũ Bool </summary>
    // public bool Armed = false;
    /// <summary> �ִϸ��̼� ���� üũ Bool </summary>
    public bool MotionEnd = true;

    [SerializeField]
    /// <summary> ù��° ������ �����ߴ��� üũ Bool </summary>
    public bool isFirst = false;
    [SerializeField]
    /// <summary> �ι�° ������ �����ߴ��� üũ Bool </summary>
    public bool isSecond = false;
    [SerializeField]
    /// <summary> �� ������ �����ߴ��� üũ Bool </summary>
    public bool isPistol = false;

    [SerializeField]
    /// <summary> ���� ���� üũ Bool </summary>
    private bool Aimed = true;
    #endregion

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ
    /// <summary> ������ ������ �̺�Ʈ �޼��� (Awake�� Start�� ������ �ڵ带 �ۼ� �� ��) </summary>
    private void OnValidate()
    {

    }

    void Awake()
    {
        Stat = new CharacterStat(); // ����ü ���� ��äȭ
        InitScripts();
        ChangeState(STATE.CREATE);
        _Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(1100.0f, 110.0f);
        _statUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1100.0f, 110.0f);
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
    #region ���� ���� ���
    /// <summary> ���� ��� ���� ��ȯ �Լ� </summary>
    /// <param name="s">����</param>
    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.NONE:
                break;
            case STATE.CREATE:
                InitStatClamp();
                InitStat();
                UpdateBackWeapon();
                GetComponentInChildren<PlayerAnimEvent>().OnAttackKnife += OnMeleeAttack;
                GetComponentInChildren<PlayerAnimEvent>().EndAttackKnife += EndMeleeAttack;
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
    /// <summary> ���� ��� Update �Լ� </summary>
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.NONE:
                break;
            case STATE.CREATE:
                break;
            /// <summary> Player�� ��� �ִ� ��� </summary>
            case STATE.ALIVE:
                AliveCoroutine();
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
    *                               Private Fields
    ***********************************************************************/
    #region Private �Լ�

    #region Init Methods
    private void InitStat()
    {
        // �ʱ� ĳ���� �ִ� ��ġ
        Stat.CycleSpeed = 1.0f;
        Stat.MaxHP = 100.0f; // �ִ� ü��
        Stat.MaxHunger = 100.0f; // �ִ� ��ⷮ
        Stat.MaxStamina = 100.0f; // �ִ� ���׹̳�
        Stat.MaxThirsty = 100.0f; // �ִ� ���� ��ġ

        //�ʱ� ĳ���� ��ġ
        Stat.HP = Stat.MaxHP;
        Stat.AP = 10.0f;
        Stat.DP = 5.0f;
        Stat.MoveSpeed = 2.0f;
        Stat.TurnSpeed = 180.0f;
        Stat.Hunger = Stat.MaxHunger;
        Stat.Thirsty = Stat.MaxThirsty;
        Stat.Stamina = Stat.MaxStamina;

        //�ʱ� ĳ���� �ɷ�ġ ����
        Stat.Strength = 0;
        Stat.Constitution = 0;
        Stat.Dexterity = 0;
        Stat.Endurance = 0;
        Stat.Intelligence = 0;
    }

    /// <summary> ĳ���� ���� ���� �ּ� �ִ밪 ���� </summary>
    private void InitStatClamp()
    {
        Stat.HP = Mathf.Clamp(Stat.HP, 0, Stat.MaxHP);
        Stat.Hunger = Mathf.Clamp(Stat.Hunger, 0, Stat.MaxHunger);
        Stat.Thirsty = Mathf.Clamp(Stat.Thirsty, 0, Stat.MaxThirsty);
        Stat.Stamina = Mathf.Clamp(Stat.Stamina, 0, Stat.MaxStamina);
    }

    /// <summary> ��ũ��Ʈ ���ε� </summary>
    private void InitScripts()
    {
        _Inventory = _canvas.GetComponentInChildren<Inventory>();
        _statUI = _canvas.GetComponentInChildren<StatUI>();
    }
    #endregion

    /// <summary> �÷��̾� �̵� �Լ� </summary>
    void Move(float MoveSpeed)
    {
        pos.x = Input.GetAxis("Horizontal");
        pos.z = Input.GetAxis("Vertical");

        base.Moving(this.transform, pos, MoveSpeed, _cameraArm);

        base.Rotate(this.transform);
    }

    /// <summary> ���� ��ƾ ���� </summary>
    void AliveCoroutine()
    {
        if (aliveCycle != null) return;
        aliveCycle = StartCoroutine(AliveCycle());
    }

    public void StatCalc()
    { 
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public �Լ�
    /// <summary> �ֹ��� ���� </summary>
    public void GetGun(int index)
    {
        if(myWeapon != null) Destroy(HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // �տ� �ִ� ������ ����
        if(isFirst) Destroy(BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����
        if(isSecond) Destroy(BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����

        myWeapon = Instantiate(_Inventory.PrimaryItems[index].ItemPrefab, HandSorket.position, HandSorket.rotation); // Gun Object ����
        myWeapon.transform.parent = HandSorket.transform; // Gun ������Ʈ ���Ͽ� �ڽ�ȭ
        myWeapon.layer = 0;
        Destroy(myWeapon.GetComponent<Rigidbody>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
        Destroy(myWeapon.GetComponent<BoxCollider>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
    }

    /// <summary> �������� ���� </summary>
    public void GetPistol()
    {
        if (myWeapon != null) Destroy(HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // �տ� �ִ� ������ ����
        if (isFirst) Destroy(BackLeftSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����
        if (isSecond) Destroy(BackRightSorket.GetComponentInChildren<WeaponItem>().gameObject); // ���� � �ִ� ������ ����

        myWeapon = Instantiate(_Inventory.SecondaryItems.ItemPrefab, HandSorket.position, HandSorket.rotation);
        myWeapon.transform.parent = HandSorket.transform;
        myWeapon.layer = 0;
        Destroy(myWeapon.GetComponent<Rigidbody>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
        Destroy(myWeapon.GetComponent<BoxCollider>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
    }

    /// <summary> ���� ���� ���� </summary>
    public void PutGun()
    {
        Destroy(HandSorket.GetComponentInChildren<WeaponItem>().gameObject); // �տ� �ִ� ������ ����
        myWeapon = null;
        isFirst = false;
        isSecond = false;
        isPistol = false;
        // Armed = false;
    }

    /// <summary> ���� ǥ�� ������Ʈ </summary>
    public void UpdateBackWeapon()
    {
        // 1. �� ���� 1�� ���Կ� ��� �ְ� / �� ���� 1���� ��� ���� / ù��° ���⸦ ������� �ƴ� ���
        if (_Inventory.PrimaryItems[0] != null && BackLeftSorket.GetComponentInChildren<WeaponItem>() == null && !isFirst)
        {
            GameObject gun1 = Instantiate(_Inventory.PrimaryItems[0].ItemPrefab, BackLeftSorket); //�ֹ��� 1�� �ִ� ��� ����
            Destroy(gun1.GetComponent<Rigidbody>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            Destroy(gun1.GetComponent<BoxCollider>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            gun1.layer = 0; // UI�� �������� �ʵ��� ���̾� ����
            gun1.transform.parent = BackLeftSorket.transform; // �Ҵ�� �� ���Ͽ� �ڽ����� ����
        }
        // 2. �� ���� 2�� ���Կ� ��� �ְ� / �� ���� 2���� ��� ���� / ù��° ���⸦ ������� ���
        if (_Inventory.PrimaryItems[1] != null && BackRightSorket.GetComponentInChildren<WeaponItem>() == null && !isSecond)
        {
            GameObject gun2 = Instantiate(_Inventory.PrimaryItems[1].ItemPrefab, BackRightSorket); //�ֹ��� 2�� �ִ� ��� ����
            Destroy(gun2.GetComponent<Rigidbody>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            Destroy(gun2.GetComponent<BoxCollider>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            gun2.layer = 0; // UI�� �������� �ʵ��� ���̾� ����
            gun2.transform.parent = BackRightSorket.transform; // �Ҵ�� �� ���Ͽ� �ڽ����� ����
        }
        if (_Inventory.SecondaryItems != null && PistolGrip.GetComponentInChildren<WeaponItem>() == null)
        {
            GameObject pistol = Instantiate(_Inventory.SecondaryItems.ItemPrefab, PistolGrip);
            Destroy(pistol.GetComponent<Rigidbody>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            Destroy(pistol.GetComponent<BoxCollider>()); // �浹 ���ɼ� �ִ� ������Ʈ ����
            pistol.layer = 0; // UI�� �������� �ʵ��� ���̾� ����
            pistol.transform.parent = PistolGrip.transform; // �Ҵ�� �㸮 ���Ͽ� �ڽ����� ����
        }
    }
    #endregion

    /***********************************************************************
    *                               Anim Methods
    ***********************************************************************/
    #region �ִϸ��̼� �޼ҵ�
    /// <summary> ���̾� ������ ���� �ִϸ��̼� ���� ���� Ȯ�� </summary>
    public void AnimStart()
    {
        if (MotionEnd) MotionEnd = false;
        myAnim.SetLayerWeight(1, 1.0f);
    }

    /// <summary> ���̾� ������ ���� �ִϸ��̼� ���� �� Ȯ�� </summary>
    public void AnimEnd()
    {
        if (!MotionEnd) MotionEnd = true;
        myAnim.SetLayerWeight(1, 0.0f);
    }
    #endregion

    /***********************************************************************
    *                               Input Methods
    ***********************************************************************/
    #region Input �Լ�
    private void InputMethods()
    {
        #region �̵� Input Methods

        ///<summary> �޸��� ���� Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Stat.Stamina > 5.0f)
            {
                myAnim.SetBool("isRun", true);

                // 1. ���� ������ �� �޸��� ���
                if (myAnim.GetBool("isArmed"))
                {
                    Stat.MoveSpeed = 2.5f;
                }
                // 2. �� ���� ������ �� �޸��� ���
                else
                {
                    Stat.MoveSpeed = 5.0f;
                }

                StopCoroutine("RecoveryStamina");
                Recovery = null;
                if (Use != null) return;
                Use = StartCoroutine("UseStamina");
            }
            else return;
        }

        ///<summary> �޸��� ��� Input �޼��� </summary>
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            myAnim.SetBool("isRun", false);

            // 1. ���� ������ �� �ȴ� ���
            if (myAnim.GetBool("isArmed"))
            {
                Stat.MoveSpeed = 1.5f;
            }
            // 2. �� ���� ������ �� �ȴ� ���
            else
            {
                Stat.MoveSpeed = 2.0f;
            }

            StopCoroutine("UseStamina");
            Use = null;
            if (Recovery != null) return;
            Recovery = StartCoroutine("RecoveryStamina");
        }
        #endregion

        #region ���� Input Methods
        ///<summary> ��� Input �޼��� </summary>
        if (Input.GetMouseButtonDown(0))
        {
            // ���� ���� �� ���
            if(myAnim.GetBool("isAiming"))
            {
                Fire(myWeapon.GetComponent<WeaponItem>().WeaponData.Damage, Stat.AP);
            }
        }

        ///<summary> ���� ���� Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.V))
        {
            if(MotionEnd)
            {
                myKnife.transform.parent = HandSorket;
                myKnife.transform.position = HandSorket.position;
                myKnife.transform.rotation = HandSorket.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f);
                myAnim.SetTrigger("MeleeAttack");
                //// ���� ���� ó��
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(MotionEnd) myAnim.SetTrigger("Reload");

            //// ������ ����
        }

        ///<summary> ù��° �ֹ��� ��� ��ȯ Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 1. �ֹ��� 1�� ���Կ� �������� �ְ� �� ������ 1�� ���Կ� �������� �ִ� ���
            if (_Inventory.PrimaryItems[0] != null && !isFirst && MotionEnd)
            {
                // if (!Armed) Armed = true;
                if (!isFirst) isFirst = true; // ù��° �ֹ��� true
                if (isSecond) isSecond = false; // �ι�° �ֹ��� false
                if (isPistol) isPistol = false; // �������� false

                if (isFirst) myAnim.SetTrigger("GetGun"); // ���� �ִϸ��̼� ���

                if (!myAnim.GetBool("isArmed")) myAnim.SetBool("isArmed", true); // ���� ���� üũ
            }
        }
        ///<summary> �ι�° �ֹ��� ��� ��ȯ Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // 1. 2�� ���Կ� �������� �ִ� ���
            if (_Inventory.PrimaryItems[1] != null && !isSecond && MotionEnd)
            {
                // if (!Armed) Armed = true;
                if (isFirst) isFirst = false; // ù��° �ֹ��� false
                if (!isSecond) isSecond = true; // �ι�° �ֹ��� true
                if (isPistol) isPistol = false; // �������� false

                if (isSecond) myAnim.SetTrigger("GetGun"); // ���� �ִϸ��̼� ���

                if (!myAnim.GetBool("isArmed")) myAnim.SetBool("isArmed", true); // ���� ���°� false �� ��� true
            }
        }
        ///<summary> �������� ��� ��ȯ Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // 1. 2�� ���Կ� �������� �ִ� ���
            if (_Inventory.SecondaryItems != null && !isPistol && MotionEnd)
            {
                // if (!Armed) Armed = true;
                if (isFirst) isFirst = false; // ù��° �ֹ��� false
                if (isSecond) isSecond = false; // �ι�° �ֹ��� false
                if (!isPistol) isPistol = true; // �������� true

                if (isPistol) myAnim.SetTrigger("GetPistol");

                if(!myAnim.GetBool("isPistol")) myAnim.SetBool("isPistol", true);

                if (!myAnim.GetBool("isArmed")) myAnim.SetBool("isArmed", true); // ���� ���°� false �� ��� true
            }
        }

        ///<summary> ��� ���� Input �޼��� </summary>
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isFirst || isSecond || isPistol)
            {
                if (HandSorket.GetComponentInChildren<WeaponItem>() != null && MotionEnd)
                {
                    if (isFirst)
                    {
                        myAnim.SetTrigger("PutGun");
                    }
                    else if (isSecond)
                    {
                        myAnim.SetTrigger("PutGun");
                    }
                    else if (isPistol)
                    {
                        myAnim.SetTrigger("PutPistol");
                    }

                    // if (Armed) Armed = false;
                    myAnim.SetBool("isAiming", false);
                    myAnim.SetBool("isArmed", false);
                }
            }
        }
        #endregion

        #region ���� ���� Input Methods

        ///<summary> ���� ���� Input �޼��� </summary>
        if (myAnim.GetBool("isArmed") && Input.GetMouseButton(1))
        {
            Aimed = true;
            myAnim.SetBool("isAiming", true);
        }

        ///<summary> ���� ���� ���� Input �޼��� </summary>
        if (myAnim.GetBool("isArmed") && Input.GetMouseButtonUp(1))
        {
            Aimed = false;
            myAnim.SetBool("isAiming", false);

        }
        #endregion

        #region UI Input Methods
        ///<summary> �κ��丮 â Ȳ��ȭ/��Ȱ��ȭ </summary>
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (ActiveInv)
            {
                _Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(1100.0f, 110.0f);
            }
            else
            {
                _Inventory.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 110.0f);
            }
            ActiveInv = !ActiveInv;
        }

        ///<summary> �÷��̾� ���� â Ȱ��ȭ/��Ȱ��ȭ </summary>
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(ActiveStat)
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
    #region Private BattleSystem �Լ�
    private void OnMeleeAttack()
    {
        
        Collider[] list = Physics.OverlapSphere(myKnife.transform.position, 1.0f, EnemyMask);
        foreach(Collider col in list)
        {
            BattleSystem bs = col.gameObject.GetComponent<BattleSystem>();
            if (bs != null)
            {
                bs.OnDamage(Stat.AP);
            }
        }
    }

    private void EndMeleeAttack()
    {
        myKnife.transform.parent = KnifeGrip;
        myKnife.transform.position = KnifeGrip.position;
        myKnife.transform.rotation = KnifeGrip.rotation;
    }

    private void Fire(float WeaponAP, float PlayerAP)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 9999.0f))
        {
            // �� ������ ���� ����
            Vector3 dir = hit.point - myWeapon.GetComponent<WeaponItem>().MuzzlePoint.position;
            // ���� ������ ����
            Quaternion rot = Quaternion.LookRotation(dir.normalized);

            // ������Ʈ ����
            GameObject bullet = Instantiate(myWeapon.GetComponent<WeaponItem>().WeaponData.Bullet, myWeapon.GetComponent<WeaponItem>().MuzzlePoint.position, rot);

            // �θ� ���� null
            bullet.transform.parent = null;
            // forward �������� �̵��ϴ� �ڷ�ƾ ȣ��
            bullet.GetComponent<AmmoItem>().Fire(WeaponAP, PlayerAP);
        }
    }
    #endregion

    /***********************************************************************
    *                       Public BattleSystem Methods
    ***********************************************************************/
    #region Public BattleSystem �Լ�

    public void OnDamage(float Damage)
    {
        if (myState == STATE.DEAD) return;
        myAnim.SetTrigger("Hit");
        Stat.HP -= Damage;
        if (Stat.HP <= 0) ChangeState(STATE.DEAD);
        else
        {
             // �ǰ� �ִϸ��̼�
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
            // ũ��Ƽ�� �ǰ� �ִϸ��̼�
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
    #region �ڷ�ƾ
    IEnumerator AliveCycle()
    {
        while (Stat.HP > Mathf.Epsilon)
        {
            // ����� ��ġ 0�϶�
            if(Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty > Mathf.Epsilon)
            {
                Debug.Log(Stat.Thirsty);
                Stat.HP -= Stat.CycleSpeed * 2f;
                Stat.Thirsty -= Stat.CycleSpeed;
            }
            // ���� ��ġ 0�϶�
            else if (Stat.Thirsty <= Mathf.Epsilon && Stat.Hunger > Mathf.Epsilon)
            {
                Stat.Hunger -= Stat.CycleSpeed;
                Stat.Stamina += Stat.StaminaCycle / 2;
                Stat.HP -= Stat.CycleSpeed;
            }
            // �� �� 0�϶�
            else if(Stat.Hunger <= Mathf.Epsilon && Stat.Thirsty <= Mathf.Epsilon)
            {
                Stat.HP -= Stat.CycleSpeed * 5;
            }
            Stat.Hunger -= Stat.CycleSpeed;
            Stat.Thirsty -= Stat.CycleSpeed;
            Stat.Stamina += Stat.StaminaCycle;
            yield return new WaitForSecondsRealtime(1.0f);
        }
        ChangeState(STATE.DEAD);
    }

    /// <summary> ���׹̳� �Ҹ� �ڷ�ƾ </summary>
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

    /// <summary> ���׹̳� ȸ�� �ڷ�ƾ </summary>
    IEnumerator RecoveryStamina()
    {
        yield return new WaitForSeconds(2.0f);
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

    IEnumerator Dead()
    {
        Text_GameOver.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }
    #endregion

}
