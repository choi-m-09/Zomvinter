using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    #region ������Ʈ ���� ����
    [SerializeField]
    /// <summary> ��ų ����Ʈ �ؽ�Ʈ </summary>
    TMPro.TMP_Text SkillPointText;
    //[SerializeField]
    //List<Player.Stat>
    #endregion

    #region ��ư ����
    [SerializeField]
    /// <summary> ��ư ���� </summary>
    Transform BtnArea;
    /// <summary> ��ư �迭 </summary>
    Button[] StatUpBtn;
    #endregion

    #region ������ ����
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

    #region ���� ����
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

    /// <summary> ��ų ����Ʈ �� </summary>
     static int SP = 0;

    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region ����Ƽ �̺�Ʈ

    void Update()
    {
        VisaulizeSP();
        VisualizeButtonArea();
        //Debug.Log("Update" + Player.Stat.Strength);
    }

    private void Start()
    {
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
        if (SP >= 1) // ��ų����Ʈ�� 0�϶��� ��밡���� ���� ���� 
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

    private void ApplyStrSkillLevel(int SkillLevel) // �� ���� ����
    {
        Player.Stat.Strength = Player.Stat.Strength +  2; //���� ���ȿ� ��ų������ Str +2
        Player.Stat.MaxHP = Player.Stat.MaxHP + (Player.Stat.Strength * 10 * (1 / 100));
        Debug.Log("Str : " + Player.Stat.Strength);
        Debug.Log("HP : " + Player.Stat.HP + "/ " + Player.Stat.MaxHP);
    }
    private void ApplyConSkillLevel(int SkillLevel) // �ִ�ü�� ���� ���� 
    {
        Player.Stat.Constitution = Player.Stat.Constitution + 2; 
        Debug.Log("Con : " + Player.Stat.Constitution);
    }
    private void ApplyDexSkillLevel(int SkillLevel) // ������ ���� ���� 
    {
        Player.Stat.Dexterity = Player.Stat.Dexterity + 2; 
        Debug.Log("Dex : " + Player.Stat.Dexterity);
    }
    private void ApplyEndSkillLevel(int SkillLevel) // ���¹̳� ���� ���� 
    {
        Player.Stat.Endurance = Player.Stat.Endurance + 2; 
        Debug.Log("End : " + Player.Stat.Endurance);
    }
    private void ApplyIntSkillLevel(int SkillLevel) // ���� ���� ���� 
    {
        Player.Stat.Intelligence = Player.Stat.Intelligence + 2;
        Debug.Log("Int : " + Player.Stat.Intelligence);
    }
    #endregion

    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region Public �Լ�
    public void AddSP(int amount)
    {
        SP += amount;
    }
    #endregion

    /***********************************************************************
    *                               Button Listener
    ***********************************************************************/
    #region ��ư ������
    /// <summary> �� ���� �� - ��ư ������ </summary>
    public void StrengthUp()
    {
        if (!isMaxSkill(StrengthLevel))
        {
            StrengthLevel = UseSP(StrengthLevel);
            StrengthBar[StrengthLevel - 1].color = Color.red;
            DeActiveSkillBtn(0, StrengthLevel);

            ApplyStrSkillLevel(StrengthLevel); //��ų ������ �� �÷��̾� ����.���� ��½�Ŵ 
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
            ApplyConSkillLevel(ConstitutionLevel); 
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
            ApplyDexSkillLevel(DexterityLevel);
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
            ApplyEndSkillLevel(EnduranceLevel);
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
            ApplyIntSkillLevel(IntelligenceLevel);
        }
    }
    #endregion
}
