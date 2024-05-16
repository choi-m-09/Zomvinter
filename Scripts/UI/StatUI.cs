using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    #region 스킬 포인트 변수
    [SerializeField]
    /// <summary> ��ų ����Ʈ �ؽ�Ʈ </summary>
    TMPro.TMP_Text SkillPointText;
    //[SerializeField]
    //List<Player.Stat>
    #endregion

    #region 스탯 버튼 변수
    [SerializeField]
    /// <summary> ��ư ���� </summary>
    Transform BtnArea;
    /// <summary> ��ư �迭 </summary>
    Button[] StatUpBtn;
    #endregion

    #region 스텟 게이지 위치 변수
    [SerializeField]
    /// <summary> �� ������ ���� </summary>
    Transform StrengthGague;
    [SerializeField]
    /// <summary> �ǰ� ������ ���� </summary>
    Transform ConstitutionGague;
    [SerializeField]
    /// <summary> ������ ������ ���� </summary>
    Transform DexterityGague;
    [SerializeField]
    /// <summary> ������ ������ ���� </summary>
    Transform EnduranceGauge;
    [SerializeField]
    /// <summary> ���� ������ ���� </summary>
    Transform IntelligenceGague;

    /// <summary> �� ������ �� </summary>
    Image[] StrengthBar;
    /// <summary> �ǰ� ������ �� </summary>
    Image[] ConstitutionBar;
    /// <summary> ������ ������ �� </summary>
    Image[] DexterityBar;
    /// <summary> ������ ������ �� </summary>
    Image[] EnduranceBar;
    /// <summary> ���� ������ �� </summary>
    Image[] IntelligenceBar;
    #endregion

    #region 스탯 레벨
    /// <summary> �� ���� �� </summary>
    int StrengthLevel = 0;
    /// <summary> �ǰ� ���� �� </summary>
    int ConstitutionLevel = 0;
    /// <summary> ������ ���� �� </summary>
    int DexterityLevel = 0;
    /// <summary> ������ ���� �� </summary>
    int EnduranceLevel = 0;
    /// <summary> ���� ���� �� </summary>
    int IntelligenceLevel = 0;
    #endregion

    Player _player;

    /// <summary> 스킬 포인트 </summary>
    static int SP = 0;

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region

    void Update()
    {
        VisaulizeSP();
        VisualizeButtonArea();
        //Debug.Log("Update" + Player.Stat.Strength);
    }

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        StrengthBar = StrengthGague.GetComponentsInChildren<Image>();
        ConstitutionBar = ConstitutionGague.GetComponentsInChildren<Image>();
        DexterityBar = DexterityGague.GetComponentsInChildren<Image>();
        EnduranceBar = EnduranceGauge.GetComponentsInChildren<Image>();
        IntelligenceBar = IntelligenceGague.GetComponentsInChildren<Image>();
        StatUpBtn = GetComponentsInChildren<Button>();
        SP = 14;
    }
    #endregion

    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region Private �Լ�
    private void VisaulizeSP()
    {
        SkillPointText.text = "SP : " + SP.ToString();
    }

    private void VisualizeButtonArea()
    {
        if (SP > 0)
        {
            BtnArea.gameObject.SetActive(true);
        }
        else
        {
            BtnArea.gameObject.SetActive(false);
        }
    }
    private void DeActiveSkillBtn(int index, int SkillLv)
    {
        if (SkillLv >= 7) { StatUpBtn[index].gameObject.SetActive(false); }
    }
    private int UseSP(int Skill)
    {
        if (SP >= 1)
        {
            if (Skill >= 0 || Skill < 7)
            {
               Skill += 1;
                SP -= 1;
                return Skill;
            }
        }
        return Skill;
    }

    private bool isMaxSkill(int SkillLevel)
    {
        bool isMax = false;
        if(SkillLevel >= 7)
        {
            isMax = true;
        }
        else
        {
            isMax = false;
        }
        return isMax;
    }

    private void ApplyStrSkillLevel()
    {
        _player.Stat.Strength = _player.Stat.Strength +  2;
        _player.Stat.DP += 10.0f;
        Player.AttackSpeed += 0.2f;
        Debug.Log(Player.AttackSpeed);
    }
    private void ApplyConSkillLevel()   
    {
        _player.Stat.Constitution = _player.Stat.Constitution + 2;
        _player.Stat.Minus_Hunger -= 0.1f;
        _player.Stat.MaxHP = _player.Stat.MaxHP + 10.0f;
        Debug.Log("Con : " + _player.Stat.Constitution);
    }
    private void ApplyDexSkillLevel() 
    {
        _player.Stat.Dexterity = _player.Stat.Dexterity + 2;
        Debug.Log("Dex : " + _player.Stat.Dexterity);
    }
    private void ApplyEndSkillLevel() 
    {
        _player.Stat.Endurance = _player.Stat.Endurance + 2;
        _player.Stat.Minus_Thirsty -= 0.1f;
        _player.Stat.MaxStamina += 15.0f;
        _player.Stat.RecoverStamina += 0.5f;
        Debug.Log("End : " + _player.Stat.Endurance);
    }
    private void ApplyIntSkillLevel() 
    {
        _player.Stat.Intelligence += 1;
        FindObjectOfType<CraftManual>().Possible_Craft(_player.Stat.Intelligence);
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public
    public void AddSP(int amount)
    {
        SP += amount;
    }
    #endregion

    /***********************************************************************
    *                               Button Listener
    ***********************************************************************/
    #region
    /// <summary> �� ���� �� - ��ư ������ </summary>
    public void StrengthUp()
    {
        if (!isMaxSkill(StrengthLevel))
        {
            StrengthLevel = UseSP(StrengthLevel);
            StrengthBar[StrengthLevel - 1].color = Color.red;
            DeActiveSkillBtn(0, StrengthLevel);
            ApplyStrSkillLevel(); 
        }
    }

    /// <summary> �ǰ� ���� �� - ��ư ������ </summary>
    public void ConstitutionUp()
    {
        if (!isMaxSkill(ConstitutionLevel))
        {
            ConstitutionLevel = UseSP(ConstitutionLevel);
            ConstitutionBar[ConstitutionLevel - 1].color = Color.red;
            DeActiveSkillBtn(1, ConstitutionLevel);
            ApplyConSkillLevel(); 
        }
    }

    /// <summary> ������ ���� �� - ��ư ������ </summary>
    public void DexterityUp()
    {
        if (!isMaxSkill(DexterityLevel))
        {
            DexterityLevel = UseSP(DexterityLevel);
            DexterityBar[DexterityLevel - 1].color = Color.red;
            DeActiveSkillBtn(2, DexterityLevel);
            ApplyDexSkillLevel();
        }
    }

    /// <summary> ������ ���� �� - ��ư ������ </summary>
    public void EnduranceUp()
    {
        if (!isMaxSkill(EnduranceLevel))
        {
            EnduranceLevel = UseSP(EnduranceLevel);
            EnduranceBar[EnduranceLevel - 1].color = Color.red;
            DeActiveSkillBtn(3, EnduranceLevel);
            ApplyEndSkillLevel();
        }
    }

    /// <summary> ���� ���� �� - ��ư ������ </summary>
    public void IntelligenceUp()
    {
        if (!isMaxSkill(IntelligenceLevel))
        {
            IntelligenceLevel = UseSP(IntelligenceLevel);
            IntelligenceBar[IntelligenceLevel - 1].color = Color.red;
            DeActiveSkillBtn(4, IntelligenceLevel);
            ApplyIntSkillLevel();
        }
    }
    #endregion
}
